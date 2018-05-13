Shader "Hidden/Clouds"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Off

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 5.0
#pragma multi_compile __ DEBUG_NO_LOW_FREQ_NOISE
#pragma multi_compile __ DEBUG_NO_HIGH_FREQ_NOISE
#pragma multi_compile __ DEBUG_NO_CURL
#pragma multi_compile __ ALLOW_IN_CLOUDS
#pragma multi_compile __ RANDOM_JITTER_WHITE RANDOM_JITTER_BLUE
#pragma multi_compile __ RANDOM_UNIT_SPHERE
#pragma multi_compile __ SLOW_LIGHTING

#include "UnityCG.cginc"

#define BIG_STEP 3.0

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 pos : SV_POSITION;
		float4 ray : TEXCOORD1;
	};


	uniform sampler2D_float _CameraDepthTexture;

	uniform float4x4 _CameraInvViewMatrix;
	uniform float4x4 _FrustumCornersES;
	uniform float4 _CameraWS;
	uniform float _FarPlane;

	uniform sampler2D _MainTex;
	uniform float4 _MainTex_TexelSize;

	uniform sampler2D _AltoClouds;
	uniform sampler3D _ShapeTexture;
	uniform sampler3D _DetailTexture;
	uniform sampler2D _WeatherTexture;
	uniform sampler2D _CurlNoise;
	uniform sampler2D _BlueNoise;
	uniform float4 _BlueNoise_TexelSize;
	uniform float4 _Randomness;
	uniform float _SampleMultiplier;

	uniform float3 _SunDir;
	uniform float3 _PlanetCenter;
	uniform float3 _SunColor;

	uniform float3 _CloudBaseColor;
	uniform float3 _CloudTopColor;

	uniform float3 _ZeroPoint;
	uniform float _SphereSize;
	uniform float2 _CloudHeightMinMax;
	uniform float _Thickness;

	uniform float _Coverage;
	uniform float _AmbientLightFactor;
	uniform float _SunLightFactor;
	uniform float _HenyeyGreensteinGForward;
	uniform float _HenyeyGreensteinGBackward;
	uniform float _LightStepLength;
	uniform float _LightConeRadius;

	uniform float _Density;

	uniform float _Scale;
	uniform float _DetailScale;
	uniform float _WeatherScale;
	uniform float _CurlDistortScale;
	uniform float _CurlDistortAmount;

	uniform float _WindSpeed;
	uniform float3 _WindDirection;
	uniform float3 _WindOffset;
	uniform float2 _CoverageWindOffset;
	uniform float2 _HighCloudsWindOffset;

	uniform float _CoverageHigh;
	uniform float _CoverageHighScale;
	uniform float _HighCloudsScale;

	uniform float2 _LowFreqMinMax;
	uniform float _HighFreqModifier;

	uniform float4 _Gradient1;
	uniform float4 _Gradient2;
	uniform float4 _Gradient3;

	uniform int _Steps;

	v2f vert(appdata v)
	{
		v2f o;

		half index = v.vertex.z;
		v.vertex.z = 0.1;

		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv.xy;

#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1 - o.uv.y;
#endif

		// Get the eyespace view ray (normalized)
		o.ray = _FrustumCornersES[(int)index];
		// Dividing by z "normalizes" it in the z axis
		// Therefore multiplying the ray by some number i gives the viewspace position
		// of the point on the ray with [viewspace z]=i
		o.ray /= abs(o.ray.z);

		// Transform the ray from eyespace to worldspace
		o.ray = mul(_CameraInvViewMatrix, o.ray);

		return o;
	}

	// http://byteblacksmith.com/improvements-to-the-canonical-one-liner-glsl-rand-for-opengl-es-2-0/
	float rand(float2 co) {
		float a = 12.9898;
		float b = 78.233;
		float c = 43758.5453;
		float dt = dot(co.xy, float2(a, b));
		float sn = fmod(dt, 3.14);

		return 2.0 * frac(sin(sn) * c) - 1.0;
	}

	float weatherDensity(float3 weatherData) // Gets weather density from weather texture sample and adds 1 to it.
	{
		return weatherData.b + 1.0;
	}

	// from GPU Pro 7 - remaps value from one range to other range
	float remap(float original_value, float original_min, float original_max, float new_min, float new_max)
	{
		return new_min + (((original_value - original_min) / (original_max - original_min)) * (new_max - new_min));
	}

	// returns height fraction [0, 1] for point in cloud
	float getHeightFractionForPoint(float3 pos)
	{
		return saturate((distance(pos,  _PlanetCenter) - (_SphereSize + _CloudHeightMinMax.x)) / _Thickness);
	}

	// samples the gradient
	float sampleGradient(float4 gradient, float height)
	{
		return smoothstep(gradient.x, gradient.y, height) - smoothstep(gradient.z, gradient.w, height);
	}

	// lerps between cloud type gradients and samples it
	float getDensityHeightGradient(float height, float3 weatherData)
	{
		float type = weatherData.g;
		float4 gradient = lerp(lerp(_Gradient1, _Gradient2, type * 2.0), _Gradient3, saturate((type - 0.5) * 2.0));
		return sampleGradient(gradient, height);
	}

	// samples weather texture
	float3 sampleWeather(float3 pos) {
		float3 weatherData = tex2Dlod(_WeatherTexture, float4((pos.xz + _CoverageWindOffset) * _WeatherScale, 0, 0)).rgb;
		weatherData.r = saturate(weatherData.r - _Coverage);
		return weatherData;
	}

	// samples cloud density
	float sampleCloudDensity(float3 p, float heightFraction, float3 weatherData, float lod, bool sampleDetail)
	{
		float3 pos = p + _WindOffset; // add wind offset
		pos += heightFraction * _WindDirection * 700.0; // shear at higher altitude

#if defined(DEBUG_NO_LOW_FREQ_NOISE)
		float cloudSample = 0.7;
		cloudSample = remap(cloudSample, _LowFreqMinMax.x, _LowFreqMinMax.y, 0.0, 1.0);
#else
		float cloudSample = tex3Dlod(_ShapeTexture, float4(pos * _Scale, lod)).r; // sample cloud shape texture
		cloudSample = remap(cloudSample * pow(1.2 - heightFraction, 0.1), _LowFreqMinMax.x, _LowFreqMinMax.y, 0.0, 1.0); // pick certain range from sample texture
#endif
		cloudSample *= getDensityHeightGradient(heightFraction, weatherData); // multiply cloud by its type gradient

		float cloudCoverage = weatherData.r;
		cloudSample = saturate(remap(cloudSample, saturate(heightFraction / cloudCoverage), 1.0, 0.0, 1.0)); // Change cloud coverage based by height and use remap to reduce clouds outside coverage
		cloudSample *= cloudCoverage; // multiply by cloud coverage to smooth them out, GPU Pro 7

#if defined(DEBUG_NO_HIGH_FREQ_NOISE)
		cloudSample = remap(cloudSample, 0.2, 1.0, 0.0, 1.0);
#else
		if (cloudSample > 0.0 && sampleDetail) // If cloud sample > 0 then erode it with detail noise
		{
#if defined(DEBUG_NO_CURL)
#else
			float3 curlNoise = mad(tex2Dlod(_CurlNoise, float4(p.xz * _CurlDistortScale, 0, 0)).rgb, 2.0, -1.0); // sample Curl noise and transform it from [0, 1] to [-1, 1]
			pos += float3(curlNoise.r, curlNoise.b, curlNoise.g) * heightFraction * _CurlDistortAmount; // distort position with curl noise
#endif
			float detailNoise = tex3Dlod(_DetailTexture, float4(pos * _DetailScale, lod)).r; // Sample detail noise

			float highFreqNoiseModifier = lerp(1.0 - detailNoise, detailNoise, saturate(heightFraction * 10.0)); // At lower cloud levels invert it to produce more wispy shapes and higher billowy

			cloudSample = remap(cloudSample, highFreqNoiseModifier * _HighFreqModifier, 1.0, 0.0, 1.0); // Erode cloud edges
		}
#endif

		return max(cloudSample * _SampleMultiplier, 0.0);
	}

	// GPU Pro 7
	float beerLaw(float density)
	{
		float d = -density * _Density;
		return max(exp(d), exp(d * 0.5)*0.7);
	}

	// GPU Pro 7
	float HenyeyGreensteinPhase(float cosAngle, float g)
	{
		float g2 = g * g;
		return ((1.0 - g2) / pow(1.0 + g2 - 2.0 * g * cosAngle, 1.5)) / 4.0 * 3.1415;
	}

	// GPU Pro 7
	float powderEffect(float density, float cosAngle)
	{
		float powder = 1.0 - exp(-density * 2.0);
		return lerp(1.0f, powder, saturate((-cosAngle * 0.5f) + 0.5f));
	}

	float calculateLightEnergy(float density, float cosAngle, float powderDensity) { // calculates direct light components and multiplies them together
		float beerPowder = 2.0 * beerLaw(density) * powderEffect(powderDensity, cosAngle);
		float HG = max(HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGForward), HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGBackward)) * 0.07 + 0.8;
		return beerPowder * HG;
	}

	float randSimple(float n) // simple hash function for more random light vectors
	{
		return mad(frac(sin(n) * 43758.5453123), 2.0, -1.0);
	}

	float3 rand3(float3 n) // random vector
	{
		return normalize(float3(randSimple(n.x), randSimple(n.y), randSimple(n.z)));
	}

	float3 sampleConeToLight(float3 pos, float3 lightDir, float cosAngle, float density, float3 initialWeather, float lod)
	{
#if defined(RANDOM_UNIT_SPHERE)
#else
		const float3 RandomUnitSphere[5] = // precalculated random vectors
		{
			{ -0.6, -0.8, -0.2 },
		{ 1.0, -0.3, 0.0 },
		{ -0.7, 0.0, 0.7 },
		{ -0.2, 0.6, -0.8 },
		{ 0.4, 0.3, 0.9 }
		};
#endif
		float heightFraction;
		float densityAlongCone = 0.0;
		const int steps = 5; // light cone step count
		float3 weatherData;
		for (int i = 0; i < steps; i++) {
			pos += lightDir * _LightStepLength; // march forward
#if defined(RANDOM_UNIT_SPHERE) // apply random vector to achive cone shape
			float3 randomOffset = rand3(pos) * _LightStepLength * _LightConeRadius * ((float)(i + 1));
#else
			float3 randomOffset = RandomUnitSphere[i] * _LightStepLength * _LightConeRadius * ((float)(i + 1));
#endif
			float3 p = pos + randomOffset; // light sample point
			// sample cloud
			heightFraction = getHeightFractionForPoint(p); 
			weatherData = sampleWeather(p);
			densityAlongCone += sampleCloudDensity(p, heightFraction, weatherData, lod + ((float)i) * 0.5, true) * weatherDensity(weatherData);
		}

#if defined(SLOW_LIGHTING) // if doing slow lighting then do more samples in straight line
		pos += 24.0 * _LightStepLength * lightDir;
		weatherData = sampleWeather(pos);
		heightFraction = getHeightFractionForPoint(pos);
		densityAlongCone += sampleCloudDensity(pos, heightFraction, weatherData, lod, true) * 2.0;
		int j = 0;
		while (1) {
			if (j > 22) {
				break;
			}
			pos += 4.25 * _LightStepLength * lightDir;
			weatherData = sampleWeather(pos);
			if (weatherData.r > 0.05) {
				heightFraction = getHeightFractionForPoint(pos);
				densityAlongCone += sampleCloudDensity(pos, heightFraction, weatherData, lod, true);
			}

			j++;
		}
#else
		pos += 32.0 * _LightStepLength * lightDir; // light sample from further away
		weatherData = sampleWeather(pos);
		heightFraction = getHeightFractionForPoint(pos);
		densityAlongCone += sampleCloudDensity(pos, heightFraction, weatherData, lod + 2, false) * weatherDensity(weatherData) * 3.0;
#endif
		
		return calculateLightEnergy(densityAlongCone, cosAngle, density) * _SunColor;
	}

	// raymarches clouds
	fixed4 raymarch(float3 ro, float3 rd, float steps, float depth, float cosAngle)
	{
		float3 pos = ro;
		fixed4 res = 0.0; // cloud color
		float lod = 0.0;
		float zeroCount = 0.0; // number of times cloud sample has been 0
		float stepLength = BIG_STEP; // step length multiplier, 1.0 when doing small steps


		for (float i = 0.0; i < steps; i += stepLength)
		{
			if (distance(_CameraWS, pos) >= depth || res.a >= 0.99) { // check if is behind some geometrical object or that cloud color aplha is almost 1
				break;  // if it is then raymarch ends
			}
			float heightFraction = getHeightFractionForPoint(pos);
#if defined(ALLOW_IN_CLOUDS) // if it is allowed to fly in the clouds, then we need to check that the sample position is above the ground and in the cloud layer
			if (pos.y < _ZeroPoint.y || heightFraction < 0.0 || heightFraction > 1.0) {
				break;
			}
#endif
			float3 weatherData = sampleWeather(pos); // sample weather
			if (weatherData.r <= 0.1) // if value is low, then continue marching, at some specific weather textures makes it a bit faster.
			{
				pos += rd * stepLength;
				zeroCount += 1.0;
				stepLength = zeroCount > 10.0 ? BIG_STEP : 1.0;
				continue;
			}

			float cloudDensity = saturate(sampleCloudDensity(pos, heightFraction, weatherData, lod, true)); // sample the cloud

			if (cloudDensity > 0.0) // check if cloud density is > 0
			{
				zeroCount = 0.0; // set zero cloud density counter to 0

				if (stepLength > 1.0) // if we did big steps before
				{
					i -= stepLength - 1.0; // then move back, previous 0 density location + one small step
					pos -= rd * (stepLength - 1.0);
					weatherData = sampleWeather(pos); // sample weather
					cloudDensity = saturate(sampleCloudDensity(pos, heightFraction, weatherData, lod, true)); // and cloud again
				}

				float4 particle = cloudDensity; // construct cloud particle
				float3 directLight = sampleConeToLight(pos, _SunDir, cosAngle, cloudDensity, weatherData, lod); // calculate direct light energy and color
				float3 ambientLight = lerp(_CloudBaseColor, _CloudTopColor, heightFraction); // and ambient

				directLight *= _SunLightFactor; // multiply them by their uniform factors
				ambientLight *= _AmbientLightFactor;

				particle.rgb = directLight + ambientLight; // add lights up and set cloud particle color

				particle.rgb *= particle.a; // multiply color by clouds density
				res = (1.0 - res.a) * particle + res; // use premultiplied alpha blending to acumulate samples
			}
			else // if cloud sample was 0, then increase zero cloud sample counter
			{
				zeroCount += 1.0;
			}
			stepLength = zeroCount > 10.0 ? BIG_STEP : 1.0; // check if we need to do big or small steps

			pos += rd * stepLength; // march forward
		}

		return res;
	}

	// https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection
	float3 findRayStartPos(float3 rayOrigin, float3 rayDirection, float3 sphereCenter, float radius)
	{
		float3 l = rayOrigin - sphereCenter;
		float a = 1.0;
		float b = 2.0 * dot(rayDirection, l);
		float c = dot(l, l) - pow(radius, 2);
		float D = pow(b, 2) - 4.0 * a * c;
		if (D < 0.0)
		{
			return rayOrigin;
		}
		else if (abs(D) - 0.00005 <= 0.0)
		{
			return rayOrigin + rayDirection * (-0.5 * b / a);
		}
		else
		{
			float q = 0.0;
			if (b > 0.0)
			{
				q = -0.5 * (b + sqrt(D));
			}
			else
			{
				q = -0.5 * (b - sqrt(D));
			}
			float h1 = q / a;
			float h2 = c / q;
			float2 t = float2(min(h1, h2), max(h1, h2));
			if (t.x < 0.0) {
				t.x = t.y;
				if (t.x < 0.0) {
					return rayOrigin;
				}
			}
			return rayOrigin + t.x * rayDirection;
		}
	}

	// http://momentsingraphics.de/?p=127#jittering
	float getRandomRayOffset(float2 uv) // uses blue noise texture to get random ray offset
	{
		float noise = tex2D(_BlueNoise, uv).x;
		noise = mad(noise, 2.0, -1.0);
		return noise;
	}

	fixed4 altoClouds(float3 ro, float3 rd, float depth, float cosAngle) { // samples high altitude clouds
		fixed4 res = 0.0;
		float3 pos = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _CloudHeightMinMax.y + 3000.0); // finds sample position
		float dist = distance(ro, pos);
		if (dist < depth && pos.y > _ZeroPoint.y && dist > 0.0) { // chekcs for depth texture, above ground 

			float alto = tex2Dlod(_AltoClouds, float4((pos.xz + _HighCloudsWindOffset) * _HighCloudsScale, 0, 0)).r * 2.0; // samples high altitude cloud texture

			float coverage = tex2Dlod(_WeatherTexture, float4((pos.xz + _HighCloudsWindOffset) * _CoverageHighScale, 0, 0)).r; // same as with volumetric clouds
			coverage = saturate(coverage - _CoverageHigh);

			alto = remap(alto, 1.0 - coverage, 1.0, 0.0, 1.0);
			alto *= coverage;
			float3 directLight = max(HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGForward), HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGBackward)) * _SunColor; // for high altitude clouds uses HG phase
			directLight *= _SunLightFactor * 0.2;
			float3 ambientLight = _CloudTopColor * _AmbientLightFactor * 1.5; // ambient light is the high cloud layer ambient color
			float4 aLparticle = float4(min(ambientLight + directLight, 0.7), alto);

			aLparticle.rgb *= aLparticle.a;

			res = aLparticle;
		}

		return saturate(res);
	}

	fixed4 frag(v2f i) : SV_Target
	{
		// ray origin (camera position)
		float3 ro = _CameraWS;
		// ray direction
		float3 rd = normalize(i.ray.xyz);

		float2 duv = i.uv;
#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			duv.y = 1 - duv.y;
#endif
		float3 rs;
		float3 re;

		float steps;
		float stepSize;
		// Ray start pos
#if defined(ALLOW_IN_CLOUDS) // if in cloud flying is allowed, then figure out if camera is below, above or in the cloud layer and set 
		// starting and end point accordingly.
		bool aboveClouds = false;
		float distanceCameraPlanet = distance(_CameraWS, _PlanetCenter);
		if (distanceCameraPlanet < _SphereSize + _CloudHeightMinMax.x) // Below clouds
		{
			rs = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _CloudHeightMinMax.x);
			if (rs.y < _ZeroPoint.y) // If ray starting position is below horizon
			{
				return 0.0;
			}
			re = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _CloudHeightMinMax.y);
			steps = lerp(_Steps, _Steps * 0.5, rd.y);
			stepSize = (distance(re, rs)) / steps;
		}
		else if (distanceCameraPlanet > _SphereSize + _CloudHeightMinMax.y) // Above clouds
		{
			rs = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _CloudHeightMinMax.y);
			re = rs + rd * _FarPlane;
			steps = lerp(_Steps, _Steps * 0.5, rd.y);
			stepSize = (distance(re, rs)) / steps;
			aboveClouds = true;
		}
		else // In clouds
		{
			rs = ro;
			re = rs + rd * _FarPlane;

			steps = lerp(_Steps, _Steps * 0.5, rd.y);
			stepSize = (distance(re, rs)) / steps;
		}

#else
		rs = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _CloudHeightMinMax.x);
		if (rs.y < _ZeroPoint.y) // If ray starting position is below horizon
		{
			return 0.0;
		}
		re = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _CloudHeightMinMax.y);
		steps = lerp(_Steps, _Steps * 0.5, rd.y);
		stepSize = (distance(re, rs)) / steps;
#endif

		// Ray end pos


#if defined(RANDOM_JITTER_WHITE)
		rs += rd * stepSize * rand(_Time.zw + duv) * BIG_STEP * 0.75;
#endif
#if defined(RANDOM_JITTER_BLUE)
		rs += rd * stepSize * BIG_STEP * 0.75 * getRandomRayOffset((duv + _Randomness.xy) * _ScreenParams.xy * _BlueNoise_TexelSize.xy);
#endif

		// Convert from depth buffer (eye space) to true distance from camera
		// This is done by multiplying the eyespace depth by the length of the "z-normalized"
		// ray (see vert()).  Think of similar triangles: the view-space z-distance between a point
		// and the camera is proportional to the absolute distance.
		float depth = Linear01Depth(tex2D(_CameraDepthTexture, duv).r);
		if (depth == 1.0) {
			depth = 100.0;
		}
		depth *= _FarPlane;
		float cosAngle = dot(rd, _SunDir);
		fixed4 clouds2D = altoClouds(ro, rd, depth, cosAngle); // sample high altitude clouds
		fixed4 clouds3D = raymarch(rs, rd * stepSize, steps, depth, cosAngle); // raymarch volumetric clouds
#if defined(ALLOW_IN_CLOUDS)
		if (aboveClouds) // use premultiplied alpha blending to combine low and high clouds
		{
			return clouds3D * (1.0 - clouds2D.a) + clouds2D;
		}
		else
		{
			return clouds2D * (1.0 - clouds3D.a) + clouds3D;
		}

#else
		return clouds2D * (1.0 - clouds3D.a) + clouds3D;
#endif
	}
		ENDCG
	}
	}
}
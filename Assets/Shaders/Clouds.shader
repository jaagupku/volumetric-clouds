Shader "Hidden/Clouds"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			#pragma multi_compile __ DEBUG_DENSITY
			#pragma multi_compile __ ALLOW_IN_CLOUDS
			
			#include "UnityCG.cginc"

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

			uniform sampler3D _ShapeTexture;
			uniform sampler3D _ErasionTexture;
			uniform sampler2D _WeatherTexture;
			uniform sampler2D _CurlNoise;

			uniform float3 _SunDir;
			uniform float3 _PlanetCenter;
			uniform float3 _SunColor;

			uniform float3 _CloudBaseColor;
			uniform float3 _CloudTopColor;

			uniform float _SphereSize;
			uniform float _StartHeight;
			uniform float _Thickness;
			uniform float _Coverage;
			uniform float _AmbientLightFactor;
			uniform float _SunLightFactor;
			uniform float _HenyeyGreensteinGForward;
			uniform float _HenyeyGreensteinGBackward;
			uniform float _InverseStep;
			uniform float _LightStepLength;

			uniform float _Density;
			
			// Temporary test uniforms
			uniform float _TestFloat;
			uniform float _TestFloat2;
			uniform float4 _TestGradient;

			uniform float _Scale;
			uniform float _ErasionScale;
			uniform float _WeatherScale;
			uniform float _CurlDistortScale;
			uniform float _CurlDistortAmount;

			uniform float _WindSpeed;
			uniform float2 _WindDirection;
			uniform float2 _WindOffset;

			uniform float _HighFreqModifier;

			uniform int _Steps;

			v2f vert (appdata v)
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

			// from GPU Pro 7
			float remap(float original_value, float original_min, float original_max, float new_min, float new_max)
			{
				return new_min + (((original_value - original_min) / (original_max - original_min)) * (new_max - new_min));
			}

			float getHeightFractionForPoint(float3 inPosition, float2 inCloudMinMax)
			{
				//float height_fraction = (inPosition.y - inCloudMinMax.x) / (inCloudMinMax.y - inCloudMinMax.x);
				return saturate((distance(inPosition,  _PlanetCenter) - (_SphereSize + inCloudMinMax.x)) / (inCloudMinMax.y - inCloudMinMax.x));
			}

			float getDensityHeightGradientForPoint(float3 p, float3 weather_data)
			{
				//return densityHeightGradient(getHeightFractionForPoint(p, float2(_StartHeight, _StartHeight + _Thickness)), weather_data.g);
				float height = getHeightFractionForPoint(p, float2(_StartHeight, _StartHeight + _Thickness));
				return (remap(height, _TestGradient.x, _TestGradient.y, 0.0, 1.0) * remap(height, _TestGradient.z, _TestGradient.w, 1.0, 0.0));
			}

			float improvedGradient(float4 gradient, float height)
			{
				return smoothstep(gradient.x, gradient.y, height) - smoothstep(gradient.z, gradient.w, height);
			}

			float sampleCloudDensity(float3 p, float3 weather_data, float lod)
			{
				float height_fraction = getHeightFractionForPoint(p, float2(_StartHeight, _StartHeight + _Thickness));

				float3 pos = p + float3(_WindOffset.x, 0.0, _WindOffset.y);
				float height_wind_speed = _WindSpeed * height_fraction * 10.0;
				//pos += float3(_WindDirection.x * height_wind_speed, 0.0, _WindDirection.y * height_wind_speed);
#if defined(DEBUG_NO_LOW_FREQ_NOISE)
				float base_cloud = 1.0;
#else
				float4 low_frequency_noises = tex3Dlod(_ShapeTexture, float4(pos * _Scale, lod));
				//float low_freq_FBM = low_frequency_noises.g * 0.625 +low_frequency_noises.b * 0.25 + low_frequency_noises.a * 0.125;
				float base_cloud = max(0.0, low_frequency_noises.r) * pow(1.15 - height_fraction, 0.7);//remap(low_frequency_noises.r, -(1.0 - low_freq_FBM), 1.0, 0.0, 1.0);//
				base_cloud = remap(base_cloud, _TestFloat, _TestFloat2, 0.0, 1.0);
				
#endif
				//float density_height_gradient = getDensityHeightGradientForPoint(p, weather_data);
				base_cloud *= improvedGradient(_TestGradient, height_fraction);//density_height_gradient;//
				
				float cloud_coverage = saturate(weather_data.r - _Coverage);

				float base_cloud_with_coverage = saturate(remap(base_cloud, 1.0 - cloud_coverage, 1.0, 0.0, 1.0)); // saturate ?

				float final_cloud = base_cloud_with_coverage * cloud_coverage;

				// TODO p.xy += distort with curl noise
#if defined(DEBUG_NO_HIGH_FREQ_NOISE)
#else
				float3 curl_noise = tex2Dlod(_CurlNoise, float4(p.xy * _Scale * _CurlDistortScale, 0, 0)).rgb * 2.0 - 1.0;

				pos += curl_noise * height_fraction * _CurlDistortAmount;

				float3 high_frequency_noises = tex3Dlod(_ErasionTexture, float4(pos * _Scale * _ErasionScale, lod)).rgb;
				float high_freq_FBM = 1.0 - high_frequency_noises.r;//high_frequency_noises.r * 0.625 + high_frequency_noises.g * 0.25 + high_frequency_noises.b * 0.125;

				float high_freq_noise_modifier = lerp(high_freq_FBM, 1.0 - high_freq_FBM, saturate(height_fraction * 10.0));

				final_cloud = remap(final_cloud, high_freq_noise_modifier * _HighFreqModifier, 1.0, 0.0, 1.0);
#endif
				
				return saturate(final_cloud *_InverseStep);
			}

			// GPU Pro 7
			float beerLaw(float density, float weather)
			{
				return exp(-density * _Density * weather);
			}

			// GPU Pro 7
			float HenyeyGreensteinPhase(float cosAngle, float g)
			{
				float g2 = g * g;
				return (1.0 - g2) / pow(1.0 + g2 - 2.0 * g * cosAngle, 1.5);
			}

			// GPU Pro 7
			float powderEffect(float density)
			{
				return 1.0 - exp(-density * 2.0);
			}

			float calculateLightEnergy(float density, float cosAngle, float g, float powderDensity, float weather) {
				return saturate(2.0 * beerLaw(density, 1.0 + weather) * powderEffect(powderDensity) * 
					(HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGForward) + HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGBackward)) * 0.5);
			}

			float3 sampleConeToLight(float3 pos, float3 lightDir, float3 weather_data, float cosAngle, float density, float lod)
			{
				float densityAlongCone = 0.0; // TODO make it cone
				float steps = 6.0;
				for (int i = 0; i < steps; i++) {
					pos += lightDir * _LightStepLength;
					densityAlongCone += sampleCloudDensity(pos, weather_data, lod);
				}
				densityAlongCone += sampleCloudDensity(pos + 6.0 * _LightStepLength * lightDir, weather_data, lod) * 2.0;
				return calculateLightEnergy(densityAlongCone, -cosAngle, 0.2, density, weather_data.b) * _SunColor;
			}

			fixed4 raymarch(float3 ro, float3 rd, fixed4 col, float steps, float stepSize, float depth)
			{
				float3 pos = ro;
				fixed4 res = fixed4(0.0, 0.0, 0.0, 0.0);
				float cosAngle = dot(rd, -_SunDir);
				float transmittance = 1.0;
				float lod = 0.0;

				for (int i = 0; i < steps; i++)
				{
					if (distance(_CameraWS, pos) >= depth || res.a >= 0.99) {
						break;
					}
#if defined(ALLOW_IN_CLOUDS)
					if (pos.y < 0.0) {
						break;
					}
#endif
					float3 weather_data = tex2Dlod(_WeatherTexture, float4(pos.xz * _WeatherScale + float2(0.5, 0.5), 0, 0)).rgb;
					if (saturate(weather_data.r - _Coverage) < 0.01)
					{
						pos += 2.0 * stepSize * rd;
						i++;
						continue;
					}

					float cloudDensity = sampleCloudDensity(pos, weather_data, lod);

					float4 particle = float4(cloudDensity, cloudDensity, cloudDensity, cloudDensity);
					if (cloudDensity > 0.0) {

						// TEST VARIABLES
						//float testVariable = sampleCloudDensity(pos, weather_data);
						//return fixed4(testVariable, testVariable, testVariable, 1.0);

						float T = 1.0 - particle.a;
						transmittance *= T;
#if defined(DEBUG_DENSITY)
#else
						float3 lightEnergy = sampleConeToLight(pos, _SunDir, weather_data, cosAngle, cloudDensity, lod + 1.0);
						float3 ambientLight = lerp(_CloudBaseColor, _CloudTopColor, getHeightFractionForPoint(pos, float2(_StartHeight, _StartHeight + _Thickness)));

						lightEnergy *= _SunLightFactor;
						ambientLight *= _AmbientLightFactor;

						particle.rgb = lightEnergy + ambientLight;
#endif
						particle.a = 1.0 - T;
						particle.rgb *= particle.a;

						res = (1.0 - res.a) * particle + res;
					}

					pos += stepSize * rd;
				}

				fixed3 color = col.rgb * (1.0 - res.a) + res.rgb;

				return fixed4(color, 1.0);
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
				return rayOrigin;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// ray origin (camera position)
				float3 ro = _CameraWS;
				// ray direction
				float3 rd = normalize(i.ray.xyz);

				//float3 planetCenter = float3(ro.x, ro.y - _SphereSize, ro.z);

				float2 duv = i.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					duv.y = 1 - duv.y;
				#endif
				float3 rs;
				float3 re;
				// Ray start pos

#if defined(ALLOW_IN_CLOUDS)
				if (distance(_CameraWS, _PlanetCenter) < _SphereSize + _StartHeight)
				{
					rs = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _StartHeight);
					if (rs.y < 0.0) // If ray starting position is below horizon
					{
						return col;
					}
				}
				else
				{
					rs = ro;
				}
				re = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _StartHeight + _Thickness);
#else
				rs = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _StartHeight);
				if (rs.y < 0.0) // If ray starting position is below horizon
				{
					return col;
				}
				re = findRayStartPos(ro, rd, _PlanetCenter, _SphereSize + _StartHeight + _Thickness);
#endif
				// TEXTURE TESTING
				//float3 high_frequency_noises = tex3Dlod(_ErasionTexture, float4(rs * 7.0 * _Scale * _ErasionScale, 0)).rgb;
				//float high_freq_FBM = high_frequency_noises.r * 0.625 + high_frequency_noises.g * 0.25 + high_frequency_noises.b * 0.125;
				//fixed4 test = tex3Dlod(_ErasionTexture, float4(rs * _ErasionScale * _Scale, 0));
				//fixed4 test = tex3Dlod(_ShapeTexture, float4(rs * _Scale, 0));
				//fixed4 test = tex2Dlod(_CurlNoise, float4(rs.xz * _Scale * _CurlDistortScale, 0, 0));
				//return test;
				
				//fixed c = test.r;//high_freq_FBM;
				//return fixed4(c, c, c, 1.0);
				
				// Ray end pos

				float steps = _Steps;
				float stepSize = _Thickness / steps;
				steps = steps * (distance(re, rs)) / _Thickness;

				rs += rd * stepSize * rand(_Time.zw + duv);

				// Convert from depth buffer (eye space) to true distance from camera
				// This is done by multiplying the eyespace depth by the length of the "z-normalized"
				// ray (see vert()).  Think of similar triangles: the view-space z-distance between a point
				// and the camera is proportional to the absolute distance.
				float depth = Linear01Depth(tex2D(_CameraDepthTexture, duv).r);
				if (depth > 0.999) {
					depth = 100.0;
				}
				depth *= _FarPlane;
				//if (length(rs - ro) < depth) {
					//return tex2Dlod(_WeatherTexture, float4(rs.xz * _WeatherScale + float2(0.5, 0.5), 0, 0));
				//	return tex3Dlod(_ShapeTexture, float4(rs * _Scale, 0));
				//}

				//fixed a = tex3Dlod(_ShapeTexture, float4(rs * _Scale * 2.0, 0)).r;
				//return fixed4(a, a, a, 1.0);

				return raymarch(rs, rd, col, steps, stepSize, depth);
			}
			ENDCG
		}
	}
}

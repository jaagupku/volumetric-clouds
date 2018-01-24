Shader "Hidden/Clouds"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			
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


			uniform sampler2D _CameraDepthTexture;

			uniform float4x4 _CameraInvViewMatrix;
			uniform float4x4 _FrustumCornersES;
			uniform float4 _CameraWS;

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;

			uniform sampler3D _ShapeTexture;
			uniform sampler3D _ErasionTexture;
			uniform sampler2D _WeatherTexture;

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
			
			uniform float _TestFloat;

			uniform float _Scale;
			uniform float _ErasionScale;
			uniform float _WeatherScale;

			uniform int _Steps;

			// https://www.gamedev.net/forums/topic/680832-horizonzero-dawn-cloud-system/?page=3
			static const float4 STRATUS_GRADIENT = float4(0.011f, 0.098f, 0.126f, 0.218f);
			static const float4 STRATOCUMULUS_GRADIENT = float4(0.0f, 0.096f, 0.311f, 0.506f);
			static const float4 CUMULUS_GRADIENT = float4(0.0f, 0.087f, 0.741f, 1.0f);

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

			// https://www.gamedev.net/forums/topic/680832-horizonzero-dawn-cloud-system/?page=3
			float4 mixGradients(float cloudType)
			{
				float stratus = 1.0f - saturate(cloudType * 2.0f);
				float stratocumulus = 1.0f - abs(cloudType - 0.5f) * 2.0f;
				float cumulus = saturate(cloudType - 0.5f) * 2.0f;
				return STRATUS_GRADIENT * stratus + STRATOCUMULUS_GRADIENT * stratocumulus + CUMULUS_GRADIENT * cumulus;
			}

			// https://www.gamedev.net/forums/topic/680832-horizonzero-dawn-cloud-system/?page=3
			float densityHeightGradient(float heightFrac, float cloudType)
			{
				float4 cloudGradient = mixGradients(cloudType);
				return smoothstep(cloudGradient.x, cloudGradient.y, heightFrac) - smoothstep(cloudGradient.z, cloudGradient.w, heightFrac);
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
				return remap(height, 0.0, 0.1, 0.0, 1.0) * remap(height, 0.15, 0.25, 1.0, 0.0);
			}

			float sampleCloudDensity(float3 p, float3 weather_data)
			{
				float height_fraction = getHeightFractionForPoint(p, float2(_StartHeight, _StartHeight + _Thickness));
				float4 low_frequency_noises = tex3Dlod(_ShapeTexture, float4(p * _Scale, 0));

				//float low_freq_FBM = low_frequency_noises.g * 0.625 + low_frequency_noises.b * 0.25 + low_frequency_noises.a * 0.125;

				float base_cloud = low_frequency_noises.r;//remap(low_frequency_noises.r, -(1.0 - low_freq_FBM), 1.0, 0.0, 1.0);

				float density_height_gradient = getDensityHeightGradientForPoint(p, weather_data);
				base_cloud *= density_height_gradient;

				float cloud_coverage = max(0.0, weather_data.r - _Coverage);
				float base_cloud_with_coverage = saturate(remap(base_cloud, 1.0 - cloud_coverage, 1.0, 0.0, 1.0));

				base_cloud_with_coverage *= cloud_coverage;

				// TODO p.xy += distort with curl noise
				float3 high_frequency_noises = tex3Dlod(_ErasionTexture, float4(p * 7.0 * _Scale * _ErasionScale, 0)).rgb;

				float high_freq_FBM = high_frequency_noises.r;//high_frequency_noises.r * 0.625 + high_frequency_noises.g * 0.25 + high_frequency_noises.b * 0.125;

				float high_freq_noise_modifier = lerp(high_freq_FBM, 1.0 - high_freq_FBM, saturate(height_fraction * 10.0));

				float final_cloud = remap(base_cloud_with_coverage, high_freq_noise_modifier * 0.2, 1.0, 0.0, 1.0);

				return final_cloud;
			}

			float beerLaw(float density)
			{
				return exp(-density);
			}

			float HenyeyGreensteinPhase(float cosAngle, float g)
			{
				float g2 = g * g;
				return (1.0 - g2) / pow(1.0 + g2 - 2.0 * g * cosAngle, 1.5);
			}

			float powderEffect(float density)
			{
				return 1.0 -exp(-density * 2.0);
			}

			float calculateLightEnergy(float density, float cosAngle, float g, float powderDensity) {
				return saturate(2.0 * beerLaw(density) * powderEffect(powderDensity) * (HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGForward) + HenyeyGreensteinPhase(cosAngle, _HenyeyGreensteinGBackward)) * 0.5);
			}

			float3 sampleConeToLight(float3 pos, float3 lightDir, float3 weather_data, float cosAngle, float density)
			{
				float densityAlongCone = 0.0; // TODO make it cone
				float steps = 6.0;
				for (int i = 0; i < steps; i++) {
					pos += lightDir * _LightStepLength;
					densityAlongCone += sampleCloudDensity(pos, weather_data);
				}
				densityAlongCone += sampleCloudDensity(pos + 6.0 * _LightStepLength * lightDir, weather_data) * 2.0;
				return calculateLightEnergy(densityAlongCone, -cosAngle, 0.2, density) * _SunColor;
			}

			fixed4 raymarch(float3 ro, float3 rd, fixed4 col, float depth, float steps, float stepSize)
			{
				float3 pos = ro;
				fixed4 res = fixed4(0.0, 0.0, 0.0, 0.0);
				float cosAngle = dot(rd, -_SunDir);
				float transmittance = 1.0;

				for (int i = 0; i < steps; i++)
				{
					if (distance(pos, ro) > depth || res.a >= 0.99) {
						break;
					}
					
					float3 weather_data = tex2Dlod(_WeatherTexture, float4(pos.xz * _WeatherScale + float2(0.5, 0.5), 0, 0)).rgb;

					float cloudDensity = sampleCloudDensity(pos, weather_data) * _InverseStep;

					float4 particle = float4(cloudDensity, cloudDensity, cloudDensity, cloudDensity);
					if (cloudDensity > 0.0) {

						// TEST VARIABLES
						//float testVariable = sampleCloudDensity(pos, weather_data);
						//return fixed4(testVariable, testVariable, testVariable, 1.0);

						float T = 1.0 - particle.a;
						transmittance *= T;
						float3 lightEnergy = sampleConeToLight(pos, _SunDir, weather_data, cosAngle, cloudDensity);
						float3 ambientLight = lerp(_CloudBaseColor, _CloudTopColor, getHeightFractionForPoint(pos, float2(_StartHeight, _StartHeight + _Thickness)));

						lightEnergy *= _SunLightFactor;
						ambientLight *= _AmbientLightFactor;

						particle.a = 1.0 - T;
						particle.rgb = lightEnergy + ambientLight;
						particle.rgb *= particle.a;

						res = (1.0 - res.a) * particle + res;
					}

					pos += stepSize * rd;
				}

				fixed3 color = lerp(col.rgb, res.rgb, res.a);

				return fixed4(color, 1.0);
			}

			// https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection
			float3 findRayStartPos(float3 rayOrigin, float3 rayDirection, float3 sphereCenter, float radius)
			{
				/*
				float3 result = rayOrigin + rayDirection * (_SphereSize + _StartHeight);
				result.y = result.y - _SphereSize;
				return result;
				*/
				/*
				float3 O = rayOrigin;
				float3 r = rayDirection;
				float3 C = float3(O.x, O.y - _SphereSize, O.z);
				float3 a = float3(O.x - C.x, O.y - C.y, O.z - C.z);
				float p = pow(r.x, 2) + pow(r.y, 2) + pow(r.z, 2);
				float b = r.x * a.x + r.y * a.y + r.z * a.z;
				float c = pow(_SphereSize + _StartHeight, 2) + pow(a.x, 2) + pow(a.y, 2) + pow(a.z, 2);

				float D = sqrt(4 * (pow(b, 2) - p * c));
				float x = (-2 * b + D) / (2 * p);
				*/

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
				return rayOrigin;// +rayDirection * x;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// ray origin (camera position)
				float3 ro = _CameraWS;
				// ray direction
				float3 rd = normalize(i.ray.xyz);

				float3 planetCenter = float3(ro.x, ro.y - _SphereSize, ro.z);

				float2 duv = i.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					duv.y = 1 - duv.y;
				#endif

				// Ray start pos
				float3 rs = findRayStartPos(ro, rd, planetCenter, _SphereSize + _StartHeight);
				


				// TEXTURE TESTING
				//float3 high_frequency_noises = tex3Dlod(_ErasionTexture, float4(rs * 7.0 * _Scale * _ErasionScale, 0)).rgb;
				//float high_freq_FBM = high_frequency_noises.r * 0.625 + high_frequency_noises.g * 0.25 + high_frequency_noises.b * 0.125;
				//fixed4 test = tex3D(_ErasionTexture, rs * 7.0 *_Scale * _TestFloat);
				
				fixed4 test = tex3Dlod(_ShapeTexture, float4(rs * _Scale, 0));
				return test;
				
				//fixed c = test.r;//high_freq_FBM;
				//return fixed4(c, c, c, 1.0);



				if (rs.y < 0.0) // If ray starting position is below horizon
				{
					return col;
				}
				// Ray end pos
				float3 re = findRayStartPos(ro, rd, planetCenter, _SphereSize + _StartHeight + _Thickness);

				float steps = _Steps;
				float stepSize = _Thickness / steps;
				steps = steps * (distance(re, rs)) / _Thickness;

				rs += rd * stepSize * rand(_Time.zw + duv);

				// Convert from depth buffer (eye space) to true distance from camera
				// This is done by multiplying the eyespace depth by the length of the "z-normalized"
				// ray (see vert()).  Think of similar triangles: the view-space z-distance between a point
				// and the camera is proportional to the absolute distance.
				float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, duv).r);
				depth *= length(i.ray);

				//if (length(rs - ro) < depth) {
					//return tex2Dlod(_WeatherTexture, float4(rs.xz * _WeatherScale + float2(0.5, 0.5), 0, 0));
				//	return tex3Dlod(_ShapeTexture, float4(rs * _Scale, 0));
				//}

				//fixed a = tex3Dlod(_ShapeTexture, float4(rs * _Scale * 2.0, 0)).r;
				//return fixed4(a, a, a, 1.0);

				return raymarch(rs, rd, col, depth, steps, stepSize);
			}
			ENDCG
		}
	}
}

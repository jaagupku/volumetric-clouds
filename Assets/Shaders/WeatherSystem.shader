Shader "Hidden/WeatherSystem"
{
	Properties
	{
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "noiseSimplex.cginc"
			#include "noiseWorley.cginc"

			uniform float3 _Randomness;

			struct v2f
			{
				float3 srcPos : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
			v2f vert (float4 objPos : POSITION)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(objPos);

				o.srcPos = mul(unity_ObjectToWorld, objPos).xyz;
				o.srcPos *= 1.0;
				o.srcPos.y += _Randomness;

				return o;
			}

			float invertedWorley(float2 pos)
			{
				return 1.0 - worley(pos, _Randomness.z + 1.0);
			}

			float invertedWorley(float2 pos, float alpha)
			{
				return 1.0 - worley(pos, alpha);
			}

			float worley(float2 pos)
			{
				return worley(pos, _Randomness.z + 1.0);
			}

			// from GPU Pro 7
			float remap(float original_value, float original_min, float original_max, float new_min, float new_max)
			{
				return new_min + (((original_value - original_min) / (original_max - original_min)) * (new_max - new_min));
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 pos = i.srcPos.xy;
				float simplexNoise = 0.0;
				simplexNoise += 1.0 * snoise(pos * 2.0);
				simplexNoise += 0.2 * snoise(pos * 9.0);
				simplexNoise += 0.09 * snoise(pos * 18.0);
				simplexNoise += 0.05 * snoise(pos * 24.0);
				
				simplexNoise = mad(simplexNoise, 0.5, 0.5);

				float cell = 0.0;

				cell += 1.0 * invertedWorley(pos * 4.0);
				cell += 0.4 * invertedWorley(pos * 9.0);
				cell += 0.1 * invertedWorley(pos * 19.0);

				float coverage = remap(saturate(simplexNoise / 1.34), saturate(1.0 - cell / 1.5), 1.0, 0.0, 1.0);
				
				coverage = saturate(mad(coverage, 0.55, 0.65));

				float density = 0.0;
				density = invertedWorley(pos, _Randomness.z + 3.0);
				density *= coverage;

				fixed4 col = fixed4(coverage, 0.0, density, 1.0);
				return col;
			}
			ENDCG
		}
	}
}

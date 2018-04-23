Shader "Unlit/TestSomeFunctions"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Gradient ("Gradient", Vector) = (0.0, 0.1, 0.2, 0.3)
		[Toggle] _BetterGradient ("BetterGradient", Float) = 0.0
	}
	Fallback "VertexLit"
	SubShader
	{
		Tags { "Queue" = "Geometry" "RenderType"="Opaque" }
		ZWrite On ZTest On

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "noiseSimplex.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float4 _Gradient;
			uniform float _BetterGradient;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float remap(float original_value, float original_min, float original_max, float new_min, float new_max)
			{
				return new_min + (((original_value - original_min) / (original_max - original_min)) * (new_max - new_min));
			}

			float remapBased(float4 gradient, float height)
			{
				return remap(height, gradient.x, gradient.y, 0.0, 1.0) * remap(height, gradient.z, gradient.w, 1.0, 0.0);
			}

			float improvedGradient(float4 gradient, float height)
			{
				return smoothstep(gradient.x, gradient.y, height) - smoothstep(gradient.z, gradient.w, height);
			}

			float noise(float x, float y) {
				return snoise(float2(x, y));
			}

			float2 ComputeCurl(float x, float y)
			{
				float eps = 0.01;
				float n1, n2, a, b;

				n1 = noise(x, y + eps);
				n2 = noise(x, y - eps);
				a = (n1 - n2) / (2.0 * eps);
				n1 = noise(x + eps, y);
				n2 = noise(x - eps, y);
				b = (n1 - n2) / (2.0 * eps);

				float2 curl = float2(a, -b);
				return curl;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//float x = i.uv.x;
				//float y = i.uv.y;
				//float scale = 5.0;
				//x *= scale;
				//y *= scale;
				//float2 curl = ComputeCurl(x, y);
				//curl = (curl + 10.0) * 0.05;
				//float r = curl.y;
				//float g = curl.y;
				//return fixed4(r, r, 0.0, 1.0);

				float a = 0.0;
				a += improvedGradient(_Gradient, i.uv.y) * _BetterGradient;
				a += remapBased(_Gradient, i.uv.y) * (1.0 - _BetterGradient);
				fixed4 col = fixed4(a, a, a, 1.0);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}

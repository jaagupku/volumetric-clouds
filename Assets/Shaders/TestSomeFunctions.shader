Shader "Unlit/TestSomeFunctions"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float a = 0.0;
				a += improvedGradient(float4(0.0, 0.1, 0.2, 0.7), i.uv.y) * _BetterGradient;
				a += remapBased(float4(0.0, 0.1, 0.2, 0.3), i.uv.y) * (1.0 - _BetterGradient);
				return fixed4(a, a, a, 1.0);
			}
			ENDCG
		}
	}
}

Shader "Hidden/CloudBlender"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Off
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
			};

			sampler2D _MainTex;
			sampler2D _Clouds;
			float4 _MainTex_TexelSize;
			
			v2f vert(appdata v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;

#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
#endif

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 back = tex2D(_MainTex, i.uv); // sample background
				fixed4 cloud = tex2D(_Clouds, i.uv); // sample cloud rendertexture
				return fixed4(back.rgb * (1.0 - cloud.a) + cloud.rgb, 1.0); // blend them
			}
			ENDCG
		}
	}
}

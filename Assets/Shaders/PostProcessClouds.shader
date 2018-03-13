Shader "Unlit/PostProcessClouds"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _Clouds;
			float4 _MainTex_TexelSize;
			
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

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 back = tex2D(_MainTex, i.uv);
				return back;
				fixed4 cloud = tex2D(_Clouds, i.uv);
				return fixed4(back.xyz + cloud.xyz, 1.0);
			}
			ENDCG
		}
	}
}

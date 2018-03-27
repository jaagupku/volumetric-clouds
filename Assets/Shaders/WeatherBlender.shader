Shader "Hidden/WeatherBlender"
{
	Properties
	{
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

			sampler2D _PrevWeather;
			sampler2D _NextWeather;
			float _Alpha;
			
			v2f vert(appdata v)
			{
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 prev = tex2D(_PrevWeather, i.uv);
				fixed4 next = tex2D(_NextWeather, i.uv);
				return lerp(prev, next, _Alpha);
			}
			ENDCG
		}
	}
}

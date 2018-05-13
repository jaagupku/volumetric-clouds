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

	v2f vert(float4 objPos : POSITION)
	{
		v2f o;

		o.pos = UnityObjectToClipPos(objPos);

		o.srcPos = mul(unity_ObjectToWorld, objPos).xyz;
		o.srcPos *= 1.0;
		o.srcPos.xy += _Randomness.xy;

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

	fixed4 frag(v2f i) : SV_Target
	{
		float2 pos = i.srcPos.xy;
		float simplexNoise = 0.0; // sample multiple simplex noises and create fBm
		simplexNoise += 1.0 * snoise(pos * 2.0);
		simplexNoise += 0.2 * snoise(pos * 9.0);
		simplexNoise += 0.09 * snoise(pos * 18.0);
		simplexNoise += 0.05 * snoise(pos * 24.0);

		simplexNoise = mad(simplexNoise, 0.5, 0.5);

		float cell = 0.0; // create worley noise fBm

		cell += 1.0 * invertedWorley(pos * 4.0);
		cell += 0.4 * invertedWorley(pos * 9.0);
		cell += 0.1 * invertedWorley(pos * 19.0);

		float coverage = remap(saturate(simplexNoise / 1.34), saturate(1.0 - cell / 1.5), 1.0, 0.0, 1.0); // modulate simplex noise by worley noise for coverage
		coverage = saturate(mad(coverage, 0.55, 0.65)); // transfer most of it to range [0, 1]

		float density = 0.0; // for rain clouds use one low frew worley noise sample
		density += invertedWorley(pos, _Randomness.z + 3.0);
		density *= coverage;

		// use worley noise at different offsets to calculate different cloud types
		float typeHigh = invertedWorley((pos + float2(-142.214, 8434.345)) * 2, _Randomness.z + 2.5);
		typeHigh += invertedWorley((pos + float2(-142.214, 8434.345)) * 1, _Randomness.z + 2.5);
		typeHigh = remap(saturate(simplexNoise / 1.34), saturate(1.0 - min(typeHigh, 1.0)), 1.0, 0.0, 1.0);
		typeHigh = smoothstep(0.1, 0.6, typeHigh) * 0.5;
		//float typeHigh = 0.0f;

		float typeMed = invertedWorley((pos + float2(1236.1234, -74.4356)) * 0.3, _Randomness.z);
		typeMed = remap(saturate(simplexNoise / 1.34), saturate(1.0 - typeMed), 1.0, 0.0, 1.0);

		float typeMed2 = invertedWorley((pos + float2(412.1234, -22.4356)) * 0.3, _Randomness.z);
		typeMed2 = remap(saturate(simplexNoise / 1.34), saturate(1.0 - typeMed), 1.0, 0.0, 1.0);
		typeMed = (smoothstep(0.1, 0.6, typeMed) + smoothstep(0.1, 0.6, typeMed2)) * 0.5;
		//float typeMed = 0.0;
		float type = saturate(typeMed + typeHigh);

		//return fixed4(0, type, 0, 1.0);
		//pos.xy -= _Randomness.xy;
		//float a = worley(pos * 8.0, 1);
		//return fixed4(a, a, a, 1.0);

		fixed4 col = fixed4(coverage, type, density, 1.0);
		return col;
	}
		ENDCG
	}
	}
}

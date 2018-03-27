public static class NoiseStrings2D
{
    public const string NormalizeOn =                   "\t\t\th = h * 0.5 + 0.5;";

    public const string TagsTransparent =               "\t\tTags {\"Queue\"=\"Transparent\"}";

    public const string BlendingAlpha =                 "\t\tBlend SrcAlpha OneMinusSrcAlpha";

    public const string InputTexturing =                "\t\t\tfloat2 uv_LowTexture;\n" +
			                                            "\t\t\tfloat2 uv_HighTexture;";
    public const string InputNormal =                   "\t\t\tfloat2 pos;\n";

    public const string LightingLambert =               "\t\t#pragma surface surf Lambert vertex:vert";
    public const string LightingBlinnPhong =            "\t\t#pragma surface surf BlinnPhong vertex:vert";

    public const string VertexLocalDispOff =            "\t\t\tOUT.pos = v.texcoord;";
    public const string VertexWorldDispOff =            "\t\t\tOUT.pos = mul (_Object2World, v.vertex).xyz;";
    public const string VertexLocalDispOn =             "\t\t\tOUT.pos = v.texcoord;\n" +
                                                        "${VertexNoise}\n" +
                                                        "\t\t\tv.vertex.xyz += v.normal * h * _Displacement;";
    public const string VertexWorldDisp =               "";

    public const string ColoringOnTexturingOff =        "\t\t\tfloat4 color = float4(0.0, 0.0, 0.0, 0.0);\n" +
                                                        "\t\t\tcolor = lerp(_LowColor, _HighColor, h);";
    public const string ColoringOnTexturingOn =         "\t\t\tfloat4 color = float4(0.0, 0.0, 0.0, 0.0);\n" +
                                                        "\t\t\tfloat4 lowTexColor = tex2D(_LowTexture, IN.uv_LowTexture);\n" +
                                                        "\t\t\tfloat4 highTexColor = tex2D(_HighTexture, IN.uv_HighTexture);\n" +
                                                        "\t\t\tcolor = lerp(_LowColor * lowTexColor, _HighColor * highTexColor, h);";
    public const string ColoringOffTexturingOn =        "\t\t\tfloat4 color = float4(0.0, 0.0, 0.0, 0.0);\n" +
                                                        "\t\t\tfloat4 lowTexColor = tex2D(_LowTexture, IN.uv_LowTexture);\n" +
                                                        "\t\t\tfloat4 highTexColor = tex2D(_HighTexture, IN.uv_HighTexture);\n" +
                                                        "\t\t\tcolor = lerp(lowTexColor, highTexColor, h);";
    public const string ColoringOffTexturingOff =       "\t\t\tfloat4 color = float4(h, h, h, h);";

    public const string AlphaOn =                       "\t\t\to.Alpha = h * _Transparency;";
    public const string AlphaOff =                      "\t\t\to.Alpha = 1.0;";

    public const string PropertiesNormal =              "\t\t_Octaves (\"Octaves\", Float) = 8.0\n" +
		                                                "\t\t_Frequency (\"Frequency\", Float) = 1.0\n" +
		                                                "\t\t_Amplitude (\"Amplitude\", Float) = 1.0\n" +
		                                                "\t\t_Lacunarity (\"Lacunarity\", Float) = 1.92\n" +
		                                                "\t\t_Persistence (\"Persistence\", Float) = 0.8\n" +
		                                                "\t\t_Offset (\"Offset\", Vector) = (0.0, 0.0, 0.0, 0.0)\n";
    public const string PropertiesRidged =              "\t\t_RidgeOffset (\"Ridge Offset\", Float) = 1.0\n";
    public const string PropertiesDerivedSwiss =        "\t\t_RidgeOffset (\"Ridge Offset\", Float) = 1.0\n" +
                                                        "\t\t_Warp (\"Warp\", Float) = 0.25\n";
    public const string PropertiesDerivedJordan =       "\t\t_Warp0 (\"Warp0\", Float) = 0.15\n" +
		                                                "\t\t_Warp (\"Warp\", Float) = 0.25\n" +
		                                                "\t\t_Damp0 (\"Damp0\", Float) = 0.8\n" +
		                                                "\t\t_Damp (\"Damp\", Float) = 1.0\n" +
		                                                "\t\t_DampScale (\"Damp Scale\", Float) = 1.0\n";
    public const string PropertiesCell =                "\t\t_CellType (\"Cell Type\", Float) = 1.0\n" +
		                                                "\t\t_DistanceFunction (\"Distance Function\", Float) = 1.0\n";
    public const string PropertiesTransparency =        "\t\t_Transparency (\"Transparency\", Range(0.0, 1.0)) = 1.0\n";
    public const string PropertiesColoring =            "\t\t_LowColor(\"Low Color\", Vector) = (0.0, 0.0, 0.0, 1.0)\n" +
		                                                "\t\t_HighColor(\"High Color\", Vector) = (1.0, 1.0, 1.0, 1.0)\n";
    public const string PropertiesTexturing =           "\t\t_LowTexture(\"Low Texture\", 2D) = \"\" {}\n" + 
		                                                "\t\t_HighTexture(\"High Texture\", 2D) = \"\" {}\n";
    public const string PropertiesDisplacement =        "\t\t_Displacement(\"Displacement\", Float) = 1.0";

    public const string UniformsNormal =                "\t\tfixed _Octaves;\n" +
                                                        "\t\tfloat _Frequency;\n" +
                                                        "\t\tfloat _Amplitude;\n" +
                                                        "\t\tfloat2 _Offset;\n" +
                                                        "\t\tfloat _Lacunarity;\n" +
                                                        "\t\tfloat _Persistence;\n";
    public const string UniformsRidged =                "\t\tfloat _RidgeOffset;\n";
    public const string UniformsDerivedSwiss =          "\t\tfloat _RidgeOffset;\n" +
                                                        "\t\tfloat _Warp;\n";
    public const string UniformsDerivedJordan =         "\t\tfloat _Warp0;\n" +
                                                        "\t\tfloat _Warp;\n" +
                                                        "\t\tfloat _Damp0;\n" +
                                                        "\t\tfloat _Damp;\n" +
                                                        "\t\tfloat _DampScale;\n";
    public const string UniformsCell =                  "\t\tfixed _CellType;\n" +
		                                                "\t\tfixed _DistanceFunction;\n";
    public const string UniformsTransparency =          "\t\tfixed _Transparency;\n";
    public const string UniformsColoring =              "\t\tfixed4 _LowColor;\n" +
                                                        "\t\tfixed4 _HighColor;\n";
    public const string UniformsTexturing =             "\t\tsampler2D _LowTexture;\n" +
                                                        "\t\tsampler2D _HighTexture;\n";
    public const string UniformsDisplacement =          "\t\tfloat _Displacement;";

    public const string NoisePerlinNormal =             "\t\t\tfloat h = PerlinNormal(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoisePerlinBillowed =           "\t\t\tfloat h = PerlinBillowed(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoisePerlinRidged =             "\t\t\tfloat h = PerlinRidged(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _RidgeOffset);";
    public const string NoisePerlinDerivedIQ =          "\t\t\tfloat h = PerlinDerivedIQ(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoisePerlinDerivedSwiss =       "\t\t\tfloat h = PerlinDerivedSwiss(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp, _RidgeOffset);";
    public const string NoisePerlinDerivedJordan =      "\t\t\tfloat h = PerlinDerivedJordan(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp0, _Warp, _Damp0, _Damp, _DampScale);";
    public const string NoiseSimplexNormal =            "\t\t\tfloat h = SimplexNormal(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseSimplexBillowed =          "\t\t\tfloat h = SimplexBillowed(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseSimplexRidged =            "\t\t\tfloat h = SimplexRidged(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _RidgeOffset);";
    public const string NoiseSimplexDerivedIQ =         "\t\t\tfloat h = SimplexDerivedIQ(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseSimplexDerivedSwiss =      "\t\t\tfloat h = SimplexDerivedSwiss(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp, _RidgeOffset);";
    public const string NoiseSimplexDerivedJordan =     "\t\t\tfloat h = SimplexDerivedJordan(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp0, _Warp, _Damp0, _Damp, _DampScale);";
    public const string NoiseValueNormal =              "\t\t\tfloat h = ValueNormal(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseValueBillowed =            "\t\t\tfloat h = ValueBillowed(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseValueRidged =              "\t\t\tfloat h = ValueRidged(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _RidgeOffset);";
    public const string NoiseValueDerivedIQ =           "\t\t\tfloat h = ValueDerivedIQ(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseValueDerivedSwiss =        "\t\t\tfloat h = ValueDerivedSwiss(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp, _RidgeOffset);";
    public const string NoiseValueDerivedJordan =       "\t\t\tfloat h = ValueDerivedJordan(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp0, _Warp, _Damp0, _Damp, _DampScale);";
    public const string NoiseCellNormal =               "\t\t\tfloat h = CellNormal(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _CellType, _DistanceFunction);";
    public const string NoiseCellFast =                 "\t\t\tfloat h = CellFast(IN.pos.xy, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";

	public const string IncludeValue =  			    "\t\t//\n" +
													    "\t\t//	FAST32_hash\n" +
													    "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
													    "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
													    "\t\t//\n" +
													    "\t\t//	The hash formula takes the form....\n" +
													    "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
													    "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
													    "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
													    "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
													    "\t\t//\n" +
													    "\t\tfloat4 FAST32_hash_2D( float2 gridcell )	//	generates a random number for each of the 4 cell corners\n" +
													    "\t\t{\n" +
													    "\t\t	//	gridcell is assumed to be an integer coordinate\n" +
													    "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
													    "\t\t	const float DOMAIN = 71.0;\n" +
													    "\t\t	const float SOMELARGEFLOAT = 951.135664;\n" +
													    "\t\t	float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );\n" +
													    "\t\t	P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;	//	truncate the domain\n" +
													    "\t\t	P += OFFSET.xyxy;								//	offset to interesting part of the noise\n" +
													    "\t\t	P *= P;											//	calculate and return the hash\n" +
													    "\t\t	return frac( P.xzxz * P.yyww * ( 1.0 / SOMELARGEFLOAT.x ) );\n" +
													    "\t\t}\n" +
													    "\t\t//\n" +
													    "\t\t//	Interpolation functions\n" +
													    "\t\t//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 )\n" +
													    "\t\t//	http://briansharpe.wordpress.com/2011/11/14/two-useful-interpolation-functions-for-noise-development/\n" +
													    "\t\t//\n" +
													    "\t\tfloat2 Interpolation_C2( float2 x ) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }\n" +
													    "\t\t//\n" +
													    "\t\t//	Value Noise 2D\n" +
													    "\t\t//	Return value range of 0.0->1.0\n" +
													    "\t\t//	http://briansharpe.files.wordpress.com/2011/11/valuesample1.jpg\n" +
													    "\t\t//\n" +
													    "\t\tfloat Value2D( float2 P )\n" +
													    "\t\t{\n" +
													    "\t\t	//	establish our grid cell and unit position\n" +
													    "\t\t	float2 Pi = floor(P);\n" +
													    "\t\t	float2 Pf = P - Pi;\n" +
													    "\t\t\n" +
													    "\t\t	//	calculate the hash.\n" +
													    "\t\t	float4 hash = FAST32_hash_2D( Pi );\n" +
													    "\t\t\n" +
													    "\t\t	//	blend the results and return\n" +
													    "\t\t	float2 blend = Interpolation_C2( Pf );\n" +
													    "\t\t	float2 res0 = lerp( hash.xy, hash.zw, blend.y );\n" +
													    "\t\t	return lerp( res0.x, res0.y, blend.x );\n" +
													    "\t\t}\n";
	public const string IncludeValueDerived =		    "\t\t//\n" +
													    "\t\t//	FAST32_hash\n" +
													    "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
													    "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
													    "\t\t//\n" +
													    "\t\t//	The hash formula takes the form....\n" +
													    "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
													    "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
													    "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
													    "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
													    "\t\t//\n" +
													    "\t\tfloat4 FAST32_hash_2D( float2 gridcell )	//	generates a random number for each of the 4 cell corners\n" +
													    "\t\t{\n" +
													    "\t\t	//	gridcell is assumed to be an integer coordinate\n" +
													    "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
													    "\t\t	const float DOMAIN = 71.0;\n" +
													    "\t\t	const float SOMELARGEFLOAT = 951.135664;\n" +
													    "\t\t	float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );\n" +
													    "\t\t	P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;	//	truncate the domain\n" +
													    "\t\t	P += OFFSET.xyxy;								//	offset to interesting part of the noise\n" +
													    "\t\t	P *= P;											//	calculate and return the hash\n" +
													    "\t\t	return frac( P.xzxz * P.yyww * ( 1.0 / SOMELARGEFLOAT.x ) );\n" +
													    "\t\t}\n" +
													    "\t\t//\n" +
													    "\t\t//	Interpolation functions\n" +
													    "\t\t//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 )\n" +
													    "\t\t//	http://briansharpe.wordpress.com/2011/11/14/two-useful-interpolation-functions-for-noise-development/\n" +
													    "\t\t//\n" +
													    "\t\tfloat4 Interpolation_C2_InterpAndDeriv( float2 x ) { return x.xyxy * x.xyxy * ( x.xyxy * ( x.xyxy * ( x.xyxy * float4( 6.0, 6.0, 0.0, 0.0 ) + float4( -15.0, -15.0, 30.0, 30.0 ) ) + float4( 10.0, 10.0, -60.0, -60.0 ) ) + float4( 0.0, 0.0, 30.0, 30.0 ) ); }\n" +
													    "\t\t//\n" +
													    "\t\t//	Value2D_Deriv\n" +
													    "\t\t//	Value2D noise with derivatives\n" +
													    "\t\t//	returns float3( value, xderiv, yderiv )\n" +
													    "\t\t//\n" +
													    "\t\tfloat3 Value2D_Deriv( float2 P )\n" +
													    "\t\t{\n" +
													    "\t\t	//	establish our grid cell and unit position\n" +
													    "\t\t	float2 Pi = floor(P);\n" +
													    "\t\t	float2 Pf = P - Pi;\n" +
													    "\t\t\n" +
													    "\t\t	//	calculate the hash.\n" +
													    "\t\t	float4 hash = FAST32_hash_2D( Pi );\n" +
													    "\t\t\n" +
													    "\t\t	//	blend the results and return\n" +
													    "\t\t	float2 blend = Interpolation_C2_InterpAndDeriv( Pf );\n" +
													    "\t\t	float4 res0 = lerp( hash.xyxz, hash.zwyw, blend.yyxx );\n" +
													    "\t\t	return float3( res0.x, 0.0, 0.0 ) + ( res0.yyw - res0.xxz ) * blend.xzw;\n" +
													    "\t\t}\n";
    public const string IncludePerlin =                 "\t\t//\n" +
                                                        "\t\t//	FAST32_hash\n" +
                                                        "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	The hash formula takes the form....\n" +
                                                        "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
                                                        "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
                                                        "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
                                                        "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
                                                        "\t\t//\n" +
                                                        "\t\tvoid FAST32_hash_2D( float2 gridcell, out float4 hash_0, out float4 hash_1 )	//	generates 2 random numbers for each of the 4 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 71.0;\n" +
                                                        "\t\t	const float2 SOMELARGEFLOATS = float2( 951.135664, 642.949883 );\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );\n" +
                                                        "\t\t	P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	P += OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	hash_0 = frac( P * ( 1.0 / SOMELARGEFLOATS.x ) );\n" +
                                                        "\t\t	hash_1 = frac( P * ( 1.0 / SOMELARGEFLOATS.y ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Interpolation functions\n" +
                                                        "\t\t//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 )\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/14/two-useful-interpolation-functions-for-noise-development/\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat2 Interpolation_C2( float2 x ) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Perlin Noise 2D  ( gradient noise )\n" +
                                                        "\t\t//	Return value range of -1.0->1.0\n" +
                                                        "\t\t//	http://briansharpe.files.wordpress.com/2011/11/perlinsample.jpg\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat Perlin2D( float2 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float2 Pi = floor(P);\n" +
                                                        "\t\t	float4 Pf_Pfmin1 = P.xyxy - float4( Pi, Pi + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	float4 hash_x, hash_y;\n" +
                                                        "\t\t	FAST32_hash_2D( Pi, hash_x, hash_y );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the gradient results\n" +
                                                        "\t\t	float4 grad_x = hash_x - 0.49999;\n" +
                                                        "\t\t	float4 grad_y = hash_y - 0.49999;\n" +
                                                        "\t\t	float4 grad_results = rsqrt( grad_x * grad_x + grad_y * grad_y ) * ( grad_x * Pf_Pfmin1.xzxz + grad_y * Pf_Pfmin1.yyww );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	#if 1\n" +
                                                        "\t\t	//	Classic Perlin Interpolation\n" +
                                                        "\t\t	grad_results *= 1.4142135623730950488016887242097;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/sqrt(0.5)\n" +
                                                        "\t\t	float2 blend = Interpolation_C2( Pf_Pfmin1.xy );\n" +
                                                        "\t\t	float2 res0 = lerp( grad_results.xy, grad_results.zw, blend.y );\n" +
                                                        "\t\t	return lerp( res0.x, res0.y, blend.x );\n" +
                                                        "\t\t	#else\n" +
                                                        "\t\t	//	Classic Perlin Surflet\n" +
                                                        "\t\t	//	http://briansharpe.wordpress.com/2012/03/09/modifications-to-classic-perlin-noise/\n" +
                                                        "\t\t	grad_results *= 2.3703703703703703703703703703704;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/cube(0.75)\n" +
                                                        "\t\t	float4 vecs_len_sq = Pf_Pfmin1 * Pf_Pfmin1;\n" +
                                                        "\t\t	vecs_len_sq = vecs_len_sq.xzxz + vecs_len_sq.yyww;\n" +
                                                        "\t\t	return dot( Falloff_Xsq_C2( min( float4( 1.0, 1.0, 1.0, 1.0 ), vecs_len_sq ) ), grad_results );\n" +
                                                        "\t\t	#endif\n" +
                                                        "\t\t}\n";
    public const string IncludePerlinDerived =          "\t\t//\n" +
                                                        "\t\t//	FAST32_hash\n" +
                                                        "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	The hash formula takes the form....\n" +
                                                        "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
                                                        "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
                                                        "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
                                                        "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
                                                        "\t\t//\n" +
                                                        "\t\tvoid FAST32_hash_2D( float2 gridcell, out float4 hash_0, out float4 hash_1 )	//	generates 2 random numbers for each of the 4 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 71.0;\n" +
                                                        "\t\t	const float2 SOMELARGEFLOATS = float2( 951.135664, 642.949883 );\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );\n" +
                                                        "\t\t	P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	P += OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	hash_0 = frac( P * ( 1.0 / SOMELARGEFLOATS.x ) );\n" +
                                                        "\t\t	hash_1 = frac( P * ( 1.0 / SOMELARGEFLOATS.y ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	PerlinSurflet2D_Deriv\n" +
                                                        "\t\t//	Perlin Surflet 2D noise with derivatives\n" +
                                                        "\t\t//	returns float3( value, xderiv, yderiv )\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat3 PerlinSurflet2D_Deriv( float2 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float2 Pi = floor(P);\n" +
                                                        "\t\t	float4 Pf_Pfmin1 = P.xyxy - float4( Pi, Pi + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	float4 hash_x, hash_y;\n" +
                                                        "\t\t	FAST32_hash_2D( Pi, hash_x, hash_y );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the gradient results\n" +
                                                        "\t\t	float4 grad_x = hash_x - 0.49999;\n" +
                                                        "\t\t	float4 grad_y = hash_y - 0.49999;\n" +
                                                        "\t\t	float4 norm = rsqrt( grad_x * grad_x + grad_y * grad_y );\n" +
                                                        "\t\t	grad_x *= norm;\n" +
                                                        "\t\t	grad_y *= norm;\n" +
                                                        "\t\t	float4 grad_results = grad_x * Pf_Pfmin1.xzxz + grad_y * Pf_Pfmin1.yyww;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	eval the surflet\n" +
                                                        "\t\t	float4 m = Pf_Pfmin1 * Pf_Pfmin1;\n" +
                                                        "\t\t	m = m.xzxz + m.yyww;\n" +
                                                        "\t\t	m = max(1.0 - m, 0.0);\n" +
                                                        "\t\t	float4 m2 = m*m;\n" +
                                                        "\t\t	float4 m3 = m*m2;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calc the deriv\n" +
                                                        "\t\t	float4 temp = -6.0 * m2 * grad_results;\n" +
                                                        "\t\t	float xderiv = dot( temp, Pf_Pfmin1.xzxz ) + dot( m3, grad_x );\n" +
                                                        "\t\t	float yderiv = dot( temp, Pf_Pfmin1.yyww ) + dot( m3, grad_y );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	sum the surflets and return all results combined in a float3\n" +
                                                        "\t\t	const float FINAL_NORMALIZATION = 2.3703703703703703703703703703704;		//	scales the final result to a strict 1.0->-1.0 range\n" +
                                                        "\t\t	return float3( dot( m3, grad_results ), xderiv, yderiv ) * FINAL_NORMALIZATION;\n" +
                                                        "\t\t}\n";
    public const string IncludeSimplex =                "\t\t//\n" +
                                                        "\t\t//	FAST32_hash\n" +
                                                        "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	The hash formula takes the form....\n" +
                                                        "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
                                                        "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
                                                        "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
                                                        "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
                                                        "\t\t//\n" +
                                                        "\t\tvoid FAST32_hash_2D( float2 gridcell, out float4 hash_0, out float4 hash_1 )	//	generates 2 random numbers for each of the 4 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 71.0;\n" +
                                                        "\t\t	const float2 SOMELARGEFLOATS = float2( 951.135664, 642.949883 );\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );\n" +
                                                        "\t\t	P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	P += OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	hash_0 = frac( P * ( 1.0 / SOMELARGEFLOATS.x ) );\n" +
                                                        "\t\t	hash_1 = frac( P * ( 1.0 / SOMELARGEFLOATS.y ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	SimplexPerlin2D  ( simplex gradient noise )\n" +
                                                        "\t\t//	Perlin noise over a simplex (triangular) grid\n" +
                                                        "\t\t//	Return value range of -1.0->1.0\n" +
                                                        "\t\t//	http://briansharpe.files.wordpress.com/2012/01/simplexperlinsample.jpg\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Implementation originally based off Stefan Gustavson's and Ian McEwan's work at...\n" +
                                                        "\t\t//	http://github.com/ashima/webgl-noise\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat SimplexPerlin2D( float2 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	simplex math constants\n" +
                                                        "\t\t	const float SKEWFACTOR = 0.36602540378443864676372317075294;			// 0.5*(sqrt(3.0)-1.0)\n" +
                                                        "\t\t	const float UNSKEWFACTOR = 0.21132486540518711774542560974902;			// (3.0-sqrt(3.0))/6.0\n" +
                                                        "\t\t	const float SIMPLEX_TRI_HEIGHT = 0.70710678118654752440084436210485;	// sqrt( 0.5 )	height of simplex triangle\n" +
                                                        "\t\t	const float3 SIMPLEX_POINTS = float3( 1.0-UNSKEWFACTOR, -UNSKEWFACTOR, 1.0-2.0*UNSKEWFACTOR );		//	vertex info for simplex triangle\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	establish our grid cell.\n" +
                                                        "\t\t	P *= SIMPLEX_TRI_HEIGHT;		// scale space so we can have an approx feature size of 1.0  ( optional )\n" +
                                                        "\t\t	float2 Pi = floor( P + dot( P, float2( SKEWFACTOR, SKEWFACTOR ) ) );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	float4 hash_x, hash_y;\n" +
                                                        "\t\t	FAST32_hash_2D( Pi, hash_x, hash_y );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	establish vectors to the 3 corners of our simplex triangle\n" +
                                                        "\t\t	float2 v0 = Pi - dot( Pi, float2( UNSKEWFACTOR, UNSKEWFACTOR ) ) - P;\n" +
                                                        "\t\t	float4 v1pos_v1hash = (v0.x < v0.y) ? float4(SIMPLEX_POINTS.xy, hash_x.y, hash_y.y) : float4(SIMPLEX_POINTS.yx, hash_x.z, hash_y.z);\n" +
                                                        "\t\t	float4 v12 = float4( v1pos_v1hash.xy, SIMPLEX_POINTS.zz ) + v0.xyxy;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the dotproduct of our 3 corner vectors with 3 random normalized vectors\n" +
                                                        "\t\t	float3 grad_x = float3( hash_x.x, v1pos_v1hash.z, hash_x.w ) - 0.49999;\n" +
                                                        "\t\t	float3 grad_y = float3( hash_y.x, v1pos_v1hash.w, hash_y.w ) - 0.49999;\n" +
                                                        "\t\t	float3 grad_results = rsqrt( grad_x * grad_x + grad_y * grad_y ) * ( grad_x * float3( v0.x, v12.xz ) + grad_y * float3( v0.y, v12.yw ) );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	Normalization factor to scale the final result to a strict 1.0->-1.0 range\n" +
                                                        "\t\t	//	x = ( sqrt( 0.5 )/sqrt( 0.75 ) ) * 0.5\n" +
                                                        "\t\t	//	NF = 1.0 / ( x * ( ( 0.5 ? x*x ) ^ 4 ) * 2.0 )\n" +
                                                        "\t\t	//	http://briansharpe.wordpress.com/2012/01/13/simplex-noise/#comment-36\n" +
                                                        "\t\t	const float FINAL_NORMALIZATION = 99.204334582718712976990005025589;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	evaluate the surflet, sum and return\n" +
                                                        "\t\t	float3 m = float3( v0.x, v12.xz ) * float3( v0.x, v12.xz ) + float3( v0.y, v12.yw ) * float3( v0.y, v12.yw );\n" +
                                                        "\t\t	m = max(0.5 - m, 0.0);		//	The 0.5 here is SIMPLEX_TRI_HEIGHT^2\n" +
                                                        "\t\t	m = m*m;\n" +
                                                        "\t\t	m = m*m;\n" +
                                                        "\t\t	return dot(m, grad_results) * FINAL_NORMALIZATION;\n" +
                                                        "\t\t}\n";
    public const string IncludeSimplexDerived =         "\t\t//\n" +
                                                        "\t\t//	FAST32_hash\n" +
                                                        "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	The hash formula takes the form....\n" +
                                                        "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
                                                        "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
                                                        "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
                                                        "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
                                                        "\t\t//\n" +
                                                        "\t\tvoid FAST32_hash_2D( float2 gridcell, out float4 hash_0, out float4 hash_1 )	//	generates 2 random numbers for each of the 4 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 71.0;\n" +
                                                        "\t\t	const float2 SOMELARGEFLOATS = float2( 951.135664, 642.949883 );\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );\n" +
                                                        "\t\t	P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	P += OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	hash_0 = frac( P * ( 1.0 / SOMELARGEFLOATS.x ) );\n" +
                                                        "\t\t	hash_1 = frac( P * ( 1.0 / SOMELARGEFLOATS.y ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	SimplexPerlin2D_Deriv\n" +
                                                        "\t\t//	SimplexPerlin2D noise with derivatives\n" +
                                                        "\t\t//	returns float3( value, xderiv, yderiv )\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat3 SimplexPerlin2D_Deriv( float2 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	simplex math constants\n" +
                                                        "\t\t	const float SKEWFACTOR = 0.36602540378443864676372317075294;			// 0.5*(sqrt(3.0)-1.0)\n" +
                                                        "\t\t	const float UNSKEWFACTOR = 0.21132486540518711774542560974902;			// (3.0-sqrt(3.0))/6.0\n" +
                                                        "\t\t	const float SIMPLEX_TRI_HEIGHT = 0.70710678118654752440084436210485;	// sqrt( 0.5 )	height of simplex triangle\n" +
                                                        "\t\t	const float3 SIMPLEX_POINTS = float3( 1.0-UNSKEWFACTOR, -UNSKEWFACTOR, 1.0-2.0*UNSKEWFACTOR );		//	vertex info for simplex triangle\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	establish our grid cell.\n" +
                                                        "\t\t	P *= SIMPLEX_TRI_HEIGHT;		// scale space so we can have an approx feature size of 1.0  ( optional )\n" +
                                                        "\t\t	float2 Pi = floor( P + dot( P, float2( SKEWFACTOR, SKEWFACTOR ) ) );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	float4 hash_x, hash_y;\n" +
                                                        "\t\t	FAST32_hash_2D( Pi, hash_x, hash_y );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	establish vectors to the 3 corners of our simplex triangle\n" +
                                                        "\t\t	float2 v0 = Pi - dot( Pi, float2( UNSKEWFACTOR, UNSKEWFACTOR ) ) - P;\n" +
                                                        "\t\t	float4 v1pos_v1hash = (v0.x < v0.y) ? float4(SIMPLEX_POINTS.xy, hash_x.y, hash_y.y) : float4(SIMPLEX_POINTS.yx, hash_x.z, hash_y.z);\n" +
                                                        "\t\t	float4 v12 = float4( v1pos_v1hash.xy, SIMPLEX_POINTS.zz ) + v0.xyxy;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the dotproduct of our 3 corner vectors with 3 random normalized vectors\n" +
                                                        "\t\t	float3 grad_x = float3( hash_x.x, v1pos_v1hash.z, hash_x.w ) - 0.49999;\n" +
                                                        "\t\t	float3 grad_y = float3( hash_y.x, v1pos_v1hash.w, hash_y.w ) - 0.49999;\n" +
                                                        "\t\t	float3 norm = rsqrt( grad_x * grad_x + grad_y * grad_y );\n" +
                                                        "\t\t	grad_x *= norm;\n" +
                                                        "\t\t	grad_y *= norm;\n" +
                                                        "\t\t	float3 grad_results = grad_x * float3( v0.x, v12.xz ) + grad_y * float3( v0.y, v12.yw );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	evaluate the surflet\n" +
                                                        "\t\t	float3 m = float3( v0.x, v12.xz ) * float3( v0.x, v12.xz ) + float3( v0.y, v12.yw ) * float3( v0.y, v12.yw );\n" +
                                                        "\t\t	m = max(0.5 - m, 0.0);		//	The 0.5 here is SIMPLEX_TRI_HEIGHT^2\n" +
                                                        "\t\t	float3 m2 = m*m;\n" +
                                                        "\t\t	float3 m4 = m2*m2;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calc the deriv\n" +
                                                        "\t\t	float3 temp = 8.0 * m2 * m * grad_results;\n" +
                                                        "\t\t	float xderiv = dot( temp, float3( v0.x, v12.xz ) ) - dot( m4, grad_x );\n" +
                                                        "\t\t	float yderiv = dot( temp, float3( v0.y, v12.yw ) ) - dot( m4, grad_y );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	const float FINAL_NORMALIZATION = 99.204334582718712976990005025589;	//	scales the final result to a strict 1.0->-1.0 range\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	sum the surflets and return all results combined in a float3\n" +
                                                        "\t\t	return float3( dot( m4, grad_results ), xderiv, yderiv ) * FINAL_NORMALIZATION;\n" +
                                                        "\t\t}\n";
    public const string IncludeCellNormal =             "\t\t//\n" +
                                                        "\t\t//	FAST32_hash\n" +
                                                        "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	The hash formula takes the form....\n" +
                                                        "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
                                                        "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
                                                        "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
                                                        "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat4 FAST32_hash_2D_Cell( float2 gridcell )	//	generates 4 different random numbers for the single given cell point\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 71.0;\n" +
                                                        "\t\t	const float4 SOMELARGEFLOATS = float4( 951.135664, 642.949883, 803.202459, 986.973274 );\n" +
                                                        "\t\t	float2 P = gridcell - floor(gridcell * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	P += OFFSET.xy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	return frac( (P.x * P.y) * ( 1.0 / SOMELARGEFLOATS.xyzw ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\tfloat Cellular2D(float2 xy, int cellType, int distanceFunction) \n" +
                                                        "\t\t{\n" +
                                                        "\t\t	int xi = int(floor(xy.x));\n" +
                                                        "\t\t	int yi = int(floor(xy.y));\n" +
                                                        "\t\t \n" +
                                                        "\t\t	float xf = xy.x - float(xi);\n" +
                                                        "\t\t	float yf = xy.y - float(yi);\n" +
                                                        "\t\t \n" +
                                                        "\t\t	float dist1 = 9999999.0;\n" +
                                                        "\t\t	float dist2 = 9999999.0;\n" +
                                                        "\t\t	float dist3 = 9999999.0;\n" +
                                                        "\t\t	float dist4 = 9999999.0;\n" +
                                                        "\t\t	float2 cell;\n" +
                                                        "\t\t \n" +
                                                        "\t\t	for (int y = -1; y <= 1; y++) {\n" +
                                                        "\t\t		for (int x = -1; x <= 1; x++) {\n" +
                                                        "\t\t			cell = FAST32_hash_2D_Cell(float2(xi + x, yi + y)).xy;\n" +
                                                        "\t\t			cell.x += (float(x) - xf);\n" +
                                                        "\t\t			cell.y += (float(y) - yf);\n" +
                                                        "\t\t			float dist = 0.0;\n" +
                                                        "\t\t			if(distanceFunction <= 1)\n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist = sqrt(dot(cell, cell));\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if(distanceFunction > 1 && distanceFunction <= 2)\n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist = dot(cell, cell);\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if(distanceFunction > 2 && distanceFunction <= 3)\n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist = abs(cell.x) + abs(cell.y);\n" +
                                                        "\t\t				dist *= dist;\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if(distanceFunction > 3 && distanceFunction <= 4)\n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist = max(abs(cell.x), abs(cell.y));\n" +
                                                        "\t\t				dist *= dist;\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if(distanceFunction > 4 && distanceFunction <= 5)\n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist = dot(cell, cell) + cell.x*cell.y;	\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if(distanceFunction > 5 && distanceFunction <= 6)\n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist = pow(abs(cell.x*cell.x*cell.x*cell.x + cell.y*cell.y*cell.y*cell.y), 0.25);\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if(distanceFunction > 6 && distanceFunction <= 7)\n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist = sqrt(abs(cell.x)) + sqrt(abs(cell.y));\n" +
                                                        "\t\t				dist *= dist;\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			if (dist < dist1) \n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist4 = dist3;\n" +
                                                        "\t\t				dist3 = dist2;\n" +
                                                        "\t\t				dist2 = dist1;\n" +
                                                        "\t\t				dist1 = dist;\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if (dist < dist2) \n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist4 = dist3;\n" +
                                                        "\t\t				dist3 = dist2;\n" +
                                                        "\t\t				dist2 = dist;\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if (dist < dist3) \n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist4 = dist3;\n" +
                                                        "\t\t				dist3 = dist;\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t			else if (dist < dist4) \n" +
                                                        "\t\t			{\n" +
                                                        "\t\t				dist4 = dist;\n" +
                                                        "\t\t			}\n" +
                                                        "\t\t		}\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t \n" +
                                                        "\t\t	if(cellType <= 1)	// F1\n" +
                                                        "\t\t		return dist1;	//	scale return value from 0.0->1.333333 to 0.0->1.0  	(2/3)^2 * 3  == (12/9) == 1.333333\n" +
                                                        "\t\t	else if(cellType > 1 && cellType <= 2)	// F2\n" +
                                                        "\t\t		return dist2;\n" +
                                                        "\t\t	else if(cellType > 2 && cellType <= 3)	// F3\n" +
                                                        "\t\t		return dist3;\n" +
                                                        "\t\t	else if(cellType > 3 && cellType <= 4)	// F4\n" +
                                                        "\t\t		return dist4;\n" +
                                                        "\t\t	else if(cellType > 4 && cellType <= 5)	// F2 - F1 \n" +
                                                        "\t\t		return dist2 - dist1;\n" +
                                                        "\t\t	else if(cellType > 5 && cellType <= 6)	// F3 - F2 \n" +
                                                        "\t\t		return dist3 - dist2;\n" +
                                                        "\t\t	else if(cellType > 6 && cellType <= 7)	// F1 + F2/2\n" +
                                                        "\t\t		return dist1 + dist2/2.0;\n" +
                                                        "\t\t	else if(cellType > 7 && cellType <= 8)	// F1 * F2\n" +
                                                        "\t\t		return dist1 * dist2;\n" +
                                                        "\t\t	else if(cellType > 8 && cellType <= 9)	// Crackle\n" +
                                                        "\t\t		return max(1.0, 10*(dist2 - dist1));\n" +
                                                        "\t\t	else\n" +
                                                        "\t\t		return dist1;\n" +
                                                        "\t\t}\n";
    public const string IncludeCellFast = "\t\t//\n" +
                                                        "\t\t//	FAST32_hash\n" +
                                                        "\t\t//	A very fast hashing function.  Requires 32bit support.\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	The hash formula takes the form....\n" +
                                                        "\t\t//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT\n" +
                                                        "\t\t//	We truncate and offset the domain to the most interesting part of the noise.\n" +
                                                        "\t\t//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.\n" +
                                                        "\t\t//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate\n" +
                                                        "\t\t//\n" +
                                                        "\t\tvoid FAST32_hash_2D( float2 gridcell, out float4 hash_0, out float4 hash_1 )	//	generates 2 random numbers for each of the 4 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t	const float2 OFFSET = float2( 26.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 71.0;\n" +
                                                        "\t\t	const float2 SOMELARGEFLOATS = float2( 951.135664, 642.949883 );\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell.xy + 1.0 );\n" +
                                                        "\t\t	P = P - floor(P * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	P += OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	hash_0 = frac( P * ( 1.0 / SOMELARGEFLOATS.x ) );\n" +
                                                        "\t\t	hash_1 = frac( P * ( 1.0 / SOMELARGEFLOATS.y ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//	convert a 0.0->1.0 sample to a -1.0->1.0 sample weighted towards the extremes\n" +
                                                        "\t\tfloat4 Cellular_weight_samples( float4 samples )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	samples = samples * 2.0 - 1.0;\n" +
                                                        "\t\t	//return (1.0 - samples * samples) * sign(samples);	// square\n" +
                                                        "\t\t	return (samples * samples * samples) - sign(samples);	// cubic (even more variance)\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Cellular Noise 2D\n" +
                                                        "\t\t//	Based off Stefan Gustavson's work at http://www.itn.liu.se/~stegu/GLSL-cellular\n" +
                                                        "\t\t//	http://briansharpe.files.wordpress.com/2011/12/cellularsample.jpg\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Speed up by using 2x2 search window instead of 3x3\n" +
                                                        "\t\t//	produces a range of 0.0->1.0\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat Cellular2D(float2 P)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float2 Pi = floor(P);\n" +
                                                        "\t\t	float2 Pf = P - Pi;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	float4 hash_x, hash_y;\n" +
                                                        "\t\t	FAST32_hash_2D( Pi, hash_x, hash_y );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	generate the 4 random points\n" +
                                                        "\t\t	#if 0\n" +
                                                        "\t\t	//	restrict the random point offset to eliminate artifacts\n" +
                                                        "\t\t	//	we'll improve the variance of the noise by pushing the points to the extremes of the jitter window\n" +
                                                        "\t\t	const float JITTER_WINDOW = 0.25;	// 0.25 will guarentee no artifacts.  0.25 is the intersection on x of graphs f(x)=( (0.5+(0.5-x))^2 + (0.5-x)^2 ) and f(x)=( (0.5+x)^2 + x^2 )\n" +
                                                        "\t\t	hash_x = Cellular_weight_samples( hash_x ) * JITTER_WINDOW + float4(0.0, 1.0, 0.0, 1.0);\n" +
                                                        "\t\t	hash_y = Cellular_weight_samples( hash_y ) * JITTER_WINDOW + float4(0.0, 0.0, 1.0, 1.0);\n" +
                                                        "\t\t	#else\n" +
                                                        "\t\t	//	non-weighted jitter window.  jitter window of 0.4 will give results similar to Stefans original implementation\n" +
                                                        "\t\t	//	nicer looking, faster, but has minor artifacts.  ( discontinuities in signal )\n" +
                                                        "\t\t	const float JITTER_WINDOW = 0.4;\n" +
                                                        "\t\t	hash_x = hash_x * JITTER_WINDOW * 2.0 + float4(-JITTER_WINDOW, 1.0-JITTER_WINDOW, -JITTER_WINDOW, 1.0-JITTER_WINDOW);\n" +
                                                        "\t\t	hash_y = hash_y * JITTER_WINDOW * 2.0 + float4(-JITTER_WINDOW, -JITTER_WINDOW, 1.0-JITTER_WINDOW, 1.0-JITTER_WINDOW);\n" +
                                                        "\t\t	#endif\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	return the closest squared distance\n" +
                                                        "\t\t	float4 dx = Pf.xxxx - hash_x;\n" +
                                                        "\t\t	float4 dy = Pf.yyyy - hash_y;\n" +
                                                        "\t\t	float4 d = dx * dx + dy * dy;\n" +
                                                        "\t\t	d.xy = min(d.xy, d.zw);\n" +
                                                        "\t\t	return min(d.x, d.y) * ( 1.0 / 1.125 );	//	scale return value from 0.0->1.125 to 0.0->1.0  ( 0.75^2 * 2.0  == 1.125 )\n" +
                                                        "\t\t}\n";
	public const string IncludeFbmValueNormal = 	    "\t\tfloat ValueNormal(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
													    "\t\t{\n" +
													    "\t\t	float sum = 0;\n" +
													    "\t\t	for (int i = 0; i < octaves; i++)\n" +
													    "\t\t	{\n" +
													    "\t\t		float h = 0;\n" +
													    "\t\t		h = Value2D((p + offset) * frequency);\n" +
													    "\t\t		sum += h*amplitude;\n" +
													    "\t\t		frequency *= lacunarity;\n" +
													    "\t\t		amplitude *= persistence;\n" +
													    "\t\t	}\n" +
													    "\t\t	return sum;\n" +
													    "\t\t}\n";
	public const string IncludeFbmValueBillowed =       "\t\tfloat ValueBillowed(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
													    "\t\t{\n" +
													    "\t\t	float sum = 0;\n" +
													    "\t\t	for (int i = 0; i < octaves; i++)\n" +
													    "\t\t	{\n" +
													    "\t\t		float h = 0;\n" +
													    "\t\t		h = abs(Value2D((p + offset) * frequency));\n" +
													    "\t\t		sum += h*amplitude;\n" +
													    "\t\t		frequency *= lacunarity;\n" +
													    "\t\t		amplitude *= persistence;\n" +
													    "\t\t	}\n" +
													    "\t\t	return sum;\n" +
													    "\t\t}\n";
    public const string IncludeFbmValueRidged =         "\t\tfloat ValueRidged(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = 0.5 * (ridgeOffset - abs(4*Value2D((p + offset) * frequency)));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmValueDerivedIQ =      "\t\tfloat ValueDerivedIQ(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t   float sum = 0;\n" +
	                                                    "\t\t   float2 dsum = float2(0.0, 0.0);\n" +
	                                                    "\t\t   for (int i = 0; i < octaves; i++)\n" +
	                                                    "\t\t   {\n" +
	                                                    "\t\t	    float3 n = Value2D_Deriv((p + offset) * frequency);\n" +
	                                                    "\t\t	    dsum += n.yz;\n" +
	                                                    "\t\t	    sum += amplitude * n.x / (1 + dot(dsum, dsum));\n" +
	                                                    "\t\t	    frequency *= lacunarity;\n" +
	                                                    "\t\t	    amplitude *= persistence;\n" +
	                                                    "\t\t   }\n" +
	                                                    "\t\t   return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmValueDerivedSwiss =   "\t\tfloat ValueDerivedSwiss(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0.0;\n" +
                                                        "\t\t	float2 dsum = float2(0.0, 0.0);\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float3 n = 0.5 * (0 + (ridgeOffset - abs(Value2D_Deriv((p+offset+warp*dsum)*frequency))));\n" +
                                                        "\t\t		sum += amplitude * n.x;\n" +
                                                        "\t\t		dsum += amplitude * n.yz * -n.x;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmValueDerivedJordan =  "\t\tfloat ValueDerivedJordan(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp0, float warp, float damp0, float damp, float damp_scale)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float3 n = Value2D_Deriv((p+offset)*frequency);\n" +
                                                        "\t\t	float3 n2 = n * n.x;\n" +
                                                        "\t\t   float sum = n2.x;\n" +
                                                        "\t\t   float2 dsum_warp = warp0*n2.yz;\n" +
                                                        "\t\t   float2 dsum_damp = damp0*n2.yz;\n" +
                                                        "\t\t   float damped_amp = amplitude * persistence;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		n = Value2D_Deriv((p+offset)*frequency+dsum_warp.xy);\n" +
                                                        "\t\t		n2 = n * n.x;\n" +
                                                        "\t\t       sum += damped_amp * n2.x;\n" +
                                                        "\t\t       dsum_warp += warp * n2.yz;\n" +
                                                        "\t\t       dsum_damp += damp * n2.yz;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t		damped_amp = amplitude * (1-damp_scale/(1+dot(dsum_damp,dsum_damp)));\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmPerlinNormal =        "\t\tfloat PerlinNormal(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = Perlin2D((p + offset) * frequency);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinBillowed =      "\t\tfloat PerlinBillowed(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = abs(Perlin2D((p + offset) * frequency));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinRidged =        "\t\tfloat PerlinRidged(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = 0.5 * (ridgeOffset - abs(4*Perlin2D((p + offset) * frequency)));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinDerivedIQ = "\t\tfloat PerlinDerivedIQ(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t   float sum = 0;\n" +
                                                        "\t\t   float2 dsum = float2(0.0, 0.0);\n" +
                                                        "\t\t   for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t   {\n" +
                                                        "\t\t	    float3 n = PerlinSurflet2D_Deriv((p + offset) * frequency);\n" +
                                                        "\t\t	    dsum += n.yz;\n" +
                                                        "\t\t	    sum += amplitude * n.x / (1 + dot(dsum, dsum));\n" +
                                                        "\t\t	    frequency *= lacunarity;\n" +
                                                        "\t\t	    amplitude *= persistence;\n" +
                                                        "\t\t   }\n" +
                                                        "\t\t   return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinDerivedSwiss = "\t\tfloat PerlinDerivedSwiss(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0.0;\n" +
                                                        "\t\t	float2 dsum = float2(0.0, 0.0);\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float3 n = 0.5 * (0 + (ridgeOffset - abs(PerlinSurflet2D_Deriv((p+offset+warp*dsum)*frequency))));\n" +
                                                        "\t\t		sum += amplitude * n.x;\n" +
                                                        "\t\t		dsum += amplitude * n.yz * -n.x;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmPerlinDerivedJordan = "\t\tfloat PerlinDerivedJordan(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp0, float warp, float damp0, float damp, float damp_scale)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float3 n = PerlinSurflet2D_Deriv((p+offset)*frequency);\n" +
                                                        "\t\t	float3 n2 = n * n.x;\n" +
                                                        "\t\t   float sum = n2.x;\n" +
                                                        "\t\t   float2 dsum_warp = warp0*n2.yz;\n" +
                                                        "\t\t   float2 dsum_damp = damp0*n2.yz;\n" +
                                                        "\t\t   float damped_amp = amplitude * persistence;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		n = PerlinSurflet2D_Deriv((p+offset)*frequency+dsum_warp.xy);\n" +
                                                        "\t\t		n2 = n * n.x;\n" +
                                                        "\t\t       sum += damped_amp * n2.x;\n" +
                                                        "\t\t       dsum_warp += warp * n2.yz;\n" +
                                                        "\t\t       dsum_damp += damp * n2.yz;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t		damped_amp = amplitude * (1-damp_scale/(1+dot(dsum_damp,dsum_damp)));\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmSimplexNormal =       "\t\tfloat SimplexNormal(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = SimplexPerlin2D((p + offset) * frequency);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexBillowed = "\t\tfloat SimplexBillowed(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = abs(SimplexPerlin2D((p + offset) * frequency));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexRidged = "\t\tfloat SimplexRidged(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = 0.5 * (ridgeOffset - abs(4*SimplexPerlin2D((p + offset) * frequency)));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexDerivedIQ = "\t\tfloat SimplexDerivedIQ(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t   float sum = 0;\n" +
                                                        "\t\t   float2 dsum = float2(0.0, 0.0);\n" +
                                                        "\t\t   for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t   {\n" +
                                                        "\t\t	    float3 n = SimplexPerlin2D_Deriv((p + offset) * frequency);\n" +
                                                        "\t\t	    dsum += n.yz;\n" +
                                                        "\t\t	    sum += amplitude * n.x / (1 + dot(dsum, dsum));\n" +
                                                        "\t\t	    frequency *= lacunarity;\n" +
                                                        "\t\t	    amplitude *= persistence;\n" +
                                                        "\t\t   }\n" +
                                                        "\t\t   return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexDerivedSwiss = "\t\tfloat SimplexDerivedSwiss(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0.0;\n" +
                                                        "\t\t	float2 dsum = float2(0.0, 0.0);\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float3 n = 0.5 * (0 + (ridgeOffset - abs(SimplexPerlin2D_Deriv((p+offset+warp*dsum)*frequency))));\n" +
                                                        "\t\t		sum += amplitude * n.x;\n" +
                                                        "\t\t		dsum += amplitude * n.yz * -n.x;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmSimplexDerivedJordan = "\t\tfloat SimplexDerivedJordan(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp0, float warp, float damp0, float damp, float damp_scale)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float3 n = SimplexPerlin2D_Deriv((p+offset)*frequency);\n" +
                                                        "\t\t	float3 n2 = n * n.x;\n" +
                                                        "\t\t   float sum = n2.x;\n" +
                                                        "\t\t   float2 dsum_warp = warp0*n2.yz;\n" +
                                                        "\t\t   float2 dsum_damp = damp0*n2.yz;\n" +
                                                        "\t\t   float damped_amp = amplitude * persistence;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		n = SimplexPerlin2D_Deriv((p+offset)*frequency+dsum_warp.xy);\n" +
                                                        "\t\t		n2 = n * n.x;\n" +
                                                        "\t\t       sum += damped_amp * n2.x;\n" +
                                                        "\t\t       dsum_warp += warp * n2.yz;\n" +
                                                        "\t\t       dsum_damp += damp * n2.yz;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t		damped_amp = amplitude * (1-damp_scale/(1+dot(dsum_damp,dsum_damp)));\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmCellNormal =          "\t\tfloat CellNormal(float2 p, int octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence, int cellType, int distanceFunction)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = Cellular2D((p+offset) * frequency, cellType, distanceFunction);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmCellFast =            "\t\tfloat CellFast(float2 p, fixed octaves, float2 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = Cellular2D((p + offset) * frequency);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
}

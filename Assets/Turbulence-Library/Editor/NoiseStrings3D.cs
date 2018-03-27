public static class NoiseStrings3D 
{
    public const string NormalizeOn =                   "\t\t\th = h * 0.5 + 0.5;";

    public const string TagsTransparent =               "\t\tTags {\"Queue\"=\"Transparent\"}";

    public const string BlendingAlpha =                 "\t\tBlend SrcAlpha OneMinusSrcAlpha";

    public const string InputTexturing =                "\t\t\tfloat2 uv_LowTexture;\n" +
                                                        "\t\t\tfloat2 uv_HighTexture;";
    public const string InputNormal =                   "\t\t\tfloat3 pos;\n";

    public const string LightingLambert = 				"\t\t#pragma surface surf Lambert vertex:vert";
    public const string LightingBlinnPhong = 			"\t\t#pragma surface surf BlinnPhong vertex:vert";

    public const string VertexLocalDispOffAnimOff =     "\t\t\tOUT.pos = v.vertex.xyz;";
    public const string VertexLocalDispOffAnimOn =      "\t\t\tOUT.pos = float3(v.texcoord.xy, _Time.y * _AnimSpeed);";
    public const string VertexLocalDispOnAnimOff = 		"\t\t\tOUT.pos = v.vertex.xyz;\n" +
                                                        "${VertexNoise}\n" +
                                                        "\t\t\tv.vertex.xyz += v.normal * h * _Displacement;";
    public const string VertexLocalDispOnAnimOn =       "\t\t\tOUT.pos = float3(v.texcoord.xy, _Time.y * _AnimSpeed);\n" +
                                                        "${VertexNoise}\n" +
                                                        "\t\t\tv.vertex.xyz += v.normal * h * _Displacement;";
    public const string VertexWorldDispOff =            "\t\t\tOUT.pos = mul (_Object2World, v.vertex).xyz;";
	public const string VertexWorldDispOn = 			"";

    public const string ColoringOnTexturingOff = 		"\t\t\tfloat4 color = float4(0.0, 0.0, 0.0, 0.0);\n" +
                                                        "\t\t\tcolor = lerp(_LowColor, _HighColor, h);";
    public const string ColoringOnTexturingOn = 		"\t\t\tfloat4 color = float4(0.0, 0.0, 0.0, 0.0);\n" +
                                                        "\t\t\tfloat4 lowTexColor = tex2D(_LowTexture, IN.uv_LowTexture);\n" +
                                                        "\t\t\tfloat4 highTexColor = tex2D(_HighTexture, IN.uv_HighTexture);\n" +
                                                        "\t\t\tcolor = lerp(_LowColor * lowTexColor, _HighColor * highTexColor, h);";
    public const string ColoringOffTexturingOn = 		"\t\t\tfloat4 color = float4(0.0, 0.0, 0.0, 0.0);\n" +
                                                        "\t\t\tfloat4 lowTexColor = tex2D(_LowTexture, IN.uv_LowTexture);\n" +
                                                        "\t\t\tfloat4 highTexColor = tex2D(_HighTexture, IN.uv_HighTexture);\n" +
                                                        "\t\t\tcolor = lerp(lowTexColor, highTexColor, h);";
    public const string ColoringOffTexturingOff = 		"\t\t\tfloat4 color = float4(h, h, h, h);";

    public const string AlphaOn = 						"\t\t\to.Alpha = h * _Transparency;";
    public const string AlphaOff = 						"\t\t\to.Alpha = 1.0;";

    public const string PropertiesNormal = 				"\t\t_Octaves (\"Octaves\", Float) = 8.0\n" +
                                                        "\t\t_Frequency (\"Frequency\", Float) = 1.0\n" +
                                                        "\t\t_Amplitude (\"Amplitude\", Float) = 1.0\n" +
                                                        "\t\t_Lacunarity (\"Lacunarity\", Float) = 1.92\n" +
                                                        "\t\t_Persistence (\"Persistence\", Float) = 0.8\n" +
                                                        "\t\t_Offset (\"Offset\", Vector) = (0.0, 0.0, 0.0, 0.0)\n";
    public const string PropertiesRidged = 				"\t\t_RidgeOffset (\"Ridge Offset\", Float) = 1.0\n";
    public const string PropertiesDerivedSwiss = 		"\t\t_RidgeOffset (\"Ridge Offset\", Float) = 1.0\n" +
                                                        "\t\t_Warp (\"Warp\", Float) = 0.25\n";
    public const string PropertiesDerivedJordan = 		"\t\t_Warp0 (\"Warp0\", Float) = 0.15\n" +
                                                        "\t\t_Warp (\"Warp\", Float) = 0.25\n" +
                                                        "\t\t_Damp0 (\"Damp0\", Float) = 0.8\n" +
                                                        "\t\t_Damp (\"Damp\", Float) = 1.0\n" +
                                                        "\t\t_DampScale (\"Damp Scale\", Float) = 1.0\n";
    public const string PropertiesCell = 				"\t\t_CellType (\"Cell Type\", Float) = 1.0\n" +
                                                        "\t\t_DistanceFunction (\"Distance Function\", Float) = 1.0\n";
    public const string PropertiesTransparency = 		"\t\t_Transparency (\"Transparency\", Range(0.0, 1.0)) = 1.0\n";
    public const string PropertiesColoring = 			"\t\t_LowColor(\"Low Color\", Vector) = (0.0, 0.0, 0.0, 1.0)\n" +
                                                        "\t\t_HighColor(\"High Color\", Vector) = (1.0, 1.0, 1.0, 1.0)\n";
    public const string PropertiesTexturing = 			"\t\t_LowTexture(\"Low Texture\", 2D) = \"\" {}\n" +
                                                        "\t\t_HighTexture(\"High Texture\", 2D) = \"\" {}\n";
    public const string PropertiesDisplacement =        "\t\t_Displacement(\"Displacement\", Float) = 1.0";
    public const string PropertiesAnimation =           "\t\t_AnimSpeed(\"Animation Speed\", Float) = 1.0";

    public const string UniformsNormal = 				"\t\tfixed _Octaves;\n" +
                                                        "\t\tfloat _Frequency;\n" +
                                                        "\t\tfloat _Amplitude;\n" +
                                                        "\t\tfloat3 _Offset;\n" +
                                                        "\t\tfloat _Lacunarity;\n" +
                                                        "\t\tfloat _Persistence;\n";
    public const string UniformsRidged = 				"\t\tfloat _RidgeOffset;\n";
    public const string UniformsDerivedSwiss = 			"\t\tfloat _RidgeOffset;\n" +
                                                        "\t\tfloat _Warp;\n";
    public const string UniformsDerivedJordan = 		"\t\tfloat _Warp0;\n" +
                                                        "\t\tfloat _Warp;\n" +
                                                        "\t\tfloat _Damp0;\n" +
                                                        "\t\tfloat _Damp;\n" +
                                                        "\t\tfloat _DampScale;\n";
    public const string UniformsCell = 					"\t\tfixed _CellType;\n" +
                                                        "\t\tfixed _DistanceFunction;\n";
    public const string UniformsTransparency = 			"\t\tfixed _Transparency;\n";
    public const string UniformsColoring = 				"\t\tfixed4 _LowColor;\n" +
                                                        "\t\tfixed4 _HighColor;\n";
    public const string UniformsTexturing = 			"\t\tsampler2D _LowTexture;\n" +
                                                        "\t\tsampler2D _HighTexture;\n";
    public const string UniformsDisplacement = 			"\t\tfloat _Displacement;";
    public const string UniformsAnimation =             "\t\tfloat _AnimSpeed;";

    public const string NoisePerlinNormal = 			"\t\t\tfloat h = PerlinNormal(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoisePerlinBillowed = 			"\t\t\tfloat h = PerlinBillowed(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoisePerlinRidged = 			"\t\t\tfloat h = PerlinRidged(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _RidgeOffset);";
    public const string NoisePerlinDerivedIQ = 			"\t\t\tfloat h = PerlinDerivedIQ(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoisePerlinDerivedSwiss = 		"\t\t\tfloat h = PerlinDerivedSwiss(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp, _RidgeOffset);";
    public const string NoisePerlinDerivedJordan = 		"\t\t\tfloat h = PerlinDerivedJordan(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp0, _Warp, _Damp0, _Damp, _DampScale);";
    public const string NoiseSimplexNormal = 			"\t\t\tfloat h = SimplexNormal(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseSimplexBillowed = 			"\t\t\tfloat h = SimplexBillowed(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseSimplexRidged = 			"\t\t\tfloat h = SimplexRidged(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _RidgeOffset);";
    public const string NoiseSimplexDerivedIQ = 		"\t\t\tfloat h = SimplexDerivedIQ(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseSimplexDerivedSwiss = 		"\t\t\tfloat h = SimplexDerivedSwiss(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp, _RidgeOffset);";
    public const string NoiseSimplexDerivedJordan = 	"\t\t\tfloat h = SimplexDerivedJordan(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp0, _Warp, _Damp0, _Damp, _DampScale);";
    public const string NoiseValueNormal = 				"\t\t\tfloat h = ValueNormal(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseValueBillowed = 			"\t\t\tfloat h = ValueBillowed(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseValueRidged = 				"\t\t\tfloat h = ValueRidged(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _RidgeOffset);";
    public const string NoiseValueDerivedIQ = 			"\t\t\tfloat h = ValueDerivedIQ(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";
    public const string NoiseValueDerivedSwiss = 		"\t\t\tfloat h = ValueDerivedSwiss(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp, _RidgeOffset);";
    public const string NoiseValueDerivedJordan = 		"\t\t\tfloat h = ValueDerivedJordan(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _Warp0, _Warp, _Damp0, _Damp, _DampScale);";
    public const string NoiseCellNormal = 				"\t\t\tfloat h = CellNormal(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence, _CellType, _DistanceFunction);";
    public const string NoiseCellFast = 				"\t\t\tfloat h = CellFast(IN.pos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);";

    public const string IncludeValue =                  "\t\t//\n" +
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
                                                        "\t\tvoid FAST32_hash_3D( float3 gridcell, out float4 lowz_hash, out float4 highz_hash )	//	generates a random number for each of the 8 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float SOMELARGEFLOAT = 635.298681;\n" +
                                                        "\t\t	const float ZINC = 48.500388;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	float3 gridcell_inc1 = step( gridcell, float3( DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5 ) ) * ( gridcell + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the noise\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell_inc1.xy ) + OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	highz_hash.xy = float2( 1.0 / ( SOMELARGEFLOAT + float2( gridcell.z, gridcell_inc1.z ) * ZINC ) );\n" +
                                                        "\t\t	lowz_hash = frac( P * highz_hash.xxxx );\n" +
                                                        "\t\t	highz_hash = frac( P * highz_hash.yyyy );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Interpolation functions\n" +
                                                        "\t\t//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 )\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/14/two-useful-interpolation-functions-for-noise-development/\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat3 Interpolation_C2( float3 x ) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Value Noise 3D\n" +
                                                        "\t\t//	Return value range of 0.0->1.0\n" +
                                                        "\t\t//	http://briansharpe.files.wordpress.com/2011/11/valuesample1.jpg\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat Value3D( float3 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float3 Pi = floor(P);\n" +
                                                        "\t\t	float3 Pf = P - Pi;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	//	( various hashing methods listed in order of speed )\n" +
                                                        "\t\t	float4 hash_lowz, hash_highz;\n" +
                                                        "\t\t	FAST32_hash_3D( Pi, hash_lowz, hash_highz );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	blend the results and return\n" +
                                                        "\t\t	float3 blend = Interpolation_C2( Pf );\n" +
                                                        "\t\t	float4 res0 = lerp( hash_lowz, hash_highz, blend.z );\n" +
                                                        "\t\t	float2 res1 = lerp( res0.xy, res0.zw, blend.y );\n" +
                                                        "\t\t	return lerp( res1.x, res1.y, blend.x );\n" +
                                                        "\t\t}\n";
    public const string IncludeValueDerived =           "\t\t//\n" +
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
                                                        "\t\tvoid FAST32_hash_3D( float3 gridcell, out float4 lowz_hash, out float4 highz_hash )	//	generates a random number for each of the 8 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float SOMELARGEFLOAT = 635.298681;\n" +
                                                        "\t\t	const float ZINC = 48.500388;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	float3 gridcell_inc1 = step( gridcell, float3( DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5 ) ) * ( gridcell + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the noise\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell_inc1.xy ) + OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	highz_hash.xy = float2( 1.0 / ( SOMELARGEFLOAT + float2( gridcell.z, gridcell_inc1.z ) * ZINC ) );\n" +
                                                        "\t\t	lowz_hash = frac( P * highz_hash.xxxx );\n" +
                                                        "\t\t	highz_hash = frac( P * highz_hash.yyyy );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Interpolation functions\n" +
                                                        "\t\t//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 )\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/14/two-useful-interpolation-functions-for-noise-development/\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat3 Interpolation_C2( float3 x ) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }\n" +
                                                        "\t\tfloat3 Interpolation_C2_Deriv( float3 x ) { return x * x * (x * (x * 30.0 - 60.0) + 30.0); }\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Value3D_Deriv\n" +
                                                        "\t\t//	Value3D noise with derivatives\n" +
                                                        "\t\t//	returns float3( value, xderiv, yderiv, zderiv )\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat4 Value3D_Deriv( float3 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float3 Pi = floor(P);\n" +
                                                        "\t\t	float3 Pf = P - Pi;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	//	( various hashing methods listed in order of speed )\n" +
                                                        "\t\t	float4 hash_lowz, hash_highz;\n" +
                                                        "\t\t	FAST32_hash_3D( Pi, hash_lowz, hash_highz );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	blend the results and return\n" +
                                                        "\t\t	float3 blend = Interpolation_C2( Pf );\n" +
                                                        "\t\t	float4 res0 = lerp( hash_lowz, hash_highz, blend.z );\n" +
                                                        "\t\t	float4 res1 = lerp( res0.xyxz, res0.zwyw, blend.yyxx );\n" +
                                                        "\t\t	float4 res3 = lerp( float4( hash_lowz.xy, hash_highz.xy ), float4( hash_lowz.zw, hash_highz.zw ), blend.y );\n" +
                                                        "\t\t	float2 res4 = lerp( res3.xz, res3.yw, blend.x );\n" +
                                                        "\t\t	return float4( res1.x, 0.0, 0.0, 0.0 ) + ( float4( res1.yyw, res4.y ) - float4( res1.xxz, res4.x ) ) * float4( blend.x, Interpolation_C2_Deriv( Pf ) );\n" +
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
                                                        "\t\t   void FAST32_hash_3D( 	float3 gridcell,\n" +
                                                        "\t\t						out float4 lowz_hash_0,\n" +
                                                        "\t\t						out float4 lowz_hash_1,\n" +
                                                        "\t\t						out float4 lowz_hash_2,\n" +
                                                        "\t\t						out float4 highz_hash_0,\n" +
                                                        "\t\t						out float4 highz_hash_1,\n" +
                                                        "\t\t						out float4 highz_hash_2	)		//	generates 3 random numbers for each of the 8 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float3 SOMELARGEFLOATS = float3( 635.298681, 682.357502, 668.926525 );\n" +
                                                        "\t\t	const float3 ZINC = float3( 48.500388, 65.294118, 63.934599 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	float3 gridcell_inc1 = step( gridcell, float3( DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5 ) ) * ( gridcell + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the noise\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell_inc1.xy ) + OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	float3 lowz_mod = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	float3 highz_mod = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell_inc1.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	lowz_hash_0 = frac( P * lowz_mod.xxxx );\n" +
                                                        "\t\t	highz_hash_0 = frac( P * highz_mod.xxxx );\n" +
                                                        "\t\t	lowz_hash_1 = frac( P * lowz_mod.yyyy );\n" +
                                                        "\t\t	highz_hash_1 = frac( P * highz_mod.yyyy );\n" +
                                                        "\t\t	lowz_hash_2 = frac( P * lowz_mod.zzzz );\n" +
                                                        "\t\t	highz_hash_2 = frac( P * highz_mod.zzzz );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Interpolation functions\n" +
                                                        "\t\t//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 )\n" +
                                                        "\t\t//	http://briansharpe.wordpress.com/2011/11/14/two-useful-interpolation-functions-for-noise-development/\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat3 Interpolation_C2( float3 x ) { return x * x * x * (x * (x * 6.0 - 15.0) + 10.0); }\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Perlin Noise 3D  ( gradient noise )\n" +
                                                        "\t\t//	Return value range of -1.0->1.0\n" +
                                                        "\t\t//	http://briansharpe.files.wordpress.com/2011/11/perlinsample.jpg\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat Perlin3D( float3 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float3 Pi = floor(P);\n" +
                                                        "\t\t	float3 Pf = P - Pi;\n" +
                                                        "\t\t	float3 Pf_min1 = Pf - 1.0;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//\n" +
                                                        "\t\t	//	classic noise.\n" +
                                                        "\t\t	//	requires 3 random values per point.  with an efficent hash function will run faster than improved noise\n" +
                                                        "\t\t	//\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	//	( various hashing methods listed in order of speed )\n" +
                                                        "\t\t	float4 hashx0, hashy0, hashz0, hashx1, hashy1, hashz1;\n" +
                                                        "\t\t	FAST32_hash_3D( Pi, hashx0, hashy0, hashz0, hashx1, hashy1, hashz1 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the gradients\n" +
                                                        "\t\t	float4 grad_x0 = hashx0 - 0.49999;\n" +
                                                        "\t\t	float4 grad_y0 = hashy0 - 0.49999;\n" +
                                                        "\t\t	float4 grad_z0 = hashz0 - 0.49999;\n" +
                                                        "\t\t	float4 grad_x1 = hashx1 - 0.49999;\n" +
                                                        "\t\t	float4 grad_y1 = hashy1 - 0.49999;\n" +
                                                        "\t\t	float4 grad_z1 = hashz1 - 0.49999;\n" +
                                                        "\t\t	float4 grad_results_0 = rsqrt( grad_x0 * grad_x0 + grad_y0 * grad_y0 + grad_z0 * grad_z0 ) * ( float2( Pf.x, Pf_min1.x ).xyxy * grad_x0 + float2( Pf.y, Pf_min1.y ).xxyy * grad_y0 + Pf.zzzz * grad_z0 );\n" +
                                                        "\t\t	float4 grad_results_1 = rsqrt( grad_x1 * grad_x1 + grad_y1 * grad_y1 + grad_z1 * grad_z1 ) * ( float2( Pf.x, Pf_min1.x ).xyxy * grad_x1 + float2( Pf.y, Pf_min1.y ).xxyy * grad_y1 + Pf_min1.zzzz * grad_z1 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	Classic Perlin Interpolation\n" +
                                                        "\t\t	float3 blend = Interpolation_C2( Pf );\n" +
                                                        "\t\t	float4 res0 = lerp( grad_results_0, grad_results_1, blend.z );\n" +
                                                        "\t\t	float2 res1 = lerp( res0.xy, res0.zw, blend.y );\n" +
                                                        "\t\t	float final = lerp( res1.x, res1.y, blend.x );\n" +
                                                        "\t\t	final *= 1.1547005383792515290182975610039;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/sqrt(0.75)\n" +
                                                        "\t\t	return final;\n" +
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
                                                        "\t\t   void FAST32_hash_3D( 	float3 gridcell,\n" +
                                                        "\t\t						out float4 lowz_hash_0,\n" +
                                                        "\t\t						out float4 lowz_hash_1,\n" +
                                                        "\t\t						out float4 lowz_hash_2,\n" +
                                                        "\t\t						out float4 highz_hash_0,\n" +
                                                        "\t\t						out float4 highz_hash_1,\n" +
                                                        "\t\t						out float4 highz_hash_2	)		//	generates 3 random numbers for each of the 8 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float3 SOMELARGEFLOATS = float3( 635.298681, 682.357502, 668.926525 );\n" +
                                                        "\t\t	const float3 ZINC = float3( 48.500388, 65.294118, 63.934599 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	float3 gridcell_inc1 = step( gridcell, float3( DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5 ) ) * ( gridcell + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the noise\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell_inc1.xy ) + OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	float3 lowz_mod = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	float3 highz_mod = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell_inc1.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	lowz_hash_0 = frac( P * lowz_mod.xxxx );\n" +
                                                        "\t\t	highz_hash_0 = frac( P * highz_mod.xxxx );\n" +
                                                        "\t\t	lowz_hash_1 = frac( P * lowz_mod.yyyy );\n" +
                                                        "\t\t	highz_hash_1 = frac( P * highz_mod.yyyy );\n" +
                                                        "\t\t	lowz_hash_2 = frac( P * lowz_mod.zzzz );\n" +
                                                        "\t\t	highz_hash_2 = frac( P * highz_mod.zzzz );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	PerlinSurflet3D_Deriv\n" +
                                                        "\t\t//	Perlin Surflet 3D noise with derivatives\n" +
                                                        "\t\t//	returns float4( value, xderiv, yderiv, zderiv )\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat4 PerlinSurflet3D_Deriv( float3 P )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float3 Pi = floor(P);\n" +
                                                        "\t\t	float3 Pf = P - Pi;\n" +
                                                        "\t\t	float3 Pf_min1 = Pf - 1.0;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	//	( various hashing methods listed in order of speed )\n" +
                                                        "\t\t	float4 hashx0, hashy0, hashz0, hashx1, hashy1, hashz1;\n" +
                                                        "\t\t	FAST32_hash_3D( Pi, hashx0, hashy0, hashz0, hashx1, hashy1, hashz1 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the gradients\n" +
                                                        "\t\t	float4 grad_x0 = hashx0 - 0.49999;\n" +
                                                        "\t\t	float4 grad_y0 = hashy0 - 0.49999;\n" +
                                                        "\t\t	float4 grad_z0 = hashz0 - 0.49999;\n" +
                                                        "\t\t	float4 norm_0 = rsqrt( grad_x0 * grad_x0 + grad_y0 * grad_y0 + grad_z0 * grad_z0 );\n" +
                                                        "\t\t	grad_x0 *= norm_0;\n" +
                                                        "\t\t	grad_y0 *= norm_0;\n" +
                                                        "\t\t	grad_z0 *= norm_0;\n" +
                                                        "\t\t	float4 grad_x1 = hashx1 - 0.49999;\n" +
                                                        "\t\t	float4 grad_y1 = hashy1 - 0.49999;\n" +
                                                        "\t\t	float4 grad_z1 = hashz1 - 0.49999;\n" +
                                                        "\t\t	float4 norm_1 = rsqrt( grad_x1 * grad_x1 + grad_y1 * grad_y1 + grad_z1 * grad_z1 );\n" +
                                                        "\t\t	grad_x1 *= norm_1;\n" +
                                                        "\t\t	grad_y1 *= norm_1;\n" +
                                                        "\t\t	grad_z1 *= norm_1;\n" +
                                                        "\t\t	float4 grad_results_0 = float2( Pf.x, Pf_min1.x ).xyxy * grad_x0 + float2( Pf.y, Pf_min1.y ).xxyy * grad_y0 + Pf.zzzz * grad_z0;\n" +
                                                        "\t\t	float4 grad_results_1 = float2( Pf.x, Pf_min1.x ).xyxy * grad_x1 + float2( Pf.y, Pf_min1.y ).xxyy * grad_y1 + Pf_min1.zzzz * grad_z1;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	get lengths in the x+y plane\n" +
                                                        "\t\t	float3 Pf_sq = Pf*Pf;\n" +
                                                        "\t\t	float3 Pf_min1_sq = Pf_min1*Pf_min1;\n" +
                                                        "\t\t	float4 vecs_len_sq = float2( Pf_sq.x, Pf_min1_sq.x ).xyxy + float2( Pf_sq.y, Pf_min1_sq.y ).xxyy;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	evaluate the surflet\n" +
                                                        "\t\t	float4 m_0 = vecs_len_sq + Pf_sq.zzzz;\n" +
                                                        "\t\t	m_0 = max(1.0 - m_0, 0.0);\n" +
                                                        "\t\t	float4 m2_0 = m_0*m_0;\n" +
                                                        "\t\t	float4 m3_0 = m_0*m2_0;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	float4 m_1 = vecs_len_sq + Pf_min1_sq.zzzz;\n" +
                                                        "\t\t	m_1 = max(1.0 - m_1, 0.0);\n" +
                                                        "\t\t	float4 m2_1 = m_1*m_1;\n" +
                                                        "\t\t	float4 m3_1 = m_1*m2_1;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calc the deriv\n" +
                                                        "\t\t	float4 temp_0 = -6.0 * m2_0 * grad_results_0;\n" +
                                                        "\t\t	float xderiv_0 = dot( temp_0, float2( Pf.x, Pf_min1.x ).xyxy ) + dot( m3_0, grad_x0 );\n" +
                                                        "\t\t	float yderiv_0 = dot( temp_0, float2( Pf.y, Pf_min1.y ).xxyy ) + dot( m3_0, grad_y0 );\n" +
                                                        "\t\t	float zderiv_0 = dot( temp_0, Pf.zzzz ) + dot( m3_0, grad_z0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	float4 temp_1 = -6.0 * m2_1 * grad_results_1;\n" +
                                                        "\t\t	float xderiv_1 = dot( temp_1, float2( Pf.x, Pf_min1.x ).xyxy ) + dot( m3_1, grad_x1 );\n" +
                                                        "\t\t	float yderiv_1 = dot( temp_1, float2( Pf.y, Pf_min1.y ).xxyy ) + dot( m3_1, grad_y1 );\n" +
                                                        "\t\t	float zderiv_1 = dot( temp_1, Pf_min1.zzzz ) + dot( m3_1, grad_z1 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	const float FINAL_NORMALIZATION = 2.3703703703703703703703703703704;	//	scales the final result to a strict 1.0->-1.0 range\n" +
                                                        "\t\t	return float4( dot( m3_0, grad_results_0 ) + dot( m3_1, grad_results_1 ) , float3(xderiv_0,yderiv_0,zderiv_0) + float3(xderiv_1,yderiv_1,zderiv_1) ) * FINAL_NORMALIZATION;\n" +
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
                                                        "\t\tvoid FAST32_hash_3D( 	float3 gridcell,\n" +
                                                        "\t\t						float3 v1_mask,		//	user definable v1 and v2.  ( 0's and 1's )\n" +
                                                        "\t\t						float3 v2_mask,\n" +
                                                        "\t\t						out float4 hash_0,\n" +
                                                        "\t\t						out float4 hash_1,\n" +
                                                        "\t\t						out float4 hash_2	)		//	generates 3 random numbers for each of the 4 3D cell corners.  cell corners:  v0=0,0,0  v3=1,1,1  the other two are user definable\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float3 SOMELARGEFLOATS = float3( 635.298681, 682.357502, 668.926525 );\n" +
                                                        "\t\t	const float3 ZINC = float3( 48.500388, 65.294118, 63.934599 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	float3 gridcell_inc1 = step( gridcell, float3( DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5) ) * ( gridcell + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	compute x*x*y*y for the 4 corners\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell_inc1.xy ) + OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	float4 V1xy_V2xy = lerp( P.xyxy, P.zwzw, float4( v1_mask.xy, v2_mask.xy ) );		//	apply mask for v1 and v2\n" +
                                                        "\t\t	P = float4( P.x, V1xy_V2xy.xz, P.z ) * float4( P.y, V1xy_V2xy.yw, P.w );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	get the lowz and highz mods\n" +
                                                        "\t\t	float3 lowz_mods = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	float3 highz_mods = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell_inc1.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	apply mask for v1 and v2 mod values\n" +
                                                        "\t\t    v1_mask = ( v1_mask.z < 0.5 ) ? lowz_mods : highz_mods;\n" +
                                                        "\t\t    v2_mask = ( v2_mask.z < 0.5 ) ? lowz_mods : highz_mods;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	compute the final hash\n" +
                                                        "\t\t	hash_0 = frac( P * float4( lowz_mods.x, v1_mask.x, v2_mask.x, highz_mods.x ) );\n" +
                                                        "\t\t	hash_1 = frac( P * float4( lowz_mods.y, v1_mask.y, v2_mask.y, highz_mods.y ) );\n" +
                                                        "\t\t	hash_2 = frac( P * float4( lowz_mods.z, v1_mask.z, v2_mask.z, highz_mods.z ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Given an arbitrary 3D point this calculates the 4 vectors from the corners of the simplex pyramid to the point\n" +
                                                        "\t\t//	It also returns the integer grid index information for the corners\n" +
                                                        "\t\t//\n" +
                                                        "\t\tvoid Simplex3D_GetCornerVectors( 	float3 P,					//	input point\n" +
                                                        "\t\t									out float3 Pi,			//	integer grid index for the origin\n" +
                                                        "\t\t									out float3 Pi_1,			//	offsets for the 2nd and 3rd corners.  ( the 4th = Pi + 1.0 )\n" +
                                                        "\t\t									out float3 Pi_2,\n" +
                                                        "\t\t									out float4 v1234_x,		//	vectors from the 4 corners to the intput point\n" +
                                                        "\t\t									out float4 v1234_y,\n" +
                                                        "\t\t									out float4 v1234_z )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//\n" +
                                                        "\t\t	//	Simplex math from Stefan Gustavson's and Ian McEwan's work at...\n" +
                                                        "\t\t	//	http://github.com/ashima/webgl-noise\n" +
                                                        "\t\t	//\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	simplex math constants\n" +
                                                        "\t\t	const float SKEWFACTOR = 1.0/3.0;\n" +
                                                        "\t\t	const float UNSKEWFACTOR = 1.0/6.0;\n" +
                                                        "\t\t	const float SIMPLEX_CORNER_POS = 0.5;\n" +
                                                        "\t\t	const float SIMPLEX_PYRAMID_HEIGHT = 0.70710678118654752440084436210485;	// sqrt( 0.5 )	height of simplex pyramid.\n" +
                                                        "\t\t\n" +
                                                        "\t\t	P *= SIMPLEX_PYRAMID_HEIGHT;		// scale space so we can have an approx feature size of 1.0  ( optional )\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	Find the vectors to the corners of our simplex pyramid\n" +
                                                        "\t\t	Pi = floor( P + dot( P, float3( SKEWFACTOR, SKEWFACTOR, SKEWFACTOR) ) );\n" +
                                                        "\t\t	float3 x0 = P - Pi + dot(Pi, float3( UNSKEWFACTOR, UNSKEWFACTOR, UNSKEWFACTOR ) );\n" +
                                                        "\t\t	float3 g = step(x0.yzx, x0.xyz);\n" +
                                                        "\t\t	float3 l = 1.0 - g;\n" +
                                                        "\t\t	Pi_1 = min( g.xyz, l.zxy );\n" +
                                                        "\t\t	Pi_2 = max( g.xyz, l.zxy );\n" +
                                                        "\t\t	float3 x1 = x0 - Pi_1 + UNSKEWFACTOR;\n" +
                                                        "\t\t	float3 x2 = x0 - Pi_2 + SKEWFACTOR;\n" +
                                                        "\t\t	float3 x3 = x0 - SIMPLEX_CORNER_POS;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	pack them into a parallel-friendly arrangement\n" +
                                                        "\t\t	v1234_x = float4( x0.x, x1.x, x2.x, x3.x );\n" +
                                                        "\t\t	v1234_y = float4( x0.y, x1.y, x2.y, x3.y );\n" +
                                                        "\t\t	v1234_z = float4( x0.z, x1.z, x2.z, x3.z );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Calculate the weights for the 3D simplex surflet\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat4 Simplex3D_GetSurfletWeights( 	float4 v1234_x,\n" +
                                                        "\t\t									float4 v1234_y,\n" +
                                                        "\t\t									float4 v1234_z )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	perlins original implementation uses the surlet falloff formula of (0.6-x*x)^4.\n" +
                                                        "\t\t	//	This is buggy as it can cause discontinuities along simplex faces.  (0.5-x*x)^3 solves this and gives an almost identical curve\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	evaluate surflet. f(x)=(0.5-x*x)^3\n" +
                                                        "\t\t	float4 surflet_weights = v1234_x * v1234_x + v1234_y * v1234_y + v1234_z * v1234_z;\n" +
                                                        "\t\t	surflet_weights = max(0.5 - surflet_weights, 0.0);		//	0.5 here represents the closest distance (squared) of any simplex pyramid corner to any of its planes.  ie, SIMPLEX_PYRAMID_HEIGHT^2\n" +
                                                        "\t\t	return surflet_weights*surflet_weights*surflet_weights;\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	SimplexPerlin3D  ( simplex gradient noise )\n" +
                                                        "\t\t//	Perlin noise over a simplex (tetrahedron) grid\n" +
                                                        "\t\t//	Return value range of -1.0->1.0\n" +
                                                        "\t\t//	http://briansharpe.files.wordpress.com/2012/01/simplexperlinsample.jpg\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Implementation originally based off Stefan Gustavson's and Ian McEwan's work at...\n" +
                                                        "\t\t//	http://github.com/ashima/webgl-noise\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat SimplexPerlin3D(float3 P)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	calculate the simplex vector and index math\n" +
                                                        "\t\t	float3 Pi;\n" +
                                                        "\t\t	float3 Pi_1;\n" +
                                                        "\t\t	float3 Pi_2;\n" +
                                                        "\t\t	float4 v1234_x;\n" +
                                                        "\t\t	float4 v1234_y;\n" +
                                                        "\t\t	float4 v1234_z;\n" +
                                                        "\t\t	Simplex3D_GetCornerVectors( P, Pi, Pi_1, Pi_2, v1234_x, v1234_y, v1234_z );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	generate the random vectors\n" +
                                                        "\t\t	//	( various hashing methods listed in order of speed )\n" +
                                                        "\t\t	float4 hash_0;\n" +
                                                        "\t\t	float4 hash_1;\n" +
                                                        "\t\t	float4 hash_2;\n" +
                                                        "\t\t	FAST32_hash_3D( Pi, Pi_1, Pi_2, hash_0, hash_1, hash_2 );\n" +
                                                        "\t\t	hash_0 -= 0.49999;\n" +
                                                        "\t\t	hash_1 -= 0.49999;\n" +
                                                        "\t\t	hash_2 -= 0.49999;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	evaluate gradients\n" +
                                                        "\t\t	float4 grad_results = rsqrt( hash_0 * hash_0 + hash_1 * hash_1 + hash_2 * hash_2 ) * ( hash_0 * v1234_x + hash_1 * v1234_y + hash_2 * v1234_z );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	Normalization factor to scale the final result to a strict 1.0->-1.0 range\n" +
                                                        "\t\t	//	x = sqrt( 0.75 ) * 0.5\n" +
                                                        "\t\t	//	NF = 1.0 / ( x * ( ( 0.5 ? x*x ) ^ 3 ) * 2.0 )\n" +
                                                        "\t\t	//	http://briansharpe.wordpress.com/2012/01/13/simplex-noise/#comment-36\n" +
                                                        "\t\t	const float FINAL_NORMALIZATION = 37.837227241611314102871574478976;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	sum with the surflet and return\n" +
                                                        "\t\t	return dot( Simplex3D_GetSurfletWeights( v1234_x, v1234_y, v1234_z ), grad_results ) * FINAL_NORMALIZATION;\n" +
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
                                                        "\t\tvoid FAST32_hash_3D( 	float3 gridcell,\n" +
                                                        "\t\t						float3 v1_mask,		//	user definable v1 and v2.  ( 0's and 1's )\n" +
                                                        "\t\t						float3 v2_mask,\n" +
                                                        "\t\t						out float4 hash_0,\n" +
                                                        "\t\t						out float4 hash_1,\n" +
                                                        "\t\t						out float4 hash_2	)		//	generates 3 random numbers for each of the 4 3D cell corners.  cell corners:  v0=0,0,0  v3=1,1,1  the other two are user definable\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float3 SOMELARGEFLOATS = float3( 635.298681, 682.357502, 668.926525 );\n" +
                                                        "\t\t	const float3 ZINC = float3( 48.500388, 65.294118, 63.934599 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	float3 gridcell_inc1 = step( gridcell, float3( DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5) ) * ( gridcell + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	compute x*x*y*y for the 4 corners\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell_inc1.xy ) + OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	float4 V1xy_V2xy = lerp( P.xyxy, P.zwzw, float4( v1_mask.xy, v2_mask.xy ) );		//	apply mask for v1 and v2\n" +
                                                        "\t\t	P = float4( P.x, V1xy_V2xy.xz, P.z ) * float4( P.y, V1xy_V2xy.yw, P.w );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	get the lowz and highz mods\n" +
                                                        "\t\t	float3 lowz_mods = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	float3 highz_mods = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell_inc1.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	apply mask for v1 and v2 mod values\n" +
                                                        "\t\t    v1_mask = ( v1_mask.z < 0.5 ) ? lowz_mods : highz_mods;\n" +
                                                        "\t\t    v2_mask = ( v2_mask.z < 0.5 ) ? lowz_mods : highz_mods;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	compute the final hash\n" +
                                                        "\t\t	hash_0 = frac( P * float4( lowz_mods.x, v1_mask.x, v2_mask.x, highz_mods.x ) );\n" +
                                                        "\t\t	hash_1 = frac( P * float4( lowz_mods.y, v1_mask.y, v2_mask.y, highz_mods.y ) );\n" +
                                                        "\t\t	hash_2 = frac( P * float4( lowz_mods.z, v1_mask.z, v2_mask.z, highz_mods.z ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Given an arbitrary 3D point this calculates the 4 vectors from the corners of the simplex pyramid to the point\n" +
                                                        "\t\t//	It also returns the integer grid index information for the corners\n" +
                                                        "\t\t//\n" +
                                                        "\t\tvoid Simplex3D_GetCornerVectors( 	float3 P,					//	input point\n" +
                                                        "\t\t									out float3 Pi,			//	integer grid index for the origin\n" +
                                                        "\t\t									out float3 Pi_1,			//	offsets for the 2nd and 3rd corners.  ( the 4th = Pi + 1.0 )\n" +
                                                        "\t\t									out float3 Pi_2,\n" +
                                                        "\t\t									out float4 v1234_x,		//	vectors from the 4 corners to the intput point\n" +
                                                        "\t\t									out float4 v1234_y,\n" +
                                                        "\t\t									out float4 v1234_z )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//\n" +
                                                        "\t\t	//	Simplex math from Stefan Gustavson's and Ian McEwan's work at...\n" +
                                                        "\t\t	//	http://github.com/ashima/webgl-noise\n" +
                                                        "\t\t	//\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	simplex math constants\n" +
                                                        "\t\t	const float SKEWFACTOR = 1.0/3.0;\n" +
                                                        "\t\t	const float UNSKEWFACTOR = 1.0/6.0;\n" +
                                                        "\t\t	const float SIMPLEX_CORNER_POS = 0.5;\n" +
                                                        "\t\t	const float SIMPLEX_PYRAMID_HEIGHT = 0.70710678118654752440084436210485;	// sqrt( 0.5 )	height of simplex pyramid.\n" +
                                                        "\t\t\n" +
                                                        "\t\t	P *= SIMPLEX_PYRAMID_HEIGHT;		// scale space so we can have an approx feature size of 1.0  ( optional )\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	Find the vectors to the corners of our simplex pyramid\n" +
                                                        "\t\t	Pi = floor( P + dot( P, float3( SKEWFACTOR, SKEWFACTOR, SKEWFACTOR) ) );\n" +
                                                        "\t\t	float3 x0 = P - Pi + dot(Pi, float3( UNSKEWFACTOR, UNSKEWFACTOR, UNSKEWFACTOR ) );\n" +
                                                        "\t\t	float3 g = step(x0.yzx, x0.xyz);\n" +
                                                        "\t\t	float3 l = 1.0 - g;\n" +
                                                        "\t\t	Pi_1 = min( g.xyz, l.zxy );\n" +
                                                        "\t\t	Pi_2 = max( g.xyz, l.zxy );\n" +
                                                        "\t\t	float3 x1 = x0 - Pi_1 + UNSKEWFACTOR;\n" +
                                                        "\t\t	float3 x2 = x0 - Pi_2 + SKEWFACTOR;\n" +
                                                        "\t\t	float3 x3 = x0 - SIMPLEX_CORNER_POS;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	pack them into a parallel-friendly arrangement\n" +
                                                        "\t\t	v1234_x = float4( x0.x, x1.x, x2.x, x3.x );\n" +
                                                        "\t\t	v1234_y = float4( x0.y, x1.y, x2.y, x3.y );\n" +
                                                        "\t\t	v1234_z = float4( x0.z, x1.z, x2.z, x3.z );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	SimplexPerlin3D_Deriv\n" +
                                                        "\t\t//	SimplexPerlin3D noise with derivatives\n" +
                                                        "\t\t//	returns float3( value, xderiv, yderiv, zderiv )\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat4 SimplexPerlin3D_Deriv(float3 P)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	calculate the simplex vector and index math\n" +
                                                        "\t\t	float3 Pi;\n" +
                                                        "\t\t	float3 Pi_1;\n" +
                                                        "\t\t	float3 Pi_2;\n" +
                                                        "\t\t	float4 v1234_x;\n" +
                                                        "\t\t	float4 v1234_y;\n" +
                                                        "\t\t	float4 v1234_z;\n" +
                                                        "\t\t	Simplex3D_GetCornerVectors( P, Pi, Pi_1, Pi_2, v1234_x, v1234_y, v1234_z );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	generate the random vectors\n" +
                                                        "\t\t	//	( various hashing methods listed in order of speed )\n" +
                                                        "\t\t	float4 hash_0;\n" +
                                                        "\t\t	float4 hash_1;\n" +
                                                        "\t\t	float4 hash_2;\n" +
                                                        "\t\t	FAST32_hash_3D( Pi, Pi_1, Pi_2, hash_0, hash_1, hash_2 );\n" +
                                                        "\t\t	hash_0 -= 0.49999;\n" +
                                                        "\t\t	hash_1 -= 0.49999;\n" +
                                                        "\t\t	hash_2 -= 0.49999;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	normalize random gradient vectors\n" +
                                                        "\t\t	float4 norm = rsqrt( hash_0 * hash_0 + hash_1 * hash_1 + hash_2 * hash_2 );\n" +
                                                        "\t\t	hash_0 *= norm;\n" +
                                                        "\t\t	hash_1 *= norm;\n" +
                                                        "\t\t	hash_2 *= norm;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	evaluate gradients\n" +
                                                        "\t\t	float4 grad_results = hash_0 * v1234_x + hash_1 * v1234_y + hash_2 * v1234_z;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	evaluate the surflet f(x)=(0.5-x*x)^3\n" +
                                                        "\t\t	float4 m = v1234_x * v1234_x + v1234_y * v1234_y + v1234_z * v1234_z;\n" +
                                                        "\t\t	m = max(0.5 - m, 0.0);		//	The 0.5 here is SIMPLEX_PYRAMID_HEIGHT^2\n" +
                                                        "\t\t	float4 m2 = m*m;\n" +
                                                        "\t\t	float4 m3 = m*m2;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calc the deriv\n" +
                                                        "\t\t	float4 temp = -6.0 * m2 * grad_results;\n" +
                                                        "\t\t	float xderiv = dot( temp, v1234_x ) + dot( m3, hash_0 );\n" +
                                                        "\t\t	float yderiv = dot( temp, v1234_y ) + dot( m3, hash_1 );\n" +
                                                        "\t\t	float zderiv = dot( temp, v1234_z ) + dot( m3, hash_2 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	const float FINAL_NORMALIZATION = 37.837227241611314102871574478976;	//	scales the final result to a strict 1.0->-1.0 range\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	sum with the surflet and return\n" +
                                                        "\t\t	return float4( dot( m3, grad_results ), xderiv, yderiv, zderiv ) * FINAL_NORMALIZATION;\n" +
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
                                                        "\t\tfloat4 FAST32_hash_3D_Cell( float3 gridcell )	//	generates 4 different random numbers for the single given cell point\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float4 SOMELARGEFLOATS = float4( 635.298681, 682.357502, 668.926525, 588.255119 );\n" +
                                                        "\t\t	const float4 ZINC = float4( 48.500388, 65.294118, 63.934599, 63.279683 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell - floor(gridcell * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	gridcell.xy += OFFSET.xy;\n" +
                                                        "\t\t	gridcell.xy *= gridcell.xy;\n" +
                                                        "\t\t	return frac( ( gridcell.x * gridcell.y ) * ( 1.0 / ( SOMELARGEFLOATS + gridcell.zzzz * ZINC ) ) );\n" +
                                                        "\t\t}\n" +
                                                        "\t\tstatic const int MinVal = -1;\n" +
                                                        "\t\tstatic const int MaxVal = 1;\n" +
                                                        "\t\tfloat Cellular3D(float3 xyz, int cellType, int distanceFunction) \n" +
                                                        "\t\t{\n" +
                                                        "\t\t	int xi = int(floor(xyz.x));\n" +
                                                        "\t\t	int yi = int(floor(xyz.y));\n" +
                                                        "\t\t	int zi = int(floor(xyz.z));\n" +
                                                        "\t\t \n" +
                                                        "\t\t	float xf = xyz.x - float(xi);\n" +
                                                        "\t\t	float yf = xyz.y - float(yi);\n" +
                                                        "\t\t	float zf = xyz.z - float(zi);\n" +
                                                        "\t\t \n" +
                                                        "\t\t	float dist1 = 9999999.0;\n" +
                                                        "\t\t	float dist2 = 9999999.0;\n" +
                                                        "\t\t	float dist3 = 9999999.0;\n" +
                                                        "\t\t	float dist4 = 9999999.0;\n" +
                                                        "\t\t	float3 cell;\n" +
                                                        "\t\t \n" +
                                                        "\t\t	for (int z = MinVal; z <= MaxVal; z++) {\n" +
                                                        "\t\t		for (int y = MinVal; y <= MaxVal; y++) {\n" +
                                                        "\t\t			for (int x = MinVal; x <= MaxVal; x++) {\n" +
                                                        "\t\t				cell = FAST32_hash_3D_Cell(float3(xi + x, yi + y, zi + z)).xyz;\n" +
                                                        "\t\t				cell.x += (float(x) - xf);\n" +
                                                        "\t\t				cell.y += (float(y) - yf);\n" +
                                                        "\t\t				cell.z += (float(z) - zf);\n" +
                                                        "\t\t				float dist = 0.0;\n" +
                                                        "\t\t				if(distanceFunction <= 1)\n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist = sqrt(dot(cell, cell));\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if(distanceFunction > 1 && distanceFunction <= 2)\n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist = dot(cell, cell);\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if(distanceFunction > 2 && distanceFunction <= 3)\n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist = abs(cell.x) + abs(cell.y) + abs(cell.z);\n" +
                                                        "\t\t					dist *= dist;\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if(distanceFunction > 3 && distanceFunction <= 4)\n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist = max(abs(cell.x), max(abs(cell.y), abs(cell.z)));\n" +
                                                        "\t\t					dist *= dist;\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if(distanceFunction > 4 && distanceFunction <= 5)\n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist = dot(cell, cell) + cell.x*cell.y + cell.x*cell.z + cell.y*cell.z;	\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if(distanceFunction > 5 && distanceFunction <= 6)\n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist = pow(abs(cell.x*cell.x*cell.x*cell.x + cell.y*cell.y*cell.y*cell.y + cell.z*cell.z*cell.z*cell.z), 0.25);\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if(distanceFunction > 6 && distanceFunction <= 7)\n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist = sqrt(abs(cell.x)) + sqrt(abs(cell.y)) + sqrt(abs(cell.z));\n" +
                                                        "\t\t					dist *= dist;\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				if (dist < dist1) \n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist4 = dist3;\n" +
                                                        "\t\t					dist3 = dist2;\n" +
                                                        "\t\t					dist2 = dist1;\n" +
                                                        "\t\t					dist1 = dist;\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if (dist < dist2) \n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist4 = dist3;\n" +
                                                        "\t\t					dist3 = dist2;\n" +
                                                        "\t\t					dist2 = dist;\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if (dist < dist3) \n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist4 = dist3;\n" +
                                                        "\t\t					dist3 = dist;\n" +
                                                        "\t\t				}\n" +
                                                        "\t\t				else if (dist < dist4) \n" +
                                                        "\t\t				{\n" +
                                                        "\t\t					dist4 = dist;\n" +
                                                        "\t\t				}\n" +
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
    public const string IncludeCellFast =               "\t\t//\n" +
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
                                                        "\t\t   void FAST32_hash_3D( 	float3 gridcell,\n" +
                                                        "\t\t						out float4 lowz_hash_0,\n" +
                                                        "\t\t						out float4 lowz_hash_1,\n" +
                                                        "\t\t						out float4 lowz_hash_2,\n" +
                                                        "\t\t						out float4 highz_hash_0,\n" +
                                                        "\t\t						out float4 highz_hash_1,\n" +
                                                        "\t\t						out float4 highz_hash_2	)		//	generates 3 random numbers for each of the 8 cell corners\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//    gridcell is assumed to be an integer coordinate\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	TODO: 	these constants need tweaked to find the best possible noise.\n" +
                                                        "\t\t	//			probably requires some kind of brute force computational searching or something....\n" +
                                                        "\t\t	const float2 OFFSET = float2( 50.0, 161.0 );\n" +
                                                        "\t\t	const float DOMAIN = 69.0;\n" +
                                                        "\t\t	const float3 SOMELARGEFLOATS = float3( 635.298681, 682.357502, 668.926525 );\n" +
                                                        "\t\t	const float3 ZINC = float3( 48.500388, 65.294118, 63.934599 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	truncate the domain\n" +
                                                        "\t\t	gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * ( 1.0 / DOMAIN )) * DOMAIN;\n" +
                                                        "\t\t	float3 gridcell_inc1 = step( gridcell, float3( DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5 ) ) * ( gridcell + 1.0 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the noise\n" +
                                                        "\t\t	float4 P = float4( gridcell.xy, gridcell_inc1.xy ) + OFFSET.xyxy;\n" +
                                                        "\t\t	P *= P;\n" +
                                                        "\t\t	P = P.xzxz * P.yyww;\n" +
                                                        "\t\t	float3 lowz_mod = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	float3 highz_mod = float3( 1.0 / ( SOMELARGEFLOATS.xyz + gridcell_inc1.zzz * ZINC.xyz ) );\n" +
                                                        "\t\t	lowz_hash_0 = frac( P * lowz_mod.xxxx );\n" +
                                                        "\t\t	highz_hash_0 = frac( P * highz_mod.xxxx );\n" +
                                                        "\t\t	lowz_hash_1 = frac( P * lowz_mod.yyyy );\n" +
                                                        "\t\t	highz_hash_1 = frac( P * highz_mod.yyyy );\n" +
                                                        "\t\t	lowz_hash_2 = frac( P * lowz_mod.zzzz );\n" +
                                                        "\t\t	highz_hash_2 = frac( P * highz_mod.zzzz );\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//	convert a 0.0->1.0 sample to a -1.0->1.0 sample weighted towards the extremes\n" +
                                                        "\t\tfloat4 Cellular_weight_samples( float4 samples )\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	samples = samples * 2.0 - 1.0;\n" +
                                                        "\t\t	//return (1.0 - samples * samples) * sign(samples);	// square\n" +
                                                        "\t\t	return (samples * samples * samples) - sign(samples);	// cubic (even more variance)\n" +
                                                        "\t\t}\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Cellular Noise 3D\n" +
                                                        "\t\t//	Based off Stefan Gustavson's work at http://www.itn.liu.se/~stegu/GLSL-cellular\n" +
                                                        "\t\t//	http://briansharpe.files.wordpress.com/2011/12/cellularsample.jpg\n" +
                                                        "\t\t//\n" +
                                                        "\t\t//	Speed up by using 2x2x2 search window instead of 3x3x3\n" +
                                                        "\t\t//	produces range of 0.0->1.0\n" +
                                                        "\t\t//\n" +
                                                        "\t\tfloat Cellular3D(float3 P)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	//	establish our grid cell and unit position\n" +
                                                        "\t\t	float3 Pi = floor(P);\n" +
                                                        "\t\t	float3 Pf = P - Pi;\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	calculate the hash.\n" +
                                                        "\t\t	//	( various hashing methods listed in order of speed )\n" +
                                                        "\t\t	float4 hash_x0, hash_y0, hash_z0, hash_x1, hash_y1, hash_z1;\n" +
                                                        "\t\t	FAST32_hash_3D( Pi, hash_x0, hash_y0, hash_z0, hash_x1, hash_y1, hash_z1 );\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	generate the 8 random points\n" +
                                                        "\t\t	//	restrict the random point offset to eliminate artifacts\n" +
                                                        "\t\t	//	we'll improve the variance of the noise by pushing the points to the extremes of the jitter window\n" +
                                                        "\t\t	const float JITTER_WINDOW = 0.166666666;	// 0.166666666 will guarentee no artifacts. It is the intersection on x of graphs f(x)=( (0.5 + (0.5-x))^2 + 2*((0.5-x)^2) ) and f(x)=( 2 * (( 0.5 + x )^2) + x * x )\n" +
                                                        "\t\t	hash_x0 = Cellular_weight_samples( hash_x0 ) * JITTER_WINDOW + float4(0.0, 1.0, 0.0, 1.0);\n" +
                                                        "\t\t	hash_y0 = Cellular_weight_samples( hash_y0 ) * JITTER_WINDOW + float4(0.0, 0.0, 1.0, 1.0);\n" +
                                                        "\t\t	hash_x1 = Cellular_weight_samples( hash_x1 ) * JITTER_WINDOW + float4(0.0, 1.0, 0.0, 1.0);\n" +
                                                        "\t\t	hash_y1 = Cellular_weight_samples( hash_y1 ) * JITTER_WINDOW + float4(0.0, 0.0, 1.0, 1.0);\n" +
                                                        "\t\t	hash_z0 = Cellular_weight_samples( hash_z0 ) * JITTER_WINDOW + float4(0.0, 0.0, 0.0, 0.0);\n" +
                                                        "\t\t	hash_z1 = Cellular_weight_samples( hash_z1 ) * JITTER_WINDOW + float4(1.0, 1.0, 1.0, 1.0);\n" +
                                                        "\t\t\n" +
                                                        "\t\t	//	return the closest squared distance\n" +
                                                        "\t\t	float4 dx1 = Pf.xxxx - hash_x0;\n" +
                                                        "\t\t	float4 dy1 = Pf.yyyy - hash_y0;\n" +
                                                        "\t\t	float4 dz1 = Pf.zzzz - hash_z0;\n" +
                                                        "\t\t	float4 dx2 = Pf.xxxx - hash_x1;\n" +
                                                        "\t\t	float4 dy2 = Pf.yyyy - hash_y1;\n" +
                                                        "\t\t	float4 dz2 = Pf.zzzz - hash_z1;\n" +
                                                        "\t\t	float4 d1 = dx1 * dx1 + dy1 * dy1 + dz1 * dz1;\n" +
                                                        "\t\t	float4 d2 = dx2 * dx2 + dy2 * dy2 + dz2 * dz2;\n" +
                                                        "\t\t	d1 = min(d1, d2);\n" +
                                                        "\t\t	d1.xy = min(d1.xy, d1.wz);\n" +
                                                        "\t\t	return min(d1.x, d1.y) * ( 9.0 / 12.0 );	//	scale return value from 0.0->1.333333 to 0.0->1.0  	(2/3)^2 * 3  == (12/9) == 1.333333\n" +
                                                        "\t\t}\n";

    public const string IncludeFbmValueNormal =         "\t\tfloat ValueNormal(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = Value3D((p + offset) * frequency);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmValueBillowed =       "\t\tfloat ValueBillowed(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = abs(Value3D((p + offset) * frequency));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmValueRidged = "\t\tfloat ValueRidged(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = 0.5 * (ridgeOffset - abs(4*Value3D((p + offset) * frequency)));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmValueDerivedIQ = "\t\tfloat ValueDerivedIQ(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t   float sum = 0;\n" +
                                                        "\t\t   float3 dsum = float3(0.0, 0.0, 0.0);\n" +
                                                        "\t\t   for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t   {\n" +
                                                        "\t\t	    float4 n = Value3D_Deriv((p + offset) * frequency);\n" +
                                                        "\t\t	    dsum += n.yzw;\n" +
                                                        "\t\t	    sum += amplitude * n.x / (1 + dot(dsum, dsum));\n" +
                                                        "\t\t	    frequency *= lacunarity;\n" +
                                                        "\t\t	    amplitude *= persistence;\n" +
                                                        "\t\t   }\n" +
                                                        "\t\t   return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmValueDerivedSwiss =   "\t\tfloat ValueDerivedSwiss(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0.0;\n" +
                                                        "\t\t	float3 dsum = float3(0.0, 0.0, 0.0);\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float4 n = 0.5 * (0 + (ridgeOffset - abs(Value3D_Deriv((p+offset+warp*dsum)*frequency))));\n" +
                                                        "\t\t		sum += amplitude * n.x;\n" +
                                                        "\t\t		dsum += amplitude * n.yzw * -n.x;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmValueDerivedJordan = "\t\tfloat ValueDerivedJordan(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp0, float warp, float damp0, float damp, float damp_scale)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float4 n = Value3D_Deriv((p+offset)*frequency);\n" +
                                                        "\t\t	float4 n2 = n * n.x;\n" +
                                                        "\t\t   float sum = n2.x;\n" +
                                                        "\t\t   float3 dsum_warp = warp0*n2.yzw;\n" +
                                                        "\t\t   float3 dsum_damp = damp0*n2.yzw;\n" +
                                                        "\t\t   float damped_amp = amplitude * persistence;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		n = Value3D_Deriv((p+offset)*frequency+dsum_warp.xyz);\n" +
                                                        "\t\t		n2 = n * n.x;\n" +
                                                        "\t\t       sum += damped_amp * n2.x;\n" +
                                                        "\t\t       dsum_warp += warp * n2.yzw;\n" +
                                                        "\t\t       dsum_damp += damp * n2.yzw;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t		damped_amp = amplitude * (1-damp_scale/(1+dot(dsum_damp,dsum_damp)));\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmPerlinNormal =        "\t\tfloat PerlinNormal(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = Perlin3D((p + offset) * frequency);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinBillowed =      "\t\tfloat PerlinBillowed(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = abs(Perlin3D((p + offset) * frequency));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinRidged =        "\t\tfloat PerlinRidged(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = 0.5 * (ridgeOffset - abs(4*Perlin3D((p + offset) * frequency)));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinDerivedIQ = "\t\tfloat PerlinDerivedIQ(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t   float sum = 0;\n" +
                                                        "\t\t   float3 dsum = float3(0.0, 0.0, 0.0);\n" +
                                                        "\t\t   for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t   {\n" +
                                                        "\t\t	    float4 n = PerlinSurflet3D_Deriv((p + offset) * frequency);\n" +
                                                        "\t\t	    dsum += n.yzw;\n" +
                                                        "\t\t	    sum += amplitude * n.x / (1 + dot(dsum, dsum));\n" +
                                                        "\t\t	    frequency *= lacunarity;\n" +
                                                        "\t\t	    amplitude *= persistence;\n" +
                                                        "\t\t   }\n" +
                                                        "\t\t   return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmPerlinDerivedSwiss = "\t\tfloat PerlinDerivedSwiss(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0.0;\n" +
                                                        "\t\t	float3 dsum = float3(0.0, 0.0, 0.0);\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float4 n = 0.5 * (0 + (ridgeOffset - abs(PerlinSurflet3D_Deriv((p+offset+warp*dsum)*frequency))));\n" +
                                                        "\t\t		sum += amplitude * n.x;\n" +
                                                        "\t\t		dsum += amplitude * n.yzw * -n.x;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmPerlinDerivedJordan = "\t\tfloat PerlinDerivedJordan(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp0, float warp, float damp0, float damp, float damp_scale)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float4 n = PerlinSurflet3D_Deriv((p+offset)*frequency);\n" +
                                                        "\t\t	float4 n2 = n * n.x;\n" +
                                                        "\t\t   float sum = n2.x;\n" +
                                                        "\t\t   float3 dsum_warp = warp0*n2.yzw;\n" +
                                                        "\t\t   float3 dsum_damp = damp0*n2.yzw;\n" +
                                                        "\t\t   float damped_amp = amplitude * persistence;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		n = PerlinSurflet3D_Deriv((p+offset)*frequency+dsum_warp.xyz);\n" +
                                                        "\t\t		n2 = n * n.x;\n" +
                                                        "\t\t       sum += damped_amp * n2.x;\n" +
                                                        "\t\t       dsum_warp += warp * n2.yzw;\n" +
                                                        "\t\t       dsum_damp += damp * n2.yzw;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t		damped_amp = amplitude * (1-damp_scale/(1+dot(dsum_damp,dsum_damp)));\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmSimplexNormal = "\t\tfloat SimplexNormal(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = SimplexPerlin3D((p + offset) * frequency);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexBillowed = "\t\tfloat SimplexBillowed(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = abs(SimplexPerlin3D((p + offset) * frequency));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexRidged = "\t\tfloat SimplexRidged(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = 0.5 * (ridgeOffset - abs(4*SimplexPerlin3D((p + offset) * frequency)));\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexDerivedIQ = "\t\tfloat SimplexDerivedIQ(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t   float sum = 0;\n" +
                                                        "\t\t   float3 dsum = float3(0.0, 0.0, 0.0);\n" +
                                                        "\t\t   for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t   {\n" +
                                                        "\t\t	    float4 n = SimplexPerlin3D_Deriv((p + offset) * frequency);\n" +
                                                        "\t\t	    dsum += n.yzw;\n" +
                                                        "\t\t	    sum += amplitude * n.x / (1 + dot(dsum, dsum));\n" +
                                                        "\t\t	    frequency *= lacunarity;\n" +
                                                        "\t\t	    amplitude *= persistence;\n" +
                                                        "\t\t   }\n" +
                                                        "\t\t   return sum;\n" +
                                                        "\t\t}\n";
    public const string IncludeFbmSimplexDerivedSwiss = "\t\tfloat SimplexDerivedSwiss(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp, float ridgeOffset)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0.0;\n" +
                                                        "\t\t	float3 dsum = float3(0.0, 0.0, 0.0);\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float4 n = 0.5 * (0 + (ridgeOffset - abs(SimplexPerlin3D_Deriv((p+offset+warp*dsum)*frequency))));\n" +
                                                        "\t\t		sum += amplitude * n.x;\n" +
                                                        "\t\t		dsum += amplitude * n.yzw * -n.x;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmSimplexDerivedJordan = "\t\tfloat SimplexDerivedJordan(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, float warp0, float warp, float damp0, float damp, float damp_scale)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float4 n = SimplexPerlin3D_Deriv((p+offset)*frequency);\n" +
                                                        "\t\t	float4 n2 = n * n.x;\n" +
                                                        "\t\t   float sum = n2.x;\n" +
                                                        "\t\t   float3 dsum_warp = warp0*n2.yzw;\n" +
                                                        "\t\t   float3 dsum_damp = damp0*n2.yzw;\n" +
                                                        "\t\t   float damped_amp = amplitude * persistence;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		n = SimplexPerlin3D_Deriv((p+offset)*frequency+dsum_warp.xyz);\n" +
                                                        "\t\t		n2 = n * n.x;\n" +
                                                        "\t\t       sum += damped_amp * n2.x;\n" +
                                                        "\t\t       dsum_warp += warp * n2.yzw;\n" +
                                                        "\t\t       dsum_damp += damp * n2.yzw;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence * saturate(sum);\n" +
                                                        "\t\t		damped_amp = amplitude * (1-damp_scale/(1+dot(dsum_damp,dsum_damp)));\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmCellNormal = "\t\tfloat CellNormal(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence, int cellType, int distanceFunction)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = Cellular3D((p+offset) * frequency, cellType, distanceFunction);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
    public const string IncludeFbmCellFast = "\t\tfloat CellFast(float3 p, fixed octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)\n" +
                                                        "\t\t{\n" +
                                                        "\t\t	float sum = 0;\n" +
                                                        "\t\t	for (int i = 0; i < octaves; i++)\n" +
                                                        "\t\t	{\n" +
                                                        "\t\t		float h = 0;\n" +
                                                        "\t\t		h = Cellular3D((p + offset) * frequency);\n" +
                                                        "\t\t		sum += h*amplitude;\n" +
                                                        "\t\t		frequency *= lacunarity;\n" +
                                                        "\t\t		amplitude *= persistence;\n" +
                                                        "\t\t	}\n" +
                                                        "\t\t	return sum;\n" +
                                                        "\t\t}";
}

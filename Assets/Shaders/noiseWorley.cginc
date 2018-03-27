//
//	Code repository for GPU noise development blog
//	http://briansharpe.wordpress.com
//	https://github.com/BrianSharpe
//
//	I'm not one for copyrights.  Use the code however you wish.
//	All I ask is that credit be given back to the blog or myself when appropriate.
//	And also to let me know if you come up with any changes, improvements, thoughts or interesting uses for this stuff. :)
//	Thanks!
//
//	Brian Sharpe
//	brisharpe CIRCLE_A yahoo DOT com
//	http://briansharpe.wordpress.com
//	https://github.com/BrianSharpe
//
//===============================================================================
//  Scape Software License
//===============================================================================
//
//Copyright (c) 2007-2012, Giliam de Carpentier
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met: 
//
//1. Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer. 
//2. Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNERS OR CONTRIBUTORS BE LIABLE 
//FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
//CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
//OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
//OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.;

//
//	FAST32_hash
//	A very fast hashing function.  Requires 32bit support.
//	http://briansharpe.wordpress.com/2011/11/15/a-fast-and-simple-32bit-floating-point-hash-function/
//
//	The hash formula takes the form....
//	hash = mod( coord.x * coord.x * coord.y * coord.y, SOMELARGEFLOAT ) / SOMELARGEFLOAT
//	We truncate and offset the domain to the most interesting part of the noise.
//	SOMELARGEFLOAT should be in the range of 400.0->1000.0 and needs to be hand picked.  Only some give good results.
//	3D Noise is achieved by offsetting the SOMELARGEFLOAT value by the Z coordinate
//
float4 FAST32_hash_2D_Cell(float2 gridcell)	//	generates 4 different random numbers for the single given cell point
{
	//	gridcell is assumed to be an integer coordinate
	const float2 OFFSET = float2(26.0, 161.0);
	const float DOMAIN = 71.0;
	const float4 SOMELARGEFLOATS = float4(951.135664, 642.949883, 803.202459, 986.973274);
	float2 P = gridcell - floor(gridcell * (1.0 / DOMAIN)) * DOMAIN;
	P += OFFSET.xy;
	P *= P;
	return frac((P.x * P.y) * (1.0 / SOMELARGEFLOATS.xyzw));
}

float Cellular2D(float2 xy, int cellType, int distanceFunction)
{
	int xi = int(floor(xy.x));
	int yi = int(floor(xy.y));

	float xf = xy.x - float(xi);
	float yf = xy.y - float(yi);

	float dist1 = 9999999.0;
	float2 cell;

	for (int y = -1; y <= 1; y++) {
		for (int x = -1; x <= 1; x++) {
			cell = FAST32_hash_2D_Cell(float2(xi + x, yi + y)).xy;
			cell.x += (float(x) - xf);
			cell.y += (float(y) - yf);
			float dist = sqrt(dot(cell, cell));
			if (dist < dist1)
			{
				dist1 = dist;
			}
		}
	}

	return dist1;
}

float worley(float2 pos, float amplitude)
{
	return Cellular2D(pos, 1, 1) * amplitude;
}
//Application to vertex program struct
struct a2v
{
	//Input position
	float4 position : POSITION;

	//Input tangent
	float4 tangent : TANGENT0;
};

//Vertex program to fragment program struct
struct v2f
{
	//Input position
	float4 position : SV_POSITION;

	//Input tangent
	float w : TEXCOORD1;
};

//Vertex program
void mainVP
(
		in a2v input,
		out v2f output,		

		const uniform float4x4 worldViewProj	//The world view projection matrix
)
{
	output.w = input.tangent.w;

	// Transform the current vertex from object space to clip space
	output.position = mul(worldViewProj, input.position);
}

//Fragment program
float4 mainFP
(
	    in v2f input
) : SV_TARGET
{
	float4 color;
	color.r = clamp(input.w, 0, 1);
	color.g = clamp(1 - input.w, 0, 1);
	color.b = 0;
	color.a = 1;
	return color;
}
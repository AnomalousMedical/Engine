//Application to vertex program struct
struct a2v
{
	//Input position
	float4 position : POSITION;

	//Input tangent
	float3 tangent : TANGENT0;

	//Input binormal
	float3 binormal : BINORMAL0;

	//Input normal
	float3 normal : NORMAL0;
};

//Vertex program to fragment program struct
struct v2f
{
	//Input tangent
	float3 tangent : TEXCOORD0;

	//Input binormal
	float3 binormal : TEXCOORD1;

	//Input normal
	float3 normal : TEXCOORD2;
};

//Pack function packs values from -1 to 1 to 0 and 1
float3 pack(float3 input)
{
	return 0.5 * input + 0.5;
}

//Unpack function unpacks values from 0 to 1 to -1 to 1
float3 unpack(float3 input)
{
	return 2.0f * (input - 0.5);
}

//Vertex program
float4 mainVP
(
		in a2v input,
		out v2f output,		

		const uniform float4x4 worldViewProj	//The world view projection matrix
) : SV_POSITION
{
	output.tangent = pack(normalize(input.tangent));
	output.binormal = pack(normalize(input.binormal));
	output.normal = pack(normalize(input.normal));

	// Transform the current vertex from object space to clip space
	return mul(worldViewProj, input.position);
}

//Fragment program
float4 showBinormalsFP
(
	    in v2f input
) : SV_TARGET
{
	float4 color;
	color.rgb = input.binormal.rgb;
	color.a = 1;
	return color;
}

float4 showTangentsFP
(
	    in v2f input
) : SV_TARGET
{
	float4 color;
	color.rgb = input.tangent.rgb;
	color.a = 1;
	return color;
}

float4 showNormalsFP
(
	    in v2f input
) : SV_TARGET
{
	float4 color;
	color.rgb = input.normal.rgb;
	color.a = 1;
	return color;
}
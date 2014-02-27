struct vertin
{
	float4   position  : POSITION;
	float4   color     : COLOR0;
};

struct vertout
{
	float4   oPosition : POSITION;
	float4   oColor    : COLOR0;
};

//Vertex program
vertout main_vp(vertin input,
             uniform float4x4 worldViewProj)	
{
	vertout output;
	
	output.oPosition = mul(worldViewProj, input.position);
	output.oColor.rgb = float3(input.color.r, input.color.g, input.color.b);
	output.oColor.a = 1.0;

	return output;
}

//Fragment program
float4 main_fp(vertout input) : COLOR0	
{
	return input.oColor;
}

//Fragment program
float4 main_fp_alpha(vertout input, uniform float4 alpha) : COLOR0	
{
	input.oColor.a = alpha.a;
	return input.oColor;
}


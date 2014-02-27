void main( in float4 inPosition : POSITION0, 
		   in float2 inTexcoord : TEXCOORD0, 

		   const uniform float4x4 worldViewProj,	//The world view projection matrix

		   out float4 outPosition : POSITION, 
		   out float2 outTexcoord : TEXCOORD0 )
{
	outPosition = mul(worldViewProj, inPosition);
	outTexcoord = inTexcoord;
}
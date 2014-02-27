void main( in float4 inPosition : POSITION0, 
		   in float4 inColor : COLOR0, 
		   in float2 inTexcoord : TEXCOORD0, 

		   const uniform float4x4 elementWorldViewProj,	//The world view projection matrix

		   out float4 outPosition : SV_POSITION, 
		   out float4 outColor : TEXCOORD0, 
		   out float2 outTexcoord : TEXCOORD1 )
{
	outPosition = mul(elementWorldViewProj, inPosition);
	outColor = inColor;
	outTexcoord = inTexcoord;
}
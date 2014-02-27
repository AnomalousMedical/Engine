void main( in float4 inPosition : POSITION0, 
		   in float4 inColor : COLOR0, 

		   const uniform float4x4 elementWorldViewProj,	//The world view projection matrix

		   out float4 outPosition : POSITION, 
		   out float4 outColor : TEXCOORD0 )
{
	outPosition = mul(elementWorldViewProj, inPosition);
	outColor = inColor;
}
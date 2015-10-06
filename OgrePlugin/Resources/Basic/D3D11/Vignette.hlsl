void main( uniform float4 bgColor,
		   in float4 inPosition : SV_POSITION, 
		   in float2 inTexcoord : TEXCOORD0, 
		   out float4 outColor : SV_TARGET ) 
{
    float2 position = inTexcoord - float2(0.5, 0.5);
    float len = length(position);
    outColor = float4( bgColor.rgb * (1.0 - len), 1.0 );
}
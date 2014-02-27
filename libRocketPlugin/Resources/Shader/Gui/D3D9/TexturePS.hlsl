void main( uniform sampler2D sampleTexture, 
			
			in float4 inPosition : POSITION, 
			in float4 inColor : TEXCOORD0, 
			in float2 inTexcoord : TEXCOORD1, 
			
			out float4 outColor : COLOR ) 
{
	outColor = tex2D(sampleTexture, inTexcoord.xy).rgba * inColor;
}
void main( uniform sampler2D sampleTexture, 
			
			in float4 inPosition : POSITION, 
			in float2 inTexcoord : TEXCOORD0, 
			
			out float4 outColor : COLOR ) 
{
	outColor = tex2D(sampleTexture, inTexcoord.xy).rgba;
}
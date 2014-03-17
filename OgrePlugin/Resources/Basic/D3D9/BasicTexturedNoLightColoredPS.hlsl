void main( uniform sampler2D sampleTexture, 
			
			in float4 inPosition : POSITION, 
			in float2 inTexcoord : TEXCOORD0, 

			uniform float4 bgColor,
			
			out float4 outColor : COLOR ) 
{
	float4 texColor = tex2D(sampleTexture, inTexcoord.xy).rgba;
	outColor.rgb = (texColor.rgb * texColor.a) + (bgColor * (1 - texColor.a));
	outColor.a = 1.0f;
}
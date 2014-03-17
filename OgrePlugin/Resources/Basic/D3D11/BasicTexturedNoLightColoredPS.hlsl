void main( uniform Texture2D<float4> sampleTexture : register(t0), 
		   uniform SamplerState sampleSampler : register(s0),
			
			in float4 inPosition : SV_POSITION, 
			in float2 inTexcoord : TEXCOORD0,

			uniform float4 bgColor,
			
			out float4 outColor : SV_TARGET ) 
{
	float4 texColor = sampleTexture.SampleLevel(sampleSampler, inTexcoord, 0).rgba;
	outColor.rgb = (texColor.rgb * texColor.a) + (bgColor * (1 - texColor.a));
	outColor.a = 1.0f;
}
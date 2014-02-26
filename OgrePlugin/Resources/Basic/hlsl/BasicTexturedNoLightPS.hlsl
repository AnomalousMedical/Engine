void main( uniform Texture2D<float4> sampleTexture : register(t0), 
		   uniform SamplerState sampleSampler : register(s0),
			
			in float4 inPosition : SV_POSITION, 
			in float2 inTexcoord : TEXCOORD0, 
			
			out float4 outColor : SV_TARGET ) 
{
	outColor = sampleTexture.SampleLevel(sampleSampler, inTexcoord, 0).rgba;
}
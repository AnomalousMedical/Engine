#include "Textures.hlsl"
SamplerState g_SamPointWrap;

float3 GetBaseColor(in int mip, in float2 uv)
{
	return g_textures[instanceData.baseTexture].SampleLevel(g_SamPointWrap, uv, mip).rgb;
}

float GetOpacity(in int mip, in float2 uv)
{
	return g_textures[instanceData.baseTexture].SampleLevel(g_SamPointWrap, uv, mip).a;
}

float3 GetEmissive(in int mip, in float2 uv)
{
	return g_textures[instanceData.emissiveTexture].SampleLevel(g_SamPointWrap, uv, mip).rgb;
}
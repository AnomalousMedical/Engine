#include "Structures.hlsl"
#include "RayUtils.hlsl"
#include "SpriteData.hlsl"

Texture2D    g_textures[$$(NUM_TEXTURES)];
SamplerState g_SamPointWrap;

[shader("anyhit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetSpriteData(attr, barycentrics, posX, posY, posZ, uv);

    Texture2D opacityTexture = g_textures[instanceData.baseTexture];

    AnyHitOpacityMapUV(barycentrics, posX, posY, posZ, opacityTexture, g_SamPointWrap, uv);
}
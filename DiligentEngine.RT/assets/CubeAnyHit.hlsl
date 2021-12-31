
#include "Structures.hlsl"
#include "RayUtils.hlsl"
#include "MeshData.hlsl"

Texture2D    g_textures[$$(NUM_TEXTURES)];
SamplerState g_SamLinearWrap;

[shader("anyhit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    GetMeshData(attr, barycentrics, posX, posY, posZ);

    Texture2D opacityTexture = g_textures[instanceData.baseTexture];

    AnyHitOpacityMap(barycentrics, posX, posY, posZ, opacityTexture, g_SamLinearWrap);
}
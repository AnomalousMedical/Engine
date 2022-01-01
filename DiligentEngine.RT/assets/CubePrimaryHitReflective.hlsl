#include "Structures.hlsl"
#include "RayUtils_OldShim.hlsl"
#include "MeshData.hlsl"

Texture2D    g_textures[$$(NUM_TEXTURES)];
SamplerState g_SamLinearWrap;

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetMeshData(attr, barycentrics, posX, posY, posZ, uv);

    LightAndShadeReflective
    (
        payload, barycentrics, 
        posX, posY, posZ,
        g_textures[instanceData.baseTexture],
        g_textures[instanceData.normalTexture],
        g_textures[instanceData.physicalTexture],
        g_SamLinearWrap,
        g_SamLinearWrap
    );
}

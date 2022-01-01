#include "Structures.hlsl"
#include "RayUtils_OldShim.hlsl"
#include "MeshData.hlsl"

Texture2D    g_textures[$$(NUM_TEXTURES)];
SamplerState g_SamLinearWrap;

[shader("closesthit")]
void main(inout EmissiveRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetMeshData(attr, barycentrics, posX, posY, posZ, uv);

    GetEmissiveLighting
    (
        payload, barycentrics, 
        posX, posY, posZ,
        g_textures[instanceData.emissiveTexture],
        g_SamLinearWrap
    );
}

#include "Structures.hlsl"
#include "RayUtils.hlsl"
#include "Lighting.hlsl"
#if DATA_TYPE_MESH
#include "MeshData.hlsl"
#include "MeshTextures.hlsl"
#endif

#if DATA_TYPE_SPRITE
#include "SpriteData.hlsl"
#include "SpriteTextures.hlsl"
#endif

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetInstanceData(attr, barycentrics, posX, posY, posZ, uv);

    int mip = GetMip();

    LightAndShadeBaseNormalPhysical
    (
        payload, barycentrics, 
        posX, posY, posZ, 
        GetBaseColor(mip, uv),
        GetSampledNormal(mip, uv),
        GetPhysical(mip, uv)
    );

    payload.Color += GetEmissive(mip, uv);
}

#include "Structures.hlsl"
#include "RayUtils.hlsl"
#include "Lighting.hlsl"
#include "MeshData.hlsl"
#include "MeshTextures.hlsl"

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
}

#include "Structures.hlsl"
#include "RayUtils.hlsl"
#include "MeshData.hlsl"
#include "MeshTextures.hlsl"

[shader("anyhit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetInstanceData(attr, barycentrics, posX, posY, posZ, uv);

    int mip = GetMip();

    AnyHitOpacityTest(GetOpacity(mip, uv));
}
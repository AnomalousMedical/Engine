#include "Structures.hlsl"
#include "RayUtils.hlsl"
#include "SpriteData.hlsl"
#include "Textures.hlsl"

[shader("anyhit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetSpriteData(attr, barycentrics, posX, posY, posZ, uv);

    int mip = GetMip();

    AnyHitOpacityTest(GetSpriteOpacity(mip, uv));
}
#include "Structures.hlsl"
#include "RayUtils.hlsl"
#include "Lighting.hlsl"
#include "SpriteData.hlsl"
#include "Textures.hlsl"

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetInstanceData(attr, barycentrics, posX, posY, posZ, uv);

    int mip = GetMip();

    LightAndShadeBaseNormalPhysicalReflective
    (
        payload, barycentrics,
        posX, posY, posZ,
        GetSpriteBaseColor(mip, uv),
        GetSampledNormal(mip, uv),
        GetPhysical(mip, uv)
    );
}
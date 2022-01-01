#include "Structures.hlsl"
#include "RayUtils_OldShim.hlsl"
#include "SpriteData.hlsl"

Texture2D    g_textures[$$(NUM_TEXTURES)];
SamplerState g_SamPointWrap;

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetSpriteData(attr, barycentrics, posX, posY, posZ, uv);

    LightAndShadeUVColorOnly
    (
        payload, barycentrics,
        posX, posY, posZ,
        g_textures[instanceData.baseTexture],
        g_SamPointWrap,
        uv
    );
}

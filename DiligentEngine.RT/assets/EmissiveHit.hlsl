#include "Structures.hlsl"
#if DATA_TYPE_MESH
#include "MeshData.hlsl"
#include "MeshTextures.hlsl"
#endif

#if DATA_TYPE_SPRITE
#include "SpriteData.hlsl"
#include "SpriteTextures.hlsl"
#endif

[shader("closesthit")]
void main(inout EmissiveRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetInstanceData(attr, barycentrics, posX, posY, posZ, uv);

    int mip = GetMip();

    payload.Color = GetEmissive(mip, uv);
}

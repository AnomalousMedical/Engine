#include "Structures.hlsl"
#include "MeshData.hlsl"
#include "Textures.hlsl"

[shader("closesthit")]
void main(inout EmissiveRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    float3 barycentrics;
    CubeAttribVertex posX, posY, posZ;
    float2 uv;
    GetMeshData(attr, barycentrics, posX, posY, posZ, uv);

    int mip = GetMip();

    payload.Color = GetEmissive(mip, uv);
}

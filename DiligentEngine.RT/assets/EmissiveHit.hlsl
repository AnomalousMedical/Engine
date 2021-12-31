#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> g_vertices;
StructuredBuffer<uint> g_indices;

[[vk::shader_record_ext]]
ConstantBuffer<BlasInstanceData> instanceData;

Texture2D    g_textures[$$(NUM_TEXTURES)];
SamplerState g_SamLinearWrap;

[shader("closesthit")]
void main(inout EmissiveRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = g_vertices[g_indices[vertId + 0]];
    CubeAttribVertex posY = g_vertices[g_indices[vertId + 1]];
    CubeAttribVertex posZ = g_vertices[g_indices[vertId + 2]];

    GetEmissiveLighting
    (
        payload, barycentrics, 
        posX, posY, posZ,
        g_textures[instanceData.emissiveTexture],
        g_SamLinearWrap
    );
}

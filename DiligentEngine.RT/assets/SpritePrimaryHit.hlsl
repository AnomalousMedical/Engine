
#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> g_vertices;
StructuredBuffer<uint> g_indices;

Texture2D    $$(COLOR_TEXTURES)[$$(NUM_TEXTURES)];
Texture2D    $$(NORMAL_TEXTURES)[$$(NUM_TEXTURES)];
SamplerState g_SamLinearWrap;
SamplerState g_SamPointWrap;

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = g_vertices[g_indices[vertId + 0]];
    CubeAttribVertex posY = g_vertices[g_indices[vertId + 1]];
    CubeAttribVertex posZ = g_vertices[g_indices[vertId + 2]];

    LightAndShade
    (
        payload, barycentrics, 
        posX, posY, posZ, 
        $$(COLOR_TEXTURES)[InstanceID()],
        $$(NORMAL_TEXTURES)[InstanceID()],
        g_SamPointWrap,
        g_SamLinearWrap
    );
}

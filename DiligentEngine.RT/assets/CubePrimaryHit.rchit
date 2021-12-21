
#include "structures.fxh"
#include "RayUtils.fxh"

StructuredBuffer<CubeAttribVertex> g_Vertices;
StructuredBuffer<uint> g_Indices;

Texture2D    g_CubeTextures[$$(NUM_TEXTURES)];
Texture2D    g_CubeNormalTextures[$$(NUM_TEXTURES)];
SamplerState g_SamLinearWrap;

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = g_Vertices[g_Indices[vertId + 0]];
    CubeAttribVertex posY = g_Vertices[g_Indices[vertId + 1]];
    CubeAttribVertex posZ = g_Vertices[g_Indices[vertId + 2]];

    LightAndShade
    (
        payload, barycentrics, 
        posX, posY, posZ, 
        g_CubeTextures[InstanceID()],
        g_CubeNormalTextures[InstanceID()],
        g_SamLinearWrap
    );
}

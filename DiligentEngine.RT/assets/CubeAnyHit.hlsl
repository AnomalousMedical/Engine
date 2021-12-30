
#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> $$(VERTICES);
StructuredBuffer<uint> $$(INDICES);

[[vk::shader_record_ext]]
ConstantBuffer<BlasInstanceData> instanceData;

Texture2D    g_textures[$$(NUM_TEXTURES)];
SamplerState g_SamLinearWrap;

[shader("anyhit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = $$(VERTICES)[$$(INDICES)[vertId + 0]];
    CubeAttribVertex posY = $$(VERTICES)[$$(INDICES)[vertId + 1]];
    CubeAttribVertex posZ = $$(VERTICES)[$$(INDICES)[vertId + 2]];

    // Sample texturing. Ray tracing shaders don't support LOD calculation, so we must specify LOD and apply filtering.
    Texture2D opacityTexture = g_textures[instanceData.baseTexture];

    AnyHitOpacityMap(barycentrics, posX, posY, posZ, opacityTexture, g_SamLinearWrap);
}



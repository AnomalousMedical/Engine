
#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> $$(VERTICES);
StructuredBuffer<uint> $$(INDICES);

Texture2D    $$(COLOR_TEXTURES); //This might be better loaded independently
SamplerState g_SamPointWrap;

[[vk::shader_record_ext]]
ConstantBuffer<SpriteFrame> spriteFrame;

[shader("anyhit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = $$(VERTICES)[$$(INDICES)[vertId + 0]];
    CubeAttribVertex posY = $$(VERTICES)[$$(INDICES)[vertId + 1]];
    CubeAttribVertex posZ = $$(VERTICES)[$$(INDICES)[vertId + 2]];

    float2 frameVertX = spriteFrame.uvs[$$(INDICES)[vertId + 0]];
    float2 frameVertY = spriteFrame.uvs[$$(INDICES)[vertId + 1]];
    float2 frameVertZ = spriteFrame.uvs[$$(INDICES)[vertId + 2]];

    float2 uv = frameVertX.xy * barycentrics.x +
        frameVertY.xy * barycentrics.y +
        frameVertZ.xy * barycentrics.z;

    // Sample texturing. Ray tracing shaders don't support LOD calculation, so we must specify LOD and apply filtering.
    Texture2D opacityTexture = $$(COLOR_TEXTURES);

    AnyHitOpacityMapUV(barycentrics, posX, posY, posZ, opacityTexture, g_SamPointWrap, uv);
}



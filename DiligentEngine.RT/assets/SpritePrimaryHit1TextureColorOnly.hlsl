
#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> $$(VERTICES);
StructuredBuffer<uint> $$(INDICES);

Texture2D    $$(COLOR_TEXTURES);
SamplerState g_SamPointWrap;

[[vk::shader_record_ext]]
ConstantBuffer<SpriteFrame> spriteFrame;

[shader("closesthit")]
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

    LightAndShadeUVColorOnly
    (
        payload, barycentrics,
        posX, posY, posZ,
        $$(COLOR_TEXTURES),
        g_SamPointWrap,
        uv
    );
}

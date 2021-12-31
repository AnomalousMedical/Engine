#include "Structures.hlsl"
#include "RayUtils.hlsl"

StructuredBuffer<CubeAttribVertex> g_vertices;
StructuredBuffer<uint> g_indices;

Texture2D    $$(COLOR_TEXTURES);
Texture2D    $$(NORMAL_TEXTURES);
Texture2D    $$(PHYSICAL_TEXTURES);
SamplerState g_SamLinearWrap;
SamplerState g_SamPointWrap;

[[vk::shader_record_ext]]
ConstantBuffer<SpriteFrame> spriteFrame;

[shader("closesthit")]
void main(inout PrimaryRayPayload payload, in BuiltInTriangleIntersectionAttributes attr)
{
    // Calculate triangle barycentrics.
    float3 barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex();

    CubeAttribVertex posX = g_vertices[g_indices[vertId + 0]];
    CubeAttribVertex posY = g_vertices[g_indices[vertId + 1]];
    CubeAttribVertex posZ = g_vertices[g_indices[vertId + 2]];

    float2 frameVertX = spriteFrame.uvs[g_indices[vertId + 0]];
    float2 frameVertY = spriteFrame.uvs[g_indices[vertId + 1]];
    float2 frameVertZ = spriteFrame.uvs[g_indices[vertId + 2]];

    float2 uv = frameVertX.xy * barycentrics.x +
        frameVertY.xy * barycentrics.y +
        frameVertZ.xy * barycentrics.z;

    LightAndShadeReflectiveUV
    (
        payload, barycentrics,
        posX, posY, posZ,
        $$(COLOR_TEXTURES),
        $$(NORMAL_TEXTURES),
        $$(PHYSICAL_TEXTURES),
        g_SamPointWrap,
        g_SamLinearWrap,
        uv
    );
}

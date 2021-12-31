StructuredBuffer<CubeAttribVertex> g_vertices;
StructuredBuffer<uint> g_indices;

[[vk::shader_record_ext]]
ConstantBuffer<BlasInstanceData> spriteFrame;

void GetSpriteData
(
    in BuiltInTriangleIntersectionAttributes attr,
    out float3 barycentrics,
    out CubeAttribVertex posX,
    out CubeAttribVertex posY,
    out CubeAttribVertex posZ,
    out float2 uv
)
{
    barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex() + spriteFrame.indexOffset;

    posX = g_vertices[g_indices[vertId + 0] + spriteFrame.vertexOffset];
    posY = g_vertices[g_indices[vertId + 1] + spriteFrame.vertexOffset];
    posZ = g_vertices[g_indices[vertId + 2] + spriteFrame.vertexOffset];

    float2 frameVertX = spriteFrame.uvs[g_indices[vertId + 0]];
    float2 frameVertY = spriteFrame.uvs[g_indices[vertId + 1]];
    float2 frameVertZ = spriteFrame.uvs[g_indices[vertId + 2]];

    uv = frameVertX.xy * barycentrics.x +
         frameVertY.xy * barycentrics.y +
         frameVertZ.xy * barycentrics.z;
}
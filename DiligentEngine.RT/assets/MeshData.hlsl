StructuredBuffer<CubeAttribVertex> g_vertices;
StructuredBuffer<uint> g_indices;

[[vk::shader_record_ext]]
ConstantBuffer<BlasInstanceData> instanceData;

void GetMeshData
(
    in BuiltInTriangleIntersectionAttributes attr,
    out float3 barycentrics,
    out CubeAttribVertex posX,
    out CubeAttribVertex posY,
    out CubeAttribVertex posZ
)
{
    barycentrics = float3(1.0 - attr.barycentrics.x - attr.barycentrics.y, attr.barycentrics.x, attr.barycentrics.y);

    uint vertId = 3 * PrimitiveIndex() + instanceData.indexOffset;

    posX = g_vertices[g_indices[vertId + 0] + instanceData.vertexOffset];
    posY = g_vertices[g_indices[vertId + 1] + instanceData.vertexOffset];
    posZ = g_vertices[g_indices[vertId + 2] + instanceData.vertexOffset];
}
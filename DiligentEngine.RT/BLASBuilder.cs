using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public class BLASDesc
    {
        public Vector3[] CubePos { get; set; }

        public Vector4[] CubeUV { get; set; }

        public Vector4[] CubeNormals { get; set; }

        public UInt32[] Indices { get; set; }
    }

    public class BLASInstance : IDisposable
    {
        public AutoPtr<IBottomLevelAS> m_pCubeBLAS {get; internal set; }
        public AutoPtr<IBuffer> pCubeAttrVertexBuffer {get; internal set; }
        public AutoPtr<IBuffer> pCubeVertexBuffer { get; internal set; }
        public AutoPtr<IBuffer> pCubeIndexBuffer { get; internal set; }

        public void Dispose()
        {
            m_pCubeBLAS.Dispose();
            pCubeIndexBuffer.Dispose();
            pCubeVertexBuffer.Dispose();
            pCubeAttrVertexBuffer.Dispose();
        }
    }

    public class BLASBuilder
    {
        private readonly GraphicsEngine graphicsEngine;

        public BLASBuilder(GraphicsEngine graphicsEngine)
        {
            this.graphicsEngine = graphicsEngine;
        }

        /// <summary>
        /// Create a blas mesh. The caller is responsible to dispose the result.
        /// </summary>
        /// <param name="blasMeshDesc">The description of the mesh to create.</param>
        /// <returns></returns>
        public unsafe BLASInstance CreateBLAS(BLASDesc blasMeshDesc)
        {
            var m_pImmediateContext = graphicsEngine.ImmediateContext;
            var m_pDevice = graphicsEngine.RenderDevice;

            var result = new BLASInstance();

            var attrVertices = new CubeAttribVertex[blasMeshDesc.CubePos.Length];

            var Indices = blasMeshDesc.Indices;

            for(var i = 0; i < blasMeshDesc.CubePos.Length; ++i)
            {
                var vertex = new CubeAttribVertex();
                vertex.pos = blasMeshDesc.CubePos[i];
                vertex.uv = blasMeshDesc.CubeUV[i];
                vertex.normal = blasMeshDesc.CubeNormals[i];
                attrVertices[i] = vertex;
            }

            for (int i = 0; i < Indices.Length; i += 3)
            {
                var index1 = Indices[i];
                var index2 = Indices[i + 1];
                var index3 = Indices[i + 2];

                var vertex1 = attrVertices[index1];
                var vertex2 = attrVertices[index2];
                var vertex3 = attrVertices[index3];

                CalculateTangentBitangent(ref vertex1, ref vertex2, ref vertex3);

                attrVertices[index1] = vertex1;
                attrVertices[index2] = vertex2;
                attrVertices[index3] = vertex3;
            }

            // Create attribs vertex buffer
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube attrib vertices";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
                BuffDesc.ElementByteStride = (uint)sizeof(CubeAttribVertex);
                BuffDesc.Mode = BUFFER_MODE.BUFFER_MODE_STRUCTURED;

                BufferData BufData = new BufferData();
                fixed (CubeAttribVertex* p_vertices = attrVertices)
                {
                    BufData.pData = new IntPtr(p_vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = BuffDesc.ElementByteStride * (uint)attrVertices.Length;
                    result.pCubeAttrVertexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeVertexBuffer != nullptr);
            }

            // Create vertex buffer
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube vertices";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING;

                BufferData BufData = new BufferData();
                fixed (Vector3* vertices = blasMeshDesc.CubePos)
                {
                    BufData.pData = new IntPtr(vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = (uint)(sizeof(Vector3) * blasMeshDesc.CubePos.Length);
                    result.pCubeVertexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeVertexBuffer != nullptr);
            }

            // Create index buffer
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube indices";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING | BIND_FLAGS.BIND_SHADER_RESOURCE;
                BuffDesc.ElementByteStride = (uint)sizeof(uint);
                BuffDesc.Mode = BUFFER_MODE.BUFFER_MODE_STRUCTURED;

                BufferData BufData = new BufferData();
                fixed (uint* p_indices = Indices)
                {
                    BufData.pData = new IntPtr(p_indices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = BuffDesc.ElementByteStride * (uint)Indices.Length;
                    result.pCubeIndexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeIndexBuffer != nullptr);
            }

            // Create & build bottom level acceleration structure
            {
                // Create BLAS
                var Triangles = new BLASTriangleDesc();
                {
                    Triangles.GeometryName = "Cube";
                    Triangles.MaxVertexCount = (uint)attrVertices.Length;
                    Triangles.VertexValueType = VALUE_TYPE.VT_FLOAT32;
                    Triangles.VertexComponentCount = 3;
                    Triangles.MaxPrimitiveCount = (uint)(Indices.Length / 3);
                    Triangles.IndexType = VALUE_TYPE.VT_UINT32;

                    var ASDesc = new BottomLevelASDesc();
                    ASDesc.Name = "Cube BLAS";
                    ASDesc.Flags = RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_PREFER_FAST_TRACE;
                    ASDesc.pTriangles = new List<BLASTriangleDesc> { Triangles };

                    result.m_pCubeBLAS = m_pDevice.CreateBLAS(ASDesc);
                    //VERIFY_EXPR(m_pCubeBLAS != nullptr);
                }

                // Create scratch buffer
                using var pScratchBuffer = m_pDevice.CreateBuffer(new BufferDesc()
                {
                    Name = "BLAS Scratch Buffer",
                    Usage = USAGE.USAGE_DEFAULT,
                    BindFlags = BIND_FLAGS.BIND_RAY_TRACING,
                    uiSizeInBytes = result.m_pCubeBLAS.Obj.ScratchBufferSizes_Build,
                }, new BufferData());

                // Build BLAS
                var TriangleData = new BLASBuildTriangleData();
                TriangleData.GeometryName = Triangles.GeometryName;
                TriangleData.pVertexBuffer = result.pCubeVertexBuffer.Obj;
                TriangleData.VertexStride = (uint)sizeof(Vector3);
                TriangleData.VertexCount = Triangles.MaxVertexCount;
                TriangleData.VertexValueType = Triangles.VertexValueType;
                TriangleData.VertexComponentCount = Triangles.VertexComponentCount;
                TriangleData.pIndexBuffer = result.pCubeIndexBuffer.Obj;
                TriangleData.PrimitiveCount = Triangles.MaxPrimitiveCount;
                TriangleData.IndexType = Triangles.IndexType;
                TriangleData.Flags = RAYTRACING_GEOMETRY_FLAGS.RAYTRACING_GEOMETRY_FLAG_OPAQUE;

                var Attribs = new BuildBLASAttribs();
                Attribs.pBLAS = result.m_pCubeBLAS.Obj;
                Attribs.pTriangleData = new List<BLASBuildTriangleData> { TriangleData };

                // Scratch buffer will be used to store temporary data during BLAS build.
                // Previous content in the scratch buffer will be discarded.
                Attribs.pScratchBuffer = pScratchBuffer.Obj;

                // Allow engine to change resource states.
                Attribs.BLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.GeometryTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.ScratchBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;

                m_pImmediateContext.BuildBLAS(Attribs);

                return result;
            }
        }

        public void CalculateTangentBitangent(ref CubeAttribVertex v1, ref CubeAttribVertex v2, ref CubeAttribVertex v3)
        {
            //This is adapted from
            //https://learnopengl.com/Advanced-Lighting/Normal-Mapping

            var edge1 = v2.pos - v1.pos;
            var edge2 = v3.pos - v1.pos;
            var deltaUV1 = v2.uv - v1.uv;
            var deltaUV2 = v3.uv - v1.uv;

            float f = 1.0f / (deltaUV1.x * deltaUV2.y - deltaUV2.x * deltaUV1.y);

            var tangent = new Vector4();
            tangent.x = f * (deltaUV2.y * edge1.x - deltaUV1.y * edge2.x);
            tangent.y = f * (deltaUV2.y * edge1.y - deltaUV1.y * edge2.y);
            tangent.z = f * (deltaUV2.y * edge1.z - deltaUV1.y * edge2.z);

            var bitangent = new Vector4();
            bitangent.x = f * (-deltaUV2.x * edge1.x + deltaUV1.x * edge2.x);
            bitangent.y = f * (-deltaUV2.x * edge1.y + deltaUV1.x * edge2.y);
            bitangent.z = f * (-deltaUV2.x * edge1.z + deltaUV1.x * edge2.z);

            v1.tangent = tangent;
            v2.tangent = tangent;
            v3.tangent = tangent;

            v1.binormal = bitangent;
            v2.binormal = bitangent;
            v3.binormal = bitangent;
        }
    }
}

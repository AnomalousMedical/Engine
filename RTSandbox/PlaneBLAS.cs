using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    class PlaneBLAS : IDisposable
    {
        AutoPtr<IBottomLevelAS> m_pCubeBLAS;

        AutoPtr<IBuffer> m_CubeAttribsCB;
        AutoPtr<IBuffer> pCubeVertexBuffer;
        AutoPtr<IBuffer> pCubeIndexBuffer;

        public unsafe PlaneBLAS(GraphicsEngine graphicsEngine)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            // clang-format off
            var pos = new Vector3[]
            {
                new Vector3(-1,-1,0), new Vector3(+1,-1,0), new Vector3(+1,+1,0), new Vector3(-1,+1,0)
            };

            var uvs = new Vector4[]
            {
                new Vector4(1,0,0,0), new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0)
            };

            var normals = new Vector4[]
            {
                new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0)
            };

            var indices = new uint[]
            {
                2,0,1,    2,3,0,
            };
            // clang-format on

            // Create a buffer with cube attributes.
            // These attributes will be used in the hit shader to calculate UVs and normal for intersection point.
            {
                var Attribs = new CubeAttribs();
                Attribs.SetUvs(uvs);
                Attribs.SetNormals(normals);

                for (int i = 0; i < indices.Length; i += 3)
                {
                    Attribs.SetPrimitive(i / 3, indices[i], indices[i + 1], indices[i + 2], 0);
                }

                var BufData = new BufferData()
                {
                    pData = new IntPtr(&Attribs), //This is stack allocated, so just get the pointer, everything else complains
                    DataSize = (uint)sizeof(CubeAttribs)
                };
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube Attribs";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_UNIFORM_BUFFER;
                BuffDesc.uiSizeInBytes = BufData.DataSize;

                m_CubeAttribsCB = m_pDevice.CreateBuffer(BuffDesc, BufData);
                //VERIFY_EXPR(m_CubeAttribsCB != nullptr);
            }

            // Create vertex buffer
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube vertices";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING;

                BufferData BufData = new BufferData();
                fixed (Vector3* vertices = pos)
                {
                    BufData.pData = new IntPtr(vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = (uint)(sizeof(Vector3) * pos.Length);
                    pCubeVertexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeVertexBuffer != nullptr);
            }

            // Create index buffer
            {
                var BuffDesc = new BufferDesc();
                BuffDesc.Name = "Cube indices";
                BuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                BuffDesc.BindFlags = BIND_FLAGS.BIND_RAY_TRACING;

                BufferData BufData = new BufferData();
                fixed (uint* vertices = indices)
                {
                    BufData.pData = new IntPtr(vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = (uint)(sizeof(uint) * indices.Length);
                    pCubeIndexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeIndexBuffer != nullptr);
            }

            // Create & build bottom level acceleration structure
            {
                // Create BLAS
                var Triangles = new BLASTriangleDesc();
                {
                    Triangles.GeometryName = "Plane";
                    Triangles.MaxVertexCount = (uint)pos.Length;
                    Triangles.VertexValueType = VALUE_TYPE.VT_FLOAT32;
                    Triangles.VertexComponentCount = 3;
                    Triangles.MaxPrimitiveCount = (uint)(indices.Length / 3);
                    Triangles.IndexType = VALUE_TYPE.VT_UINT32;

                    var ASDesc = new BottomLevelASDesc();
                    ASDesc.Name = "Plane BLAS";
                    ASDesc.Flags = RAYTRACING_BUILD_AS_FLAGS.RAYTRACING_BUILD_AS_PREFER_FAST_TRACE;
                    ASDesc.pTriangles = new List<BLASTriangleDesc> { Triangles };

                    m_pCubeBLAS = m_pDevice.CreateBLAS(ASDesc);
                    //VERIFY_EXPR(m_pCubeBLAS != nullptr);
                }

                // Create scratch buffer
                using var pScratchBuffer = m_pDevice.CreateBuffer(new BufferDesc()
                {
                    Name = "BLAS Scratch Buffer",
                    Usage = USAGE.USAGE_DEFAULT,
                    BindFlags = BIND_FLAGS.BIND_RAY_TRACING,
                    uiSizeInBytes = m_pCubeBLAS.Obj.ScratchBufferSizes_Build,
                }, new BufferData());

                // Build BLAS
                var TriangleData = new BLASBuildTriangleData();
                TriangleData.GeometryName = Triangles.GeometryName;
                TriangleData.pVertexBuffer = pCubeVertexBuffer.Obj;
                TriangleData.VertexStride = (uint)sizeof(Vector3);
                TriangleData.VertexCount = Triangles.MaxVertexCount;
                TriangleData.VertexValueType = Triangles.VertexValueType;
                TriangleData.VertexComponentCount = Triangles.VertexComponentCount;
                TriangleData.pIndexBuffer = pCubeIndexBuffer.Obj;
                TriangleData.PrimitiveCount = Triangles.MaxPrimitiveCount;
                TriangleData.IndexType = Triangles.IndexType;
                TriangleData.Flags = RAYTRACING_GEOMETRY_FLAGS.RAYTRACING_GEOMETRY_FLAG_OPAQUE;

                var Attribs = new BuildBLASAttribs();
                Attribs.pBLAS = m_pCubeBLAS.Obj;
                Attribs.pTriangleData = new List<BLASBuildTriangleData> { TriangleData };

                // Scratch buffer will be used to store temporary data during BLAS build.
                // Previous content in the scratch buffer will be discarded.
                Attribs.pScratchBuffer = pScratchBuffer.Obj;

                // Allow engine to change resource states.
                Attribs.BLASTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.GeometryTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;
                Attribs.ScratchBufferTransitionMode = RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION;

                m_pImmediateContext.BuildBLAS(Attribs);
            }
        }

        public void Dispose()
        {
            m_pCubeBLAS.Dispose();
            pCubeIndexBuffer.Dispose();
            pCubeVertexBuffer.Dispose();
            m_CubeAttribsCB.Dispose();
        }

        public IBuffer Attribs => m_CubeAttribsCB.Obj;

        public IBottomLevelAS BLAS => m_pCubeBLAS.Obj;
    }
}

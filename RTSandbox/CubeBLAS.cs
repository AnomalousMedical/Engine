using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTSandbox
{
    class CubeBLAS : IDisposable
    {
        AutoPtr<IBottomLevelAS> m_pCubeBLAS;

        AutoPtr<IBuffer> m_CubeAttribsCB;
        AutoPtr<IBuffer> pCubeVertexBuffer;
        AutoPtr<IBuffer> pCubeIndexBuffer;

        public unsafe CubeBLAS(GraphicsEngine graphicsEngine)
        {
            var m_pDevice = graphicsEngine.RenderDevice;
            var m_pImmediateContext = graphicsEngine.ImmediateContext;

            var CubePos = new Vector3[]
            {
                new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,+0.5f,-0.5f), new Vector3(+0.5f,+0.5f,-0.5f), new Vector3(+0.5f,-0.5f,-0.5f), //Front -z
                new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,-0.5f,+0.5f), new Vector3(+0.5f,-0.5f,+0.5f), new Vector3(+0.5f,-0.5f,-0.5f), //Top -y
                new Vector3(+0.5f,-0.5f,-0.5f), new Vector3(+0.5f,-0.5f,+0.5f), new Vector3(+0.5f,+0.5f,+0.5f), new Vector3(+0.5f,+0.5f,-0.5f), //Left +x
                new Vector3(+0.5f,+0.5f,-0.5f), new Vector3(+0.5f,+0.5f,+0.5f), new Vector3(-0.5f,+0.5f,+0.5f), new Vector3(-0.5f,+0.5f,-0.5f), //Bottom +y
                new Vector3(-0.5f,+0.5f,-0.5f), new Vector3(-0.5f,+0.5f,+0.5f), new Vector3(-0.5f,-0.5f,+0.5f), new Vector3(-0.5f,-0.5f,-0.5f), //Right -x
                new Vector3(-0.5f,-0.5f,+0.5f), new Vector3(+0.5f,-0.5f,+0.5f), new Vector3(+0.5f,+0.5f,+0.5f), new Vector3(-0.5f,+0.5f,+0.5f), //Back +z
            };

            var CubeUV = new Vector4[]
            {
                new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0), new Vector4(1,0,0,0), //Front -z
                new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0), new Vector4(1,0,0,0), //Top -y
                new Vector4(0,0,0,0), new Vector4(1,0,0,0), new Vector4(1,1,0,0), new Vector4(0,1,0,0), //Left +x
                new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0), new Vector4(1,0,0,0), //Bottom +y
                new Vector4(1,1,0,0), new Vector4(0,1,0,0), new Vector4(0,0,0,0), new Vector4(1,0,0,0), //Right -x
                new Vector4(1,0,0,0), new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0)  //Back +z
            };

            var CubeTangents = new Vector4[]
            {
                new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), //Front -z
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), //Top -y
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), //Left +x
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), //Bottom +y
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), //Right -x
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0)  //Back +z, this is right
            };

            var CubeBinormals = new Vector4[]
            {
                new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), //Front -z
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), //Top -y
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), //Left +x
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), //Bottom +y
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), //Right -x
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0)  //Back +z, this is right
            };

            var CubeNormals = new Vector4[]
            {
                new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), //Front -z
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), //Top -y
                new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), //Left +x
                new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), //Bottom +y
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), //Right -x
                new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0)  //Back +z
            };

            var Indices = new uint[]
            {
                2,0,1,    2,3,0,
                4,6,5,    4,7,6,
                8,10,9,   8,11,10,
                12,14,13, 12,15,14,
                16,18,17, 16,19,18,
                20,21,22, 20,22,23
            };
            // clang-format on

            // Create a buffer with cube attributes.
            // These attributes will be used in the hit shader to calculate UVs and normal for intersection point.
            {
                var Attribs = new CubeAttribs();
                Attribs.SetUvs(CubeUV);
                Attribs.SetTangents(CubeTangents);
                Attribs.SetBinormals(CubeBinormals);
                Attribs.SetNormals(CubeNormals);

                for (int i = 0; i < Indices.Length; i += 3)
                {
                    Attribs.SetPrimitive(i / 3, Indices[i], Indices[i + 1], Indices[i + 2], 0);
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
                fixed (Vector3* vertices = CubePos)
                {
                    BufData.pData = new IntPtr(vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = (uint)(sizeof(Vector3) * CubePos.Length);
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
                fixed (uint* vertices = Indices)
                {
                    BufData.pData = new IntPtr(vertices);
                    BufData.DataSize = BuffDesc.uiSizeInBytes = (uint)(sizeof(uint) * Indices.Length);
                    pCubeIndexBuffer = m_pDevice.CreateBuffer(BuffDesc, BufData);
                }

                //VERIFY_EXPR(pCubeIndexBuffer != nullptr);
            }

            // Create & build bottom level acceleration structure
            {
                // Create BLAS
                var Triangles = new BLASTriangleDesc();
                {
                    Triangles.GeometryName = "Cube";
                    Triangles.MaxVertexCount = (uint)CubePos.Length;
                    Triangles.VertexValueType = VALUE_TYPE.VT_FLOAT32;
                    Triangles.VertexComponentCount = 3;
                    Triangles.MaxPrimitiveCount = (uint)(Indices.Length / 3);
                    Triangles.IndexType = VALUE_TYPE.VT_UINT32;

                    var ASDesc = new BottomLevelASDesc();
                    ASDesc.Name = "Cube BLAS";
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

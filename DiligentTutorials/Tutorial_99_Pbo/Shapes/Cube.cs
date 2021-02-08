using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using float3 = Engine.Vector3;
using float2 = Engine.Vector2;

namespace Tutorial_99_Pbo.Shapes
{
    class Cube : IDisposable
    {
        private AutoPtr<IBuffer> m_CubeVertexBuffer;
        private AutoPtr<IBuffer> m_CubeSkinVertexBuffer;
        private AutoPtr<IBuffer> m_CubeIndexBuffer;

        public IBuffer VertexBuffer => m_CubeVertexBuffer.Obj;
        public IBuffer SkinVertexBuffer => m_CubeSkinVertexBuffer.Obj;
        public IBuffer IndexBuffer => m_CubeIndexBuffer.Obj;

        public uint NumIndices => 36;



        public Cube(GraphicsEngine graphicsEngine)
        {
            CreateVertexBuffer(graphicsEngine.RenderDevice);
            CreateIndexBuffer(graphicsEngine.RenderDevice);
        }

        public void Dispose()
        {
            m_CubeSkinVertexBuffer.Dispose();
            m_CubeVertexBuffer.Dispose();
            m_CubeIndexBuffer.Dispose();
        }

        unsafe void CreateVertexBuffer(IRenderDevice m_pDevice)
        {
            // Cube vertices

            //      (-1,+1,+1)________________(+1,+1,+1)
            //               /|              /|
            //              / |             / |
            //             /  |            /  |
            //            /   |           /   |
            //(-1,-1,+1) /____|__________/(+1,-1,+1)
            //           |    |__________|____|
            //           |   /(-1,+1,-1) |    /(+1,+1,-1)
            //           |  /            |   /
            //           | /             |  /
            //           |/              | /
            //           /_______________|/
            //        (-1,-1,-1)       (+1,-1,-1)
            //

            // clang-format off
            // This time we have to duplicate verices because texture coordinates cannot
            // be shared
            var CubeVerts = new GLTFVertex[]
            {
                new GLTFVertex{pos = new float3(-1,-1,-1), uv0 = new float2(0,1), normal = new float3(0, 0, -1)},
                new GLTFVertex{pos = new float3(-1,+1,-1), uv0 = new float2(0,0), normal = new float3(0, 0, -1)},
                new GLTFVertex{pos = new float3(+1,+1,-1), uv0 = new float2(1,0), normal = new float3(0, 0, -1)},
                new GLTFVertex{pos = new float3(+1,-1,-1), uv0 = new float2(1,1), normal = new float3(0, 0, -1)},
//                                     new                   uv0 = new              normal = new 
                new GLTFVertex{pos = new float3(-1,-1,-1), uv0 = new float2(0,1), normal = new float3(0, -1, 0)},
                new GLTFVertex{pos = new float3(-1,-1,+1), uv0 = new float2(0,0), normal = new float3(0, -1, 0)},
                new GLTFVertex{pos = new float3(+1,-1,+1), uv0 = new float2(1,0), normal = new float3(0, -1, 0)},
                new GLTFVertex{pos = new float3(+1,-1,-1), uv0 = new float2(1,1), normal = new float3(0, -1, 0)},
//                                     new                   uv0 = new              normal = new 
                new GLTFVertex{pos = new float3(+1,-1,-1), uv0 = new float2(0,1), normal = new float3(+1, 0, 0)},
                new GLTFVertex{pos = new float3(+1,-1,+1), uv0 = new float2(1,1), normal = new float3(+1, 0, 0)},
                new GLTFVertex{pos = new float3(+1,+1,+1), uv0 = new float2(1,0), normal = new float3(+1, 0, 0)},
                new GLTFVertex{pos = new float3(+1,+1,-1), uv0 = new float2(0,0), normal = new float3(+1, 0, 0)},
//                                     new                   uv0 = new              normal = new 
                new GLTFVertex{pos = new float3(+1,+1,-1), uv0 = new float2(0,1), normal = new float3(0, +1, 0)},
                new GLTFVertex{pos = new float3(+1,+1,+1), uv0 = new float2(0,0), normal = new float3(0, +1, 0)},
                new GLTFVertex{pos = new float3(-1,+1,+1), uv0 = new float2(1,0), normal = new float3(0, +1, 0)},
                new GLTFVertex{pos = new float3(-1,+1,-1), uv0 = new float2(1,1), normal = new float3(0, +1, 0)},
//                                     new                   uv0 = new              normal = new 
                new GLTFVertex{pos = new float3(-1,+1,-1), uv0 = new float2(1,0), normal = new float3(-1, 0, 0)},
                new GLTFVertex{pos = new float3(-1,+1,+1), uv0 = new float2(0,0), normal = new float3(-1, 0, 0)},
                new GLTFVertex{pos = new float3(-1,-1,+1), uv0 = new float2(0,1), normal = new float3(-1, 0, 0)},
                new GLTFVertex{pos = new float3(-1,-1,-1), uv0 = new float2(1,1), normal = new float3(-1, 0, 0)},
//                                     new                   uv0 = new              normal = new 
                new GLTFVertex{pos = new float3(-1,-1,+1), uv0 = new float2(1,1), normal = new float3(0, 0, +1)},
                new GLTFVertex{pos = new float3(+1,-1,+1), uv0 = new float2(0,1), normal = new float3(0, 0, +1)},
                new GLTFVertex{pos = new float3(+1,+1,+1), uv0 = new float2(0,0), normal = new float3(0, 0, +1)},
                new GLTFVertex{pos = new float3(-1,+1,+1), uv0 = new float2(1,0), normal = new float3(0, 0, +1)}
            };

            {
                // Create a vertex buffer that stores cube vertices
                BufferDesc VertBuffDesc = new BufferDesc();
                VertBuffDesc.Name = "Cube vertex buffer";
                VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
                VertBuffDesc.uiSizeInBytes = (uint)(sizeof(GLTFVertex) * CubeVerts.Length);
                BufferData VBData = new BufferData();
                fixed (GLTFVertex* vertices = CubeVerts)
                {
                    VBData.pData = new IntPtr(vertices);
                    VBData.DataSize = (uint)(sizeof(GLTFVertex) * CubeVerts.Length);
                    m_CubeVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
                }
            }

            var CubeSkinVerts = new GLTFVertexSkinAttribs[]
            {
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},

                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},

                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},

                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},

                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},

                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
            };

            {
                // Create a vertex buffer that stores cube vertices
                BufferDesc VertBuffDesc = new BufferDesc();
                VertBuffDesc.Name = "Cube skin vertex buffer";
                VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
                VertBuffDesc.uiSizeInBytes = (uint)(sizeof(GLTFVertex) * CubeVerts.Length);
                BufferData VBData = new BufferData();
                fixed (GLTFVertex* vertices = CubeVerts)
                {
                    VBData.pData = new IntPtr(vertices);
                    VBData.DataSize = (uint)(sizeof(GLTFVertex) * CubeVerts.Length);
                    m_CubeSkinVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
                }
            }
        }

        unsafe void CreateIndexBuffer(IRenderDevice m_pDevice)
        {
            // Original ccw indices (culled by backface cull, cube was inside out)
            //var Indices = new UInt32[]
            //{
            //    //ccw
            //    2,0,1,    2,3,0,
            //    4,6,5,    4,7,6,
            //    8,10,9,   8,11,10,
            //    12,14,13, 12,15,14,
            //    16,18,17, 16,19,18,
            //    20,21,22, 20,22,23
            //};

            ////make cw
            //for (var i = 0; i < Indices.Length; i += 3)
            //{
            //    var temp = Indices[i];
            //    Indices[i] = Indices[i + 2];
            //    Indices[i + 2] = temp;
            //}
            //// END

            //Clockwise indices
            var Indices = new UInt32[]
            {
                1,0,2,       0,3,2,
                5,6,4,       6,7,4,
                9,10,8,      10,11,8,
                13,14,12,    14,15,12,
                17,18,16,    18,19,16,
                22,21,20,    23,22,20,
            };

            BufferDesc IndBuffDesc = new BufferDesc();
            IndBuffDesc.Name = "Cube index buffer";
            IndBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
            IndBuffDesc.BindFlags = BIND_FLAGS.BIND_INDEX_BUFFER;
            IndBuffDesc.uiSizeInBytes = (uint)(sizeof(UInt32) * Indices.Length);
            BufferData IBData = new BufferData();
            fixed (UInt32* pIndices = Indices)
            {
                IBData.pData = new IntPtr(pIndices);
                IBData.DataSize = (uint)(sizeof(UInt32) * Indices.Length);
                m_CubeIndexBuffer = m_pDevice.CreateBuffer(IndBuffDesc, IBData);
            }
        }
    }
}

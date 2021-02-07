﻿using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_99_Pbo.Shapes
{
    class Cube : IDisposable
    {
        private AutoPtr<IBuffer> m_CubeVertexBuffer;
        private AutoPtr<IBuffer> m_CubeIndexBuffer;

        public IBuffer VertexBuffer => m_CubeVertexBuffer.Obj;
        public IBuffer IndexBuffer => m_CubeIndexBuffer.Obj;

        public uint NumIndices => 36;

        [StructLayout(LayoutKind.Sequential)]
        struct Vertex
        {
            public Vector3 pos;
            public Vector2 uv;
        };

        public Cube(GraphicsEngine graphicsEngine)
        {
            CreateVertexBuffer(graphicsEngine.RenderDevice);
            CreateIndexBuffer(graphicsEngine.RenderDevice);
        }

        public void Dispose()
        {
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
            var CubeVerts = new Vertex[]
            {
                new Vertex{pos = new Vector3(-1,-1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(-1,+1,-1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(+1,+1,-1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(+1,-1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(-1,-1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(-1,-1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(+1,-1,+1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(+1,-1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(+1,-1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(+1,-1,+1), uv = new Vector2(1,1)},
                new Vertex{pos = new Vector3(+1,+1,+1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(+1,+1,-1), uv = new Vector2(0,0)},

                new Vertex{pos = new Vector3(+1,+1,-1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(+1,+1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(-1,+1,+1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(-1,+1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(-1,+1,-1), uv = new Vector2(1,0)},
                new Vertex{pos = new Vector3(-1,+1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(-1,-1,+1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(-1,-1,-1), uv = new Vector2(1,1)},

                new Vertex{pos = new Vector3(-1,-1,+1), uv = new Vector2(1,1)},
                new Vertex{pos = new Vector3(+1,-1,+1), uv = new Vector2(0,1)},
                new Vertex{pos = new Vector3(+1,+1,+1), uv = new Vector2(0,0)},
                new Vertex{pos = new Vector3(-1,+1,+1), uv = new Vector2(1,0)}
            };


            // Create a vertex buffer that stores cube vertices
            BufferDesc VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name = "Cube vertex buffer";
            VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
            VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.uiSizeInBytes = (uint)(sizeof(Vertex) * CubeVerts.Length);
            BufferData VBData = new BufferData();
            fixed (Vertex* vertices = CubeVerts)
            {
                VBData.pData = new IntPtr(vertices);
                VBData.DataSize = (uint)(sizeof(Vertex) * CubeVerts.Length);
                m_CubeVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
            }
        }

        unsafe void CreateIndexBuffer(IRenderDevice m_pDevice)
        {
            // clang-format off
            var Indices = new UInt32[]
            {
                2,0,1,    2,3,0,
                4,6,5,    4,7,6,
                8,10,9,   8,11,10,
                12,14,13, 12,15,14,
                16,18,17, 16,19,18,
                20,21,22, 20,22,23
            };
            // clang-format on

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

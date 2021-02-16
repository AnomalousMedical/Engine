using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    class SharpGuiBuffer : IDisposable
    {
        private AutoPtr<IBuffer> m_CubeVertexBuffer;
        private AutoPtr<IBuffer> m_CubeIndexBuffer;
        private readonly GraphicsEngine graphicsEngine;

        public IBuffer VertexBuffer => m_CubeVertexBuffer.Obj;
        public IBuffer IndexBuffer => m_CubeIndexBuffer.Obj; 

        public SharpGuiBuffer(GraphicsEngine graphicsEngine)
        {
            this.graphicsEngine = graphicsEngine;
            CreateVertexBuffer();
            CreateIndexBuffer();
        }

        public void Dispose()
        {
            m_CubeIndexBuffer.Dispose();
            m_CubeVertexBuffer.Dispose();
        }

        unsafe void CreateVertexBuffer()
        {
            var m_pDevice = graphicsEngine.RenderDevice;

            // Layout of this structure matches the one we defined in the pipeline state

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
            var CubeVerts = new SharpImGuiVertex[]
            {
                new SharpImGuiVertex{pos = new Vector3(-1,-1,-1), color = new Vector4(1,0,0,1)},
                new SharpImGuiVertex{pos = new Vector3(-1,+1,-1), color = new Vector4(0,1,0,1)},
                new SharpImGuiVertex{pos = new Vector3(+1,+1,-1), color = new Vector4(0,0,1,1)},
                new SharpImGuiVertex{pos = new Vector3(+1,-1,-1), color = new Vector4(1,1,1,1)},

                new SharpImGuiVertex{pos = new Vector3(-1,-1,+1), color = new Vector4(1,1,0,1)},
                new SharpImGuiVertex{pos = new Vector3(-1,+1,+1), color = new Vector4(0,1,1,1)},
                new SharpImGuiVertex{pos = new Vector3(+1,+1,+1), color = new Vector4(1,0,1,1)},
                new SharpImGuiVertex{pos = new Vector3(+1,-1,+1), color = new Vector4(0.2f,0.2f,0.2f,1)},
            };
            // clang-format on

            // Create a vertex buffer that stores cube vertices
            BufferDesc VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name = "Cube vertex buffer";
            VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
            VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.uiSizeInBytes = (uint)(sizeof(SharpImGuiVertex) * CubeVerts.Length);
            BufferData VBData = new BufferData();
            fixed (SharpImGuiVertex* vertices = CubeVerts)
            {
                VBData.pData = new IntPtr(vertices);
                VBData.DataSize = (uint)(sizeof(SharpImGuiVertex) * CubeVerts.Length);
                m_CubeVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
            }
        }

        unsafe void CreateIndexBuffer()
        {
            var m_pDevice = graphicsEngine.RenderDevice;

            // clang-format off
            var Indices = new UInt32[]
            {
                2,0,1, 2,3,0,
                4,6,5, 4,7,6,
                0,7,4, 0,3,7,
                1,0,4, 1,4,5,
                1,5,2, 5,6,2,
                3,6,7, 3,2,6
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

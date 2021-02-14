using DiligentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DiligentEngine.GltfPbr;

namespace DiligentEngine.GltfPbr.Shapes
{
    public class Plane : IDisposable
    {
        private AutoPtr<IBuffer> vertexBuffer;
        private AutoPtr<IBuffer> skinVertexBuffer;
        private AutoPtr<IBuffer> indexBuffer;

        public IBuffer VertexBuffer => vertexBuffer.Obj;
        public IBuffer SkinVertexBuffer => skinVertexBuffer.Obj;
        public IBuffer IndexBuffer => indexBuffer.Obj;

        public uint NumIndices => 6;

        public Plane(GraphicsEngine graphicsEngine)
        {
            CreateVertexBuffer(graphicsEngine.RenderDevice);
            CreateSkinVerts(graphicsEngine.RenderDevice);
            CreateIndexBuffer(graphicsEngine.RenderDevice);
        }

        public void Dispose()
        {
            skinVertexBuffer.Dispose();
            vertexBuffer.Dispose();
            indexBuffer.Dispose();
        }

        unsafe void CreateVertexBuffer(IRenderDevice m_pDevice)
        {
            var CubeVerts = new GLTFVertex[]
            {
                new GLTFVertex{pos = new Vector3(-1,+1,0), uv0 = new Vector2(0,0), normal = new Vector3(0, 0, -1)},
                new GLTFVertex{pos = new Vector3(+1,+1,0), uv0 = new Vector2(1,0), normal = new Vector3(0, 0, -1)},
                new GLTFVertex{pos = new Vector3(+1,-1,0), uv0 = new Vector2(1,1), normal = new Vector3(0, 0, -1)},
                new GLTFVertex{pos = new Vector3(-1,-1,0), uv0 = new Vector2(0,1), normal = new Vector3(0, 0, -1)},
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
                    vertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
                }
            }
        }

        private unsafe void CreateSkinVerts(IRenderDevice m_pDevice)
        {
            var CubeSkinVerts = new GLTFVertexSkinAttribs[]
            {
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
                new GLTFVertexSkinAttribs{joint0 = Vector4.Zero, weight0 = Vector4.Zero},
            };

            // Create a vertex buffer that stores cube vertices
            BufferDesc VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name = "Cube skin vertex buffer";
            VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
            VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.uiSizeInBytes = (uint)(sizeof(GLTFVertexSkinAttribs) * CubeSkinVerts.Length);
            BufferData VBData = new BufferData();
            fixed (GLTFVertexSkinAttribs* vertices = CubeSkinVerts)
            {
                VBData.pData = new IntPtr(vertices);
                VBData.DataSize = (uint)(sizeof(GLTFVertexSkinAttribs) * CubeSkinVerts.Length);
                skinVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
            }
        }

        unsafe void CreateIndexBuffer(IRenderDevice m_pDevice)
        {
            var Indices = new UInt32[]
            {
                0,1,2,   2,3,0
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
                indexBuffer = m_pDevice.CreateBuffer(IndBuffDesc, IBData);
            }
        }

        // vertices

        // -1, +1 --------------- +1, +1
        //    |  0               1   |
        //    |                      |
        //    |                      |
        //    |                      |
        //    |                      |
        //    |  3               2   |
        // -1, -1 --------------- +1, -1
    }
}

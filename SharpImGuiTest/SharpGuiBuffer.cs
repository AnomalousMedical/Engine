using DiligentEngine;
using Engine;
using Engine.Platform;
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
        private readonly OSWindow osWindow;
        public int MaxNumberOfQuads = 1000;
        SharpImGuiVertex[] verts;
        private int currentQuad = 0;

        public IBuffer VertexBuffer => m_CubeVertexBuffer.Obj;
        public IBuffer IndexBuffer => m_CubeIndexBuffer.Obj;

        public SharpGuiBuffer(GraphicsEngine graphicsEngine, OSWindow osWindow)
        {
            verts = new SharpImGuiVertex[MaxNumberOfQuads * 4];

            this.graphicsEngine = graphicsEngine;
            this.osWindow = osWindow;
            CreateVertexBuffer();
            CreateIndexBuffer();
        }

        public void Dispose()
        {
            m_CubeIndexBuffer.Dispose();
            m_CubeVertexBuffer.Dispose();
        }

        public void Begin()
        {
            currentQuad = 0;
            NumIndices = 0;
        }

        public void DrawQuad(int x, int y, int width, int height, Color color)
        {
            float left = x / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float right = (x + width) / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float top = y / (float)osWindow.WindowHeight * -2.0f + 1.0f;
            float bottom = (y + height) / (float)osWindow.WindowHeight * -2.0f + 1.0f;

            verts[currentQuad].pos = new Vector3(left, top, 0);
            verts[currentQuad + 1].pos = new Vector3(right, top, 0);
            verts[currentQuad + 2].pos = new Vector3(right, bottom, 0);
            verts[currentQuad + 3].pos = new Vector3(left, bottom, 0);

            verts[currentQuad].color = color;
            verts[currentQuad + 1].color = color;
            verts[currentQuad + 2].color = color;
            verts[currentQuad + 3].color = color;

            currentQuad += 4;
            NumIndices += 6;
        }

        public unsafe void MapBuffers()
        {
            IntPtr data = graphicsEngine.ImmediateContext.MapBuffer(m_CubeVertexBuffer.Obj, MAP_TYPE.MAP_WRITE, MAP_FLAGS.MAP_FLAG_DISCARD);

            var dest = new Span<SharpImGuiVertex>(data.ToPointer(), verts.Length);
            var src = new Span<SharpImGuiVertex>(verts);
            src.CopyTo(dest);
            //SharpImGuiVertex* vertex = (SharpImGuiVertex*)data.ToPointer();
            //for(var i = 0; i < currentQuad; ++i)
            //{
            //    vertex[i] = verts[i];
            //}

            graphicsEngine.ImmediateContext.UnmapBuffer(m_CubeVertexBuffer.Obj, MAP_TYPE.MAP_WRITE);
        }

        public uint NumIndices { get; private set; }

        unsafe void CreateVertexBuffer()
        {
            var m_pDevice = graphicsEngine.RenderDevice;

            // Create a vertex buffer that stores cube vertices
            BufferDesc VertBuffDesc = new BufferDesc();
            VertBuffDesc.Name = "Cube vertex buffer";
            VertBuffDesc.Usage = USAGE.USAGE_DYNAMIC;
            VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
            VertBuffDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_WRITE;
            VertBuffDesc.uiSizeInBytes = (uint)(sizeof(SharpImGuiVertex) * verts.Length);
            
            m_CubeVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc);
            
        }

        unsafe void CreateIndexBuffer()
        {
            var m_pDevice = graphicsEngine.RenderDevice;

            var Indices = new UInt32[MaxNumberOfQuads * 6];

            uint indexBlock = 0;
            for(int i = 0; i < Indices.Length; i += 6)
            {
                Indices[i] = indexBlock;
                Indices[i + 1] = indexBlock + 1;
                Indices[i + 2] = indexBlock + 2;

                Indices[i + 3] = indexBlock + 2;
                Indices[i + 4] = indexBlock + 3;
                Indices[i + 5] = indexBlock;

                indexBlock += 4;
            }

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

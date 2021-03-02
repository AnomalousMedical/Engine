using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr.Shapes
{
    public class Mesh : IDisposable
    {
        private AutoPtr<IBuffer> vertexBuffer;
        private AutoPtr<IBuffer> skinVertexBuffer;
        private AutoPtr<IBuffer> indexBuffer;

        public IBuffer VertexBuffer => vertexBuffer.Obj;
        public IBuffer SkinVertexBuffer => skinVertexBuffer.Obj;
        public IBuffer IndexBuffer => indexBuffer.Obj;

        public uint NumIndices => numIndices;

        private uint numIndices = 0;
        private uint currentVert = 0;
        private uint indexBlock = 0;
        private uint currentIndex = 0;

        private GLTFVertex[] quadVerts;
        private GLTFVertexSkinAttribs[] quadSkinVerts;
        private UInt32[] indices;

        public Mesh()
        {

        }

        public void Dispose()
        {
            skinVertexBuffer?.Dispose();
            vertexBuffer?.Dispose();
            indexBuffer?.Dispose();
        }

        public void Begin(uint numQuads)
        {
            var NumVertices = numQuads * 4;
            numIndices = numQuads * 6;

            quadVerts = new GLTFVertex[NumVertices];
            quadSkinVerts = new GLTFVertexSkinAttribs[NumVertices];
            indices = new UInt32[numIndices];
        }

        public void AddQuad(in Vector3 topLeft, in Vector3 topRight, in Vector3 bottomRight, in Vector3 bottomLeft, in Vector3 normal, in Vector2 uvTopLeft, in Vector2 uvBottomRight)
        {
            quadVerts[currentVert].pos = topLeft;
            quadVerts[currentVert].normal = normal;
            quadVerts[currentVert].uv0 = new Vector2(uvTopLeft.x, uvTopLeft.y);
            quadVerts[currentVert].uv1 = new Vector2(uvTopLeft.x, uvTopLeft.y);
            quadSkinVerts[currentVert] = new GLTFVertexSkinAttribs { joint0 = new Vector4(0, 0, 0, 0), weight0 = Vector4.Zero };

            ++currentVert;
            quadVerts[currentVert].pos = topRight;
            quadVerts[currentVert].normal = normal;
            quadVerts[currentVert].uv0 = new Vector2(uvBottomRight.x, uvTopLeft.y);
            quadVerts[currentVert].uv1 = new Vector2(uvBottomRight.x, uvTopLeft.y);
            quadSkinVerts[currentVert] = new GLTFVertexSkinAttribs { joint0 = new Vector4(0, 0, 0, 0), weight0 = Vector4.Zero };

            ++currentVert;
            quadVerts[currentVert].pos = bottomRight;
            quadVerts[currentVert].normal = normal;
            quadVerts[currentVert].uv0 = new Vector2(uvBottomRight.x, uvBottomRight.y);
            quadVerts[currentVert].uv1 = new Vector2(uvBottomRight.x, uvBottomRight.y);
            quadSkinVerts[currentVert] = new GLTFVertexSkinAttribs { joint0 = new Vector4(0, 0, 0, 0), weight0 = Vector4.Zero };

            ++currentVert;
            quadVerts[currentVert].pos = bottomLeft;
            quadVerts[currentVert].normal = normal;
            quadVerts[currentVert].uv0 = new Vector2(uvTopLeft.x, uvBottomRight.y);
            quadVerts[currentVert].uv1 = new Vector2(uvTopLeft.x, uvBottomRight.y);
            quadSkinVerts[currentVert] = new GLTFVertexSkinAttribs { joint0 = new Vector4(0, 0, 0, 0), weight0 = Vector4.Zero };

            ++currentVert;

            //Add Index
            indices[currentIndex] = indexBlock;
            indices[currentIndex + 1] = indexBlock + 1;
            indices[currentIndex + 2] = indexBlock + 2;

            indices[currentIndex + 3] = indexBlock + 2;
            indices[currentIndex + 4] = indexBlock + 3;
            indices[currentIndex + 5] = indexBlock;

            indexBlock += 4;
            currentIndex += 6;
        }

        public unsafe void End(IRenderDevice m_pDevice)
        {
            {
                // Create a vertex buffer that stores cube vertices
                BufferDesc VertBuffDesc = new BufferDesc();
                VertBuffDesc.Name = "Mesh vertex buffer";
                VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
                VertBuffDesc.uiSizeInBytes = (uint)(sizeof(GLTFVertex) * quadVerts.Length);
                BufferData VBData = new BufferData();
                fixed (GLTFVertex* vertices = quadVerts)
                {
                    VBData.pData = new IntPtr(vertices);
                    VBData.DataSize = (uint)(sizeof(GLTFVertex) * quadVerts.Length);
                    vertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
                }
            }

            {
                // Create a vertex buffer that stores skin vertices
                BufferDesc VertBuffDesc = new BufferDesc();
                VertBuffDesc.Name = "Mesh skin vertex buffer";
                VertBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                VertBuffDesc.BindFlags = BIND_FLAGS.BIND_VERTEX_BUFFER;
                VertBuffDesc.uiSizeInBytes = (uint)(sizeof(GLTFVertexSkinAttribs) * quadSkinVerts.Length);
                BufferData VBData = new BufferData();
                fixed (GLTFVertexSkinAttribs* vertices = quadSkinVerts)
                {
                    VBData.pData = new IntPtr(vertices);
                    VBData.DataSize = (uint)(sizeof(GLTFVertexSkinAttribs) * quadSkinVerts.Length);
                    skinVertexBuffer = m_pDevice.CreateBuffer(VertBuffDesc, VBData);
                }
                quadSkinVerts = null;
            }

            {
                BufferDesc IndBuffDesc = new BufferDesc();
                IndBuffDesc.Name = "Mesh index buffer";
                IndBuffDesc.Usage = USAGE.USAGE_IMMUTABLE;
                IndBuffDesc.BindFlags = BIND_FLAGS.BIND_INDEX_BUFFER;
                IndBuffDesc.uiSizeInBytes = (uint)(sizeof(UInt32) * indices.Length);
                BufferData IBData = new BufferData();
                fixed (UInt32* pIndices = indices)
                {
                    IBData.pData = new IntPtr(pIndices);
                    IBData.DataSize = (uint)(sizeof(UInt32) * indices.Length);
                    indexBuffer = m_pDevice.CreateBuffer(IndBuffDesc, IBData);
                }
                indices = null;
            }
        }
    }
}

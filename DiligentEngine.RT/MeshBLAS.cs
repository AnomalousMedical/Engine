using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public class MeshBLAS : IDisposable
    {
        private BLASInstance instance;

        public BLASInstance Instance => instance;

        public uint NumIndices => numIndices;

        private uint numIndices = 0;
        private uint currentVert = 0;
        private uint indexBlock = 0;
        private uint currentIndex = 0;

        private readonly BLASDesc blasDesc = new BLASDesc();
        private readonly BLASBuilder blasBuilder;

        public MeshBLAS(BLASBuilder blasBuilder)
        {
            this.blasBuilder = blasBuilder;
        }

        public void Dispose()
        {
            instance?.Dispose();
        }

        public void Begin(uint numQuads)
        {
            var NumVertices = numQuads * 4;
            numIndices = numQuads * 6;

            blasDesc.CubePos = new Vector3[NumVertices];
            blasDesc.CubeUV = new Vector4[NumVertices];
            blasDesc.CubeNormals = new Vector4[NumVertices];
            blasDesc.Indices = new UInt32[numIndices];
        }

        public void AddQuad(in Vector3 topLeft, in Vector3 topRight, in Vector3 bottomRight, in Vector3 bottomLeft, in Vector3 topLeftNormal, in Vector3 topRightNormal, in Vector3 bottomRightNormal, in Vector3 bottomLeftNormal, in Vector2 uvTopLeft, in Vector2 uvBottomRight)
        {
            blasDesc.CubePos[currentVert] = topLeft;
            blasDesc.CubeNormals[currentVert] = new Vector4(topLeftNormal);
            blasDesc.CubeUV[currentVert] = new Vector4(uvTopLeft.x, uvTopLeft.y, 0, 0);

            ++currentVert;
            blasDesc.CubePos[currentVert] = topRight;
            blasDesc.CubeNormals[currentVert] = new Vector4(topRightNormal);
            blasDesc.CubeUV[currentVert] = new Vector4(uvBottomRight.x, uvTopLeft.y, 0, 0);

            ++currentVert;
            blasDesc.CubePos[currentVert] = bottomRight;
            blasDesc.CubeNormals[currentVert] = new Vector4(bottomRightNormal);
            blasDesc.CubeUV[currentVert] = new Vector4(uvBottomRight.x, uvBottomRight.y, 0, 0);

            ++currentVert;
            blasDesc.CubePos[currentVert] = bottomLeft;
            blasDesc.CubeNormals[currentVert] = new Vector4(bottomLeftNormal);
            blasDesc.CubeUV[currentVert] = new Vector4(uvTopLeft.x, uvBottomRight.y, 0, 0);

            ++currentVert;

            //Add Index
            blasDesc.Indices[currentIndex] = indexBlock;
            blasDesc.Indices[currentIndex + 1] = indexBlock + 1;
            blasDesc.Indices[currentIndex + 2] = indexBlock + 2;

            blasDesc.Indices[currentIndex + 3] = indexBlock + 2;
            blasDesc.Indices[currentIndex + 4] = indexBlock + 3;
            blasDesc.Indices[currentIndex + 5] = indexBlock;

            indexBlock += 4;
            currentIndex += 6;
        }

        public unsafe void End()
        {
            instance = blasBuilder.CreateBLAS(blasDesc);
        }
    }
}

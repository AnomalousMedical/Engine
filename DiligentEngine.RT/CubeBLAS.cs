using DiligentEngine;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT
{
    public class CubeBLAS : IDisposable
    {
        private BLASInstance instance;

        public BLASInstance Instance => instance;

        public unsafe CubeBLAS(BLASBuilder blasBuilder)
        {
            var blasDesc = new BLASDesc();

            //blasDesc.CubePos = new Vector3[]
            //{
            //    new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,+0.5f,-0.5f), new Vector3(+0.5f,+0.5f,-0.5f)
            //};

            //blasDesc.CubeUV = new Vector4[]
            //{
            //    new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0)
            //};

            //blasDesc.CubeNormals = new Vector4[]
            //{
            //    new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0)
            //};

            //blasDesc.Indices = new uint[]
            //{
            //    2,0,1
            //};

            blasDesc.CubePos = new Vector3[]
            {
                new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,+0.5f,-0.5f), new Vector3(+0.5f,+0.5f,-0.5f), new Vector3(+0.5f,-0.5f,-0.5f), //Back -z
                new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,-0.5f,+0.5f), new Vector3(+0.5f,-0.5f,+0.5f), new Vector3(+0.5f,-0.5f,-0.5f), //Top -y
                new Vector3(+0.5f,-0.5f,-0.5f), new Vector3(+0.5f,-0.5f,+0.5f), new Vector3(+0.5f,+0.5f,+0.5f), new Vector3(+0.5f,+0.5f,-0.5f), //Left +x
                new Vector3(+0.5f,+0.5f,-0.5f), new Vector3(+0.5f,+0.5f,+0.5f), new Vector3(-0.5f,+0.5f,+0.5f), new Vector3(-0.5f,+0.5f,-0.5f), //Bottom +y
                new Vector3(-0.5f,+0.5f,-0.5f), new Vector3(-0.5f,+0.5f,+0.5f), new Vector3(-0.5f,-0.5f,+0.5f), new Vector3(-0.5f,-0.5f,-0.5f), //Right -x
                new Vector3(-0.5f,-0.5f,+0.5f), new Vector3(+0.5f,-0.5f,+0.5f), new Vector3(+0.5f,+0.5f,+0.5f), new Vector3(-0.5f,+0.5f,+0.5f), //Front +z
            };

            blasDesc.CubeUV = new Vector4[]
            {
                new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0), new Vector4(1,0,0,0), //Back -z
                new Vector4(1,0,0,0), new Vector4(1,1,0,0), new Vector4(0,1,0,0), new Vector4(0,0,0,0), //Top -y
                new Vector4(0,0,0,0), new Vector4(1,0,0,0), new Vector4(1,1,0,0), new Vector4(0,1,0,0), //Left +x
                new Vector4(1,0,0,0), new Vector4(1,1,0,0), new Vector4(0,1,0,0), new Vector4(0,0,0,0), //Bottom +y
                new Vector4(1,1,0,0), new Vector4(0,1,0,0), new Vector4(0,0,0,0), new Vector4(1,0,0,0), //Right -x
                new Vector4(1,0,0,0), new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0)  //Front +z
            };

            blasDesc.CubeNormals = new Vector4[]
            {
                new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), new Vector4(0, 0, -1, 0), //Back -z
                new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), new Vector4(0, -1, 0, 0), //Top -y
                new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), new Vector4(+1, 0, 0, 0), //Left +x
                new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), new Vector4(0, +1, 0, 0), //Bottom +y
                new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), new Vector4(-1, 0, 0, 0), //Right -x
                new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0)  //Front +z
            };

            blasDesc.Indices = new uint[]
            {
                2,0,1,    2,3,0, //Back -z
                4,6,5,    4,7,6,
                8,10,9,   8,11,10,
                12,14,13, 12,15,14,
                16,18,17, 16,19,18,
                20,21,22, 20,22,23  //Front +z
            };

            instance = blasBuilder.CreateBLAS(blasDesc);
        }

        public void Dispose()
        {
            instance.Dispose();
        }
    }
}

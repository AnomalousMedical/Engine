﻿//using Engine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DiligentEngine.RT.Sprites
//{
//    public class SpriteInstanceFactory
//    {
//        private readonly BLASBuilder blasBuilder;
//        private readonly SpriteMaterialManager spriteMaterialManager;

//        public SpriteInstanceFactory(BLASBuilder blasBuilder, SpriteMaterialManager spriteMaterialManager)
//        {
//            this.blasBuilder = blasBuilder;
//            this.spriteMaterialManager = spriteMaterialManager;
//        }

//        public async Task<SpriteInstance> CreateSprite()
//        {
//            //This is in the diligent coords
//            var blasDesc = new BLASDesc();

//            blasDesc.CubePos = new Vector3[]
//            {
//                new Vector3(-0.5f,-0.5f,+0.0f), new Vector3(+0.5f,-0.5f,+0.0f), new Vector3(+0.5f,+0.5f,+0.0f), new Vector3(-0.5f,+0.5f,+0.0f), //Front +z
//            };

//            blasDesc.CubeUV = new Vector4[]
//            {
//                new Vector4(1,0,0,0), new Vector4(0,0,0,0), new Vector4(0,1,0,0), new Vector4(1,1,0,0)  //Front +z
//            };

//            blasDesc.CubeNormals = new Vector4[]
//            {
//                new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0), new Vector4(0, 0, +1, 0)  //Front +z
//            };

//            blasDesc.Indices = new uint[]
//            {
//                0,1,2, 0,2,3  //Front +z
//            };

//            var setupShader = primaryHitShader.Setup(blasDesc.Name, 5);
//            instance = await blasBuilder.CreateBLAS(blasDesc);
//            await setupShader;
//        }
//    }
//}

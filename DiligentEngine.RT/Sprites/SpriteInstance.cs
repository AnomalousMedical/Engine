using DiligentEngine;
using DiligentEngine.RT.Resources;
using DiligentEngine.RT.ShaderSets;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    public class SpriteInstance : IDisposable
    {
        private readonly SpriteMaterial spriteMaterial;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private readonly ActiveTextures activeTextures;
        private readonly SpritePlaneBLAS spritePlaneBLAS;
        private PrimaryHitShader primaryHitShader;
        private readonly PrimaryHitShader.Factory primaryHitShaderFactory;
        private HLSL.BlasInstanceData blasInstanceData;

        public BLASInstance Instance => spritePlaneBLAS.Instance;

        public SpriteInstance
        (
            SpritePlaneBLAS spritePlaneBLAS,
            PrimaryHitShader primaryHitShader,
            PrimaryHitShader.Factory primaryHitShaderFactory,
            SpriteMaterial spriteMaterial,
            ISpriteMaterialManager spriteMaterialManager,
            ActiveTextures activeTextures
        )
        {
            this.spritePlaneBLAS = spritePlaneBLAS;
            this.primaryHitShader = primaryHitShader;
            this.primaryHitShaderFactory = primaryHitShaderFactory;
            this.spriteMaterial = spriteMaterial;
            this.spriteMaterialManager = spriteMaterialManager;
            this.activeTextures = activeTextures;
            blasInstanceData = this.activeTextures.AddActiveTexture(spriteMaterial);
        }

        public void Dispose()
        {
            this.activeTextures.RemoveActiveTexture(spriteMaterial);
            primaryHitShaderFactory.TryReturn(primaryHitShader);
            spriteMaterialManager.Return(spriteMaterial);
        }

        public unsafe void Bind(String instanceName, IShaderBindingTable sbt, ITopLevelAS tlas, SpriteFrame frame)
        {
            blasInstanceData.vertexOffset = spritePlaneBLAS.Instance.VertexOffset;
            blasInstanceData.indexOffset = spritePlaneBLAS.Instance.IndexOffset;
            blasInstanceData.u1 = frame.Right; blasInstanceData.v1 = frame.Top;
            blasInstanceData.u2 = frame.Left; blasInstanceData.v2 = frame.Top;
            blasInstanceData.u3 = frame.Left; blasInstanceData.v3 = frame.Bottom;
            blasInstanceData.u4 = frame.Right; blasInstanceData.v4 = frame.Bottom;
            fixed (HLSL.BlasInstanceData* ptr = &this.blasInstanceData)
            {
                primaryHitShader.BindSbt(instanceName, sbt, tlas, new IntPtr(ptr), (uint)sizeof(HLSL.BlasInstanceData));
            }
        }
    }
}

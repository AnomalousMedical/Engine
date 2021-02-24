using DiligentEngine;
using DiligentEngine.GltfPbr;
using Engine;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class CC0TextureManager : PooledResourceManager<IShaderResourceBinding>, ICC0TextureManager
    {
        private readonly CC0TextureLoader textureLoader;
        private readonly PbrRenderer pbrRenderer;
        private readonly IPbrCameraAndLight pbrCameraAndLight;

        public CC0TextureManager(
            CC0TextureLoader textureLoader,
            PbrRenderer pbrRenderer,
            IPbrCameraAndLight pbrCameraAndLight,
            ILogger<PooledResourceManager<IShaderResourceBinding>> logger)
            :base(logger)
        {
            this.textureLoader = textureLoader;
            this.pbrRenderer = pbrRenderer;
            this.pbrCameraAndLight = pbrCameraAndLight;
        }

        protected override async Task<CreateResult> Create(string baseName)
        {
            AutoPtr<IShaderResourceBinding> result = null;
            await Task.Run(() =>
            {
                using var ccoTextures = textureLoader.LoadTextureSet(baseName);
                result = pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    baseColorMap: ccoTextures.BaseColorMap,
                    normalMap: ccoTextures.NormalMap,
                    physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                    aoMap: ccoTextures.AmbientOcclusionMap
                );
            });
            return new CreateResult()
            {
                disposable = result,
                pooled = result.Obj,
            };
        }
    }
}

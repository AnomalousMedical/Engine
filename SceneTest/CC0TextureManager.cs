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
    public class CCOTextureBindingDescription
    {
        public CCOTextureBindingDescription()
        {
        }

        public CCOTextureBindingDescription(string baseName, bool getShadow = false)
        {
            this.BaseName = baseName;
            this.GetShadow = getShadow;
        }

        public String BaseName { get; }

        public bool GetShadow { get; }

        public override bool Equals(object obj)
        {
            return obj is CCOTextureBindingDescription description &&
                   BaseName == description.BaseName &&
                   GetShadow == description.GetShadow;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BaseName, GetShadow);
        }
    }

    class CC0TextureManager : ICC0TextureManager
    {
        private PooledResourceManager<CCOTextureBindingDescription, IShaderResourceBinding> pooledResources = new PooledResourceManager<CCOTextureBindingDescription, IShaderResourceBinding>();
        private readonly CC0TextureLoader textureLoader;
        private readonly PbrRenderer pbrRenderer;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly SimpleShadowMapRenderer shadowMapRenderer;

        public CC0TextureManager(
            CC0TextureLoader textureLoader,
            PbrRenderer pbrRenderer,
            IPbrCameraAndLight pbrCameraAndLight,
            SimpleShadowMapRenderer shadowMapRenderer
            )
        {
            this.textureLoader = textureLoader;
            this.pbrRenderer = pbrRenderer;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.shadowMapRenderer = shadowMapRenderer;
        }

        public Task<IShaderResourceBinding> Checkout(CCOTextureBindingDescription desc)
        {
            return pooledResources.Checkout(desc, async () =>
            {
                AutoPtr<IShaderResourceBinding> result = null;
                await Task.Run(() =>
                {
                    using var ccoTextures = textureLoader.LoadTextureSet(desc.BaseName);
                    result = pbrRenderer.CreateMaterialSRB(
                        pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                        pLightAttribs: pbrCameraAndLight.LightAttribs,
                        baseColorMap: ccoTextures.BaseColorMap,
                        normalMap: ccoTextures.NormalMap,
                        physicalDescriptorMap: ccoTextures.PhysicalDescriptorMap,
                        aoMap: ccoTextures.AmbientOcclusionMap,
                        shadowMapSRV: desc.GetShadow ? shadowMapRenderer.ShadowMapSRV : null
                    );
                });
                return pooledResources.CreateResult(result.Obj, result);
            });
        }

        public void Return(IShaderResourceBinding binding)
        {
            pooledResources.Return(binding);
        }

        public void TryReturn(IShaderResourceBinding binding)
        {
            if(binding != null)
            {
                Return(binding);
            }
        }
    }
}

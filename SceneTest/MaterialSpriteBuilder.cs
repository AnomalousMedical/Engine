using DiligentEngine;
using DiligentEngine.GltfPbr;
using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    public class MaterialSpriteBindingDescription
    {
        public String ColorMap { get; set; }

        public Dictionary<uint, (String basePath, String ext)> Materials { get; set; }
}

    class MaterialSpriteBuilder : IMaterialSpriteBuilder
    {
        private readonly ICC0MaterialTextureBuilder cc0MaterialTextureBuilder;
        private readonly IResourceProvider<MaterialSpriteBuilder> resourceProvider;
        private readonly TextureLoader textureLoader;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly PbrRenderer pbrRenderer;

        public MaterialSpriteBuilder(
            ICC0MaterialTextureBuilder cc0MaterialTextureBuilder,
            IResourceProvider<MaterialSpriteBuilder> resourceProvider,
            TextureLoader textureLoader,
            IPbrCameraAndLight pbrCameraAndLight,
            PbrRenderer pbrRenderer
        )
        {
            this.cc0MaterialTextureBuilder = cc0MaterialTextureBuilder;
            this.resourceProvider = resourceProvider;
            this.textureLoader = textureLoader;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.pbrRenderer = pbrRenderer;
        }

        public Task<AutoPtr<IShaderResourceBinding>> CreateSpriteAsync(MaterialSpriteBindingDescription desc)
        {
            return Task.Run(() =>
            {
                using var stream =
                            resourceProvider.openFile(desc.ColorMap);
                using var image = FreeImageBitmap.FromStream(stream);

                var scale = Math.Min(1024 / image.Width, 1024 / image.Height);

                using var ccoTextures = cc0MaterialTextureBuilder.CreateMaterialSet(image, scale, desc.Materials);

                using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

                using var normalTexture = ccoTextures.NormalMap != null ?
                    textureLoader.CreateTextureFromImage(ccoTextures.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                using var physicalTexture = ccoTextures.PhysicalDescriptorMap != null ?
                    textureLoader.CreateTextureFromImage(ccoTextures.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                using var aoTexture = ccoTextures.AmbientOcclusionMap != null ?
                    textureLoader.CreateTextureFromImage(ccoTextures.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                return pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    baseColorMap: colorTexture?.Obj,
                    normalMap: normalTexture?.Obj,
                    physicalDescriptorMap: physicalTexture?.Obj,
                    aoMap: aoTexture?.Obj,
                    alphaMode: PbrAlphaMode.ALPHA_MODE_MASK,
                    isSprite: true
                );
            });
        }
    }
}

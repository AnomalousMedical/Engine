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
    public class MaterialSpriteMaterialDescription
    {
        public uint Color { get; }
        public String BasePath { get; }
        public String Ext { get; }

        public MaterialSpriteMaterialDescription(uint color, string basePath, string ext)
        {
            this.Color = color;
            this.BasePath = basePath;
            this.Ext = ext;
        }

        public override bool Equals(object obj)
        {
            return obj is MaterialSpriteMaterialDescription description &&
                   Color == description.Color &&
                   BasePath == description.BasePath &&
                   Ext == description.Ext;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, BasePath, Ext);
        }
    }

    public class MaterialSpriteBindingDescription
    {
        public MaterialSpriteBindingDescription(string colorMap, HashSet<MaterialSpriteMaterialDescription> materials)
        {
            ColorMap = colorMap;
            Materials = materials;
        }

        public String ColorMap { get; }

        public HashSet<MaterialSpriteMaterialDescription> Materials { get; }

        public override bool Equals(object obj)
        {
            return obj is MaterialSpriteBindingDescription description &&
                   ColorMap == description.ColorMap &&
                   EqualityComparer<HashSet<MaterialSpriteMaterialDescription>>.Default.Equals(Materials, description.Materials);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(ColorMap);
            if (Materials != null)
            {
                foreach (var mat in Materials.OrderBy(i => i.Color))
                {
                    hashCode.Add(mat);
                }
            }
            else
            {
                hashCode.Add<Object>(null);
            }
            return hashCode.ToHashCode();
        }
    }

    class MaterialSpriteBuilder : IMaterialSpriteBuilder
    {
        private readonly ICC0MaterialTextureBuilder cc0MaterialTextureBuilder;
        private readonly IResourceProvider<MaterialSpriteBuilder> resourceProvider;
        private readonly TextureLoader textureLoader;
        private readonly IPbrCameraAndLight pbrCameraAndLight;
        private readonly PbrRenderer pbrRenderer;
        private readonly ISpriteMaterialTextureManager spriteMaterialTextureManager;

        public MaterialSpriteBuilder(
            ICC0MaterialTextureBuilder cc0MaterialTextureBuilder,
            IResourceProvider<MaterialSpriteBuilder> resourceProvider,
            TextureLoader textureLoader,
            IPbrCameraAndLight pbrCameraAndLight,
            PbrRenderer pbrRenderer,
            ISpriteMaterialTextureManager spriteMaterialTextureManager
        )
        {
            this.cc0MaterialTextureBuilder = cc0MaterialTextureBuilder;
            this.resourceProvider = resourceProvider;
            this.textureLoader = textureLoader;
            this.pbrCameraAndLight = pbrCameraAndLight;
            this.pbrRenderer = pbrRenderer;
            this.spriteMaterialTextureManager = spriteMaterialTextureManager;
        }

        public async Task<ISpriteMaterial> CreateSpriteMaterialAsync(MaterialSpriteBindingDescription desc)
        {
            using var image = await Task.Run(() =>
            {
                using var stream = resourceProvider.openFile(desc.ColorMap);
                return FreeImageBitmap.FromStream(stream);
            });

            //Order is important here, image is flipped when the texture is created below
            //Also need to get back onto the main thread to lookup the textures in the other manager
            //This makes it take multiple frames, but living with that for now
            //Ideally this should go back into 1 task.run to run at full speed, but this manager only
            //has main thread sync support
            var spriteMatTextures = await spriteMaterialTextureManager.Checkout(image, new SpriteMaterialTextureDescription(desc.ColorMap, desc.Materials));

            return await Task.Run(() =>
            {
                using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);

                var srb = pbrRenderer.CreateMaterialSRB(
                    pCameraAttribs: pbrCameraAndLight.CameraAttribs,
                    pLightAttribs: pbrCameraAndLight.LightAttribs,
                    baseColorMap: colorTexture?.Obj,
                    normalMap: spriteMatTextures.NormalTexture,
                    physicalDescriptorMap: spriteMatTextures.PhysicalTexture,
                    aoMap: spriteMatTextures.AoTexture,
                    alphaMode: PbrAlphaMode.ALPHA_MODE_MASK,
                    isSprite: true
                );

                return new SpriteMaterial(srb, image.Width, image.Height, spriteMaterialTextureManager, spriteMatTextures);
            });
        }
    }
}

using DiligentEngine;
using Engine;
using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.RT.Sprites
{
    public class SpriteMaterialTextureItem
    {
        public uint Color { get; }
        public String BasePath { get; }
        public String Ext { get; }

        public SpriteMaterialTextureItem(uint color, string basePath, string ext)
        {
            this.Color = color;
            this.BasePath = basePath;
            this.Ext = ext;
        }

        public override bool Equals(object obj)
        {
            return obj is SpriteMaterialTextureItem description &&
                   Color == description.Color &&
                   BasePath == description.BasePath &&
                   Ext == description.Ext;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, BasePath, Ext);
        }
    }

    public class SpriteMaterialDescription
    {
        public SpriteMaterialDescription(string colorMap, HashSet<SpriteMaterialTextureItem> materials)
        {
            ColorMap = colorMap;
            Materials = materials;
        }

        public String ColorMap { get; }

        public HashSet<SpriteMaterialTextureItem> Materials { get; }

        public override bool Equals(object obj)
        {
            return obj is SpriteMaterialDescription description &&
                   ColorMap == description.ColorMap &&
                    (
                        (Materials == null && description.Materials == null) ||
                        (Materials?.SetEquals(description.Materials) == true)
                    );
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(ColorMap);
            if (Materials != null && Materials.Count > 0) //Null and empty considered the same
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

    class SpriteMaterialManager : ISpriteMaterialManager
    {
        private readonly PooledResourceManager<SpriteMaterialDescription, ISpriteMaterial> pooledResources
            = new PooledResourceManager<SpriteMaterialDescription, ISpriteMaterial>();

        private readonly IResourceProvider<SpriteMaterialManager> resourceProvider;
        private readonly TextureLoader textureLoader;
        private readonly ISpriteMaterialTextureManager spriteMaterialTextureManager;

        public SpriteMaterialManager(
            IResourceProvider<SpriteMaterialManager> resourceProvider,
            TextureLoader textureLoader,
            ISpriteMaterialTextureManager spriteMaterialTextureManager
        )
        {
            this.resourceProvider = resourceProvider;
            this.textureLoader = textureLoader;
            this.spriteMaterialTextureManager = spriteMaterialTextureManager;
        }

        public Task<ISpriteMaterial> Checkout(SpriteMaterialDescription desc)
        {
            return pooledResources.Checkout(desc, async () =>
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
                    using var colorTexture = textureLoader.CreateTextureFromImage(image, 1, "colorTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D, false);

                    var result = new SpriteMaterial(image.Width, image.Height, spriteMaterialTextureManager, spriteMatTextures, colorTexture.Obj);
                    return pooledResources.CreateResult(result);
                });
            });
        }

        public void TryReturn(ISpriteMaterial item)
        {
            if (item != null)
            {
                Return(item);
            }
        }

        public void Return(ISpriteMaterial item)
        {
            pooledResources.Return(item);
        }
    }
}

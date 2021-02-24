using DiligentEngine;
using DiligentEngine.GltfPbr;
using Engine;
using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    public class SpriteMaterialTextureDescription
    {
        public SpriteMaterialTextureDescription(string baseMap, HashSet<MaterialSpriteMaterialDescription> materials)
        {
            BaseMap = baseMap;
            Materials = materials;
        }

        String BaseMap { get; }

        public HashSet<MaterialSpriteMaterialDescription> Materials { get; }

        public override bool Equals(object obj)
        {
            return obj is SpriteMaterialTextureDescription description &&
                   BaseMap == description.BaseMap &&
                   EqualityComparer<HashSet<MaterialSpriteMaterialDescription>>.Default.Equals(Materials, description.Materials);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(BaseMap);
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

    class SpriteMaterialTextures : IDisposable
    {
        AutoPtr<ITexture> normalTexture;
        AutoPtr<ITexture> physicalTexture;
        AutoPtr<ITexture> aoTexture;

        public SpriteMaterialTextures(AutoPtr<ITexture> normalTexture, AutoPtr<ITexture> physicalTexture, AutoPtr<ITexture> aoTexture)
        {
            this.normalTexture = normalTexture;
            this.physicalTexture = physicalTexture;
            this.aoTexture = aoTexture;
        }

        public void Dispose()
        {
            normalTexture?.Dispose();
            physicalTexture?.Dispose();
            aoTexture?.Dispose();
        }


        public ITexture NormalTexture => normalTexture?.Obj;
        public ITexture PhysicalTexture => physicalTexture?.Obj;
        public ITexture AoTexture => aoTexture?.Obj;
    }

    class SpriteMaterialTextureManager : ISpriteMaterialTextureManager
    {
        private readonly PooledResourceManager<SpriteMaterialTextureDescription, SpriteMaterialTextures> pooledResources
            = new PooledResourceManager<SpriteMaterialTextureDescription, SpriteMaterialTextures>();

        private readonly ICC0MaterialTextureBuilder cc0MaterialTextureBuilder;
        private readonly TextureLoader textureLoader;

        public SpriteMaterialTextureManager(
             ICC0MaterialTextureBuilder cc0MaterialTextureBuilder,
             TextureLoader textureLoader
            )
        {
            this.cc0MaterialTextureBuilder = cc0MaterialTextureBuilder;
            this.textureLoader = textureLoader;
        }

        public Task<SpriteMaterialTextures> Checkout(FreeImageBitmap image, SpriteMaterialTextureDescription desc)
        {
            return pooledResources.Checkout(desc, () =>
            {
                return Task.Run(() =>
                {
                    var scale = Math.Min(1024 / image.Width, 1024 / image.Height); //This needs to become configurable

                    using var ccoTextures = cc0MaterialTextureBuilder.CreateMaterialSet(image, scale, desc.Materials?.ToDictionary(k => k.Color, e => (e.BasePath, e.Ext)));

                    var normalTexture = ccoTextures.NormalMap != null ?
                        textureLoader.CreateTextureFromImage(ccoTextures.NormalMap, 1, "normalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                    var physicalTexture = ccoTextures.PhysicalDescriptorMap != null ?
                        textureLoader.CreateTextureFromImage(ccoTextures.PhysicalDescriptorMap, 1, "physicalTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                    var aoTexture = ccoTextures.AmbientOcclusionMap != null ?
                        textureLoader.CreateTextureFromImage(ccoTextures.AmbientOcclusionMap, 1, "aoTexture", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false) : null;

                    var result = new SpriteMaterialTextures(normalTexture, physicalTexture, aoTexture);

                    return pooledResources.CreateResult(result);
                });
            });
        }

        public void Return(SpriteMaterialTextures binding)
        {
            pooledResources.Return(binding);
        }
    }
}

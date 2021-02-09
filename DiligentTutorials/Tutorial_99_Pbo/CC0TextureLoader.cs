using DiligentEngine;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial_99_Pbo
{
    /// <summary>
    /// This loader can load the textures from https://cc0textures.com. It will reformat for the gltf renderer.
    /// </summary>
    public class CC0TextureResult : IDisposable
    {
        private AutoPtr<ITexture> baseColorMap;
        private AutoPtr<ITexture> normalMap;
        private AutoPtr<ITexture> physicalDescriptorMap;

        internal CC0TextureResult()
        {
        }

        public void Dispose()
        {
            baseColorMap?.Dispose();
            normalMap?.Dispose();
            physicalDescriptorMap?.Dispose();
        }

        public ITexture BaseColorMap => baseColorMap?.Obj;
        internal void SetBaseColorMap(AutoPtr<ITexture> value)
        {
            this.baseColorMap = value;
        }

        public ITexture NormalMap => normalMap?.Obj;
        internal void SetNormalMap(AutoPtr<ITexture> value)
        {
            this.normalMap = value;
        }
        public ITexture PhysicalDescriptorMap => physicalDescriptorMap?.Obj;
        internal void SetPhysicalDescriptorMap(AutoPtr<ITexture> value)
        {
            this.physicalDescriptorMap = value;
        }

    }

    class CC0TextureLoader
    {
        private readonly TextureLoader textureLoader;

        public CC0TextureLoader(TextureLoader textureLoader)
        {
            this.textureLoader = textureLoader;
        }

        public CC0TextureResult LoadTextureSet(String basePath, String ext = "jpg")
        {
            //In this function the auto pointers are handed off to the result, which will be managed by the caller to erase the resources.
            var result = new CC0TextureResult();

            var colorMapPath = $"{basePath}_Color.{ext}";
            var normalMapPath = $"{basePath}_Normal.{ext}";
            var roughnessMapPath = $"{basePath}_Roughness.{ext}";
            var metalnessMapPath = $"{basePath}_Metalness.{ext}";

            if (File.Exists(colorMapPath))
            {
                using (var baseColorStream = File.Open(colorMapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var baseColorMap = textureLoader.LoadTexture(baseColorStream, "baseColorMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
                    result.SetBaseColorMap(baseColorMap);
                }
            }

            if (File.Exists(normalMapPath))
            {
                using (var normalStream = File.Open(normalMapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var normalMap = textureLoader.LoadTexture(normalStream, "baseColorMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
                    result.SetNormalMap(normalMap);
                }
            }
            {
                FreeImageBitmap roughnessBmp = null;
                FreeImageBitmap metalnessBmp = null;
                try
                {
                    if (File.Exists(roughnessMapPath))
                    {
                        using var roughnessStream = File.Open(roughnessMapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        roughnessBmp = FreeImageBitmap.FromStream(roughnessStream);
                    }

                    if (File.Exists(metalnessMapPath))
                    {
                        using var metalnessStream = File.Open(metalnessMapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        metalnessBmp = FreeImageBitmap.FromStream(metalnessStream);
                    }

                    FreeImageBitmap physicalDescriptorBmp = null; //Just a pointer, won't need its own disposal
                    if (roughnessBmp != null && metalnessBmp != null)
                    {
                        roughnessBmp.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP);
                        roughnessBmp.SetChannel(metalnessBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_BLUE);
                        physicalDescriptorBmp = roughnessBmp;
                    }
                    else if(metalnessBmp != null)
                    {
                        metalnessBmp.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP);
                        physicalDescriptorBmp = metalnessBmp;
                    }
                    else if(roughnessBmp != null)
                    {
                        roughnessBmp.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP);
                        physicalDescriptorBmp = roughnessBmp;
                    }

                    if (physicalDescriptorBmp != null)
                    {
                        physicalDescriptorBmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        var physicalDescriptorMap = textureLoader.CreateTextureFromImage(physicalDescriptorBmp, 1, "physicalDescriptorMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
                        result.SetPhysicalDescriptorMap(physicalDescriptorMap);
                    }
                }
                finally
                {
                    roughnessBmp?.Dispose();
                    metalnessBmp?.Dispose();
                }

            }

            return result;
        }
    }
}

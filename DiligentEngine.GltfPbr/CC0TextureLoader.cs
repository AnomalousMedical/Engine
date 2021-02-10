using DiligentEngine;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    /// <summary>
    /// This loader can load the textures from https://cc0textures.com. It will reformat for the gltf renderer.
    /// </summary>
    public class CC0TextureLoader
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
            var ambientOcclusionMapPath = $"{basePath}_AmbientOcclusion.{ext}";

            if (File.Exists(colorMapPath))
            {
                using (var stream = File.Open(colorMapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var baseColorMap = textureLoader.LoadTexture(stream, "baseColorMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
                    result.SetBaseColorMap(baseColorMap);
                }
            }

            if (File.Exists(normalMapPath))
            {
                using (var stream = File.Open(normalMapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var normalMap = textureLoader.LoadTexture(stream, "normalMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
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
                        using var stream = File.Open(roughnessMapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        roughnessBmp = FreeImageBitmap.FromStream(stream);
                    }

                    if (File.Exists(metalnessMapPath))
                    {
                        using var stream = File.Open(metalnessMapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        metalnessBmp = FreeImageBitmap.FromStream(stream);
                    }

                    if (roughnessBmp != null || metalnessBmp != null)
                    {
                        int width = 0;
                        int height = 0;

                        if(roughnessBmp != null)
                        {
                            width = roughnessBmp.Width;
                            height = roughnessBmp.Height;
                        }

                        if(metalnessBmp != null)
                        {
                            width = metalnessBmp.Width;
                            height = metalnessBmp.Height;
                        }

                        using var physicalDescriptorBmp = new FreeImageBitmap(width, height, PixelFormat.Format32bppArgb);
                        physicalDescriptorBmp.FillBackground(0);
                        if (metalnessBmp != null)
                        {
                            physicalDescriptorBmp.SetChannel(metalnessBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_BLUE);
                        }
                        if (roughnessBmp != null)
                        {
                            physicalDescriptorBmp.SetChannel(roughnessBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_GREEN);
                        }
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

            if (File.Exists(ambientOcclusionMapPath))
            {
                using (var stream = File.Open(normalMapPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var map = textureLoader.LoadTexture(stream, "ambientOcclusionMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
                    result.SetAmbientOcclusionMap(map);
                }
            }

            return result;
        }
    }
}

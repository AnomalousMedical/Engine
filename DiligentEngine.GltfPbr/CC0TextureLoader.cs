using DiligentEngine;
using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
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
        private readonly IResourceProvider<CC0TextureLoader> resourceProvider;

        public CC0TextureLoader(TextureLoader textureLoader, IResourceProvider<CC0TextureLoader> resourceProvider)
        {
            this.textureLoader = textureLoader;
            this.resourceProvider = resourceProvider;
        }

        public CC0TextureResult LoadTextureSet(String basePath, String ext = "jpg", string colorPath = null, string colorExt = null)
        {
            //In this function the auto pointers are handed off to the result, which will be managed by the caller to erase the resources.
            var result = new CC0TextureResult();

            var colorMapPath = $"{colorPath ?? basePath}_Color.{colorExt ?? ext}";
            var normalMapPath = $"{basePath}_Normal.{ext}";
            var roughnessMapPath = $"{basePath}_Roughness.{ext}";
            var metalnessMapPath = $"{basePath}_Metalness.{ext}";
            var ambientOcclusionMapPath = $"{basePath}_AmbientOcclusion.{ext}";

            if (resourceProvider.fileExists(colorMapPath))
            {
                using (var stream = resourceProvider.openFile(colorMapPath))
                {
                    var baseColorMap = textureLoader.LoadTexture(stream, "baseColorMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
                    result.SetBaseColorMap(baseColorMap);
                }
            }

            if (resourceProvider.fileExists(normalMapPath))
            {
                using (var stream = resourceProvider.openFile(normalMapPath))
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
                    if (resourceProvider.fileExists(roughnessMapPath))
                    {
                        using var stream = resourceProvider.openFile(roughnessMapPath);
                        roughnessBmp = FreeImageBitmap.FromStream(stream);
                    }

                    if (resourceProvider.fileExists(metalnessMapPath))
                    {
                        using var stream = resourceProvider.openFile(metalnessMapPath);
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
                        unsafe 
                        { 
                            var firstPixel = ((uint*)physicalDescriptorBmp.Scan0.ToPointer()) - ((physicalDescriptorBmp.Height - 1) * physicalDescriptorBmp.Width);
                            var size = physicalDescriptorBmp.Width * physicalDescriptorBmp.Height;
                            var span = new Span<UInt32>(firstPixel, size);
                            span.Fill(PbrRenderer.DefaultPhysical);
                        }
                        if (metalnessBmp != null)
                        {
                            physicalDescriptorBmp.SetChannel(metalnessBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_BLUE);
                        }
                        if (roughnessBmp != null)
                        {
                            physicalDescriptorBmp.SetChannel(roughnessBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_GREEN);
                        }

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

            if (resourceProvider.fileExists(ambientOcclusionMapPath))
            {
                using (var stream = resourceProvider.openFile(normalMapPath))
                {
                    var map = textureLoader.LoadTexture(stream, "ambientOcclusionMap", RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D_ARRAY, false);
                    result.SetAmbientOcclusionMap(map);
                }
            }

            return result;
        }
    }
}

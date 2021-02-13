﻿using Engine;
using Engine.Resources;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    public class CC0MaterialTextureBuffers : IDisposable
    {
        private FreeImageBitmap normalMap;
        private FreeImageBitmap physicalDescriptorMap;
        private FreeImageBitmap ambientOcclusionMap;

        internal CC0MaterialTextureBuffers()
        {
        }

        public void Dispose()
        {
            normalMap?.Dispose();
            physicalDescriptorMap?.Dispose();
            ambientOcclusionMap?.Dispose();
        }

        public FreeImageBitmap NormalMap => normalMap;
        internal void SetNormalMap(FreeImageBitmap value)
        {
            this.normalMap = value;
        }
        public FreeImageBitmap PhysicalDescriptorMap => physicalDescriptorMap;
        internal void SetPhysicalDescriptorMap(FreeImageBitmap value)
        {
            this.physicalDescriptorMap = value;
        }
        public FreeImageBitmap AmbientOcclusionMap => ambientOcclusionMap;
        internal void SetAmbientOcclusionMap(FreeImageBitmap value)
        {
            this.ambientOcclusionMap = value;
        }
    }

    /// <summary>
    /// This class builds a texture out of other cc0 textures.
    /// </summary>
    public class CC0MaterialTextureBuilder : ICC0MaterialTextureBuilder
    {
        private readonly TextureLoader textureLoader;
        private readonly IResourceProvider<CC0MaterialTextureBuilder> resourceProvider;

        public CC0MaterialTextureBuilder(TextureLoader textureLoader, IResourceProvider<CC0MaterialTextureBuilder> resourceProvider)
        {
            this.textureLoader = textureLoader;
            this.resourceProvider = resourceProvider;
        }

        public CC0MaterialTextureBuffers CreateMaterialSet(FreeImageBitmap indexImage, int scale, Dictionary<uint, (String basePath, String ext)> materialIds)
        {
            if (materialIds.Count == 0)
            {
                throw new InvalidOperationException("No material ids defined.");
            }

            var destSize = new IntSize2(indexImage.Width * scale, indexImage.Height * scale);
            var result = new CC0MaterialTextureBuffers();
            var materialSets = new Dictionary<string, CC0MaterialTextureBuffers>();
            try
            {
                foreach (var textureSet in materialIds.Values)
                {
                    var textures = LoadTextureSet(textureSet.basePath, textureSet.ext ?? "jpg");
                    materialSets.Add(textureSet.basePath, textures);

                    var exampleSet = materialSets.First().Value;
                    var example = exampleSet.NormalMap ?? exampleSet.PhysicalDescriptorMap ?? exampleSet.AmbientOcclusionMap;

                    unsafe
                    {
                        var indexScan0 = (UInt32*)indexImage.Scan0.ToPointer();
                        for (var y = 0; y < indexImage.Height; ++y)
                        {
                            var scanline = indexScan0 - y * indexImage.Width;
                            for (var x = 0; x < indexImage.Width; ++x)
                            {
                                if (materialIds.TryGetValue(scanline[x], out var mat)
                                 && materialSets.TryGetValue(mat.basePath, out var matSet))
                                {
                                    var scaledX = x * scale;
                                    var scaledY = y * scale;
                                    SetMaterialPixel(scaledX, scaledY, scale, scale, matSet, result, destSize);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                foreach (var textureSet in materialSets.Values)
                {
                    textureSet.Dispose();
                }
            }

            return result;
        }

        private CC0MaterialTextureBuffers LoadTextureSet(String basePath, String ext)
        {
            //In this function the auto pointers are handed off to the result, which will be managed by the caller to erase the resources.
            var result = new CC0MaterialTextureBuffers();

            var normalMapPath = $"{basePath}_Normal.{ext}";
            var roughnessMapPath = $"{basePath}_Roughness.{ext}";
            var metalnessMapPath = $"{basePath}_Metalness.{ext}";
            var ambientOcclusionMapPath = $"{basePath}_AmbientOcclusion.{ext}";

            if (resourceProvider.fileExists(normalMapPath))
            {
                using (var stream = resourceProvider.openFile(normalMapPath))
                {
                    var map = FreeImageBitmap.FromStream(stream);
                    map.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP); //Cheat and convert color depth
                    result.SetNormalMap(map); //Pointer handled by returned collection
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

                        if (roughnessBmp != null)
                        {
                            width = roughnessBmp.Width;
                            height = roughnessBmp.Height;
                        }

                        if (metalnessBmp != null)
                        {
                            width = metalnessBmp.Width;
                            height = metalnessBmp.Height;
                        }

                        var physicalDescriptorBmp = new FreeImageBitmap(width, height, PixelFormat.Format32bppArgb);
                        physicalDescriptorBmp.FillBackground(PbrRenderer.DefaultPhysical);
                        if (metalnessBmp != null)
                        {
                            physicalDescriptorBmp.SetChannel(metalnessBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_BLUE);
                        }
                        if (roughnessBmp != null)
                        {
                            physicalDescriptorBmp.SetChannel(roughnessBmp, FREE_IMAGE_COLOR_CHANNEL.FICC_GREEN);
                        }

                        result.SetPhysicalDescriptorMap(physicalDescriptorBmp); //Pointer handled by returned collection
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
                    var map = FreeImageBitmap.FromStream(stream);
                    map.ConvertColorDepth(FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP); //Cheat and convert color depth
                    result.SetAmbientOcclusionMap(map); //Pointer handled by returned collection
                }
            }

            return result;
        }

        private void SetMaterialPixel(int x, int y, int width, int height, CC0MaterialTextureBuffers src, CC0MaterialTextureBuffers dest, IntSize2 destSize)
        {
            if (src.NormalMap != null)
            {
                if (dest.NormalMap == null)
                {
                    var bmp = new FreeImageBitmap(destSize.Width, destSize.Height, src.NormalMap.PixelFormat);
                    bmp.FillBackground(PbrRenderer.DefaultNormal);
                    dest.SetNormalMap(bmp);
                }

                CopyPixelsWrapped(x, y, width, height, src.NormalMap, dest.NormalMap);
            }

            if (src.PhysicalDescriptorMap != null)
            {
                if (dest.PhysicalDescriptorMap == null)
                {
                    var bmp = new FreeImageBitmap(destSize.Width, destSize.Height, src.PhysicalDescriptorMap.PixelFormat);
                    bmp.FillBackground(PbrRenderer.DefaultPhysical);
                    dest.SetPhysicalDescriptorMap(bmp);
                }

                CopyPixelsWrapped(x, y, width, height, src.PhysicalDescriptorMap, dest.PhysicalDescriptorMap);
            }

            if (src.AmbientOcclusionMap != null)
            {
                if (dest.AmbientOcclusionMap == null)
                {
                    var bmp = new FreeImageBitmap(destSize.Width, destSize.Height, src.AmbientOcclusionMap.PixelFormat);
                    bmp.FillBackground(0xffffffff);
                    dest.SetAmbientOcclusionMap(bmp);
                }

                CopyPixelsWrapped(x, y, width, height, src.AmbientOcclusionMap, dest.AmbientOcclusionMap);
            }
        }

        private void CopyPixelsWrapped(int x, int y, int width, int height, FreeImageBitmap src, FreeImageBitmap dest)
        {
            //Coordinates come in as if the dest was mapped over the source, this is also inifinte
            //Normalize the location to within the first source square (or just outside it if overlapping some)
            var srcX = x % src.Width;
            var srcY = y % src.Height;

            var xEnd = srcX + width;
            var yEnd = srcY + height;

            //Does width and height fit in the current size?
            if (xEnd < src.Width && yEnd < src.Height)
            {
                var srcRect = new IntRect(srcX, srcY, width, height);
                var destRect = new IntRect(x, y, width, height);
                CopyPixels(srcRect, destRect, src, dest);
            }
            //Does width fit
            else if (xEnd < src.Width)
            {
                //Copy height we can
                var srcRect = new IntRect(srcX, srcY, width, src.Height);
                var destRect = new IntRect(x, y, width, src.Height);
                CopyPixels(srcRect, destRect, src, dest);
                //Wrap around and copy the rest
                var remaining = height - src.Height;
                srcRect = new IntRect(srcX, 0, width, remaining);
                destRect = new IntRect(x, y + src.Height, width, remaining);
                CopyPixels(srcRect, destRect, src, dest);
            }
            //Does height fit
            else if (yEnd < src.Height)
            {

            }
            //Neither dimension fits
            else
            {

            }
        }

        private unsafe void CopyPixels(IntRect srcRect, IntRect destRect, FreeImageBitmap src, FreeImageBitmap dest)
        {
            var pSrc = (uint*)src.Scan0.ToPointer();
            var pDest = (uint*)dest.Scan0.ToPointer();

            var xEnd = srcRect.Width;
            var yEnd = srcRect.Height;

            //Does width and height fit in the current size?
            if (srcRect.Width >= src.Width && srcRect.Height >= src.Height)
            {
                throw new InvalidOperationException($"Rect: {srcRect} exceeds the source image size of {src.Width}, {src.Height}.");
            }

            for (var i = 0; i < srcRect.Height; ++i)
            {
                var spSrc  = new Span<uint>(pSrc  - ((srcRect.Top  + i) * src.Width)  + srcRect.Left,  srcRect.Width);
                var spDest = new Span<uint>(pDest - ((destRect.Top + i) * dest.Width) + destRect.Left, destRect.Width);

                spSrc.CopyTo(spDest);
                Console.WriteLine($"{i}, {i * src.Width} {destRect.Left}");
            }
        }
    }
}
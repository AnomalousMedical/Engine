using System;
using System.Collections.Generic;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;
using FreeImageAPI;
using System.IO;

namespace DiligentEngine
{
    public class TextureLoader
    {
        private readonly GraphicsEngine graphicsEngine;

        public TextureLoader(GraphicsEngine graphicsEngine)
        {
            this.graphicsEngine = graphicsEngine;
        }

        TEXTURE_FORMAT GetFormat(FreeImageBitmap bitmap, bool isSRGB)
        {
            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    return isSRGB ? TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM_SRGB : TEXTURE_FORMAT.TEX_FORMAT_BGRA8_UNORM;

                default:
                    return TEXTURE_FORMAT.TEX_FORMAT_UNKNOWN;
            }
        }

        public AutoPtr<ITexture> LoadTexture(Stream stream)
        {
            using (var bmp = FreeImageBitmap.FromStream(stream))
            {
                //THIS SUCKS - Rotating in memory, but only way for now, need to figure out how to read backward.
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                //TextureLoadInfo loadInfo;
                //loadInfo.IsSRGB = true;
                return CreateTextureFromImage(bmp, 1);
            }
        }

        /// <summary>
        /// This function loads stuff, but its really incomplete.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="MipLevels"></param>
        /// <param name="pDevice"></param>
        /// <returns></returns>
        public AutoPtr<ITexture> CreateTextureFromImage(FreeImageBitmap bitmap, int MipLevels)
        {
            uint width = (uint)bitmap.Width;
            uint height = (uint)bitmap.Height;

            //const auto& ImgDesc = pSrcImage->GetDesc();
            TextureDesc TexDesc = new TextureDesc();
            //TexDesc.Name      = TexLoadInfo.Name;
            TexDesc.Type = RESOURCE_DIMENSION.RESOURCE_DIM_TEX_2D;
            TexDesc.Width = width;
            TexDesc.Height = height;
            TexDesc.MipLevels = ComputeMipLevelsCount(TexDesc.Width, TexDesc.Height);
            if (MipLevels > 0)
            {
                TexDesc.MipLevels = (uint)Math.Min(TexDesc.MipLevels, MipLevels);
            }
            TexDesc.Usage = USAGE.USAGE_IMMUTABLE;
            TexDesc.BindFlags = BIND_FLAGS.BIND_SHADER_RESOURCE;
            TexDesc.Format = GetFormat(bitmap, true); //bool IsSRGB = true;// (ImgDesc.NumComponents >= 3 && ChannelDepth == 8) ? TexLoadInfo.IsSRGB : false;
            TexDesc.CPUAccessFlags = CPU_ACCESS_FLAGS.CPU_ACCESS_NONE;

            Uint32 NumComponents = 4;// ImgDesc.NumComponents == 3 ? 4 : ImgDesc.NumComponents;            
            //else
            //{
            //    const auto& TexFmtDesc = GetTextureFormatAttribs(TexDesc.Format);
            //    if (TexFmtDesc.NumComponents != NumComponents)
            //        LOG_ERROR_AND_THROW("Incorrect number of components ", ImgDesc.NumComponents, ") for texture format ", TexFmtDesc.Name);
            //    if (TexFmtDesc.ComponentSize != ChannelDepth / 8)
            //        LOG_ERROR_AND_THROW("Incorrect channel size ", ChannelDepth, ") for texture format ", TexFmtDesc.Name);
            //}


            var pSubResources = new List<TextureSubResData>(MipLevels);
            //std::vector<std::vector<Uint8>> Mips(MipLevels);

            if (NumComponents == 3)
            {
                //VERIFY_EXPR(NumComponents == 4);
                //auto RGBAStride = ImgDesc.Width * NumComponents * ChannelDepth / 8;
                //RGBAStride = (RGBAStride + 3) & (-4);
                //Mips[0].resize(size_t{ RGBAStride}
                //*size_t{ ImgDesc.Height});
                //pSubResources[0].pData = Mips[0].data();
                //pSubResources[0].Stride = RGBAStride;
                //if (ChannelDepth == 8)
                //{
                //    RGBToRGBA<Uint8>(pSrcImage->GetData()->GetDataPtr(), ImgDesc.RowStride,
                //                     Mips[0].data(), RGBAStride,
                //                     ImgDesc.Width, ImgDesc.Height);
                //}
                //else if (ChannelDepth == 16)
                //{
                //    RGBToRGBA<Uint16>(pSrcImage->GetData()->GetDataPtr(), ImgDesc.RowStride,
                //                      Mips[0].data(), RGBAStride,
                //                      ImgDesc.Width, ImgDesc.Height);
                //}
            }
            else
            {
                if (bitmap.Stride > 0)
                {
                    pSubResources.Add(new TextureSubResData()
                    {
                        pData = bitmap.Scan0,
                        Stride = (Uint32)(bitmap.Stride),
                    });
                }
                else
                {
                    //Freeimage scan0 gives the last line for some reason, this gives the first to allow the negative scan to become positive
                    var stride = -bitmap.Stride;
                    pSubResources.Add(new TextureSubResData()
                    {
                        pData = bitmap.Scan0 - (stride * (bitmap.Height - 1)),
                        Stride = (Uint32)stride,
                    });
                }
            }

            //Mip maps
            //var MipWidth = TexDesc.Width;
            //var MipHeight = TexDesc.Height;
            //for (Uint32 m = 1; m < TexDesc.MipLevels; ++m)
            //{
            //    var CoarseMipWidth = Math.Max(MipWidth / 2u, 1u);
            //    var CoarseMipHeight = Math.Max(MipHeight / 2u, 1u);
            //    var CoarseMipStride = CoarseMipWidth * NumComponents * ChannelDepth / 8;
            //    CoarseMipStride = (CoarseMipStride + 3) & (-4);
            //    Mips[m].resize(size_t{ CoarseMipStride} *size_t{ CoarseMipHeight});

            //    if (TexLoadInfo.GenerateMips)
            //    {
            //        ComputeMipLevel(MipWidth, MipHeight, TexDesc.Format,
            //                        pSubResources[m - 1].pData, pSubResources[m - 1].Stride,
            //                        Mips[m].data(), CoarseMipStride);
            //    }

            //    pSubResources[m].pData = Mips[m].data();
            //    pSubResources[m].Stride = CoarseMipStride;

            //    MipWidth = CoarseMipWidth;
            //    MipHeight = CoarseMipHeight;
            //}

            TextureData TexData = new TextureData();
            TexData.pSubResources = pSubResources;

            return graphicsEngine.RenderDevice.CreateTexture(TexDesc, TexData); //This does not do anything with this pointer, just pass it along and let the caller handle it
        }

        Uint32 ComputeMipLevelsCount(Uint32 Width, Uint32 Height)
        {
            return ComputeMipLevelsCount(Math.Max(Width, Height));
        }

        Uint32 ComputeMipLevelsCount(Uint32 Width)
        {
            if (Width == 0)
                return 0;

            int MipLevels = 0; //Was Uint32, but c# cannot do that
            while ((Width >> MipLevels) > 0)
            {
                ++MipLevels;
            }
            //VERIFY(Width >= (1U << (MipLevels - 1)) && Width < (1U << MipLevels), "Incorrect number of Mip levels");
            return (Uint32)MipLevels;
        }
    }
}

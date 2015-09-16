﻿using Engine;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    /// <summary>
    /// Simple image format, stores all pages for a texture flat in a file then stores index data and finally the size of the index.
    /// So the files look mostly like the following:
    /// Image1 Image2 Image3 Image1Info Image2Info Image3Info IndexInfo
    /// Image is an image in a format that freeimage can load (probably png).
    /// 
    /// ImageXInfo looks like the following:
    /// ImageStart - int - Index into image array for image source data in bytes
    /// ImageSize - int - The size of the image in bytes
    /// 
    /// IndexInfo looks like the following:
    /// numImages - int - the number of images in the file
    /// ImageType - int - the type of the image (png jpeg etc).
    /// ImageXSize - int - the size of the image
    /// ImageYSize - int - the size of the image
    /// PageSize - int - the size of the pages stored in this image
    /// IndexStart - int - where the index starts in the file.
    /// </summary>
    public class PagedImage : IDisposable
    {
        public const String FileExtension = ".ptex";

        enum ImageType
        {
            PNG = 0
        }

        private int numImages;
        private int imageType;
        private int imageXSize;
        private int imageYSize;
        private int pageSize;
        private int indexStart;
        private const int HeaderSize = sizeof(int) * 6;

        private MemoryBlock memoryBlock;
        private List<ImageInfo> pages;
        private List<MipIndexInfo> mipIndices;

        public PagedImage()
        {

        }

        public void Dispose()
        {
            if (memoryBlock != null)
            {
                memoryBlock.Dispose();
                memoryBlock = null;
            }
        }

        /// <summary>
        /// Create a paged image from a FreeImageBitmap instance, note that this function is destructive
        /// to the passed in FreeImageBitmap since it will resize it while creating all the mip levels
        /// for the paged image. However, you will still need to dispose the image you pass in, this class
        /// makes a copy while destroying the image, but does not take ownership of the original.
        /// </summary>
        /// <param name="image">The image to extract pages from.</param>
        /// <param name="pageSize">The size of the pages to extract.</param>
        public static void fromBitmap(FreeImageBitmap image, int pageSize, int padding, Stream stream)
        {
            PagedImage pagedImage = new PagedImage();
            int padding2x = padding * 2;

            //Kind of weird, ogre and freeimage are backwards from one another in terms of scanline 0 being the top or bottom
            //This easily fixes the math below by just flipping the image first. Note that pages must be flipped back over because
            //of this when they are saved.
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pagedImage.numImages = 0;
            pagedImage.imageType = (int)ImageType.PNG;
            pagedImage.imageXSize = image.Width;
            pagedImage.imageYSize = image.Height;
            pagedImage.pageSize = pageSize;
            if (pagedImage.imageXSize != pagedImage.imageYSize)
            {
                throw new InvalidDataException("Image must be a square");
            }

            int mipLevelCount = 0;
            int numPages = 0;
            for (int i = pagedImage.imageXSize; i >= pageSize; i >>= 1)
            {
                ++mipLevelCount;
                int pagesForMip = i / pageSize;
                numPages += pagesForMip * pagesForMip;
            }
            pagedImage.pages = new List<ImageInfo>(numPages);
            pagedImage.mipIndices = new List<MipIndexInfo>(mipLevelCount);

            bool topSide, leftSide, rightSide, bottomSide;
            IntRect imageRect = new IntRect();

            using (FreeImageBitmap page = new FreeImageBitmap(pageSize + padding2x, pageSize + padding2x, FreeImageAPI.PixelFormat.Format32bppArgb))
            {
                for (int mip = 0; mip < mipLevelCount; ++mip)
                {
                    //Setup mip level
                    if (mip != 0)
                    {
                        image.Rescale(image.Width >> 1, image.Height >> 1, FREE_IMAGE_FILTER.FILTER_BILINEAR);
                    }
                    int size = image.Width / pageSize;
                    pagedImage.mipIndices.Add(new MipIndexInfo(pagedImage.numImages, size));

                    //Calculate pages, note that there is overlap by padding between them this is intentional
                    for (int y = 0; y < size; ++y)
                    {
                        imageRect.Height = page.Height;
                        imageRect.Top = y * pageSize - padding;
                        topSide = imageRect.Top < 0;
                        if (topSide)
                        {
                            imageRect.Top = 0;
                            imageRect.Height -= padding;
                        }

                        bottomSide = imageRect.Bottom > image.Height;
                        if (bottomSide)
                        {
                            if (topSide)
                            {
                                //Take entire image
                                imageRect.Top = 0;
                                imageRect.Height = image.Height;
                            }
                            else
                            {
                                //Extra row on top, bottom flush with texture bottom will add extra pixel row on bottom will add extra pixel row on bottom below
                                imageRect.Top = image.Height - pageSize - padding;
                                imageRect.Height -= padding;
                            }
                        }

                        for (int x = 0; x < size; ++x)
                        {
                            imageRect.Width = page.Width;
                            imageRect.Left = x * pageSize - padding;
                            leftSide = imageRect.Left < 0;
                            if (leftSide)
                            {
                                imageRect.Left = 0;
                                imageRect.Width -= padding;
                            }

                            rightSide = imageRect.Right > image.Width;
                            if (rightSide)
                            {
                                if (leftSide)
                                {
                                    //Take entire image
                                    imageRect.Left = 0;
                                    imageRect.Width = image.Width;
                                }
                                else
                                {
                                    //Extra row on left, right flush with texture right will add extra pixel row on right below
                                    imageRect.Left = image.Width - pageSize - padding;
                                    imageRect.Width -= padding;
                                }
                            }

                            using (var pageBox = page.createPixelBox(PixelFormat.PF_A8R8G8B8))
                            {
                                if (topSide)
                                {
                                    pageBox.Top += (uint)padding;
                                }

                                if (bottomSide)
                                {
                                    pageBox.Bottom -= (uint)padding;
                                }

                                if (leftSide)
                                {
                                    pageBox.Left += (uint)padding;
                                }

                                if (rightSide)
                                {
                                    pageBox.Right -= (uint)padding;
                                }

                                using (var imageBox = image.createPixelBox())
                                {
                                    imageBox.Left = (uint)imageRect.Left;
                                    imageBox.Right = (uint)imageRect.Right;
                                    imageBox.Top = (uint)imageRect.Top;
                                    imageBox.Bottom = (uint)imageRect.Bottom;

                                    PixelBox.BulkPixelConversion(imageBox, pageBox);
                                }

                                if (topSide)
                                {
                                    using (PixelBox altSrcBox = image.createPixelBox())
                                    {
                                        pageBox.Top = 0;
                                        pageBox.Bottom = (uint)padding;
                                        pageBox.Left = (uint)padding;
                                        pageBox.Right = (uint)(page.Width - padding);

                                        altSrcBox.Top = (uint)imageRect.Top;
                                        altSrcBox.Bottom = (uint)(imageRect.Top + padding);
                                        altSrcBox.Left = (uint)imageRect.Left;
                                        altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                                        PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                                    }
                                }

                                if (bottomSide)
                                {
                                    using (PixelBox altSrcBox = image.createPixelBox())
                                    {
                                        pageBox.Top = (uint)(page.Height - padding);
                                        pageBox.Bottom = (uint)page.Height;
                                        pageBox.Left = (uint)padding;
                                        pageBox.Right = (uint)(page.Width - padding);

                                        altSrcBox.Top = (uint)(imageRect.Bottom - padding);
                                        altSrcBox.Bottom = (uint)imageRect.Bottom;
                                        altSrcBox.Left = (uint)imageRect.Left;
                                        altSrcBox.Right = (uint)(imageRect.Left + pageSize);

                                        PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                                    }
                                }

                                if (leftSide)
                                {
                                    using (PixelBox altSrcBox = image.createPixelBox())
                                    {
                                        pageBox.Top = 1;
                                        pageBox.Bottom = (uint)(page.Height - padding);
                                        pageBox.Left = 0;
                                        pageBox.Right = (uint)padding;

                                        altSrcBox.Top = (uint)imageRect.Top;
                                        altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                                        altSrcBox.Left = (uint)imageRect.Left;
                                        altSrcBox.Right = (uint)(imageRect.Left + padding);

                                        PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                                    }
                                }

                                if (rightSide)
                                {
                                    using (PixelBox altSrcBox = image.createPixelBox())
                                    {
                                        pageBox.Top = 1;
                                        pageBox.Bottom = (uint)(page.Height - padding);
                                        pageBox.Left = (uint)(page.Width - padding);
                                        pageBox.Right = (uint)page.Width;

                                        altSrcBox.Top = (uint)imageRect.Top;
                                        altSrcBox.Bottom = (uint)(imageRect.Top + pageSize);
                                        altSrcBox.Left = (uint)(imageRect.Right - padding);
                                        altSrcBox.Right = (uint)imageRect.Right;

                                        PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                                    }
                                }
                            }
                            int startPos = (int)stream.Position;
                            page.RotateFlip(RotateFlipType.RotateNoneFlipY); //Have to flip the page over for ogre to be happy
                            page.Save(stream, FREE_IMAGE_FORMAT.FIF_PNG);
                            ++pagedImage.numImages;
                            pagedImage.pages.Add(new ImageInfo(startPos, (int)(stream.Position)));
                        }
                    }
                }
            }
            pagedImage.indexStart = (int)stream.Position;

            using (BinaryWriter sw = new BinaryWriter(stream, Encoding.Default, true))
            {
                foreach (var imageInfo in pagedImage.pages)
                {
                    imageInfo.write(sw);
                }
                sw.Write(pagedImage.numImages);
                sw.Write(pagedImage.imageType);
                sw.Write(pagedImage.imageXSize);
                sw.Write(pagedImage.imageYSize);
                sw.Write(pagedImage.pageSize);
                sw.Write(pagedImage.indexStart);
            }
        }

        /// <summary>
        /// Load the PagedImage from a stream source.
        /// </summary>
        /// <param name="source"></param>
        public void load(Stream source)
        {
            if (memoryBlock != null)
            {
                memoryBlock.Dispose();
            }
            memoryBlock = new MemoryBlock(source);

            long headerBegin = memoryBlock.Length - HeaderSize;
            using (BinaryReader headerReader = new BinaryReader(memoryBlock.getSubStream(headerBegin, memoryBlock.Length)))
            {
                numImages = headerReader.ReadInt32();
                imageType = headerReader.ReadInt32();
                imageXSize = headerReader.ReadInt32();
                imageYSize = headerReader.ReadInt32();
                pageSize = headerReader.ReadInt32();
                indexStart = headerReader.ReadInt32();
            }

            using (BinaryReader pageIndexReader = new BinaryReader(memoryBlock.getSubStream(indexStart, headerBegin)))
            { 
                pages = new List<ImageInfo>(numImages);
                mipIndices = new List<MipIndexInfo>();

                int pageStride = imageXSize / pageSize;
                int pagesForLevel = pageStride * pageStride;

                int mipPage = 0;
                mipIndices.Add(new MipIndexInfo(0, pageStride));
                for (int i = 0; i < numImages; ++i)
                {
                    if (mipPage == pagesForLevel)
                    {
                        pageStride >>= 1;
                        pagesForLevel >>= 2;
                        mipPage = 0;
                        mipIndices.Add(new MipIndexInfo(i, pageStride));
                    }
                    pages.Add(new ImageInfo(pageIndexReader.ReadInt32(), pageIndexReader.ReadInt32()));
                    ++mipPage;
                }
            }
        }

        /// <summary>
        /// Load the PagedImage attributes only from a stream source. You won't be able to actually use the image
        /// loaded this way but you will be able to see its attribtues (width, height etc).
        /// </summary>
        /// <param name="source"></param>
        public void loadInfoOnly(Stream source)
        {
            if (memoryBlock != null)
            {
                memoryBlock.Dispose();
            }
            memoryBlock = null;

            using (BinaryReader sr = new BinaryReader(source, Encoding.Default, true))
            {
                sr.BaseStream.Seek(-HeaderSize, SeekOrigin.End);
                numImages = sr.ReadInt32();
                imageType = sr.ReadInt32();
                imageXSize = sr.ReadInt32();
                imageYSize = sr.ReadInt32();
                pageSize = sr.ReadInt32();
                indexStart = sr.ReadInt32();
            }
        }

        /// <summary>
        /// Get a FreeImageBitmap for the page specified by x, y, mip, you must dispose the returned image.
        /// </summary>
        /// <returns>A FreeImageBitmap the caller takes ownership of.</returns>
        public Image getImage(int x, int y, int mip)
        {
            MipIndexInfo mipIndex = mipIndices[mip];
            ImageInfo imageInfo = pages[mipIndex.getIndex(x, y)];
            using (Stream imageStream = imageInfo.openStream(memoryBlock))
            {
                //As of right now we are always png, but if you add more formats deal with that here
                Image image = new Image();
                image.load(imageStream, "png");
                return image;
            }
        }

        public ulong Size
        {
            get
            {
                return (ulong)memoryBlock.Length;
            }
        }

        public int Width
        {
            get
            {
                return imageXSize;
            }
        }

        public int Height
        {
            get
            {
                return imageYSize;
            }
        }

        class MipIndexInfo
        {
            private int startIndex;
            private int pageStride; //The amount of pages in one row

            public MipIndexInfo(int startIndex, int pageStride)
            {
                this.startIndex = startIndex;
                this.pageStride = pageStride;
            }

            public int getIndex(int x, int y)
            {
                return startIndex + y * pageStride + x;
            }
        }

        class ImageInfo
        {
            private int imageStart;
            private int imageEnd;

            public ImageInfo(int imageStart, int imageEnd)
            {
                this.imageStart = imageStart;
                this.imageEnd = imageEnd;
            }

            public Stream openStream(MemoryBlock sourceBytes)
            {
                return sourceBytes.getSubStream(imageStart, imageEnd);
            }

            public void write(BinaryWriter sw)
            {
                sw.Write(imageStart);
                sw.Write(imageEnd);
            }
        }
    }
}

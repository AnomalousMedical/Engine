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
        public const String FileExtension = ".pgpng";

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
        private const int HeaderSize = -sizeof(int) * 6;

        private MemoryStream stream;
        private List<ImageInfo> pages;
        private List<MipIndexInfo> mipIndices;

        public PagedImage()
        {

        }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
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
        public void fromBitmap(FreeImageBitmap image, int pageSize, int padding)
        {
            int padding2x = padding * 2;

            if (stream != null)
            {
                stream.Dispose();
            }

            //Kind of weird, ogre and freeimage are backwards from one another in terms of scanline 0 being the top or bottom
            //This easily fixes the math below by just flipping the image first.
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            this.numImages = 0;
            this.imageType = (int)ImageType.PNG;
            this.imageXSize = image.Width;
            this.imageYSize = image.Height;
            this.pageSize = pageSize;
            if (imageXSize != imageYSize)
            {
                throw new InvalidDataException("Image must be a square");
            }

            int mipLevelCount = 0;
            int numPages = 0;
            for (int i = imageXSize; i >= pageSize; i >>= 1)
            {
                ++mipLevelCount;
                int pagesForMip = i / pageSize;
                numPages += pagesForMip * pagesForMip;
            }
            stream = new MemoryStream(image.DataSize); //Probably way too much, but decent capactiy guess
            pages = new List<ImageInfo>(numPages);
            mipIndices = new List<MipIndexInfo>(mipLevelCount);

            bool topSide, leftSide, rightSide, bottomSide;

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
                    mipIndices.Add(new MipIndexInfo(numImages, size));

                    using (var imageBox = image.createPixelBox())
                    {
                        //Extract pages
                        for (int y = 0; y < size; ++y)
                        {
                            imageBox.Top = (uint)(y * pageSize - padding);
                            topSide = imageBox.Top < 0;
                            if (topSide)
                            {
                                imageBox.Top = 0;
                            }

                            imageBox.Bottom = imageBox.Top + (uint)(pageSize + padding2x);
                            bottomSide = imageBox.Bottom > image.Height;
                            if(bottomSide)
                            {
                                imageBox.Bottom = (uint)image.Height;
                                imageBox.Top = (uint)(imageBox.Bottom - pageSize - padding);
                            }

                            for (int x = 0; x < size; ++x)
                            {
                                imageBox.Left = (uint)(x * pageSize);
                                leftSide = imageBox.Left < 0;
                                if(leftSide)
                                {
                                    imageBox.Left = 0;
                                }

                                imageBox.Right = imageBox.Left + (uint)(pageSize + padding2x);
                                rightSide = imageBox.Right > image.Width;
                                if(rightSide)
                                {
                                    imageBox.Right = (uint)image.Width;
                                    imageBox.Left = (uint)(imageBox.Right - pageSize - padding);
                                }

                                using (var pageBox = page.createPixelBox(PixelFormat.PF_A8R8G8B8))
                                {
                                    if(topSide)
                                    {
                                        pageBox.Top += (uint)padding;
                                    }

                                    if (bottomSide)
                                    {
                                        pageBox.Bottom -= (uint)padding;
                                    }

                                    if(leftSide)
                                    {
                                        pageBox.Left += (uint)padding;
                                    }

                                    if(rightSide)
                                    {
                                        pageBox.Right -= (uint)padding;
                                    }

                                    PixelBox.BulkPixelConversion(imageBox, pageBox);

                                    if (topSide)
                                    {
                                        using (PixelBox altSrcBox = image.createPixelBox())
                                        {
                                            pageBox.Top = 0;
                                            pageBox.Bottom = (uint)padding;
                                            pageBox.Left = (uint)padding;
                                            pageBox.Right = (uint)(page.Width - padding);

                                            altSrcBox.Top = imageBox.Top;
                                            altSrcBox.Bottom = (uint)(imageBox.Top + padding);
                                            altSrcBox.Left = imageBox.Left;
                                            altSrcBox.Right = imageBox.Right;

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

                                            altSrcBox.Top = (uint)(imageBox.Bottom - padding);
                                            altSrcBox.Bottom = (uint)imageBox.Bottom;
                                            altSrcBox.Left = imageBox.Left;
                                            altSrcBox.Right = imageBox.Right;

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

                                            altSrcBox.Top = imageBox.Top;
                                            altSrcBox.Bottom = imageBox.Bottom;
                                            altSrcBox.Left = imageBox.Left;
                                            altSrcBox.Right = (uint)(imageBox.Left + padding);

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

                                            altSrcBox.Top = imageBox.Top;
                                            altSrcBox.Bottom = imageBox.Bottom;
                                            altSrcBox.Left = (uint)(imageBox.Right - padding);
                                            altSrcBox.Right = imageBox.Right;

                                            PixelBox.BulkPixelConversion(altSrcBox, pageBox);
                                        }
                                    }
                                }
                                int startPos = (int)stream.Position;
                                page.Save(stream, FREE_IMAGE_FORMAT.FIF_PNG);
                                ++numImages;
                                pages.Add(new ImageInfo(startPos, (int)(stream.Position - startPos)));
                            }
                        }
                    }
                }
            }
            indexStart = (int)stream.Position;
        }

        /// <summary>
        /// Load the PagedImage from a stream source.
        /// </summary>
        /// <param name="source"></param>
        public void load(Stream source)
        {
            if (stream != null)
            {
                stream.Dispose();
            }
            stream = new MemoryStream((int)source.Length);
            source.CopyTo(stream);
            using (BinaryReader sr = new BinaryReader(source, Encoding.Default, true))
            {
                sr.BaseStream.Seek(HeaderSize, SeekOrigin.End);
                numImages = sr.ReadInt32();
                imageType = sr.ReadInt32();
                imageXSize = sr.ReadInt32();
                imageYSize = sr.ReadInt32();
                pageSize = sr.ReadInt32();
                indexStart = sr.ReadInt32();

                pages = new List<ImageInfo>(numImages);
                mipIndices = new List<MipIndexInfo>();

                int pageStride = imageXSize / pageSize;
                int pagesForLevel = pageStride * pageStride;

                sr.BaseStream.Seek(indexStart, SeekOrigin.Begin);
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
                    pages.Add(new ImageInfo(sr.ReadInt32(), sr.ReadInt32()));
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
            if (stream != null)
            {
                stream.Dispose();
            }
            stream = null;

            using (BinaryReader sr = new BinaryReader(source, Encoding.Default, true))
            {
                sr.BaseStream.Seek(HeaderSize, SeekOrigin.End);
                numImages = sr.ReadInt32();
                imageType = sr.ReadInt32();
                imageXSize = sr.ReadInt32();
                imageYSize = sr.ReadInt32();
                pageSize = sr.ReadInt32();
                indexStart = sr.ReadInt32();
            }
        }

        public void save(Stream outputStream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(outputStream);
            using (BinaryWriter sw = new BinaryWriter(outputStream, Encoding.Default, true))
            {
                foreach (var imageInfo in pages)
                {
                    imageInfo.write(sw);
                }
                sw.Write(numImages);
                sw.Write(imageType);
                sw.Write(imageXSize);
                sw.Write(imageYSize);
                sw.Write(pageSize);
                sw.Write(indexStart);
            }
        }

        /// <summary>
        /// Get a pixelbox for the page specified by x, y, mip, you must dispose the returned pixel box.
        /// </summary>
        /// <returns>A pixel box the caller takes ownership of.</returns>
        public FreeImageBitmap getImage(int x, int y, int mip)
        {
            MipIndexInfo mipIndex = mipIndices[mip];
            ImageInfo imageInfo = pages[mipIndex.getIndex(x, y)];
            using (Stream imageStream = imageInfo.openStream(stream.GetBuffer()))
            {
                return new FreeImageBitmap(imageStream);
            }
        }

        public ulong Size
        {
            get
            {
                return (ulong)stream.Length;
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
            private int imageLength;

            public ImageInfo(int imageStart, int imageLength)
            {
                this.imageStart = imageStart;
                this.imageLength = imageLength;
            }

            public Stream openStream(byte[] sourceBytes)
            {
                return new MemoryStream(sourceBytes, imageStart, imageLength, false);
            }

            public void write(BinaryWriter sw)
            {
                sw.Write(imageStart);
                sw.Write(imageLength);
            }
        }
    }
}

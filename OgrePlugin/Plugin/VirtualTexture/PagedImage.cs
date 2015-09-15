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
        public void fromBitmap(FreeImageBitmap image, int pageSize)
        {
            if (stream != null)
            {
                stream.Dispose();
            }

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

            using (FreeImageBitmap page = new FreeImageBitmap(pageSize, pageSize, FreeImageAPI.PixelFormat.Format32bppArgb))
            {
                for (int mip = 0; mip < mipLevelCount; ++mip)
                {
                    if (mip != 0)
                    {
                        image.Rescale(image.Width >> 1, image.Height >> 1, FREE_IMAGE_FILTER.FILTER_BILINEAR);
                    }
                    int size = image.Width / pageSize;
                    mipIndices.Add(new MipIndexInfo(numImages, size * size));
                    using (var imageBox = image.createPixelBox())
                    {
                        for (int y = 0; y < size; ++y)
                        {
                            imageBox.Top = (uint)((size - 1 - y) * pageSize);
                            imageBox.Bottom = imageBox.Top + (uint)pageSize;
                            for (int x = 0; x < size; ++x)
                            {
                                imageBox.Left = (uint)(x * pageSize);
                                imageBox.Right = imageBox.Left + (uint)pageSize;
                                using (var pageBox = page.createPixelBox(PixelFormat.PF_A8R8G8B8))
                                {
                                    PixelBox.BulkPixelConversion(imageBox, pageBox);
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
            stream.Seek(HeaderSize, SeekOrigin.End);
            using (BinaryReader sr = new BinaryReader(source, Encoding.Default, true))
            {
                numImages = sr.ReadInt32();
                imageType = sr.ReadInt32();
                imageXSize = sr.ReadInt32();
                imageYSize = sr.ReadInt32();
                pageSize = sr.ReadInt32();
                indexStart = sr.ReadInt32();

                pages = new List<ImageInfo>(numImages);
                mipIndices = new List<MipIndexInfo>();

                int pagesForLevel = imageXSize * imageYSize / pageSize;

                stream.Seek(indexStart, SeekOrigin.Begin);
                int mipPage = 0;
                mipIndices.Add(new MipIndexInfo(0, pagesForLevel));
                for (int i = 0; i < numImages; ++i)
                {
                    if (mipPage > pagesForLevel)
                    {
                        pagesForLevel >>= 2;
                        mipPage = 0;
                        mipIndices.Add(new MipIndexInfo(i, pagesForLevel));
                    }
                    pages.Add(new ImageInfo(sr.ReadInt32(), sr.ReadInt32()));
                    ++mipPage;
                }
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

        private static void debug_saveImage(FreeImageBitmap page, String fileName)
        {
            using (Stream test = File.Open(fileName, FileMode.Create))
            {
                page.Save(test, FREE_IMAGE_FORMAT.FIF_PNG);
            }
        }

        /// <summary>
        /// Get a pixelbox for the page specified by x, y, mip, you must dispose the returned pixel box.
        /// </summary>
        /// <returns>A pixel box the caller takes ownership of.</returns>
        public PixelBox getImage(int x, int y, int mip)
        {
            MipIndexInfo mipIndex = mipIndices[mip];
            ImageInfo imageInfo = pages[mipIndex.getIndex(x, y)];
            using (Stream imageStream = imageInfo.openStream(stream.GetBuffer()))
            {
                FreeImageBitmap fiBmp = new FreeImageBitmap(imageStream); //dispose me
                return new PixelBox();
            }
        }

        class MipIndexInfo
        {
            private int startIndex;
            private int mipPageCount;

            public MipIndexInfo(int startIndex, int mipPageCount)
            {
                this.startIndex = startIndex;
                this.mipPageCount = mipPageCount;
            }

            public int getIndex(int x, int y)
            {
                return startIndex + y * mipPageCount + x;
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

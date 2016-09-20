using Engine;
using FreeImageAPI;
using OgrePlugin.Plugin.VirtualTexture;
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
    /// magicNumber - uint - the magic number 0x50544558 (ascii PTEX packed into a uint)
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
        public const uint MagicNumber = 0x50544558;

        public enum ImageType : int
        {
            PNG = 0,
            WEBP = 1,
        }

        //Magic number is also here, but we don't store it, just check at load
        private int numImages;
        private int imageType;
        private int imageXSize;
        private int imageYSize;
        private int pageSize;
        private int indexStart;
        private const int HeaderSize = sizeof(int) * 7; //Extra compared to fields above since the first is the magic number

        private MemoryBlock memoryBlock;
        private List<ImageInfo> pages;
        private List<MipIndexInfo> mipIndices;
        private String loadType;
        private Func<Stream> streamProvider;

        internal List<ImageInfo> Pages { get { return pages; } }

        internal int NumImages
        {
            get
            {
                return this.numImages;
            }
            set
            {
                this.numImages = value;
            }
        }

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
        public static void fromBitmap(FreeImageBitmap image, int pageSize, int padding, Stream stream, ImageType imageType, int maxSize, bool lossless, FREE_IMAGE_FILTER filter, ImagePageSizeStrategy pageSizeStrategy, Action<FreeImageBitmap> afterResize = null)
        {
            stream.Seek(HeaderSize, SeekOrigin.Begin); //Leave some space for the header

            if (image.Width > maxSize)
            {
                Logging.Log.Info("Image size {0} was too large, resizing to {1}", image.Width, maxSize);
                pageSizeStrategy.rescaleImage(image, new Size(maxSize, maxSize), filter);
            }

            PagedImage pagedImage = new PagedImage();
            int padding2x = padding * 2;
            FREE_IMAGE_FORMAT outputFormat;
            FREE_IMAGE_SAVE_FLAGS saveFlags = FREE_IMAGE_SAVE_FLAGS.DEFAULT;
            switch (imageType)
            {
                default:
                case ImageType.PNG:
                    outputFormat = FREE_IMAGE_FORMAT.FIF_PNG;
                    break;
                case ImageType.WEBP:
                    outputFormat = FREE_IMAGE_FORMAT.FIF_WEBP;
                    if (lossless)
                    {
                        saveFlags = FREE_IMAGE_SAVE_FLAGS.WEBP_LOSSLESS;
                    }
                    else
                    {
                        saveFlags = (FREE_IMAGE_SAVE_FLAGS)90;
                    }
                    break;
            }

            //Kind of weird, ogre and freeimage are backwards from one another in terms of scanline 0 being the top or bottom
            //This easily fixes the math below by just flipping the image first. Note that pages must be flipped back over because
            //of this when they are saved.
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pagedImage.numImages = 0;
            pagedImage.imageType = (int)imageType;
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

            IntSize2 fullPageSize = new IntSize2(pageSize + padding2x, pageSize + padding2x);
            for (int mip = 0; mip < mipLevelCount; ++mip)
            {
                //Setup mip level
                if (mip != 0)
                {
                    pageSizeStrategy.rescaleImage(image, new Size(image.Width >> 1, image.Height >> 1), filter);
                    if (afterResize != null)
                    {
                        //Flip so external functions aren't confused
                        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        afterResize(image);
                        //Flip back for correct math
                        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }
                }
                int size = image.Width / pageSize;
                pagedImage.mipIndices.Add(new MipIndexInfo(pagedImage.numImages, size));
                pageSizeStrategy.extractPage(image, padding, stream, pagedImage, pageSize, fullPageSize, size, outputFormat, saveFlags);
            }

            //Write half size mip
            int halfPageSize = pageSize >> 1;
            IntSize2 halfSizePadded = new IntSize2(halfPageSize + padding2x, halfPageSize + padding2x);
            pageSizeStrategy.rescaleImage(image, new Size(image.Width >> 1, image.Height >> 1), filter);
            pageSizeStrategy.extractPage(image, padding, stream, pagedImage, halfPageSize, halfSizePadded, 1, outputFormat, saveFlags);

            pagedImage.indexStart = (int)stream.Position;

            using (BinaryWriter sw = new BinaryWriter(stream, Encoding.Default, true))
            {
                foreach (var imageInfo in pagedImage.pages)
                {
                    imageInfo.write(sw);
                }

                //Go back to header reserved space
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.Write(MagicNumber);
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
        /// <param name="streamProvider">A function that opens a stream to the source data for the PagedImage. This should always be careful to open the same stream. The PagedImage lazy loads its image this way on demand.</param>
        public void load(Func<Stream> streamProvider)
        {
            if (memoryBlock != null)
            {
                memoryBlock.Dispose();
            }

            this.streamProvider = streamProvider;
            using (Stream source = streamProvider())
            {
                using (BinaryReader headerReader = new BinaryReader(source, Encoding.Default, true))
                {
                    uint magicNumber = headerReader.ReadUInt32();
                    if (magicNumber != MagicNumber)
                    {
                        throw new FormatException("The specified stream does not contain a valid ptex file.");
                    }

                    numImages = headerReader.ReadInt32();
                    imageType = headerReader.ReadInt32();
                    imageXSize = headerReader.ReadInt32();
                    imageYSize = headerReader.ReadInt32();
                    pageSize = headerReader.ReadInt32();
                    indexStart = headerReader.ReadInt32();
                }

                switch ((ImageType)imageType)
                {
                    default:
                    case ImageType.PNG:
                        loadType = "png";
                        break;
                    case ImageType.WEBP:
                        loadType = "webp";
                        break;
                }

                using (BinaryReader pageIndexReader = new BinaryReader(source, Encoding.Default, true))
                {
                    pageIndexReader.BaseStream.Seek(indexStart, SeekOrigin.Begin);
                    pages = new List<ImageInfo>(numImages);
                    mipIndices = new List<MipIndexInfo>();

                    int pageStride = imageXSize / pageSize;
                    int pagesForLevel = pageStride * pageStride;

                    int mipPage = 0;
                    mipIndices.Add(new MipIndexInfo(0, pageStride));
                    int halfSizeIndex = numImages - 1;
                    for (int i = 0; i < numImages; ++i)
                    {
                        if (mipPage == pagesForLevel && i != halfSizeIndex)
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

                memoryBlock = new MemoryBlock(indexStart);

                //Load two smallest mip levels the half size and the normal
                loadMipLevel(mipIndices.Count - 1, source);
                loadMipLevel(mipIndices.Count, source);
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
                uint magicNumber = sr.ReadUInt32();
                if (magicNumber != MagicNumber)
                {
                    throw new FormatException("The specified stream does not contain a valid ptex file.");
                }

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
        /// If mip exceeds the mips stored in the image you will get an image that is half the page size with the
        /// complete texture on it a half sized smallest (or highest) mip. This can be used to allow a paged texture
        /// to work at one page size smaller than the stored page size or other operations since you could scale the
        /// smallest mip returned, however, this gives a way to have the image with correct padding available quickly.
        /// </summary>
        /// <returns>A FreeImageBitmap the caller takes ownership of.</returns>
        public Image getImage(int x, int y, int mip)
        {
            //using (LogPerformanceBlock lpb = new LogPerformanceBlock(String.Format("Decompressed {0} in {{0}} ms", loadType)))
            {
                ImageInfo imageInfo;
                if (mip < mipIndices.Count)
                {
                    MipIndexInfo mipIndex = mipIndices[mip];
                    if (!mipIndex.Loaded)
                    {
                        using (Stream source = streamProvider())
                        {
                            loadMipLevel(mip, source);
                        }
                    }
                    imageInfo = pages[mipIndex.getIndex(x, y)];
                }
                else
                {
                    imageInfo = pages[pages.Count - 1]; //Half size smallest mip
                }

                using (Stream imageStream = imageInfo.openStream(memoryBlock))
                {
                    Image image = new Image();
                    image.load(imageStream, loadType);
                    return image;
                }
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

        public int PageSize
        {
            get
            {
                return pageSize;
            }
        }

        public int MipLevels
        {
            get
            {
                return mipIndices.Count;
            }
        }

        private void loadMipLevel(int mipLevel, Stream source)
        {
            int begin;
            int length;
            if (mipLevel < mipIndices.Count)
            {
                var mipInfo = mipIndices[mipLevel];
                var firstPage = pages[mipInfo.getIndex(0, 0)];
                begin = firstPage.ImageStart;
                if (mipLevel < mipIndices.Count - 1)
                {
                    //Go to next mip level's first page and then back one to get the requested level's last page.
                    var nextMipInfo = mipIndices[mipLevel + 1];
                    int index = nextMipInfo.getIndex(0, 0) - 1;
                    var lastPage = pages[index];
                    length = lastPage.ImageEnd - begin;
                }
                else //Higheset normal mip
                {
                    length = firstPage.ImageSize;
                }
                mipInfo.Loaded = true;
            }
            else
            {
                //Half size normal
                var pageInfo = pages[pages.Count - 1];
                begin = pageInfo.ImageStart;
                length = pageInfo.ImageSize;
            }
            source.Seek(begin, SeekOrigin.Begin);
            memoryBlock.loadStream(source, begin, length);
        }

        class MipIndexInfo
        {
            private int startIndex;
            private int pageStride; //The amount of pages in one row
            private bool infoLoaded = false; //True if the pages have been loaded from disk for this mip level

            public MipIndexInfo(int startIndex, int pageStride)
            {
                this.startIndex = startIndex;
                this.pageStride = pageStride;
            }

            public int getIndex(int x, int y)
            {
                return startIndex + y * pageStride + x;
            }

            public bool Loaded
            {
                get
                {
                    return infoLoaded;
                }
                set
                {
                    infoLoaded = value;
                }
            }
        }

        public class ImageInfo
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

            public int ImageStart
            {
                get
                {
                    return imageStart;
                }
            }

            public int ImageEnd
            {
                get
                {
                    return imageEnd;
                }
            }

            public int ImageSize
            {
                get
                {
                    return imageEnd - imageStart;
                }
            }
        }
    }
}

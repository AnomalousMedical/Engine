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
    public class PagedImage
    {
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
                for(int i = 0; i < numImages; ++i)
                {
                    if(mipPage > pagesForLevel)
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
        }
    }
}

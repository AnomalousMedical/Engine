using Engine;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.Plugin.VirtualTexture
{
    public interface ImagePageSizeStrategy
    {
        void rescaleImage(FreeImageBitmap image, Size size, FREE_IMAGE_FILTER filter);

        void extractPage(FreeImageBitmap image, int padding, Stream stream, PagedImage pagedImage, int pageSize, IntSize2 fullPageSize, int size, FREE_IMAGE_FORMAT outputFormat, FREE_IMAGE_SAVE_FLAGS saveFlags);
    }
}

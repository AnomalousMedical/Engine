using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class ClassicImageCacheHandle : TextureCacheHandle
    {
        private Image image;

        public ClassicImageCacheHandle(Image image, bool destroyOnNoRef) : base(destroyOnNoRef)
        {
            this.image = image;
        }

        protected override void disposing()
        {
            image.Dispose();
        }

        public override ulong Size
        {
            get
            {
                return image.Size;
            }
        }

        public override TexturePageHandle createTexturePageHandle(VTexPage page, IndirectionTexture indirectionTexture, int padding, int padding2, int textelsPerPage, int mipOffset)
        {
            PixelBox sourceBox;
            int mipCount = image.NumMipmaps;
            if (mipCount == 0) //We always have to take from the largest size
            {
                sourceBox = image.getPixelBox(0, 0);
            }
            else
            {
                sourceBox = image.getPixelBox(0, (uint)(page.mip - mipOffset));
            }

            IntSize2 largestSupportedPageIndex = indirectionTexture.NumPages;
            largestSupportedPageIndex.Width >>= page.mip;
            largestSupportedPageIndex.Height >>= page.mip;
            if (page.x != 0 && page.y != 0 && page.x + 1 != largestSupportedPageIndex.Width && page.y + 1 != largestSupportedPageIndex.Height)
            {
                //Can grab a complete page from the texture
                sourceBox.Rect = new IntRect(page.x * textelsPerPage - padding, page.y * textelsPerPage - padding, textelsPerPage + padding2, textelsPerPage + padding2);
            }
            else
            {
                //Have to get a partial page
                sourceBox.Rect = new IntRect(page.x * textelsPerPage, page.y * textelsPerPage, textelsPerPage, textelsPerPage);
            }

            return new TexturePageHandle(sourceBox, this);
        }
    }
}

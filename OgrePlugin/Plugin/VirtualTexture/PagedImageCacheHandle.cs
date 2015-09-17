using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class PagedImageCacheHandle : TextureCacheHandle
    {
        private PagedImage pagedImage;

        public PagedImageCacheHandle(PagedImage pagedImage, bool destroyOnNoRef) : base(destroyOnNoRef)
        {
            this.pagedImage = pagedImage;
        }

        protected override void disposing()
        {
            pagedImage.Dispose();
        }

        public override ulong Size
        {
            get
            {
                return pagedImage.Size;
            }
        }

        public override TexturePageHandle createTexturePageHandle(VTexPage page, IndirectionTexture indirectionTexture, int padding, int padding2, int textelsPerPage, int mipOffset)
        {
            int mip = page.mip;
            int x = page.x;
            int y = page.y;
            bool halfSizePages = textelsPerPage != pagedImage.PageSize;

            if(halfSizePages)
            {
                x /= 2;
                y /= 2;
            }

            var image = pagedImage.getImage(x, y, mip - mipOffset);
            var pixelBox = image.getPixelBox();

            if (halfSizePages && image.Width == pagedImage.PageSize + padding2)
            {
                int subpageX = page.x % 2;
                int subpageY = page.y % 2;
                int offsetMultiple = (int)image.Width / 2 - padding;
                int halfSize = textelsPerPage + padding2;

                pixelBox.Rect = new Engine.IntRect(offsetMultiple * subpageX, offsetMultiple * subpageY, halfSize, halfSize);

                //Logging.Log.Debug(pixelBox.Rect.ToString());
            }

            return new TexturePageHandle(pixelBox, this, image);
        }
    }
}

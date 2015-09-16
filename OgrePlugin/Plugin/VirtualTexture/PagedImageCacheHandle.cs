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
            var image = pagedImage.getImage(page.x, page.y, page.mip - mipOffset);
            return new TexturePageHandle(image.getPixelBox(), this, image);
        }
    }
}

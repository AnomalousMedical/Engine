using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    class TexturePageHandle : IDisposable
    {
        private PixelBox pixelBox;
        private TextureCacheHandle cacheHandle;

        public TexturePageHandle(PixelBox pixelBox, TextureCacheHandle cacheHandle)
        {
            this.pixelBox = pixelBox;
            this.cacheHandle = cacheHandle;
        }

        public void Dispose()
        {
            pixelBox.Dispose();
            cacheHandle.Dispose();
        }

        public PixelBox PixelBox
        {
            get
            {
                return pixelBox;
            }
        }
    }
}

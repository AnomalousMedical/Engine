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
        private IDisposable additionalDisposable;

        public TexturePageHandle(PixelBox pixelBox, TextureCacheHandle cacheHandle, IDisposable additionalDisposable = null)
        {
            this.pixelBox = pixelBox;
            this.cacheHandle = cacheHandle;
            this.additionalDisposable = additionalDisposable;
        }

        public void Dispose()
        {
            pixelBox.Dispose();
            cacheHandle.Dispose();
            if(additionalDisposable != null)
            {
                additionalDisposable.Dispose();
            }
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

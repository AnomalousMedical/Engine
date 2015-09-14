using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    /// <summary>
    /// This class helps manage a handle to a resource in the TextureCache. This uses reference counting,
    /// which is incremented internally for the checkout and is decremented on dispose. The idea is to
    /// allow for a using block when using these handles.
    /// </summary>
    class TextureCacheHandle : IDisposable
    {
        private int numCheckouts = 0;
        private bool destroyOnNoRef = false;
        private Image image;
        private Object lockObject = new object();

        public TextureCacheHandle(Image image, bool destroyOnNoRef)
        {
            this.image = image;
            this.destroyOnNoRef = destroyOnNoRef;
        }

        /// <summary>
        /// Dispose, call when finished with a checked out handle. Destroys
        /// the image if there are no more outstanding resources and the cahce requested it.
        /// </summary>
        public void Dispose()
        {
            lock (lockObject)
            {
                --numCheckouts;
                if (destroyOnNoRef && numCheckouts == 0)
                {
                    image.Dispose();
                }
            }
        }

        /// <summary>
        /// Get a pixel box for the image held by this handle. You must dispose the returned object.
        /// </summary>
        /// <param name="face"></param>
        /// <param name="mipmap"></param>
        /// <returns></returns>
        public PixelBox getPixelBox(uint mipmap = 0)
        {
            int mipCount = image.NumMipmaps;
            if (mipCount == 0) //We always have to take from the largest size
            {
                return image.getPixelBox(0, 0);
            }
            else
            {
                return image.getPixelBox(0, mipmap);
            }
        }

        /// <summary>
        /// The image for this handle.
        /// </summary>
        public Image Image
        {
            get
            {
                return image;
            }
        }

        /// <summary>
        /// The size of the image.
        /// </summary>
        public ulong Size
        {
            get
            {
                return image.Size;
            }
        }

        /// <summary>
        /// Internal function to increment the counter.
        /// </summary>
        internal void checkout()
        {
            lock (lockObject)
            {
                ++numCheckouts;
            }
        }

        /// <summary>
        /// Internal management function to destroy the image if possible.
        /// </summary>
        internal void destroyIfPossible()
        {
            lock (lockObject)
            {
                destroyOnNoRef = true;
                if (numCheckouts == 0)
                {
                    image.Dispose();
                }
            }
        }
    }

}

﻿using Engine;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin.VirtualTexture
{
    /// <summary>
    /// A texture cache that textures can be added to, it will automatically flush itself as items are added
    /// that make it too large.
    /// </summary>
    class TextureCache : IDisposable
    {
        private LinkedList<String> lastAccessedOrder = new LinkedList<string>();
        private Dictionary<String, TextureCacheHandle> loadedImages = new Dictionary<string, TextureCacheHandle>();
        private UInt64 maxCacheSize;
        private UInt64 currentCacheSize;
        private Object syncObject = new object();
        private bool texturesArePaged;

        public TextureCache(UInt64 maxCacheSize, bool texturesArePaged)
        {
            this.maxCacheSize = maxCacheSize;
            this.texturesArePaged = texturesArePaged;
            PerformanceMonitor.addValueProvider("Virtual Texture Cache Size", () => Prettify.GetSizeReadable((long)currentCacheSize));
        }

        public void Dispose()
        {
            PerformanceMonitor.removeValueProvider("Virtual Texture Cache Size");
            clear();
        }

        /// <summary>
        /// Get a texture from the cache, you need to dispose the TextureCacheHandle that is
        /// returned through the image out variable.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        private bool TryGetValue(string textureName, out TextureCacheHandle image)
        {
            lock (syncObject)
            {
                bool ret = loadedImages.TryGetValue(textureName, out image);
                if (ret)
                {
                    lastAccessedOrder.Remove(textureName);
                    lastAccessedOrder.AddFirst(textureName);
                    image.checkout();
                }
                return ret;
            }
        }

        /// <summary>
        /// Add a texture to the cache, this will return a TextureCacheHandle that you MUST dispose
        /// when you aren't using it anymore.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="image"></param>
        private TextureCacheHandle Add(string textureName, Image image)
        {
            lock (syncObject)
            {
                TextureCacheHandle handle;
                UInt64 imageSize = image.Size;
                if (imageSize < maxCacheSize) //Image itself can fit
                {
                    while (currentCacheSize + imageSize > maxCacheSize && lastAccessedOrder.Last != null)
                    {
                        //Drop oldest images until there is enough space
                        String last = lastAccessedOrder.Last.Value;
                        lastAccessedOrder.RemoveLast();
                        var destroyImage = loadedImages[last];
                        loadedImages.Remove(last);
                        currentCacheSize -= destroyImage.Size;
                        destroyImage.destroyIfPossible();
                    }
                    currentCacheSize += image.Size;
                    handle = new ClassicImageCacheHandle(image, false);
                    loadedImages.Add(textureName, handle);
                    lastAccessedOrder.AddFirst(textureName);
                }
                else
                {
                    handle = new ClassicImageCacheHandle(image, true);
                }
                handle.checkout();
                return handle;
            }
        }

        /// <summary>
        /// Add a texture to the cache, this will return a TextureCacheHandle that you MUST dispose
        /// when you aren't using it anymore.
        /// </summary>
        /// <param name="textureName"></param>
        /// <param name="image"></param>
        private TextureCacheHandle Add(string textureName, PagedImage image)
        {
            lock (syncObject)
            {
                TextureCacheHandle handle;
                UInt64 imageSize = image.Size;
                if (imageSize < maxCacheSize) //Image itself can fit
                {
                    while (currentCacheSize + imageSize > maxCacheSize && lastAccessedOrder.Last != null)
                    {
                        //Drop oldest images until there is enough space
                        String last = lastAccessedOrder.Last.Value;
                        lastAccessedOrder.RemoveLast();
                        var destroyImage = loadedImages[last];
                        loadedImages.Remove(last);
                        currentCacheSize -= destroyImage.Size;
                        destroyImage.destroyIfPossible();
                    }
                    currentCacheSize += image.Size;
                    handle = new PagedImageCacheHandle(image, false);
                    loadedImages.Add(textureName, handle);
                    lastAccessedOrder.AddFirst(textureName);
                }
                else
                {
                    handle = new PagedImageCacheHandle(image, true);
                }
                handle.checkout();
                return handle;
            }
        }

        internal void clear()
        {
            lock(syncObject)
            {
                foreach (var image in loadedImages.Values)
                {
                    image.destroyIfPossible();
                }
                loadedImages.Clear();
                lastAccessedOrder.Clear();
                currentCacheSize = 0;
            }
        }

        internal TexturePageHandle getImage(VTexPage page, IndirectionTexture indirectionTexture, OriginalTextureInfo textureUnit, int textelsPerPage, int padding, int padding2)
        {
            String textureName;
            if (texturesArePaged)
            {
                textureName = textureUnit.TextureFileName; 
            }
            else
            {
                textureName = String.Format("{0}_{1}", textureUnit.TextureFileName, indirectionTexture.RealTextureSize.Width >> page.mip);
            }
            TextureCacheHandle cacheHandle;
            if (!this.TryGetValue(textureName, out cacheHandle))
            {
                String file = textureUnit.TextureFileName;
                if (texturesArePaged) //Paged Images
                {
                    //using (var perfMon = new LogPerformanceBlock(String.Format("Loaded image {0} in {{0}} ms", file), Logging.LogLevel.Info, "TextureCache"))
                    //{
                        PagedImage pagedImage = new PagedImage();
                        pagedImage.load(() => VirtualFileSystem.Instance.openStream(file, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read));
                        cacheHandle = this.Add(textureName, pagedImage);
                    //}
                }
                else //Normal Images
                {
                    String extension = Path.GetExtension(file);
                    String directFile = textureUnit.TextureFileName.Substring(0, file.Length - extension.Length);
                    directFile = String.Format("{0}_{1}{2}", directFile, indirectionTexture.RealTextureSize.Width >> page.mip, extension);
                    if (VirtualFileSystem.Instance.exists(directFile))
                    {
                        var image = doLoadImage(extension, directFile);
                        cacheHandle = this.Add(textureName, image);
                    }
                    else
                    {
                        //Not using cache for full size images, this is a rare case that we are not really supporting right now
                        Image image = doLoadImage(extension, textureUnit.TextureFileName);

                        //If we aren't mip 0 resize accordingly
                        if (page.mip > image.NumMipmaps && page.mip != 0)
                        {
                            using (Image original = image)
                            {
                                image = new Image(original.Width >> page.mip, original.Height >> page.mip, original.Depth, original.Format, original.NumFaces, original.NumMipmaps);
                                using (var src = original.getPixelBox())
                                {
                                    using (var dest = image.getPixelBox())
                                    {
                                        Image.Scale(src, dest, Image.Filter.FILTER_BILINEAR);
                                    }
                                }
                            }
                        }
                        cacheHandle = this.Add(textureName, image);
                    }
                }
            }
            return cacheHandle.createTexturePageHandle(page, indirectionTexture, padding, padding2, textelsPerPage, textureUnit.MipOffset);
        }

        private Image doLoadImage(String extension, String file)
        {
            using (var perfMon = new LogPerformanceBlock(String.Format("Loaded image {0} in {{0}} ms", file), Logging.LogLevel.Info, "TextureCache"))
            {
                var image = new Image();
                using (Stream stream = VirtualFileSystem.Instance.openStream(file, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read))
                {
                    if (extension.Length > 0)
                    {
                        extension = extension.Substring(1);
                    }
                    image.load(stream, extension);
                }
                return image;
            }
        }

        public UInt64 CurrentCacheSize
        {
            get
            {
                return currentCacheSize;
            }
        }

        public UInt64 MaxCacheSize
        {
            get
            {
                return maxCacheSize;
            }
        }
    }
}

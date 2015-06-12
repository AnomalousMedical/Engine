#if !FIXLATER_DISABLED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeImageAPI;
using Engine;

namespace OgrePlugin
{
    /// <summary>
    /// A single page in an image atlas.
    /// </summary>
    public class ImageAtlasPage : IDisposable
    {
        private ImagePackTreeNode rootNode;
        private TexturePtr texture;
        private HardwarePixelBufferSharedPtr bufferPtr;
        Dictionary<String, ImagePackTreeNode> imageInfo = new Dictionary<string, ImagePackTreeNode>();

        public ImageAtlasPage(String textureName, String groupName, int width, int height)
        {
            rootNode = new ImagePackTreeNode(new Size(width, height));
            texture = TextureManager.getInstance().createManual(textureName, groupName, TextureType.TEX_TYPE_2D, (uint)width, (uint)height, 1, 0, OgrePlugin.PixelFormat.PF_A8R8G8B8, TextureUsage.TU_STATIC_WRITE_ONLY, null, false, 0);
            bufferPtr = texture.Value.getBuffer();
            this.TextureName = textureName;
            this.GroupName = groupName;
        }

        public void Dispose()
        {
            bufferPtr.Dispose();
            texture.Dispose();
        }

        /// <summary>
        /// Add an image to this page, will return true if the image was sucessfully added to the page.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        internal unsafe bool addImage(String name, FreeImageBitmap image)
        {
            ImagePackTreeNode node = rootNode.insert(name, image);
            if (node != null)
            {
                using (PixelBox pixelBox = new PixelBox(0, 0, image.Width, image.Height, OgreDrawingUtility.getOgreFormat(image.PixelFormat), image.GetScanlinePointer(0).ToPointer()))
                {
                    Rectangle locationRect = node.LocationRect;
                    bufferPtr.Value.blitFromMemory(pixelBox, new IntRect(locationRect.Left, locationRect.Top, locationRect.Right, locationRect.Bottom));
                    imageInfo.Add(name, node);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool hasImage(String name)
        {
            return imageInfo.ContainsKey(name);
        }

        /// <summary>
        /// Get an image location for a given named image.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <returns>True if the image was found, false if not.</returns>
        public bool tryGetImageLocation(String name, out Rectangle location)
        {
            ImagePackTreeNode node;
            if (imageInfo.TryGetValue(name, out node))
            {
                location = node.LocationRect;
                return true;
            }
            location = new Rectangle();
            return false;
        }

        public String TextureName { get; private set; }

        public String GroupName { get; private set; }
    }
}
#endif
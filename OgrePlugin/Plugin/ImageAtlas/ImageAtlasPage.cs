using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OgreWrapper;
using System.Drawing.Imaging;

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
            texture = TextureManager.getInstance().createManual(textureName, groupName, TextureType.TEX_TYPE_2D, (uint)width, (uint)height, 0, 0, OgreWrapper.PixelFormat.PF_A8R8G8B8, TextureUsage.TU_STATIC_WRITE_ONLY, false, 0);
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
        internal unsafe bool addImage(String name, Bitmap image)
        {
            ImagePackTreeNode node = rootNode.insert(name, image);
            if (node != null)
            {
                BitmapData bmpData = image.LockBits(new Rectangle(new Point(), image.Size), ImageLockMode.ReadOnly, image.PixelFormat);
                using (PixelBox pixelBox = new PixelBox(0, 0, bmpData.Width, bmpData.Height, OgreDrawingUtility.getOgreFormat(image.PixelFormat), bmpData.Scan0.ToPointer()))
                {
                    Rectangle locationRect = node.LocationRect;
                    bufferPtr.Value.blitFromMemory(pixelBox, locationRect.Left, locationRect.Top, locationRect.Right, locationRect.Bottom);
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

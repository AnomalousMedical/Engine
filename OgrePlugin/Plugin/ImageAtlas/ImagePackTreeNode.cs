#if !FIXLATER_DISABLED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeImageAPI;

namespace OgrePlugin
{
    /// <summary>
    /// Based on http://www.blackpawn.com/texts/lightmaps/default.html
    /// </summary>
    class ImagePackTreeNode
    {
        private ImagePackTreeNode()
        {

        }

        public ImagePackTreeNode(Size size)
        {
            rc = new Rectangle(0, 0, size.Width, size.Height);
        }

        ImagePackTreeNode[] children = new ImagePackTreeNode[2];
        Rectangle rc;
        String imageID = null;

        public bool IsLeaf
        {
            get
            {
                return children[0] == null && children[1] == null;
            }
        }

        public Rectangle LocationRect
        {
            get
            {
                return rc;
            }
        }

        public String ImageID
        {
            get
            {
                return imageID;
            }
        }

        public ImagePackTreeNode insert(String imageID, FreeImageBitmap img)
        {
            //If not a leaf and both children are full
            if (!IsLeaf)
            {
                ImagePackTreeNode newNode = children[0].insert(imageID, img);
                if (newNode != null)
                {
                    return newNode;
                }

                return children[1].insert(imageID, img);
            }
            else
            {
                //If there is already an image, return
                if (this.imageID != null)
                {
                    return null;
                }

                Rectangle imageRect = new Rectangle(rc.Left, rc.Top, img.Width, img.Height);

                //If the area is too small return
                if (rc.Width < imageRect.Width || rc.Height < imageRect.Height)
                {
                    return null;
                }

                //If were just right accept
                if (rc.Width == imageRect.Width && rc.Height == imageRect.Height)
                {
                    this.imageID = imageID;
                    return this;
                }

                //Otherwise gotta split this node and create some kids
                this.children[0] = new ImagePackTreeNode();
                this.children[1] = new ImagePackTreeNode();

                int dw = rc.Width - imageRect.Width;
                int dh = rc.Height - imageRect.Height;

                if (dw > dh)
                {
                    children[0].rc = new Rectangle(rc.Left,         rc.Top, imageRect.Width,            rc.Height);
                    children[1].rc = new Rectangle(rc.Left + imageRect.Width, rc.Top, rc.Width - imageRect.Width, rc.Height);
                }
                else
                {
                    children[0].rc = new Rectangle(rc.Left, rc.Top,           rc.Width, imageRect.Height);
                    children[1].rc = new Rectangle(rc.Left, rc.Top + imageRect.Height, rc.Width, rc.Height - imageRect.Height);
                }

                return children[0].insert(imageID, img);
            }
        }
    }
}
#endif
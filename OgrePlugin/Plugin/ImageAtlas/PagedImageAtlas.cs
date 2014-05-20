#if !FIXLATER_DISABLED
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OgrePlugin
{
    public class PagedImageAtlas : IDisposable
    {
        private String name;
        private String groupName;
        private int pageWidth;
        private int pageHeight;
        private ImageAtlasPage lastPage;
        private List<ImageAtlasPage> allPages = new List<ImageAtlasPage>();

        public PagedImageAtlas(String name, String groupName, int pageWidth = 1024, int pageHeight = 1024)
        {
            this.name = name;
            this.groupName = groupName;
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
        }

        public void Dispose()
        {
            foreach (ImageAtlasPage page in allPages)
            {
                page.Dispose();
            }
        }

        /// <summary>
        /// Add an image. This will attempt to put the image on whatever page it
        /// can fit on. This will produce better packing for different sized
        /// images, but will slow down as pages are added, since it has to try
        /// to add to them all.
        /// </summary>
        /// <param name="name">The name of the image</param>
        /// <param name="image">The image</param>
        /// <returns>The page the image was added to.</returns>
        public ImageAtlasPage addImage(String name, Bitmap image)
        {
            checkImageDimensions(image);
            foreach (ImageAtlasPage page in allPages)
            {
                if (page.addImage(name, image))
                {
                    return page;
                }
            }
            ImageAtlasPage newPage = new ImageAtlasPage(String.Format("{0}_Page{1}", this.name, allPages.Count), groupName, pageWidth, pageHeight);
            newPage.addImage(name, image);
            allPages.Add(newPage);
            return newPage;
        }

        /// <summary>
        /// Add an image to the end of the pages. This is a good function to use
        /// if all the images are the same size, since it will only add images
        /// to the last page. This will result in worse packing (if the images
        /// are different sizes), but allows images to be added to the atlas
        /// quicker.
        /// </summary>
        /// <param name="name">The name of the image.</param>
        /// <param name="image">A bitmap of the image.</param>
        /// <returns>The page the image was added to.</returns>
        public ImageAtlasPage addImageToEnd(String name, Bitmap image)
        {
            checkImageDimensions(image);
            if (lastPage == null)
            {
                lastPage = new ImageAtlasPage(String.Format("{0}_Page{1}", this.name, allPages.Count), groupName, pageWidth, pageHeight);
                allPages.Add(lastPage);
            }
            if(!lastPage.addImage(name, image))
            {
                lastPage = null;
                return addImageToEnd(name, image);
            }
            return lastPage;
        }

        /// <summary>
        /// Get the page a specific image is on.
        /// </summary>
        /// <param name="name">The name of the image.</param>
        /// <returns>The page the image is on.</returns>
        public ImageAtlasPage getImagePage(String name)
        {
            foreach (ImageAtlasPage page in allPages)
            {
                if (page.hasImage(name))
                {
                    return page;
                }
            }
            return null;
        }

        private void checkImageDimensions(Bitmap image)
        {
            if (image.Width > pageWidth || image.Height > pageHeight)
            {
                throw new ImageAtlasException("The image is too large for the pages in this image atlas.");
            }
        }
    }
}
#endif
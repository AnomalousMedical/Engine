using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OgrePlugin
{
    public class PagedImageAtlas
    {
        private String name;
        private String groupName;
        private int pageWidth;
        private int pageHeight;
        private ImageAtlasPage currentPage;
        private List<ImageAtlasPage> allPages = new List<ImageAtlasPage>();

        public PagedImageAtlas(String name, String groupName, int pageWidth = 1024, int pageHeight = 1024)
        {
            this.name = name;
            this.groupName = groupName;
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
        }

        public ImageAtlasPage addImage(String name, Bitmap image)
        {
            if (image.Width > pageWidth || image.Height > pageHeight)
            {
                throw new ImageAtlasException("The image is too large for the pages in this image atlas.");
            }
            if (currentPage == null)
            {
                currentPage = new ImageAtlasPage(String.Format("{0}_Page{1}", this.name, allPages.Count), groupName, pageWidth, pageHeight);
                allPages.Add(currentPage);
            }
            if(!currentPage.addImage(name, image))
            {
                currentPage = null;
                return addImage(name, image);
            }
            return currentPage;
        }

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ImageAtlasPacker
{
    class BitmapEntry : IDisposable
    {
        public BitmapEntry(String imageFile, Bitmap bitmap, ListViewItem listItem)
        {
            this.ImageFile = imageFile;
            this.Bitmap = bitmap;
            this.ListItem = listItem;
        }

        public void Dispose()
        {
            Bitmap.Dispose();
        }

        public String ImageFile { get; set; }

        public Bitmap Bitmap { get; set; }

        public ListViewItem ListItem { get; set; }
    }
}

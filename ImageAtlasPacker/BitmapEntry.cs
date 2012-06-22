using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageAtlasPacker
{
    public class BitmapEntry : IDisposable
    {
        public BitmapEntry(String imageFile, Bitmap bitmap, ListViewItem listItem)
        {
            this.ImageFile = imageFile;
            this.Bitmap = bitmap;
            this.ListItem = listItem;
        }

        internal void resizeImage(int width, int height)
        {
            if (Bitmap.Width != width || Bitmap.Height != height)
            {
                Bitmap oldBitmap = Bitmap;
                Bitmap = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(Bitmap))
                {
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.DrawImage(oldBitmap, 0, 0, Bitmap.Width, Bitmap.Height);
                }
                oldBitmap.Dispose();
            }
        }

        public void Dispose()
        {
            Bitmap.Dispose();
        }

        public String ImageFile { get; set; }

        public Bitmap Bitmap { get; set; }

        public ListViewItem ListItem { get; set; }

        public Rectangle ImageLocation { get; set; }

        public String ImageKey
        {
            get
            {
                return Path.GetFileNameWithoutExtension(ImageFile);
            }
        }
    }
}

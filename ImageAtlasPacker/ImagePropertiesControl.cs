using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ImageAtlasPacker
{
    public partial class ImagePropertiesControl : UserControl
    {
        public event Action<String> DroppedTemplateFile;

        private List<BitmapEntry> images = new List<BitmapEntry>();

        public ImagePropertiesControl()
        {
            InitializeComponent();
            this.Disposed += new EventHandler(ImagePropertiesControl_Disposed);

            inputTextureList.DragEnter += dragEnter;
            inputTextureList.DragDrop += dragDrop;
        }

        void ImagePropertiesControl_Disposed(object sender, EventArgs e)
        {
            foreach (BitmapEntry entry in images)
            {
                entry.Dispose();
            }
        }

        public PictureBox PictureBox { get; set; }

        public IEnumerable<BitmapEntry> Bitmaps
        {
            get
            {
                return images;
            }
        }

        private void addTexturesButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                openFiles(openFileDialog.FileNames);
            }
        }

        private void removeTextures_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listItem in inputTextureList.SelectedItems)
            {
                BitmapEntry entry = listItem.Tag as BitmapEntry;
                images.Remove(entry);
                inputTextureList.Items.Remove(listItem);
                entry.Dispose();
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (resizeImages.Checked)
            {
                foreach (BitmapEntry entry in images)
                {
                    entry.resizeImage((int)resizeWidth.Value, (int)resizeHeight.Value);
                }
            }

            int padding = (int)this.padding.Value;
            var halfPadding = padding / 2;
            ImagePackTreeNode imageInfo = new ImagePackTreeNode(new Size((int)widthText.Value, (int)heightText.Value), padding);
            Bitmap atlas = new Bitmap((int)widthText.Value, (int)heightText.Value, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(atlas))
            {
                foreach (BitmapEntry entry in images)
                {
                    ImagePackTreeNode node = imageInfo.insert(entry.ImageFile, entry.Bitmap);
                    if (node != null)
                    {
                        //Center image
                        var size = entry.Bitmap.Size;
                        var imageLocationRect = new Rectangle(
                            node.LocationRect.X + halfPadding,
                            node.LocationRect.Y + halfPadding,
                            size.Width,
                            size.Height
                            );

                        g.DrawImage(entry.Bitmap, imageLocationRect);
                        entry.ImageLocation = imageLocationRect;
                        entry.ImageNodeLocation = node.LocationRect;

                        padFillColor(entry.Bitmap, g, halfPadding, node.LocationRect);
                    }
                    else
                    {
                        MessageBox.Show(this, String.Format("Ran out of room placing image {0}. Please increase image size.", entry.ImageFile), "Size Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            if (PictureBox.Image != null)
            {
                PictureBox.Image.Dispose();
            }
            PictureBox.Image = atlas;
            PictureBox.Size = atlas.Size;
        }

        /// <summary>
        /// This one seems to work the best, but falls apart at lower mip levels on the virtual texture.
        /// </summary>
        private static void padFillColor(Bitmap image, Graphics g, int padding, Rectangle fullImageRect)
        {
            //Pad by repeating
            //Top
            Rectangle srcBox = new Rectangle(0, 0, image.Width, 1);
            Rectangle destBox = new Rectangle(fullImageRect.X + padding, 0, image.Width, 1);

            for (int i = 0; i < padding; ++i)
            {
                destBox.Y = fullImageRect.Y + i;
                g.DrawImage(image, destBox, srcBox, GraphicsUnit.Pixel);
            }

            //Bottom
            srcBox = new Rectangle(0, image.Height - 1, image.Width, 1);
            destBox = new Rectangle(fullImageRect.X + padding, 0, image.Width, 1);

            for (int i = 0; i < padding; ++i)
            {
                destBox.Y = fullImageRect.Bottom - i - 1;
                g.DrawImage(image, destBox, srcBox, GraphicsUnit.Pixel);
            }

            //Left
            srcBox = new Rectangle(0, 0, 1, image.Height);
            destBox = new Rectangle(0, fullImageRect.Y + padding, 1, image.Height);

            for (int i = 0; i < padding; ++i)
            {
                destBox.X = fullImageRect.X + i;
                g.DrawImage(image, destBox, srcBox, GraphicsUnit.Pixel);
            }

            //Right
            srcBox = new Rectangle(image.Width - 1, 0, 1, image.Height);
            destBox = new Rectangle(0, fullImageRect.Y + padding, 1, image.Height);

            for (int i = 0; i < padding; ++i)
            {
                destBox.X = fullImageRect.Right - i - 1;
                g.DrawImage(image, destBox, srcBox, GraphicsUnit.Pixel);
            }
        }

        private void displayChangeButton_Click(object sender, EventArgs e)
        {
            if (inputTextureList.View == View.LargeIcon)
            {
                inputTextureList.View = View.SmallIcon;
            }
            else
            {
                inputTextureList.View = View.LargeIcon;
            }
        }

        void dragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        void dragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            openFiles(files);
        }

        private void openFiles(IEnumerable<String> filenames)
        {
            foreach (String imageFile in filenames)
            {
                try
                {
                    Bitmap bmp = new Bitmap(imageFile);
                    previewImageListLarge.Images.Add(imageFile, bmp);
                    previewImageListSmall.Images.Add(imageFile, bmp);
                    ListViewItem listItem = inputTextureList.Items.Add(imageFile, Path.GetFileName(imageFile), imageFile);
                    listItem.Tag = new BitmapEntry(imageFile, bmp, listItem);
                    images.Add((BitmapEntry)listItem.Tag);
                }
                catch (ArgumentException)
                {
                    if (imageFile.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (DroppedTemplateFile != null)
                        {
                            DroppedTemplateFile.Invoke(imageFile);
                        }
                    }
                }
            }

            if (resizeImages.Checked)
            {
                int width = (int)widthText.Value;
                int resizedW = (int)resizeWidth.Value;
                int resizedH = (int)resizeHeight.Value;
                int imagesPerRow = width / resizedW;
                if (images.Count < imagesPerRow)
                {
                    widthText.Value = images.Count * resizedW;
                    heightText.Value = resizedH;
                }
                else
                {
                    heightText.Value = images.Count / imagesPerRow * resizedH;
                }
            }
        }

        private void halveButton_Click(object sender, EventArgs e)
        {
            widthText.Value = (int)widthText.Value / 2;
            heightText.Value = (int)heightText.Value / 2;

            resizeHeight.Value = (int)resizeHeight.Value / 2;
            resizeWidth.Value = (int)resizeWidth.Value / 2;

            updateButton_Click(sender, e);
        }
    }
}

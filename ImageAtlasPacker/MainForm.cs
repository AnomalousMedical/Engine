using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace ImageAtlasPacker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            imagePropertiesControl1.DroppedTemplateFile += imagePropertiesControl1_DroppedTemplateFile;
        }

        private void saveAtlasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image bitmap = pictureBox1.Image;
            if (bitmap != null)
            {
                if (saveImageDialog.ShowDialog(this) == DialogResult.OK)
                {
                    ImageFormat format = ImageFormat.Jpeg;
                    switch (saveImageDialog.FilterIndex)
                    {
                        case 1:
                            format = ImageFormat.Jpeg;
                            break;
                        case 2:
                            format = ImageFormat.Png;
                            break;
                        case 3:
                            format = ImageFormat.Tiff;
                            break;
                        case 4:
                            format = ImageFormat.Bmp;
                            break;
                    }
                    bitmap.Save(saveImageDialog.FileName, format);
                    String shortFile = Path.GetFileName(saveImageDialog.FileName);
                    String xmlFile = Path.GetDirectoryName(saveImageDialog.FileName) + "/" + Path.GetFileNameWithoutExtension(saveImageDialog.FileName) + ".xml";
                    int atlasWidth = bitmap.Width;
                    int atlasHeight = bitmap.Height;
                    using (StreamWriter fs = new StreamWriter(File.Open(xmlFile, FileMode.Create, FileAccess.Write)))
                    {
                        fs.WriteLine(imageIndexControl1.HeaderText);
                        foreach (BitmapEntry entry in imagePropertiesControl1.Bitmaps)
                        {
                            String indexText = imageIndexControl1.IndexText.Replace("${IMAGE_KEY}", entry.ImageKey);
                            indexText = indexText.Replace("${X_LOC}", entry.ImageLocation.Left.ToString());
                            indexText = indexText.Replace("${Y_LOC}", entry.ImageLocation.Top.ToString());
                            indexText = indexText.Replace("${WIDTH}", entry.ImageLocation.Width.ToString());
                            indexText = indexText.Replace("${HEIGHT}", entry.ImageLocation.Height.ToString());

                            indexText = indexText.Replace("${NODE_X_LOC}", entry.ImageNodeLocation.Left.ToString());
                            indexText = indexText.Replace("${NODE_Y_LOC}", entry.ImageNodeLocation.Top.ToString());
                            indexText = indexText.Replace("${NODE_WIDTH}", entry.ImageNodeLocation.Width.ToString());
                            indexText = indexText.Replace("${NODE_HEIGHT}", entry.ImageNodeLocation.Height.ToString());

                            indexText = indexText.Replace("${IMAGE_FILE}", shortFile);
                            indexText = indexText.Replace("${ATLAS_WIDTH}", atlasWidth.ToString());
                            indexText = indexText.Replace("${ATLAS_HEIGHT}", atlasHeight.ToString());

                            indexText = indexText.Replace("${U_LEFT}", (entry.ImageLocation.Left / (float)atlasWidth).ToString());
                            indexText = indexText.Replace("${V_TOP}", (entry.ImageLocation.Top / (float)atlasWidth).ToString());
                            indexText = indexText.Replace("${U_RIGHT}", ((entry.ImageLocation.Width + entry.ImageLocation.Left) / (float)atlasWidth).ToString());
                            indexText = indexText.Replace("${V_BOTTOM}", ((entry.ImageLocation.Height + entry.ImageLocation.Top) / (float)atlasWidth).ToString());

                            indexText = indexText.Replace("${NODE_U_LEFT}", (entry.ImageNodeLocation.Left / (float)atlasWidth).ToString());
                            indexText = indexText.Replace("${NODE_V_TOP}", (entry.ImageNodeLocation.Top / (float)atlasWidth).ToString());
                            indexText = indexText.Replace("${NODE_U_RIGHT}", ((entry.ImageNodeLocation.Width + entry.ImageNodeLocation.Left) / (float)atlasWidth).ToString());
                            indexText = indexText.Replace("${NODE_V_BOTTOM}", ((entry.ImageNodeLocation.Height + entry.ImageNodeLocation.Top) / (float)atlasWidth).ToString());

                            fs.WriteLine(indexText);
                        }
                        fs.WriteLine(imageIndexControl1.FooterText);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Please create an image before saving.");
            }
        }

        private const string HEADER = "__IAEHEADER__";
        private const string FOOTER = "__IAEFOOTER__";
        private const string INDEX = "__IAEINDEX__";

        private void saveIndexTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveTemplateDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(File.Open(openTemplateDialog.FileName, FileMode.Create, FileAccess.Write)))
                {
                    sw.WriteLine(HEADER);
                    sw.WriteLine(imageIndexControl1.HeaderText);
                    sw.WriteLine(INDEX);
                    sw.WriteLine(imageIndexControl1.IndexText);
                    sw.WriteLine(FOOTER);
                    sw.WriteLine(imageIndexControl1.FooterText);
                }
            }
        }

        enum LoadTemplateMode
        {
            Header,
            Index,
            Footer,
        }

        void imagePropertiesControl1_DroppedTemplateFile(string obj)
        {
            openTemplate(obj);
        }

        private void loadIndexTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openTemplateDialog.ShowDialog(this) == DialogResult.OK)
            {
                openTemplate(openTemplateDialog.FileName);
            }
        }

        private void openTemplate(String filename)
        {
            using (StreamReader sr = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read)))
            {
                StringBuilder section = new StringBuilder();
                LoadTemplateMode mode = LoadTemplateMode.Header;
                String line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line == HEADER)
                    {
                        loadSection(section, mode);
                        mode = LoadTemplateMode.Header;
                        section = new StringBuilder();
                    }
                    else if (line == INDEX)
                    {
                        loadSection(section, mode);
                        mode = LoadTemplateMode.Index;
                        section = new StringBuilder();
                    }
                    else if (line == FOOTER)
                    {
                        loadSection(section, mode);
                        mode = LoadTemplateMode.Footer;
                        section = new StringBuilder();
                    }
                    else
                    {
                        section.Append(line);
                        section.Append(Environment.NewLine);
                    }
                }
                loadSection(section, mode);
            }
        }

        private void loadSection(StringBuilder section, LoadTemplateMode mode)
        {
            switch (mode)
            {
                case LoadTemplateMode.Header:
                    imageIndexControl1.HeaderText = section.ToString();
                    break;

                case LoadTemplateMode.Index:
                    imageIndexControl1.IndexText = section.ToString();
                    break;

                case LoadTemplateMode.Footer:
                    imageIndexControl1.FooterText = section.ToString();
                    break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

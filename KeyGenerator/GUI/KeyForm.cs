using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine;
using System.IO;
using System.Drawing.Imaging;

namespace KeyGenerator
{
    public partial class KeyForm : Form
    {
        private ConfigFile configFile;
        private ConfigSection keySection;
        private List<String> usedKeys = new List<string>();
        private Image keyCardBitmap;

        public KeyForm()
        {
            InitializeComponent();
            keyCardPreview.SizeMode = PictureBoxSizeMode.Zoom;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (configFile != null)
            {
                configFile.writeConfigFile();
            }
            base.OnClosed(e);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                usedKeys.Clear();
                if (configFile != null)
                {
                    configFile.writeConfigFile();
                }
                configFile = new ConfigFile(openFileDialog.FileName);
                configFile.loadConfigFile();
                keySection = configFile.createOrRetrieveConfigSection("ValidKeys");
                programNameText.Text = Path.GetFileName(openFileDialog.FileName);
                ConfigIterator keyIter = new ConfigIterator(keySection, "Key");
                while (keyIter.hasNext())
                {
                    usedKeys.Add(keyIter.next());
                }
            }
        }

        private void generateKey_Click(object sender, EventArgs e)
        {
            if (programNameText.Text == String.Empty || programNameText.Text == null)
            {
                MessageBox.Show(this, "Please enter a program name.");
            }
            else
            {
                if (configFile == null)
                {
                    configFile = new ConfigFile(programNameText.Text);
                    if (File.Exists(programNameText.Text))
                    {
                        if (MessageBox.Show(this, "A key file for that program name is already at {0}. Would you like to overwrite it?", "Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            configFile.loadConfigFile();
                        }
                    }
                    keySection = configFile.createOrRetrieveConfigSection("ValidKeys");
                    ConfigIterator keyIter = new ConfigIterator(keySection, "Key");
                    while (keyIter.hasNext())
                    {
                        usedKeys.Add(keyIter.next());
                    }
                }
                for (int i = 0; i < numberOfKeys.Value; ++i)
                {
                    String key = KeyCreator.createKey(programNameText.Text);
                    while (usedKeys.Contains(key))
                    {
                        key = KeyCreator.createKey(programNameText.Text);
                        while (!KeyChecker.checkValid(programNameText.Text, key))
                        {
                            key = KeyCreator.createKey(programNameText.Text);
                        }
                    }
                    keySection.setValue("Key" + usedKeys.Count, key);
                    usedKeys.Add(key);
                    keyText.Text = key;
                    configFile.writeConfigFile();
                    if (keyCardBitmap != null)
                    {
                        using (Image cloneImage = (Image)keyCardBitmap.Clone())
                        {
                            using (Graphics g = Graphics.FromImage(cloneImage))
                            {
                                using (Font font = new Font(FontFamily.GenericMonospace, 14))
                                {
                                    SizeF stringSize = g.MeasureString(key, font);
                                    float xLoc = (cloneImage.Width - stringSize.Width) / 2.0f;
                                    int yLoc = 1391;
                                    using (Brush brush = new SolidBrush(System.Drawing.Color.Black))
                                    {
                                        g.DrawString(key, font, brush, new Point((int)xLoc, yLoc));
                                    }
                                }

                                if (!Directory.Exists(programNameText.Text + "KeyCards"))
                                {
                                    Directory.CreateDirectory(programNameText.Text + "KeyCards");
                                }
                                cloneImage.Save(programNameText.Text + "KeyCards/" + key + ".tif", ImageFormat.Tiff);
                            }
                        }
                    }
                }
            }
        }

        private void openImageButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (keyCardBitmap != null)
                {
                    keyCardBitmap.Dispose();
                }
                keyCardPreview.Image = null;
                keyCardBitmap = Bitmap.FromFile(openFileDialog.FileName);
                keyCardPreview.Image = keyCardBitmap;
            }
        }
    }
}

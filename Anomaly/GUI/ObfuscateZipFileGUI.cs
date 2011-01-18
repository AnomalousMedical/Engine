using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Anomaly
{
    public partial class ObfuscateZipFileGUI : Form
    {
        public ObfuscateZipFileGUI()
        {
            InitializeComponent();
        }

        private void browseSourceFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sourceTextBox.Text = openFileDialog1.FileName;
                destTextBox.Text = sourceTextBox.Text.Replace(".zip", ".dat");
            }
        }

        private void browseDestFileButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                destTextBox.Text = saveFileDialog1.FileName;
            }
        }

        private void obfuscateButton_Click(object sender, EventArgs e)
        {
            if (sourceTextBox.Text != null)
            {
                if (destTextBox.Text != null)
                {
                    PublishController.obfuscateZipFile(sourceTextBox.Text, destTextBox.Text);
                }
            }
        }
    }
}

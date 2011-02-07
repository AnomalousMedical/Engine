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

namespace KeyGenerator
{
    public partial class KeyForm : Form
    {
        private ConfigFile configFile;
        private ConfigSection keySection;
        private List<String> usedKeys = new List<string>();

        public KeyForm()
        {
            InitializeComponent();
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
                programNameText.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
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
                String key = KeyCreator.createKey(programNameText.Text);
                while (usedKeys.Contains(key) && !KeyChecker.checkValid(programNameText.Text, key))
                {
                    key = KeyCreator.createKey(programNameText.Text);
                }
                keySection.setValue("Key" + usedKeys.Count, key);
                usedKeys.Add(key);
                keyText.Text = key;
                Clipboard.SetData(DataFormats.Text, key);
            }
        }
    }
}

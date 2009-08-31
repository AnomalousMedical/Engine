using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Resources;
using Engine;
using System.IO;

namespace Anomaly
{
    partial class PublishGUI : Form
    {
        private PublishController fileList;
        private Dictionary<String, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();

        public PublishGUI()
        {
            InitializeComponent();
        }

        public void initialize(AnomalyController controller)
        {
            fileList = new PublishController(controller.AnomalyProject);
        }

        public void scanResources(ResourceManager resourceManager)
        {
            fileList.scanResources(resourceManager);
            groups.Clear();
            fileView.Groups.Clear();
            fileView.Items.Clear();
            foreach (FileInfo file in fileList.getPrettyFileList())
            {
                String directory = Path.GetDirectoryName(file.File);
                ListViewGroup group;
                groups.TryGetValue(directory, out group);
                if (group == null)
                {
                    group = new ListViewGroup(directory);
                    groups.Add(directory, group);
                    fileView.Groups.Add(group);
                }
                ListViewItem listViewFile = new ListViewItem(Path.GetFileName(file.File), group);
                fileView.Items.Add(listViewFile);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            scanResources(PluginManager.Instance.PrimaryResourceManager);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            fileNameColumn.Width = fileView.Width;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                outputLocationTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void publishButton_Click(object sender, EventArgs e)
        {
            if (outputLocationTextBox.Text == String.Empty)
            {
                MessageBox.Show(this, "Please input a destination for copying.", "Publish Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (archiveCheckBox.Checked && archiveNameText.Text == String.Empty)
            {
                MessageBox.Show(this, "Please input an archive name.", "Publish Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    String destination = Path.GetFullPath(outputLocationTextBox.Text);
                    if (destination.ToLower() != Path.GetFullPath(Resource.ResourceRoot).ToLower())
                    {
                        fileList.copyResources(destination, archiveNameText.Text, archiveCheckBox.Checked, obfuscateCheckBox.Checked);
                        MessageBox.Show(this, String.Format("Finished publishing resources to:\n{0}.", destination), "Publish Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(this, String.Format("Can not publish resources to the same directory as the resource root:\n{0}.", Path.GetFullPath(Resource.ResourceRoot)), "Publish Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, String.Format("Error copying files:\n{0}.", ex.Message), "Publish Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Resources;
using Engine;
using System.IO;
using Editor;

namespace Anomaly
{
    partial class PublishGUI : Form
    {
        private PublishController fileList;
        private Dictionary<String, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();

        public PublishGUI()
        {
            InitializeComponent();
            fileView.CheckBoxes = true;
            fileView.ItemChecked += new ItemCheckedEventHandler(fileView_ItemChecked);
            fileView.MouseClick += new MouseEventHandler(fileView_MouseClick);
        }

        public void initialize(AnomalyController controller)
        {
            fileList = new PublishController(controller.Solution);
            fileList.DirectoryIgnored += new EventHandler<PublishControllerEventArgs>(fileList_DirectoryIgnored);

            resourceProfileCombo.Items.Add("None");
            foreach (String profileFile in Directory.GetFiles(controller.Solution.WorkingDirectory, "*.rpr", SearchOption.TopDirectoryOnly))
            {
                resourceProfileCombo.Items.Add(Path.GetFileNameWithoutExtension(profileFile));
            }
            resourceProfileCombo.Items.Add("New...");
        }

        public void scanResources(String resourceProfile)
        {
            fileList.clearIgnoreDirectories();
            fileList.clearIgnoreFiles();
            fileList.scanResources();
            if(resourceProfile != null)
            {
                fileList.openResourceProfile(resourceProfile);
            }
            groups.Clear();
            fileView.Groups.Clear();
            fileView.Items.Clear();
            foreach (VirtualFileInfo file in fileList.getPrettyFileList())
            {
                String directory = file.DirectoryName;
                if (!fileList.isIgnoreDirectory(directory) && !fileList.isIgnoreFile(file))
                {
                    ListViewGroup group;
                    groups.TryGetValue(directory, out group);
                    if (group == null)
                    {
                        group = new ListViewGroup(directory);
                        groups.Add(directory, group);
                        fileView.Groups.Add(group);
                    }
                    ListViewItem listViewFile = new ListViewItem(file.Name, group);
                    listViewFile.Checked = true;
                    listViewFile.Tag = file;
                    fileView.Items.Add(listViewFile);
                }
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            allowResourceProfileSelectedIndexChanged = false;
            resourceProfileCombo.SelectedIndex = 0;
            allowResourceProfileSelectedIndexChanged = true;
            scanResources(null);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            fileNameColumn.Width = fileView.Width;
        }

        void fileView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                fileList.removeIgnoreFile((VirtualFileInfo)e.Item.Tag);
            }
            else
            {
                fileList.addIgnoreFile((VirtualFileInfo)e.Item.Tag);
            }
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
                    fileList.copyResources(destination, archiveNameText.Text, archiveCheckBox.Checked, obfuscateCheckBox.Checked);
                    MessageBox.Show(this, String.Format("Finished publishing resources to:\n{0}.", destination), "Publish Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, String.Format("Error copying files:\n{0}.", ex.Message), "Publish Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private ListViewItem directoryRemoveItem;

        void fileView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                directoryRemoveItem = fileView.GetItemAt(e.X, e.Y);
                groupMenuStrip.Show(fileView.PointToScreen(e.Location), ToolStripDropDownDirection.Default);
            }
        }

        private void ignoreDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VirtualFileInfo vfi = (VirtualFileInfo)directoryRemoveItem.Tag;
            fileList.addIgnoreDirectory(vfi.DirectoryName);
        }

        void fileList_DirectoryIgnored(object sender, PublishControllerEventArgs e)
        {
            ListViewGroup group = groups[e.Path];
            ListViewItem[] groupItems = new ListViewItem[group.Items.Count];
            group.Items.CopyTo(groupItems, 0);
            foreach (ListViewItem item in groupItems)
            {
                fileView.Items.Remove(item);
            }
        }

        bool allowResourceProfileSelectedIndexChanged = true;

        private void resourceProfileCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (allowResourceProfileSelectedIndexChanged)
            {
                if (resourceProfileCombo.SelectedIndex == resourceProfileCombo.Items.Count - 1)
                {
                    InputResult result = InputBox.GetInput("New Resource Profile", "Enter the name of the new resource profile.", this, null);
                    if (result.ok)
                    {
                        resourceProfileCombo.Items.Insert(resourceProfileCombo.SelectedIndex, result.text);
                        allowResourceProfileSelectedIndexChanged = false;
                        resourceProfileCombo.SelectedItem = result.text;
                        allowResourceProfileSelectedIndexChanged = true;
                        archiveNameText.Text = result.text;
                    }
                }
                else if (resourceProfileCombo.SelectedIndex == 0)
                {
                    scanResources(null);
                    archiveNameText.Text = "";
                }
                else
                {
                    scanResources(resourceProfileCombo.SelectedItem.ToString());
                    archiveNameText.Text = resourceProfileCombo.SelectedItem.ToString();
                }
            }
        }

        private void saveResourceProfile_Click(object sender, EventArgs e)
        {
            if (resourceProfileCombo.SelectedIndex != 0)
            {
                fileList.saveResourceProfile(resourceProfileCombo.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show(this, "Cannot save the \"None\" resource profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

﻿using Anomalous.OSPlatform;
using Engine;
using Engine.Threads;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly.GUI
{
    class PublishWindow : Dialog
    {
        private PublishController publishController;
        bool allowResourceProfileSelectedIndexChanged = true;
        private AnomalyController controller;

        private ComboBox resourceProfileCombo;
        private EditBox outputLocationTextBox;
        private EditBox archiveNameText;
        private CheckButton archiveCheckBox;
        private CheckButton obfuscateCheckBox;
        private FileBrowserTree fileTree;

        public PublishWindow(AnomalyController controller)
            : base("Anomaly.GUI.PublishWindow.PublishWindow.layout")
        {
            this.controller = controller;

            resourceProfileCombo = window.findWidget("Profile") as ComboBox;
            resourceProfileCombo.EventComboChangePosition += resourceProfileCombo_EventComboChangePosition;

            Button browseButton = window.findWidget("Browse") as Button;
            browseButton.MouseButtonClick += browseButton_MouseButtonClick;

            Button publishButton = window.findWidget("Publish") as Button;
            publishButton.MouseButtonClick += publishButton_MouseButtonClick;

            Button saveProfile = window.findWidget("Save") as Button;
            saveProfile.MouseButtonClick += saveProfile_MouseButtonClick;

            archiveCheckBox = new CheckButton(window.findWidget("CreateArchive") as Button);
            obfuscateCheckBox = new CheckButton(window.findWidget("Obfuscate") as Button);

            outputLocationTextBox = window.findWidget("OutputDir") as EditBox;
            outputLocationTextBox.OnlyText = AnomalyConfig.LastPublishDirectory;

            archiveNameText = window.findWidget("ArchiveName") as EditBox;

            publishController = new PublishController(controller.Solution);
            //fileList.DirectoryIgnored += new EventHandler<PublishControllerEventArgs>(fileList_DirectoryIgnored);
            //fileList.ExternalFileAdded += new EventHandler<PublishControllerEventArgs>(fileList_ExternalFileAdded);

            resourceProfileCombo.addItem("None");
            foreach (String profileFile in Directory.EnumerateFiles(controller.Solution.WorkingDirectory, "*.rpr", SearchOption.TopDirectoryOnly))
            {
                resourceProfileCombo.addItem(Path.GetFileNameWithoutExtension(profileFile));
            }
            resourceProfileCombo.addItem("New...");

            fileTree = new FileBrowserTree(window.findWidget("Scroller") as ScrollView, publishController);
        }

        public override void Dispose()
        {
            fileTree.Dispose();
            base.Dispose();
        }

        public void scanResources(String resourceProfile)
        {
            publishController.scanResources(resourceProfile);
            fileTree.refreshFiles();
        }

        protected override void onShown(EventArgs args)
        {
            base.onShown(args);
            allowResourceProfileSelectedIndexChanged = false;
            resourceProfileCombo.SelectedIndex = 0;
            allowResourceProfileSelectedIndexChanged = true;
            scanResources(null);
        }

        void browseButton_MouseButtonClick(Widget source, EventArgs e)
        {
            DirDialog dirDialog = new DirDialog(controller.MainWindow);
            dirDialog.showModal((result, dir) =>
            {
                if (result == NativeDialogResult.OK)
                {
                    outputLocationTextBox.OnlyText = dir;
                }
            });

        }

        void publishButton_MouseButtonClick(Widget source, EventArgs e)
        {
            if (outputLocationTextBox.OnlyText == String.Empty)
            {
                MessageBox.show("Please input a destination for copying.", "Publish Error!", MessageBoxStyle.IconError | MessageBoxStyle.Ok);
            }
            else if (archiveCheckBox.Checked && archiveNameText.OnlyText == String.Empty)
            {
                MessageBox.show("Please input an archive name.", "Publish Error!", MessageBoxStyle.IconError | MessageBoxStyle.Ok);
            }
            else
            {
                try
                {
                    AnomalyConfig.LastPublishDirectory = outputLocationTextBox.OnlyText;
                    String destination = Path.GetFullPath(outputLocationTextBox.OnlyText);
                    window.ClientWidget.Enabled = false;
                    Task task = new Task(() =>
                        {
                            publishController.copyResources(destination, archiveNameText.OnlyText, archiveCheckBox.Checked, obfuscateCheckBox.Checked);
                            ThreadManager.invoke(() =>
                            {
                                MessageBox.show(String.Format("Finished publishing resources to:\n{0}.", destination), "Publish Complete", MessageBoxStyle.IconInfo | MessageBoxStyle.Ok);
                                window.ClientWidget.Enabled = true;
                            });
                        });
                    task.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.show(String.Format("Error copying files:\n{0}.", ex.Message), "Publish Error!", MessageBoxStyle.IconError | MessageBoxStyle.Ok);
                }
            }
        }

        void resourceProfileCombo_EventComboChangePosition(Widget source, EventArgs e)
        {
            if (allowResourceProfileSelectedIndexChanged)
            {
                if (resourceProfileCombo.SelectedIndex == resourceProfileCombo.ItemCount - 1) //New item
                {
                    InputBox.GetInput("New Resource Profile", "Enter the name of the new resource profile.", true, delegate(String result, ref String message)
                    {
                        resourceProfileCombo.insertItemAt(resourceProfileCombo.SelectedIndex, result);
                        allowResourceProfileSelectedIndexChanged = false;
                        resourceProfileCombo.SelectedIndex = resourceProfileCombo.findItemIndexWith(result);
                        allowResourceProfileSelectedIndexChanged = true;
                        archiveNameText.OnlyText = result;

                        return true;
                    });
                }
                else if (resourceProfileCombo.SelectedIndex == 0)
                {
                    scanResources(null);
                    archiveNameText.OnlyText = "";
                }
                else
                {
                    scanResources(resourceProfileCombo.SelectedItemName);
                    archiveNameText.OnlyText = resourceProfileCombo.SelectedItemName;
                }
            }
        }

        void saveProfile_MouseButtonClick(Widget source, EventArgs e)
        {
            if (resourceProfileCombo.SelectedIndex != 0)
            {
                publishController.saveResourceProfile(resourceProfileCombo.SelectedItemName);
            }
            else
            {
                MessageBox.show("Cannot save the \"None\" resource profile.", "Error", MessageBoxStyle.IconError | MessageBoxStyle.Ok);
            }
        }

        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    fileNameColumn.Width = fileView.Width;
        //}

        //void fileView_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    if (e.Item.Checked)
        //    {
        //        fileList.removeIgnoreFile((VirtualFileInfo)e.Item.Tag);
        //    }
        //    else
        //    {
        //        fileList.addIgnoreFile((VirtualFileInfo)e.Item.Tag);
        //    }
        //}

        //private ListViewItem directoryRemoveItem;

        //void fileView_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        directoryRemoveItem = fileView.GetItemAt(e.X, e.Y);
        //        groupMenuStrip.Show(fileView.PointToScreen(e.Location), ToolStripDropDownDirection.Default);
        //    }
        //}

        //private void ignoreDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    VirtualFileInfo vfi = (VirtualFileInfo)directoryRemoveItem.Tag;
        //    fileList.addIgnoreDirectory(vfi.DirectoryName);
        //}

        //void fileList_DirectoryIgnored(object sender, PublishControllerEventArgs e)
        //{
        //    ListViewGroup group;
        //    if(groups.TryGetValue(e.FileInfo.FullName, out group))
        //    {
        //        ListViewItem[] groupItems = new ListViewItem[group.Items.Count];
        //        group.Items.CopyTo(groupItems, 0);
        //        foreach (ListViewItem item in groupItems)
        //        {
        //            fileView.Items.Remove(item);
        //        }
        //    }
        //}

        //void fileList_ExternalFileAdded(object sender, PublishControllerEventArgs e)
        //{
        //    if (allowExternalFileChanges)
        //    {
        //        addFileToGUIList(e.FileInfo);
        //    }
        //}

        //private void addDirectoryButton_Click(object sender, EventArgs e)
        //{
        //    if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        String resourceRoot = controller.Solution.ResourceRoot;
        //        String folder = folderBrowserDialog.SelectedPath.Substring(resourceRoot.Length);
        //        if (VirtualFileSystem.Instance.exists(folder))
        //        {
        //            fileList.addExternalDirectory(VirtualFileSystem.Instance.getFileInfo(folder), recursiveCheckBox.Checked);
        //        }
        //        else
        //        {
        //            MessageBox.Show(this, String.Format("Could not add path {0} because it is not under the resource root {1}.", folderBrowserDialog.SelectedPath, resourceRoot), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        //private void addFileButton_Click(object sender, EventArgs e)
        //{
        //    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
        //    {
        //        String resourceRoot = controller.Solution.ResourceRoot;
        //        String file = openFileDialog.FileName.Substring(resourceRoot.Length);
        //        if (VirtualFileSystem.Instance.exists(file))
        //        {
        //            fileList.addExternalFile(VirtualFileSystem.Instance.getFileInfo(file));
        //        }
        //        else
        //        {
        //            MessageBox.Show(this, String.Format("Could not add path {0} because it is not under the resource root {1}.", openFileDialog.FileName, resourceRoot), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}
    }
}

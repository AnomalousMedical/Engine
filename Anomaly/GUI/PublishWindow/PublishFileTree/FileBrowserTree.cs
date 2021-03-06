﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGUIPlugin;
using System.IO;
using Engine.Platform;
using Engine;
using Logging;
using Anomalous.GuiFramework;

namespace Anomaly.GUI
{
    delegate void FileBrowserEvent(FileBrowserTree tree, String path);
    delegate void FileBrowserNodeContextEvent(FileBrowserTree tree, String path, bool isDirectory, bool isTopLevel);

    /// <summary>
    /// A tree view for a publish controller's files.
    /// </summary>
    class FileBrowserTree : IDisposable
    {
        private readonly char[] SEPS = new char[] { '/', '\\' };

        private IntVector2 lastMouseEventPos;
        private Tree fileTree;
        private PublishController publishController;
        private DirectoryNode baseNode;

        public event FileBrowserEvent FileSelected;
        public event FileBrowserNodeContextEvent NodeContextEvent;

        public FileBrowserTree(ScrollView treeScrollView, PublishController publishController)
        {
            this.publishController = publishController;

            fileTree = new Tree(treeScrollView);
            fileTree.NodeMouseReleased += new EventHandler<TreeMouseEventArgs>(fileTree_NodeMouseReleased);

            baseNode = new DirectoryNode("Publishing Files", this);
            fileTree.Nodes.add(baseNode);
            baseNode.Expanded = true;
        }

        public void Dispose()
        {
            fileTree.Dispose();
        }

        public void refreshFiles()
        {
            fileTree.SuppressLayout = true;
            baseNode.Children.clear();
            foreach (VirtualFileInfo file in publishController.getPrettyFileList())
            {
                fileAdded(file);
            }
            fileTree.SuppressLayout = false;
            layout();
        }

        public void layout()
        {
            fileTree.layout();
        }

        public void showContextMenu(ContextMenu contextMenu)
        {
            contextMenu.showMenu(lastMouseEventPos);
        }

        private DirectoryNode getNodeForPath(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return baseNode;
            }

            String[] names = path.Split(SEPS, StringSplitOptions.RemoveEmptyEntries);
            DirectoryNode parent = baseNode;
            DirectoryNode result = null;
            for (int i = 0; i < names.Length; ++i)
            {
                result = parent.Children.findByText(names[i]) as DirectoryNode;
                if (result == null)
                {
                    result = new DirectoryNode(names[i], this);
                    parent.addDirectoryNode(result);
                }
                parent = result;
            }
            return result;
        }

        private void fileAdded(VirtualFileInfo file)
        {
            String parentPath = file.DirectoryName;
            DirectoryNode node = getNodeForPath(file.DirectoryName) as DirectoryNode;
            node.addFileNode(new FileNode(file));
        }

        void fileTree_NodeMouseReleased(object sender, TreeMouseEventArgs e)
        {
            if (e.Button == MouseButtonCode.MB_BUTTON1)
            {
                lastMouseEventPos = e.MousePosition;
                fileTree.SelectedNode = e.Node;
                DirectoryNode dirNode = e.Node as DirectoryNode;
                if (dirNode != null)
                {
                    if (NodeContextEvent != null)
                    {
                        NodeContextEvent.Invoke(this, dirNode.DirectoryPath, true, dirNode.Parent == null);
                    }
                }
                else
                {
                    FileNode fileNode = e.Node as FileNode;
                    if (fileNode != null)
                    {
                        if (NodeContextEvent != null)
                        {
                            NodeContextEvent.Invoke(this, fileNode.File.Name, false, fileNode.Parent == null);
                        }
                    }
                }
            }
        }
    }
}
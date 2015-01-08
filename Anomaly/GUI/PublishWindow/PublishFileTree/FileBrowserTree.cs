using System;
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

            baseNode = new DirectoryNode("", this);
            fileTree.Nodes.add(baseNode);
            baseNode.Expanded = true;


            publishController.DirectoryIgnored += publishController_DirectoryIgnored;
            publishController.ExternalFileAdded += publishController_ExternalFileAdded;
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
            PopupMenu popupMenu = (PopupMenu)Gui.Instance.createWidgetT("PopupMenu", "PopupMenu", 0, 0, 1, 1, Align.Default, "Overlapped", "");
            popupMenu.Visible = false;
            popupMenu.ItemAccept += new MyGUIEvent(popupMenu_ItemAccept);
            popupMenu.Closed += new MyGUIEvent(popupMenu_Closed);
            foreach (ContextMenuItem item in contextMenu.Items)
            {
                MenuItem menuItem = popupMenu.addItem(item.Text, MenuItemType.Normal, item.Text);
                menuItem.UserObject = item;
            }
            LayerManager.Instance.upLayerItem(popupMenu);
            popupMenu.setPosition(lastMouseEventPos.x, lastMouseEventPos.y);
            popupMenu.ensureVisible();
            popupMenu.setVisibleSmooth(true);
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
            node.addFileNode(new FileNode(file.Name));
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
                            NodeContextEvent.Invoke(this, fileNode.FilePath, false, fileNode.Parent == null);
                        }
                    }
                }
            }
        }

        void popupMenu_ItemAccept(Widget source, EventArgs e)
        {
            MenuCtrlAcceptEventArgs mcae = (MenuCtrlAcceptEventArgs)e;
            ((ContextMenuItem)mcae.Item.UserObject).execute();
        }

        void popupMenu_Closed(Widget source, EventArgs e)
        {
            Gui.Instance.destroyWidget(source);
        }

        void publishController_ExternalFileAdded(object sender, PublishControllerEventArgs e)
        {
            //if (allowExternalFileChanges)
            //{
            //    addFileToGUIList(e.FileInfo);
            //}
        }

        void publishController_DirectoryIgnored(object sender, PublishControllerEventArgs e)
        {
            //ListViewGroup group;
            //if (groups.TryGetValue(e.FileInfo.FullName, out group))
            //{
            //    ListViewItem[] groupItems = new ListViewItem[group.Items.Count];
            //    group.Items.CopyTo(groupItems, 0);
            //    foreach (ListViewItem item in groupItems)
            //    {
            //        fileView.Items.Remove(item);
            //    }
            //}
        }
    }
}
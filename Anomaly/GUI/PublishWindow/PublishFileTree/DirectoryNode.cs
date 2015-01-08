using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MyGUIPlugin;

namespace Anomaly.GUI
{
    class DirectoryNode : TreeNode
    {
        private FileBrowserTree projectExplorer;
        private bool sortStatusDirty = true;

        public DirectoryNode(String directory, FileBrowserTree projectExplorer)
        {
            this.projectExplorer = projectExplorer;
            this.ImageResource = "EditorFileIcon/FolderIcon";
            changePath(directory);
        }

        public void changePath(String directory)
        {
            Text = Path.GetFileName(directory);
            DirectoryPath = directory;
        }

        protected override void expandedStatusChanged(bool expanding)
        {
            if(sortStatusDirty)
            {
                Children.sort(FileTreeSorter.Compare);
                sortStatusDirty = false;
            }
            base.expandedStatusChanged(expanding);
        }

        public String DirectoryPath { get; set; }

        public void addDirectoryNode(DirectoryNode node)
        {
            Children.add(node);
            if(Expanded)
            {
                sortStatusDirty = false;
                Children.sort(FileTreeSorter.Compare);
            }
            else
            {
                sortStatusDirty = true;
            }
        }

        public void addFileNode(FileNode node)
        {
            Children.add(node);
            if (Expanded)
            {
                sortStatusDirty = false;
                Children.sort(FileTreeSorter.Compare);
            }
            else
            {
                sortStatusDirty = true;
            }
        }
    }
}

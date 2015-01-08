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

        public String DirectoryPath { get; set; }

        public void addDirectoryNode(DirectoryNode node)
        {
            Children.add(node);
            Children.sort(FileTreeSorter.Compare);
        }

        public void addFileNode(FileNode node)
        {
            Children.add(node);
            Children.sort(FileTreeSorter.Compare);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MyGUIPlugin;
using Engine;

namespace Anomaly.GUI
{
    class FileNode : TreeNode
    {
        public FileNode(VirtualFileInfo file)
        {
            Text = file.Name;
            this.File = file;
            ImageResource = "EditorFileIcon/" + Path.GetExtension(file.Name).ToLowerInvariant();
        }

        public VirtualFileInfo File { get; set; }
    }
}

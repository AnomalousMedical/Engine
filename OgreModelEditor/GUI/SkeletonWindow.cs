using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OgreWrapper;
using WeifenLuo.WinFormsUI.Docking;

namespace OgreModelEditor
{
    public partial class SkeletonWindow : DockContent
    {
        public SkeletonWindow()
        {
            InitializeComponent();
        }

        public void setSkeleton(SkeletonInstance skeleton)
        {
            skeletonTree.Nodes.Clear();
            for (ushort i = 0; i < skeleton.getNumBones(); i++)
            {
                skeletonTree.Nodes.Add(new TreeNode(skeleton.getBone(i).getName()));
            }
        }

        public void clearSkeleton()
        {
            skeletonTree.Nodes.Clear();
        }
    }
}

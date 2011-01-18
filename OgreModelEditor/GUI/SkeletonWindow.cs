using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                Bone bone = skeleton.getBone(i);
                TreeNode skeletonNode = new TreeNode(bone.getName());
                TreeNode positionNode = new TreeNode("Position " + bone.getPosition());
                skeletonNode.Nodes.Add(positionNode);
                TreeNode rotationNode = new TreeNode("Rotation" + bone.getOrientation());
                skeletonNode.Nodes.Add(rotationNode);
                skeletonTree.Nodes.Add(skeletonNode);
            }
        }

        public void clearSkeleton()
        {
            skeletonTree.Nodes.Clear();
        }
    }
}

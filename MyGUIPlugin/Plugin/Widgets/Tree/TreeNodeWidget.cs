using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public abstract class TreeNodeWidget : IDisposable
    {
        protected TreeNode treeNode;

        public abstract void Dispose();

        public abstract void createWidget(Widget parent, String caption, String imageResource);

        public abstract void destroyWidget();

        public abstract void setCoord(int left, int top, int width, int height);

        public abstract void updateExpandedStatus(bool expanded);

        internal void setTreeNode(TreeNode node)
        {
            this.treeNode = node;
        }

        protected void fireExpandToggled()
        {
            if (treeNode != null)
            {
                treeNode.Expanded = !treeNode.Expanded;
                treeNode.Tree.layout();
            }
        }

        protected void fireNodeSelected()
        {
            if (treeNode != null)
            {
                treeNode.setSelected();
            }
        }
    }
}

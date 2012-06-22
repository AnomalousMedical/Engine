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

        public abstract void updateSelectionStatus(bool selected);

        internal void setTreeNode(TreeNode node)
        {
            this.treeNode = node;
        }

        internal abstract void updateText();

        internal abstract void updateImageResource();

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
                treeNode.Tree.SelectedNode = treeNode;
            }
        }

        protected void fireNodeMouseClicked()
        {
            if (treeNode != null)
            {
                treeNode.Tree.fireNodeMouseClicked(treeNode);
            }
        }

        protected void fireNodeMouseDoubleClicked()
        {
            if (treeNode != null)
            {
                treeNode.Tree.fireNodeMouseDoubleClicked(treeNode);
            }
        }

        internal void fireNodeMousePressed(MouseEventArgs me)
        {
            if (treeNode != null)
            {
                treeNode.Tree.fireNodeMousePressed(treeNode, me);
            }
        }

        internal void fireNodeMouseReleased(MouseEventArgs me)
        {
            if (treeNode != null)
            {
                treeNode.Tree.fireNodeMouseReleased(treeNode, me);
            }
        }
    }
}

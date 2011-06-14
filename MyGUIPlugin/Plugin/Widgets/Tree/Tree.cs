using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class Tree : IDisposable
    {
        public event EventHandler<TreeCancelEventArgs> BeforeSelect;
        public event EventHandler<TreeEventArgs> AfterSelect;
        public event EventHandler<TreeEventArgs> NodeMouseClick;
        public event EventHandler<TreeEventArgs> NodeMouseDoubleClick;
        public event EventHandler<TreeMouseEventArgs> NodeMousePressed;
        public event EventHandler<TreeMouseEventArgs> NodeMouseReleased;

        private TreeNodeCollection rootNodes;
        private ScrollView scrollView;
        private TreeNode selectedNode = null;
        private TreeCancelEventArgs eventArgs = new TreeCancelEventArgs();
        private TreeMouseEventArgs mouseEventArgs = new TreeMouseEventArgs();

        public Tree(ScrollView scrollView)
        {
            this.scrollView = scrollView;
            rootNodes = new TreeNodeCollection(null);
            rootNodes.Tree = this;
            ItemIndentation = 10;
            NodeHeight = 20;
            SuppressLayout = false;
        }

        public void Dispose()
        {
            rootNodes.Dispose();
        }

        public void layout()
        {
            if (!SuppressLayout)
            {
                int width = scrollView.ClientCoord.width;
                int currentY = 0;
                foreach (TreeNode rootNode in rootNodes)
                {
                    layoutNode(rootNode, 0, width, ref currentY);
                }
                scrollView.CanvasSize = new Size2(width, currentY);
            }
        }

        public void expandAll()
        {
            this.SuppressLayout = true;
            foreach (TreeNode node in rootNodes)
            {
                node.expandAll();
            }
            this.SuppressLayout = false;
            this.layout();
        }

        private void layoutNode(TreeNode node, int currentIndent, int currentWidth, ref int currentY)
        {
            node.setWidgetCoord(currentIndent, currentY, currentWidth, NodeHeight);
            currentY += NodeHeight;
            if (node.Expanded)
            {
                currentIndent += ItemIndentation;
                currentWidth -= ItemIndentation;
                foreach (TreeNode child in node.Children)
                {
                    layoutNode(child, currentIndent, currentWidth, ref currentY);
                }
            }
        }

        public TreeNodeCollection Nodes
        {
            get
            {
                return rootNodes;
            }
        }

        public TreeNode SelectedNode
        {
            get
            {
                return selectedNode;
            }
            set
            {
                eventArgs.reset();
                eventArgs.Node = selectedNode;
                if (BeforeSelect != null)
                {
                    BeforeSelect.Invoke(this, eventArgs);
                }
                if (!eventArgs.Cancel)
                {
                    if (selectedNode != null)
                    {
                        selectedNode.alertSelection(false);
                    }
                    selectedNode = value;
                    if (selectedNode != null)
                    {
                        selectedNode.alertSelection(true);
                    }
                    eventArgs.Node = selectedNode;
                    if (AfterSelect != null)
                    {
                        AfterSelect.Invoke(this, eventArgs);
                    }
                }
            }
        }

        public int ItemIndentation { get; set; }

        public int NodeHeight { get; set; }

        public bool SuppressLayout { get; set; }

        internal ScrollView Widget
        {
            get
            {
                return scrollView;
            }
        }

        internal void fireNodeMouseClicked(TreeNode node)
        {
            if (NodeMouseClick != null)
            {
                eventArgs.reset();
                eventArgs.Node = node;
                NodeMouseClick.Invoke(this, eventArgs);
            }
        }

        internal void fireNodeMouseDoubleClicked(TreeNode node)
        {
            if (NodeMouseDoubleClick != null)
            {
                eventArgs.reset();
                eventArgs.Node = node;
                NodeMouseDoubleClick.Invoke(this, eventArgs);
            }
        }

        internal void fireNodeMousePressed(TreeNode node, MouseEventArgs me)
        {
            if (NodeMousePressed != null)
            {
                mouseEventArgs.setData(me);
                mouseEventArgs.Node = node;
                NodeMousePressed.Invoke(this, mouseEventArgs);
            }
        }

        internal void fireNodeMouseReleased(TreeNode node, MouseEventArgs me)
        {
            if (NodeMouseReleased != null)
            {
                mouseEventArgs.setData(me);
                mouseEventArgs.Node = node;
                NodeMouseReleased.Invoke(this, mouseEventArgs);
            }
        }
    }
}

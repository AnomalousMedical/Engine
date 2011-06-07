using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    public class Tree : IDisposable
    {
        private TreeNodeCollection rootNodes;
        private ScrollView scrollView;

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
    }
}

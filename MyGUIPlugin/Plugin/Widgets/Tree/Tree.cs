using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    /// <summary>
    /// A tree widget.
    /// </summary>
    /// <remarks>
    /// The view can be customized by some user options on the ScrollView.
    /// 
    /// ItemIndentation - The indentation on each level.
    /// NodeHeight - The height of each node.
    /// </remarks>
    public class Tree : IDisposable
    {
        public event EventHandler<TreeCancelEventArgs> BeforeSelect;
        public event EventHandler<TreeEventArgs> AfterSelect;
        public event EventHandler<TreeEventArgs> NodeMouseDoubleClick;
        public event EventHandler<TreeMouseEventArgs> NodeMousePressed;
        public event EventHandler<TreeMouseEventArgs> NodeMouseReleased;

        private TreeNodeCollection rootNodes;
        private ScrollView scrollView;
        private TreeNode selectedNode = null;
        private TreeCancelEventArgs eventArgs = new TreeCancelEventArgs();
        private TreeMouseEventArgs mouseEventArgs = new TreeMouseEventArgs();
        private bool allowClickEvents = true;

        public Tree(ScrollView scrollView)
        {
            this.scrollView = scrollView;
            scrollView.CanvasPositionChanged += scrollView_CanvasPositionChanged;
            rootNodes = new TreeNodeCollection(null);
            rootNodes.Tree = this;
            SuppressLayout = false;

            String read;
            int intValue;

            //Try to get properties from the widget itself.
            read = scrollView.getUserString("ItemIndentation");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                ItemIndentation = ScaleHelper.Scaled(intValue);
            }
            else
            {
                ItemIndentation = ScaleHelper.Scaled(10);
            }

            read = scrollView.getUserString("NodeHeight");
            if (read != null && NumberParser.TryParse(read, out intValue))
            {
                NodeHeight = ScaleHelper.Scaled(intValue);
            }
            else
            {
                NodeHeight = ScaleHelper.Scaled(20);
            }
        }

        public void Dispose()
        {
            scrollView.CanvasPositionChanged -= scrollView_CanvasPositionChanged;
            rootNodes.Dispose();
        }

        public void layout()
        {
            if (!SuppressLayout)
            {
                int width = scrollView.ViewCoord.width;
                int currentY = 0;
                foreach (TreeNode rootNode in rootNodes)
                {
                    layoutNode(rootNode, 0, width, ref currentY);
                }
                scrollView.CanvasSize = new IntSize2(width, currentY);
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

        /// <summary>
        /// This will be true if the scroll view for this tree contains the given point.
        /// </summary>
        /// <param name="x">The x coord of the point.</param>
        /// <param name="y">The y coord of the point.</param>
        /// <returns></returns>
        public bool contains(int x, int y)
        {
            return scrollView.contains(x, y);
        }

        public TreeNode itemAt(int x, int y)
        {
            return findItemAt(rootNodes, x, y);
        }

        private TreeNode findItemAt(TreeNodeCollection nodes, int x, int y)
        {
            foreach(var node in nodes)
            {
                if(node.contains(x, y))
                {
                    return node;
                }
                else
                {
                    if(node.Expanded)
                    {
                        TreeNode found = findItemAt(node.Children, x, y);
                        if(found != null)
                        {
                            return found;
                        }
                    }
                }
            }
            return null;
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

        public bool Enabled
        {
            get
            {
                return scrollView.Enabled;
            }
            set
            {
                scrollView.Enabled = value;
            }
        }

        internal ScrollView Widget
        {
            get
            {
                return scrollView;
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

        void scrollView_CanvasPositionChanged(Widget source, EventArgs e)
        {
            allowClickEvents = false;
        }

        /// <summary>
        /// This value is tracked to allow click events or not, it will
        /// be set to false whenever the scroll view scrolls, this way if
        /// the user is scrolling a view the item that is clicked after the scroll
        /// completes will not be activated. This prevents items activating
        /// if this button grid is scrolled with a finger.
        /// </summary>
        internal bool AllowClickEvents
        {
            get
            {
                return allowClickEvents;
            }
            set
            {
                allowClickEvents = value;
            }
        }
    }
}

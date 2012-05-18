using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class TreeNode : IDisposable
    {
        private TreeNodeCollection children;
        private Tree tree;
        private bool expanded = false;
        private TreeNodeWidget nodeWidget;
        private String text;

        public TreeNode()
            : this("")
        {

        }

        public TreeNode(String text)
            :this(text, new DefaultTreeNodeWidget())
        {
            
        }

        public TreeNode(String text, TreeNodeWidget nodeWidget)
        {
            children = new TreeNodeCollection(this);
            this.text = text;
            this.nodeWidget = nodeWidget;
            nodeWidget.setTreeNode(this);
        }

        public virtual void Dispose()
        {
            nodeWidget.Dispose();
            children.Dispose();
        }

        public bool Visible
        {
            get
            {
                if (Parent == null)
                {
                    return Expanded;
                }
                return Parent.Expanded;
            }
        }

        public bool Selected
        {
            get
            {
                return tree.SelectedNode == this;
            }
        }

        internal void createWidget(Widget parent)
        {
            nodeWidget.createWidget(parent, Text, null);
            nodeWidget.updateExpandedStatus(expanded);
            if (Expanded)
            {
                foreach (TreeNode child in children)
                {
                    child.createWidget(parent);
                }
            }
        }

        internal void destroyWidget()
        {
            nodeWidget.destroyWidget();
            if (Expanded)
            {
                foreach (TreeNode child in children)
                {
                    child.destroyWidget();
                }
            }
        }

        internal void setWidgetCoord(int left, int top, int width, int height)
        {
            nodeWidget.setCoord(left, top, width, height);
        }

        /// <summary>
        /// This will be called by TreeNodeCollection when this node gains its
        /// first child or looses its last child.
        /// </summary>
        internal void alertHasChildrenChanged()
        {
            nodeWidget.updateExpandedStatus(expanded);
        }

        /// <summary>
        /// This will be called by the tree when it changes the selection status
        /// of a node.
        /// </summary>
        /// <param name="selected">True to be selected.</param>
        internal void alertSelection(bool selected)
        {
            nodeWidget.updateSelectionStatus(selected);
        }

        public Tree Tree
        {
            get
            {
                return tree;
            }
            internal set
            {
                tree = value;
                children.Tree = value;
            }
        }

        public TreeNode Parent { get; internal set; }

        public TreeNodeCollection Children
        {
            get
            {
                return children;
            }
        }

        public String Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                nodeWidget.updateText();
            }
        }

        /// <summary>
        /// User data for this object. Does not get used by the TreeNode.
        /// </summary>
        public Object UserData { get; set; }

        public bool Expanded
        {
            get
            {
                return expanded;
            }
            set
            {
                if (expanded != value)
                {
                    expanded = value;
                    expandedStatusChanged();
                    if (expanded)
                    {
                        if (Visible)
                        {
                            foreach (TreeNode node in children)
                            {
                                node.createWidget(Tree.Widget);
                            }
                        }
                    }
                    else
                    {
                        foreach (TreeNode node in children)
                        {
                            node.destroyWidget();
                        }
                    }
                    nodeWidget.updateExpandedStatus(expanded);
                    Tree.layout();
                }
            }
        }

        public virtual bool HasChildren
        {
            get
            {
                return children.Count > 0;
            }
        }

        internal void expandAll()
        {
            Expanded = true;
            foreach (TreeNode node in children)
            {
                node.expandAll();
            }
        }

        /// <summary>
        /// This is called when the expanded status of a node changes.
        /// </summary>
        protected virtual void expandedStatusChanged()
        {

        }
    }
}

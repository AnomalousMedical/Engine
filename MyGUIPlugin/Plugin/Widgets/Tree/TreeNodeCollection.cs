using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MyGUIPlugin
{
    public class TreeNodeCollection : IEnumerable<TreeNode>, IDisposable
    {
        private List<TreeNode> items;
        private Tree tree;
        private TreeNode parent;

        internal TreeNodeCollection(TreeNode parent)
        {
            items = new List<TreeNode>();
            this.parent = parent;
        }

        /// <summary>
        /// All items in this collection will be disposed when it is disposed or cleared.
        /// </summary>
        public void Dispose()
        {
            foreach (TreeNode item in items)
            {
                item.Dispose();
            }
        }

        public virtual void add(TreeNode item)
        {
            items.Add(item);
            item.Tree = tree;
            item.Parent = parent;
            if (parent == null || parent.Expanded)
            {
                item.createWidget(Tree.Widget);
            }
            if (items.Count == 1 && parent != null)
            {
                parent.alertHasChildrenChanged();
            }
        }

        /// <summary>
        /// If an item is removed from the collection the user must dispose it.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public virtual void remove(TreeNode item)
        {
            items.Remove(item);
            item.Tree = null;
            item.Parent = null;
            if (parent == null || parent.Expanded)
            {
                item.destroyWidget();
            }
            if (items.Count == 0 && parent != null)
            {
                parent.alertHasChildrenChanged();
            }
        }

        /// <summary>
        /// Clear all elements in this collection. This will call dispose on the
        /// elements that are in the collection.
        /// </summary>
        public virtual void clear()
        {
            foreach (TreeNode item in items)
            {
                item.Dispose();
            }
            items.Clear();
        }

        public int getItemIndex(TreeNode item)
        {
            return items.IndexOf(item);
        }

        public TreeNode findByText(String text)
        {
            foreach (TreeNode node in items)
            {
                if (node.Text == text)
                {
                    return node;
                }
            }
            return null;
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        internal Tree Tree
        {
            get
            {
                return tree;
            }
            set
            {
                this.tree = value;
                foreach (TreeNode item in items)
                {
                    item.Tree = tree;
                }
            }
        }

        public TreeNode this[int i]
        {
            get
            {
                return items[i];
            }
            set
            {
                items.Insert(i, value);
                if (items.Count > i + 1)
                {
                    items.RemoveAt(i + 1);
                }
            }
        }

        public IEnumerator<TreeNode> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;

namespace Editor
{
    public partial class BrowserWindow : Form
    {
        public BrowserWindow()
        {
            InitializeComponent();
        }

        public void setBrowser(Browser browser)
        {
            browserTree.Nodes.Clear();
            TreeNode parentNode = addNodes(browser.getTopNode());
            browserTree.Nodes.Add(parentNode);
            parentNode.Expand();
        }

        private TreeNode addNodes(BrowserNode node)
        {
            TreeNode treeNode = new TreeNode(node.Text);
            treeNode.Tag = node.Value;
            foreach (BrowserNode child in node.getChildIterator())
            {
                treeNode.Nodes.Add(addNodes(child));
            }
            return treeNode;
        }

        /// <summary>
        /// The value that is selected on the tree. Can be null.
        /// </summary>
        public Object SelectedValue
        {
            get
            {
                if (browserTree.SelectedNode != null)
                {
                    return browserTree.SelectedNode.Tag;
                }
                return null;
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

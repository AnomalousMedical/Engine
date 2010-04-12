using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;
using Logging;
using Engine;

namespace Editor
{
    /// <summary>
    /// This is a tree node that wraps EditInterfaces.
    /// </summary>
    class EditInterfaceTreeNode : TreeNode
    {
        #region Fields

        private EditInterface editInterface;
        private SubInterfaceAdded interfaceAdded;
        private SubInterfaceRemoved interfaceRemoved;
        private EditInterfaceView view;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the EditInterface to wrap.
        /// </summary>
        /// <param name="editInterface">The EditInterface this tree node holds.</param>
        public EditInterfaceTreeNode(EditInterface editInterface, EditInterfaceView view)
            :base(editInterface.getName())
        {
            this.editInterface = editInterface;
            this.view = view;
            interfaceAdded = new SubInterfaceAdded(subInterfaceAdded);
            interfaceRemoved = new SubInterfaceRemoved(subInterfaceRemoved);
            editInterface.OnSubInterfaceAdded += interfaceAdded;
            editInterface.OnSubInterfaceRemoved += interfaceRemoved;
            editInterface.OnBackColorChanged += backColorChanged;
            editInterface.OnForeColorChanged += foreColorChanged;
            BackColor = System.Drawing.Color.FromArgb(editInterface.BackColor.toARGB());
            ForeColor = System.Drawing.Color.FromArgb(editInterface.ForeColor.toARGB());
            ImageKey = EditInterfaceIconCollection.getImageKey(editInterface.IconReferenceTag);
            SelectedImageKey = EditInterfaceIconCollection.getImageKey(editInterface.IconReferenceTag);
            if (editInterface.hasSubEditInterfaces())
            {
                foreach (EditInterface subInterface in editInterface.getSubEditInterfaces())
                {
                    this.Nodes.Add(new EditInterfaceTreeNode(subInterface, view));
                }
            }
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Disconnect the callbacks from this node and all children. Used to
        /// stop listening for updates from the EditInterface this node wraps.
        /// </summary>
        public void removeCallbacks()
        {
            editInterface.OnSubInterfaceAdded -= subInterfaceAdded;
            editInterface.OnSubInterfaceRemoved -= subInterfaceRemoved;
            editInterface.OnBackColorChanged -= backColorChanged;
            editInterface.OnForeColorChanged -= foreColorChanged;
            foreach (EditInterfaceTreeNode node in Nodes)
            {
                node.removeCallbacks();
            }
        }

        /// <summary>
        /// Callback when a sub interface is added.
        /// </summary>
        /// <param name="editInterface"></param>
        private void subInterfaceAdded(EditInterface editInterface)
        {
            this.Nodes.Add(new EditInterfaceTreeNode(editInterface, view));
            view.nodeAdded(this);
        }

        /// <summary>
        /// Callback when a sub interface is removed.
        /// </summary>
        /// <param name="editInterface"></param>
        private void subInterfaceRemoved(EditInterface editInterface)
        {
            EditInterfaceTreeNode matchingNode = null;
            foreach (EditInterfaceTreeNode node in Nodes)
            {
                if (node.EditInterface == editInterface)
                {
                    matchingNode = node;
                    break;
                }
            }
            if (matchingNode != null)
            {
                view.nodeRemoved(this);
                this.Nodes.Remove(matchingNode);
            }
            else
            {
                Log.Default.sendMessage("Malformed EditInterfaceTreeNodes the EditInterface {0} does not contain a child named {1} when remove was attempted.", LogLevel.Error, "Editor", this.editInterface.getName(), editInterface.getName());
            }
        }

        void foreColorChanged(EditInterface editInterface)
        {
            ForeColor = System.Drawing.Color.FromArgb(editInterface.ForeColor.toARGB());
        }

        void backColorChanged(EditInterface editInterface)
        {
            BackColor = System.Drawing.Color.FromArgb(editInterface.BackColor.toARGB());
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// Get the EditInterface in this tree node.
        /// </summary>
        public EditInterface EditInterface
        {
            get
            {
                return editInterface;
            }
        }

        #endregion Properties
    }
}

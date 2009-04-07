using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;

namespace Editor
{
    /// <summary>
    /// This is a tree node that wraps EditInterfaces.
    /// </summary>
    class EditInterfaceTreeNode : TreeNode
    {
        #region Fields

        private EditInterface editInterface;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the EditInterface to wrap.
        /// </summary>
        /// <param name="editInterface">The EditInterface this tree node holds.</param>
        public EditInterfaceTreeNode(EditInterface editInterface)
            :base(editInterface.getName())
        {
            this.editInterface = editInterface;
            foreach (EditInterface subInterface in editInterface.getSubEditInterfaces())
            {
                this.Nodes.Add(new EditInterfaceTreeNode(subInterface));
            }
        }

        #endregion Constructors

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

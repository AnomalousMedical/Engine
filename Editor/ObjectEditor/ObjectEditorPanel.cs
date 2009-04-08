using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;

namespace Editor
{
    /// <summary>
    /// This panel is a combination of the EditInterfaceView and the Properties
    /// table for a simple editor.
    /// </summary>
    public partial class ObjectEditorPanel : UserControl
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public ObjectEditorPanel()
        {
            InitializeComponent();
            editInterfaceView.OnEditInterfaceSelectionChanged += new EditInterfaceSelectionChanged(propertiesTable.showEditableProperties);
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Set the current EditInterface shown on this panel.
        /// </summary>
        /// <param name="editor">The editor to show.</param>
        public void setEditInterface(EditInterface editor)
        {
            editInterfaceView.setEditInterface(editor);
            propertiesTable.showEditableProperties(editor);
        }

        #endregion Functions

        
    }
}

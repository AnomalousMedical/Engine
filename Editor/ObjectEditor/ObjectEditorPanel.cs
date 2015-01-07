using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;
using Anomalous.GuiFramework.Editor;

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
            editInterfaceView.OnEditInterfaceSelectionChanging += editInterfaceView_OnEditInterfaceSelectionChanging;
            editInterfaceView.OnEditInterfaceSelectionChanged += editInterfaceView_OnEditInterfaceSelectionChanged;
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

        public bool validateEditInterface(out String errorMessage)
        {
            return editInterfaceView.validateAllInterfaces(out errorMessage);
        }

        public void clearEditInterface()
        {
            editInterfaceView.clearEditInterface();
        }

        /// <summary>
        /// Callback for when the EditInterface changes.
        /// </summary>
        /// <param name="evt"></param>
        void editInterfaceView_OnEditInterfaceSelectionChanged(EditInterfaceViewEventArgs evt)
        {
            propertiesTable.showEditableProperties(evt.EditInterface);
        }

        /// <summary>
        /// Callback for when the EditInterface is about to change.
        /// </summary>
        /// <param name="evt"></param>
        void editInterfaceView_OnEditInterfaceSelectionChanging(EditInterfaceViewEventArgs evt)
        {
            String error;
            if (!propertiesTable.validateCurrentSettings(out error))
            {
                evt.Cancel = true;
                MessageBox.Show(this, error, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Functions

        
    }
}

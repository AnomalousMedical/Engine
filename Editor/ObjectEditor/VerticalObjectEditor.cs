using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Engine.Editing;

namespace Editor
{
    public partial class VerticalObjectEditor : DockContent, IObjectEditorGUI
    {
        private object currentEditingObject;
        private EditInterface currentEditInterface;
        private ObjectEditorGUIEvent currentCommitCallback;

        public VerticalObjectEditor()
        {
            InitializeComponent();
            editInterfaceView.OnEditInterfaceSelectionChanging += new EditInterfaceSelectionChanging(editInterfaceView_OnEditInterfaceSelectionChanging);
            editInterfaceView.OnEditInterfaceSelectionChanged += new EditInterfaceSelectionChanged(editInterfaceView_OnEditInterfaceSelectionChanged);
        }

        /// <summary>
        /// Set the current EditInterface shown on this panel.
        /// </summary>
        /// <param name="editor">The editor to show.</param>
        public void setEditInterface(EditInterface editInterface, object editingObject, ObjectEditorGUIEvent CommitObjectChangesCallback)
        {
            if (currentCommitCallback != null)
            {
                currentCommitCallback.Invoke(currentEditInterface, currentEditingObject);
            }

            editInterfaceView.clearEditInterface();
            editInterfaceView.setEditInterface(editInterface);
            propertiesTable.showEditableProperties(editInterface);
            this.Text = editInterface.getName();

            currentEditInterface = editInterface;
            currentEditingObject = editingObject;
            currentCommitCallback = CommitObjectChangesCallback;
        }

        public void clearEditInterface()
        {
            editInterfaceView.clearEditInterface();
            propertiesTable.showEditableProperties(null);
        }

        /// <summary>
        /// Callback for when the EditInterface changes.
        /// </summary>
        /// <param name="evt"></param>
        void editInterfaceView_OnEditInterfaceSelectionChanged(EditInterfaceViewEvent evt)
        {
            propertiesTable.showEditableProperties(evt.EditInterface);
        }

        /// <summary>
        /// Callback for when the EditInterface is about to change.
        /// </summary>
        /// <param name="evt"></param>
        void editInterfaceView_OnEditInterfaceSelectionChanging(EditInterfaceViewEvent evt)
        {
            String error;
            if (!propertiesTable.validateCurrentSettings(out error))
            {
                evt.Cancel = true;
                MessageBox.Show(this, error, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

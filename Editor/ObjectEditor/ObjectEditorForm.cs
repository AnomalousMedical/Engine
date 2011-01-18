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
    public partial class ObjectEditorForm : Form, IObjectEditorGUI
    {
        /// <summary>
        /// Called when the main object in this ui has changed.
        /// </summary>
        public event ObjectEditorGUIEvent MainInterfaceChanged;

        /// <summary>
        /// Called when the active interface has changed.
        /// </summary>
        public event ObjectEditorGUIEvent ActiveInterfaceChanged;

        /// <summary>
        /// Called when a field has been changed.
        /// </summary>
        public event ObjectEditorGUIEvent FieldChanged;

        public ObjectEditorForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ObjectEditorForm_FormClosing);
        }

        void ObjectEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            String errorMessage;
            if (!objectEditorPanel.validateEditInterface(out errorMessage))
            {
                MessageBox.Show(this, errorMessage, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        public void setEditInterface(EditInterface editInterface, object editingObject, ObjectEditorGUIEvent FieldChangedCallback, ObjectEditorGUIEvent EditingCompletedCallback)
        {
            objectEditorPanel.setEditInterface(editInterface);
            this.ShowDialog();
            objectEditorPanel.clearEditInterface();
            if (EditingCompletedCallback != null)
            {
                EditingCompletedCallback.Invoke(editInterface, editingObject);
            }
        }

        public void clearEditInterface()
        {
            objectEditorPanel.clearEditInterface();
        }
    }
}

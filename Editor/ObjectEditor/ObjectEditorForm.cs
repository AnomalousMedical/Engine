using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Editing;

namespace Editor
{
    public partial class ObjectEditorForm : Form, IObjectEditorGUI
    {
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

        public void setEditInterface(EditInterface editInterface, object editingObject, ObjectEditorGUIEvent CommitObjectChangesCallback)
        {
            objectEditorPanel.setEditInterface(editInterface);
            this.ShowDialog();
            objectEditorPanel.clearEditInterface();
            if (CommitObjectChangesCallback != null)
            {
                CommitObjectChangesCallback.Invoke(editInterface, editingObject);
            }
        }
    }
}

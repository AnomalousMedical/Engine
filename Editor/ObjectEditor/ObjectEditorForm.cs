using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Editor
{
    public partial class ObjectEditorForm : Form
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

        public ObjectEditorPanel EditorPanel
        {
            get
            {
                return objectEditorPanel;
            }
        }
    }
}

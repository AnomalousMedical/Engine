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

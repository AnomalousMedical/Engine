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
    public partial class MultiEnumEditorCellPopup : Form
    {
        public MultiEnumEditorCellPopup()
        {
            InitializeComponent();
        }

        public void populateList(Type enumType)
        {
            multiEnumEditor.populateList(enumType);
        }

        public object Value
        {
            get
            {
                return multiEnumEditor.Value;
            }
            set
            {
                multiEnumEditor.Value = value;
            }
        }
    }
}

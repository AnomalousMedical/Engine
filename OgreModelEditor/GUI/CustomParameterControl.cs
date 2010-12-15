using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using OgreWrapper;
using Engine;

namespace OgreModelEditor
{
    public partial class CustomParameterControl : DockContent
    {
        public CustomParameterControl()
        {
            InitializeComponent();
        }
        
        public SubEntity SubEntity { get; set; }

        private void setValue_Click(object sender, EventArgs e)
        {
            if (SubEntity != null)
            {
                Quaternion value = new Quaternion();
                if (value.setValue(valueText.Text))
                {
                    SubEntity.setCustomParameter((int)indexUpDown.Value, value);
                }
                else
                {
                    MessageBox.Show(this, "Could not interpret the value please enter 4 numbers separated by commas.");
                }
            }
        }

        private void getValue_Click(object sender, EventArgs e)
        {
            if (SubEntity != null)
            {
                Quaternion value = SubEntity.getCustomParameter((int)indexUpDown.Value);
                valueText.Text = value.ToString();
            }
        }
    }
}

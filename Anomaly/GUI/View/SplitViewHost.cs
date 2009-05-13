using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Anomaly
{
    public partial class SplitViewHost : DockContent
    {
        private List<Control> savedControls = new List<Control>();

        public SplitViewHost()
        {
            InitializeComponent();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            savedControls.Clear();
            foreach (Control control in Controls)
            {
                savedControls.Add(control);
            }
            this.Controls.Clear();
            base.OnHandleDestroyed(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            foreach (Control control in savedControls)
            {
                Controls.Add(control);
            }
            savedControls.Clear();
            base.OnHandleCreated(e);
        }
    }
}

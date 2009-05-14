using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Engine;

namespace Editor
{
    public partial class DebugVisualizer : DockContent
    {
        private DebugInterface debugInterface;

        public DebugVisualizer()
        {
            InitializeComponent();
            enableAllCheckBox.CheckedChanged += new EventHandler(enableAllCheckBox_CheckedChanged);
            visualListBox.ItemCheck += new ItemCheckEventHandler(visualListBox_ItemCheck);
        }

        public void initialize(DebugInterface debugInterface)
        {
            this.Text = debugInterface.Name;
            this.debugInterface = debugInterface;
            foreach (DebugEntry entry in debugInterface.getEntries())
            {
                visualListBox.Items.Add(entry, false);
            }
        }

        void enableAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            debugInterface.setEnabled(enableAllCheckBox.Checked);
        }

        void visualListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            DebugEntry entry = (DebugEntry)visualListBox.Items[e.Index];
            entry.setEnabled(e.NewValue == CheckState.Checked);
        }
    }
}

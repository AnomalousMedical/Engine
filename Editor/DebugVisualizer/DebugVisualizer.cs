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
        private const String PERSIST_FORMAT = "{0}:{1}";
        private static readonly char[] sep = { ':' };

        /// <summary>
        /// Attempt to restore this visualizer from a given persist string. If
        /// the string describes a DebugVisualizer this function will return
        /// true and name will be filled in with the name of the window.
        /// </summary>
        /// <param name="persistString">The persist string.</param>
        /// <param name="name">Will get the name of the window if the string describes a DebugVisualizer.</param>
        /// <returns>True if the string is a debug visualizer.</returns>
        public static bool RestoreFromPersistance(String persistString, out String name)
        {
            String[] parsed = persistString.Split(sep);
            if (parsed.Length == 2 && parsed[0] == typeof(DebugVisualizer).ToString())
            {
                name = parsed[1];
                return true;
            }
            name = null;
            return false;
        }

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
            depthCheckCheck.Checked = debugInterface.isDepthTestingEnabled();
            foreach (DebugEntry entry in debugInterface.getEntries())
            {
                visualListBox.Items.Add(entry, false);
            }
        }

        protected override string GetPersistString()
        {
            return String.Format(PERSIST_FORMAT, this.GetType().ToString(), this.Text);
        }

        private void enableAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            debugInterface.setEnabled(enableAllCheckBox.Checked);
        }

        private void visualListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            DebugEntry entry = (DebugEntry)visualListBox.Items[e.Index];
            entry.setEnabled(e.NewValue == CheckState.Checked);
        }

        private void depthCheckCheck_CheckedChanged(object sender, EventArgs e)
        {
            debugInterface.setDepthTesting(depthCheckCheck.Checked);
        }
    }
}

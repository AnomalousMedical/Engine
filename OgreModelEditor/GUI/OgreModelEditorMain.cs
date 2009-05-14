using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Editor;

namespace OgreModelEditor
{
    partial class OgreModelEditorMain : EditorMainForm
    {
        private OgreModelEditorController controller;

        public OgreModelEditorMain()
        {
            InitializeComponent();
        }

        public void initialize(OgreModelEditorController controller)
        {
            this.controller = controller;
        }

        public void showDockContent(DockContent content)
        {
            content.Show(dockPanel);
        }

        public void hideDockContent(DockContent content)
        {
            content.DockHandler.Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            controller.shutdown();
            base.OnClosing(e);
        }
    }
}

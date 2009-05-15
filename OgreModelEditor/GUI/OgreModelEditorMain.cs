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
        private FileTracker fileTracker = new FileTracker("*.mesh|*.mesh");

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileTracker.openFile(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.openModel(fileTracker.getCurrentFile());
                updateWindowTitle(fileTracker.getCurrentFile());
            }
        }

        private void saveModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileTracker.saveFile(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.saveModel(fileTracker.getCurrentFile());
                updateWindowTitle(fileTracker.getCurrentFile());
            }
        }

        private void saveModelAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileTracker.saveFileAs(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.saveModel(fileTracker.getCurrentFile());
                updateWindowTitle(fileTracker.getCurrentFile());
            }
        }

        private void defineExternalResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.editExternalResources();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.refreshResources();
        }

        private void binormalViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.setBinormalDebug();
        }

        private void tangentViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.setTangentDebug();
        }

        private void normalViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.setNormalDebug();
        }

        private void modelViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.setNormalMaterial();
        }

        private void recalculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.buildTangentVectors();
            controller.buildBinormalVectors();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F1)
            {
                controller.setNormalMaterial();
            }
            if (e.KeyCode == Keys.F2)
            {
                controller.setBinormalDebug();
            }
            if (e.KeyCode == Keys.F3)
            {
                controller.setTangentDebug();
            }
            if (e.KeyCode == Keys.F4)
            {
                controller.setNormalDebug();
            }
            if (e.KeyCode == Keys.F5)
            {
                controller.refreshResources();
            }
        }
    }
}

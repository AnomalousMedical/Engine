using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Platform;
using Engine;
using Editor;
using WeifenLuo.WinFormsUI.Docking;

namespace Anomaly
{
    partial class AnomalyMain : EditorMainForm 
    {
        private AnomalyController controller;
        FileTracker fileTracker;

        public AnomalyMain()
        {
            InitializeComponent();
            setDockPanel(dockPanel);
        }

        public void initialize(AnomalyController controller)
        {
            fileTracker = new FileTracker("*.sim.xml|*.sim.xml");
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (controller != null)
            {
                controller.shutdown();
            }
            base.OnFormClosing(e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ResourceController.editResources();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = fileTracker.saveFile(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.saveScene(filename);
                updateWindowTitle(filename);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = fileTracker.saveFileAs(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.saveScene(filename);
                updateWindowTitle(filename);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = fileTracker.openFile(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.loadScene(filename);
                updateWindowTitle(filename);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createNewScene();
            clearWindowTitle();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.SceneController.editScene();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            playButton.Enabled = false;
            pauseButton.Enabled = true;
            controller.setDynamicMode();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            playButton.Enabled = true;
            pauseButton.Enabled = false;
            controller.setStaticMode();
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            controller.enableMoveTool();
            moveButton.Checked = true;
            rotateButton.Checked = false;
        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            controller.enableRotateTool();
            moveButton.Checked = false;
            rotateButton.Checked = true;
        }

        private void showStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showStatsToolStripMenuItem.Checked = !showStatsToolStripMenuItem.Checked;
            controller.ViewController.showStats(showStatsToolStripMenuItem.Checked);
        }

        private void oneWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ViewController.createOneWaySplit();
        }

        private void twoWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ViewController.createTwoWaySplit();
        }

        private void threeWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ViewController.createThreeWayUpperSplit();
        }

        private void fourWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ViewController.createFourWaySplit();
        }

        private void maximizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}

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
using System.IO;

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
            List<CommandManager> commands = PluginManager.Instance.createDebugCommands();
            if(commands.Count > 0)
            {
                ToolStripMenuItem debugMenu = new ToolStripMenuItem("Debug");
                mainMenu.Items.Add(debugMenu);
                foreach (CommandManager commandManager in commands)
                {
                    ToolStripMenuItem pluginMenu = new ToolStripMenuItem(commandManager.Name);
                    debugMenu.DropDownItems.Add(pluginMenu);
                    foreach (EngineCommand command in commandManager.getCommandList())
                    {
                        ToolStripMenuItem commandItem = new ToolStripMenuItem(command.PrettyName);
                        commandItem.Tag = command;
                        commandItem.Click += commandItem_Click;
                        pluginMenu.DropDownItems.Add(commandItem);
                    }
                }
            }
        }

        void commandItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem commandItem = sender as ToolStripMenuItem;
            if (commandItem != null)
            {
                EngineCommand command = commandItem.Tag as EngineCommand;
                if (command != null)
                {
                    command.execute();
                }
            }
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

        private void selectButton_Click(object sender, EventArgs e)
        {
            controller.enableSelectTool();
            moveButton.Checked = false;
            rotateButton.Checked = false;
            selectButton.Checked = true;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            controller.enableMoveTool();
            moveButton.Checked = true;
            rotateButton.Checked = false;
            selectButton.Checked = false;
        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            controller.enableRotateTool();
            moveButton.Checked = false;
            rotateButton.Checked = true;
            selectButton.Checked = false;
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

        private void importInstancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    controller.importInstances(openFileDialog.FileName);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(String.Format("Error reading {0}:\n{1}.", openFileDialog.FileName, ex.Message), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

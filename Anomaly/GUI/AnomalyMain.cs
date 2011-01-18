using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private PublishGUI publishGUI = new PublishGUI();
        private ObfuscateZipFileGUI obfuscateGUI = new ObfuscateZipFileGUI();

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
            publishGUI.initialize(controller);
            this.controller = controller;
            this.Text = controller.Solution.Name + " - Anomaly";
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.saveSolution();
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

        private void publishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            publishGUI.ShowDialog(this);
        }

        private void obfuscateArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            obfuscateGUI.ShowDialog(this);
        }

        private void viewResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ResourceController.viewResources();
        }
    }
}

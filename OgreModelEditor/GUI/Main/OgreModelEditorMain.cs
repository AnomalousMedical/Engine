using Anomalous.GuiFramework;
using Editor;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    class OgreModelEditorMain : Component
    {
        private OgreModelEditorController controller;
        private FileTracker fileTracker = new FileTracker("*.mesh|*.mesh");

        public OgreModelEditorMain(OgreModelEditorController controller)
            :base("OgreModelEditor.GUI.Main.OgreModelEditorMain.layout")
        {
            this.controller = controller;
            LayoutContainer = new MyGUISingleChildLayoutContainer(widget);

            MenuBar menuBar = widget.findWidget("MenuBar") as MenuBar;
            MenuItem fileItem = menuBar.addItem("File", MenuItemType.Popup);
            MenuControl file = menuBar.createItemPopupMenuChild(fileItem);
            MenuItem exit = file.addItem("Exit", MenuItemType.Normal);
            exit.MouseButtonClick += exit_MouseButtonClick;
        }

        void exit_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.exit();
        }

        public SingleChildLayoutContainer LayoutContainer { get; private set; }

        public void currentFileChanged(String filename)
        {
            //fileTracker.setCurrentFile(filename);
            //updateWindowTitle(fileTracker.getCurrentFile());
        }

        //private void openToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    fileTracker.openFile(this);
        //    if (fileTracker.lastDialogAccepted())
        //    {
        //        controller.openModel(fileTracker.getCurrentFile());
        //    }
        //}

        //private void saveModelToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    fileTracker.saveFile(this);
        //    if (fileTracker.lastDialogAccepted())
        //    {
        //        controller.saveModel(fileTracker.getCurrentFile());
        //        updateWindowTitle(fileTracker.getCurrentFile());
        //    }
        //}

        //private void saveModelAsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    fileTracker.saveFileAs(this);
        //    if (fileTracker.lastDialogAccepted())
        //    {
        //        controller.saveModel(fileTracker.getCurrentFile());
        //        updateWindowTitle(fileTracker.getCurrentFile());
        //    }
        //}

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

        public void setTextureNames(IEnumerable<String> textureNames)
        {
            //viewTextureToolStripMenuItem.DropDownItems.Clear();
            //foreach (String texName in textureNames)
            //{
            //    ToolStripMenuItem item = new ToolStripMenuItem(texName, null, textureNameClicked);
            //    viewTextureToolStripMenuItem.DropDownItems.Add(item);
            //}
        }

        private void textureNameClicked(Object sender, EventArgs e)
        {
            //ToolStripMenuItem toolItem = sender as ToolStripMenuItem;
            //if (toolItem != null)
            //{
            //    controller.setTextureDebug(toolItem.Text);
            //}
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    if (e.KeyCode == Keys.F1)
        //    {
        //        controller.setNormalMaterial();
        //    }
        //    if (e.KeyCode == Keys.F2)
        //    {
        //        controller.setBinormalDebug();
        //    }
        //    if (e.KeyCode == Keys.F3)
        //    {
        //        controller.setTangentDebug();
        //    }
        //    if (e.KeyCode == Keys.F4)
        //    {
        //        controller.setNormalDebug();
        //    }
        //    if (e.KeyCode == Keys.F5)
        //    {
        //        controller.refreshResources();
        //    }
        //}

        private void oneWinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createOneWindow();
        }

        private void twoWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createTwoWindows();
        }

        private void threeWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createThreeWindows();
        }

        private void fourWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createFourWindows();
        }

        private void showStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //showStatsToolStripMenuItem.Checked = !showStatsToolStripMenuItem.Checked;
            //controller.showStats(showStatsToolStripMenuItem.Checked);
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            //controller.enableSelectTool();
            //moveButton.Checked = false;
            //rotateButton.Checked = false;
            //selectButton.Checked = true;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            //controller.enableMoveTool();
            //moveButton.Checked = true;
            //rotateButton.Checked = false;
            //selectButton.Checked = false;
        }

        private void rotateButton_Click(object sender, EventArgs e)
        {
            //controller.enableRotateTool();
            //moveButton.Checked = false;
            //rotateButton.Checked = true;
            //selectButton.Checked = false;
        }

        //protected override void OnDragEnter(DragEventArgs drgevent)
        //{
        //    base.OnDragEnter(drgevent);
        //    if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        String[] files = drgevent.Data.GetData(DataFormats.FileDrop) as String[];
        //        if (files.Length > 0)
        //        {
        //            if (files[0].EndsWith(".mesh"))
        //            {
        //                drgevent.Effect = DragDropEffects.All;
        //            }
        //        }
        //    }
        //}

        //protected override void OnDragDrop(DragEventArgs drgevent)
        //{
        //    base.OnDragDrop(drgevent);
        //    if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        String[] files = drgevent.Data.GetData(DataFormats.FileDrop) as String[];
        //        if (files.Length > 0)
        //        {
        //            if (files[0].EndsWith(".mesh"))
        //            {
        //                controller.openModel(files[0]);
        //            }
        //        }
        //    }
        //}

        private void showSkeletonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //showSkeletonToolStripMenuItem.Checked = !showSkeletonToolStripMenuItem.Checked;
            //controller.setShowSkeleton(showSkeletonToolStripMenuItem.Checked);
        }

        private void batchUpgradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            //{
            //    if (folderBrowser.ShowDialog(this) == DialogResult.OK)
            //    {
            //        String path = folderBrowser.SelectedPath;
            //        controller.batchResaveMeshes(path);
            //    }
            //}
        }

        private void exportToJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            //{
            //    if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            //    {
            //        controller.saveModelJSON(saveFileDialog.FileName);
            //    }
            //}
        }
    }
}

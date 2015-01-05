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

        private MenuItem showSkeleton;
        private MenuControl textureMenu;

        public OgreModelEditorMain(OgreModelEditorController controller)
            :base("OgreModelEditor.GUI.Main.OgreModelEditorMain.layout")
        {
            this.controller = controller;
            LayoutContainer = new MyGUISingleChildLayoutContainer(widget);

            MenuBar menuBar = widget.findWidget("MenuBar") as MenuBar;
            MenuItem fileItem = menuBar.addItem("File", MenuItemType.Popup);
            MenuControl file = menuBar.createItemPopupMenuChild(fileItem);
            MenuItem open = file.addItem("Open", MenuItemType.Normal);
            open.MouseButtonClick += open_MouseButtonClick;
            MenuItem save = file.addItem("Save", MenuItemType.Normal);
            save.MouseButtonClick += save_MouseButtonClick;
            MenuItem saveAs = file.addItem("Save As");
            saveAs.MouseButtonClick += saveAs_MouseButtonClick;
            MenuItem batchUpgrade = file.addItem("Batch Upgrade");
            batchUpgrade.MouseButtonClick += batchUpgrade_MouseButtonClick;
            MenuItem exportToJson = file.addItem("Export to JSON");
            exportToJson.MouseButtonClick += exportToJson_MouseButtonClick;
            MenuItem exit = file.addItem("Exit", MenuItemType.Normal);
            exit.MouseButtonClick += exit_MouseButtonClick;

            MenuItem resourcesItem = menuBar.addItem("Resources", MenuItemType.Popup);
            MenuControl resources = menuBar.createItemPopupMenuChild(resourcesItem);
            MenuItem reloadAll = resources.addItem("Reload All");
            reloadAll.MouseButtonClick += reloadAll_MouseButtonClick;
            MenuItem defineExternal = resources.addItem("Define External Resources");
            defineExternal.MouseButtonClick += defineExternal_MouseButtonClick;

            MenuItem debugItem = menuBar.addItem("Debug", MenuItemType.Popup);
            MenuControl debug = menuBar.createItemPopupMenuChild(debugItem);
            MenuItem viewShaded = debug.addItem("View Shaded");
            viewShaded.MouseButtonClick += viewShaded_MouseButtonClick;
            MenuItem viewBinormals = debug.addItem("View Binormals");
            viewBinormals.MouseButtonClick += viewBinormals_MouseButtonClick;
            MenuItem viewTangents = debug.addItem("View Tangents");
            viewTangents.MouseButtonClick += viewTangents_MouseButtonClick;
            MenuItem viewNormals = debug.addItem("View Normals");
            viewNormals.MouseButtonClick += viewNormals_MouseButtonClick;
            MenuItem viewTexture = debug.addItem("View Texture", MenuItemType.Popup);
            textureMenu = debug.createItemPopupMenuChild(viewTexture);
            //viewTexture.MouseButtonClick += viewTexture_MouseButtonClick;
            showSkeleton = debug.addItem("Show Skeleton");
            showSkeleton.MouseButtonClick += showSkeleton_MouseButtonClick;

            //Buttons
            ButtonGroup toolButtons = new ButtonGroup();
            Button none = widget.findWidget("None") as Button;
            none.MouseButtonClick += none_MouseButtonClick;
            toolButtons.addButton(none);

            Button move = widget.findWidget("Move") as Button;
            move.MouseButtonClick += move_MouseButtonClick;
            toolButtons.addButton(move);

            Button rotate = widget.findWidget("Rotate") as Button;
            rotate.MouseButtonClick += rotate_MouseButtonClick;
            toolButtons.addButton(rotate);
            
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

        void open_MouseButtonClick(Widget source, EventArgs e)
        {
            //    fileTracker.openFile(this);
            //    if (fileTracker.lastDialogAccepted())
            //    {
            //        controller.openModel(fileTracker.getCurrentFile());
            //    }
        }

        void save_MouseButtonClick(Widget source, EventArgs e)
        {
            //    fileTracker.saveFile(this);
            //    if (fileTracker.lastDialogAccepted())
            //    {
            //        controller.saveModel(fileTracker.getCurrentFile());
            //        updateWindowTitle(fileTracker.getCurrentFile());
            //    }
        }

        void saveAs_MouseButtonClick(Widget source, EventArgs e)
        {
            //    fileTracker.saveFileAs(this);
            //    if (fileTracker.lastDialogAccepted())
            //    {
            //        controller.saveModel(fileTracker.getCurrentFile());
            //        updateWindowTitle(fileTracker.getCurrentFile());
            //    }
        }

        private void defineExternal_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.editExternalResources();
        }

        private void reloadAll_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.refreshResources();
        }

        private void viewBinormals_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.setBinormalDebug();
        }

        private void viewTangents_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.setTangentDebug();
        }

        private void viewNormals_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.setNormalDebug();
        }

        private void viewShaded_MouseButtonClick(Widget source, EventArgs e)
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
            textureMenu.removeAllItems();
            foreach (String texName in textureNames)
            {
                MenuItem item = textureMenu.addItem(texName, MenuItemType.Normal);
                item.MouseButtonClick += textureItem_MouseButtonClick;
            }
        }

        void textureItem_MouseButtonClick(Widget source, EventArgs e)
        {
            MenuItem toolItem = source as MenuItem;
            if (toolItem != null)
            {
                controller.setTextureDebug(toolItem.Caption);
            }
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

        void none_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.enableSelectTool();
        }

        void rotate_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.enableRotateTool();
        }

        void move_MouseButtonClick(Widget source, EventArgs e)
        {
            controller.enableMoveTool();
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

        private void showSkeleton_MouseButtonClick(Widget source, EventArgs e)
        {
            showSkeleton.Selected = !showSkeleton.Selected;
            controller.setShowSkeleton(showSkeleton.Selected);
        }

        private void batchUpgrade_MouseButtonClick(Widget source, EventArgs e)
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

        private void exportToJson_MouseButtonClick(Widget source, EventArgs e)
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

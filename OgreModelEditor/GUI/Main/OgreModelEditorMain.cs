using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Editor;
using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    class OgreModelEditorMain : Component
    {
        private static ButtonEvent showShadedShortcut;
        private static ButtonEvent showBiNormalShortcut;
        private static ButtonEvent showTangentShortcut;
        private static ButtonEvent showNormalShortcut;
        private static ButtonEvent refreshShortcut;
        private static ButtonEvent showSolidShortcut;
        private static ButtonEvent showWireframeShortcut;
        private static ButtonEvent showPointShortcut;

        static OgreModelEditorMain()
        {
            showShadedShortcut = new ButtonEvent(EventLayers.AfterMain);
            showShadedShortcut.addButton(KeyboardButtonCode.KC_F1);
            DefaultEvents.registerDefaultEvent(showShadedShortcut);

            showBiNormalShortcut = new ButtonEvent(EventLayers.AfterMain);
            showBiNormalShortcut.addButton(KeyboardButtonCode.KC_F2);
            DefaultEvents.registerDefaultEvent(showBiNormalShortcut);

            showTangentShortcut = new ButtonEvent(EventLayers.AfterMain);
            showTangentShortcut.addButton(KeyboardButtonCode.KC_F3);
            DefaultEvents.registerDefaultEvent(showTangentShortcut);

            showNormalShortcut = new ButtonEvent(EventLayers.AfterMain);
            showNormalShortcut.addButton(KeyboardButtonCode.KC_F4);
            DefaultEvents.registerDefaultEvent(showNormalShortcut);

            refreshShortcut = new ButtonEvent(EventLayers.AfterMain);
            refreshShortcut.addButton(KeyboardButtonCode.KC_F5);
            DefaultEvents.registerDefaultEvent(refreshShortcut);

            showSolidShortcut = new ButtonEvent(EventLayers.AfterMain);
            showSolidShortcut.addButton(KeyboardButtonCode.KC_F6);
            DefaultEvents.registerDefaultEvent(showSolidShortcut);

            showWireframeShortcut = new ButtonEvent(EventLayers.AfterMain);
            showWireframeShortcut.addButton(KeyboardButtonCode.KC_F7);
            DefaultEvents.registerDefaultEvent(showWireframeShortcut);

            showPointShortcut = new ButtonEvent(EventLayers.AfterMain);
            showPointShortcut.addButton(KeyboardButtonCode.KC_F8);
            DefaultEvents.registerDefaultEvent(showPointShortcut);
        }

        private OgreModelEditorController controller;
        private FileTracker fileTracker = new FileTracker()
            {
                Filter = "*.mesh|*.mesh"
            };

        private MenuItem showSkeleton;
        private MenuControl textureMenu;
        private MenuItem showStats;
        private MenuItem saveMicrocode;

        public OgreModelEditorMain(OgreModelEditorController controller)
            :base("OgreModelEditor.GUI.Main.OgreModelEditorMain.layout")
        {
            this.controller = controller;
            LayoutContainer = new MyGUISingleChildLayoutContainer(widget);

            MenuBar menuBar = widget.findWidget("MenuBar") as MenuBar;
            menuBar.AutoAcceptRunAction = true;
            MenuItem fileItem = menuBar.addItem("File", MenuItemType.Popup);
            MenuControl file = menuBar.createItemPopupMenuChild(fileItem);
            file.addItemAction("Open", open);
            file.addItemAction("Save", save);
            file.addItemAction("Save As", saveAs);
            file.addItemAction("Batch Upgrade", batchUpgrade);
            file.addItemAction("Export to JSON", exportToJson);
            MenuItem exit = file.addItemAction("Exit", controller.exit);

            MenuItem resourcesItem = menuBar.addItem("Resources", MenuItemType.Popup);
            MenuControl resources = menuBar.createItemPopupMenuChild(resourcesItem);
            resources.addItemAction("Reload All (F5)", reloadAll);
            resources.addItemAction("Define External Resources", defineExternal);
            resources.addItemAction("Delete Shader Microcode Cache", controller.clearMicrocodeCache);
            saveMicrocode = resources.addItemAction("Save Shader Microcode Cache", changeSaveMicrocode);
            saveMicrocode.Selected = OgreConfig.SaveMicrocodeCache;

            MenuItem debugItem = menuBar.addItem("Debug", MenuItemType.Popup);
            MenuControl debug = menuBar.createItemPopupMenuChild(debugItem);
            debug.addItemAction("View Shaded (F1)", viewShaded);
            debug.addItemAction("View Binormals (F2)", viewBinormals);
            debug.addItemAction("View Tangents (F3)", viewTangents);
            debug.addItemAction("View Normals (F4)", viewNormals);
            debug.addItemAction("View Parity", controller.setParityDebug);
            MenuItem viewTexture = debug.addItem("View Texture", MenuItemType.Popup);
            textureMenu = debug.createItemPopupMenuChild(viewTexture);
            showSkeleton = debug.addItemAction("Show Skeleton", changeShowSkeleton);
            debug.addItemAction("Show Virtual Texture Debugger", controller.showVirtualTextureDebugger);

            MenuItem modelItem = menuBar.addItem("Model", MenuItemType.Popup);
            MenuControl model = menuBar.createItemPopupMenuChild(modelItem);
            model.addItemAction("Recalculate Tangents", recalculateTangents);
            model.addItemAction("Invert Tangent W", controller.inverseTangentW);

            MenuItem windowItem = menuBar.addItem("Window", MenuItemType.Popup);
            MenuControl window = menuBar.createItemPopupMenuChild(windowItem);
            showStats = window.addItemAction("Show Stats", changeShowStats);
            showStats.Selected = controller.ShowStats;
            MenuItem layoutItem = window.addItem("Layout", MenuItemType.Popup);
            MenuControl layout = window.createItemPopupMenuChild(layoutItem);
            layout.addItemAction("One Window", oneWindow);
            layout.addItemAction("Two Window", twoWindow);
            layout.addItemAction("Three Window", threeWindow);
            layout.addItemAction("Four Window", fourWindow);
            MenuItem renderingModeItem = window.addItem("Rendering Mode", MenuItemType.Popup);
            MenuControl renderingMode = window.createItemPopupMenuChild(renderingModeItem);
            renderingMode.addItemAction("Solid (F6)", setSolid);
            renderingMode.addItemAction("Wireframe (F7)", setWireframe);
            renderingMode.addItemAction("Point (F8)", setPoints);
            window.addItemAction("Change Background Color", changeBackgroundColor);

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

            showBiNormalShortcut.FirstFrameUpEvent += layer => viewBinormals();
            showTangentShortcut.FirstFrameUpEvent += layer => viewTangents();
            showNormalShortcut.FirstFrameUpEvent += layer => viewNormals();
            showShadedShortcut.FirstFrameUpEvent += layer => viewShaded();
            refreshShortcut.FirstFrameUpEvent += layer => reloadAll();
            showSolidShortcut.FirstFrameUpEvent += layer => setSolid();
            showWireframeShortcut.FirstFrameUpEvent += layer => setWireframe();
            showPointShortcut.FirstFrameUpEvent += layer => setPoints();
        }

        public SingleChildLayoutContainer LayoutContainer { get; private set; }

        public void currentFileChanged(String filename)
        {
            fileTracker.CurrentFile = filename;
            controller.updateWindowTitle(fileTracker.CurrentFile);
        }

        void open()
        {
            fileTracker.openFile(file => controller.openModel(file));
        }

        void save()
        {
            fileTracker.saveFile(file => controller.saveModel(file));
        }

        void saveAs()
        {
            fileTracker.saveFileAs(file =>
                {
                    controller.saveModel(file);
                    controller.updateWindowTitle(file);
                });
        }

        private void defineExternal()
        {
            controller.editExternalResources();
        }

        private void reloadAll()
        {
            controller.refreshResources();
        }

        private void viewBinormals()
        {
            controller.setBinormalDebug();
        }

        private void viewTangents()
        {
            controller.setTangentDebug();
        }

        private void viewNormals()
        {
            controller.setNormalDebug();
        }

        private void viewShaded()
        {
            controller.setNormalMaterial();
        }

        private void recalculateTangents()
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

        private void oneWindow()
        {
            controller.createOneWindow();
        }

        private void twoWindow()
        {
            controller.createTwoWindows();
        }

        private void threeWindow()
        {
            controller.createThreeWindows();
        }

        void fourWindow()
        {
            controller.createFourWindows();
        }

        private void changeShowStats()
        {
            controller.ShowStats = showStats.Selected = !showStats.Selected;
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

        private void changeSaveMicrocode()
        {
            saveMicrocode.Selected = !saveMicrocode.Selected;
            OgreConfig.SaveMicrocodeCache = saveMicrocode.Selected;
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

        private void changeShowSkeleton()
        {
            showSkeleton.Selected = !showSkeleton.Selected;
            controller.setShowSkeleton(showSkeleton.Selected);
        }

        private void batchUpgrade()
        {

            DirDialog folderBrowser = new DirDialog(controller.MainWindow);
            folderBrowser.showModal((result, path) =>
                {
                    if(result == NativeDialogResult.OK)
                    {
                        controller.batchResaveMeshes(path);
                    }
                });
        }

        private void exportToJson()
        {
            FileSaveDialog saveDialog = new FileSaveDialog(controller.MainWindow);
            saveDialog.showModal((result, path) =>
                {
                    if(result == NativeDialogResult.OK)
                    {
                        controller.saveModelJSON(path);
                    }
                });
        }

        private void changeBackgroundColor()
        {
            ColorDialog colorDialog = new ColorDialog(controller.MainWindow);
            colorDialog.showModal((result, color) =>
                {
                    if(result == NativeDialogResult.OK)
                    {
                        controller.FrameClear.ClearColor = color;
                    }
                });
        }

        private void setSolid()
        {
            controller.SceneViewController.ActiveWindow.RenderingMode = RenderingMode.Solid;
        }

        private void setWireframe()
        {
            controller.SceneViewController.ActiveWindow.RenderingMode = RenderingMode.Wireframe;
        }

        private void setPoints()
        {
            controller.SceneViewController.ActiveWindow.RenderingMode = RenderingMode.Points;
        }
    }
}

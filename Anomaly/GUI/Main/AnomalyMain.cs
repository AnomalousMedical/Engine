using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Editor;
using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly.GUI
{
    class AnomalyMain : Component
    {
        static AnomalyMain()
        {
            
        }

        private AnomalyController controller;

        private MenuItem showStats;

        public AnomalyMain(AnomalyController controller)
            : base("Anomaly.GUI.Main.AnomalyMain.layout")
        {
            this.controller = controller;
            LayoutContainer = new MyGUISingleChildLayoutContainer(widget);

            MenuBar menuBar = widget.findWidget("MenuBar") as MenuBar;
            menuBar.AutoAcceptRunAction = true;
            MenuItem fileItem = menuBar.addItem("File", MenuItemType.Popup);
            MenuControl file = menuBar.createItemPopupMenuChild(fileItem);
            file.addItemAction("Save", save);
            file.addItemAction("Build", controller.build);
            file.addItemAction("Force Save All", forceSave);
            file.addItemAction("Exit", controller.shutdown);

            MenuItem editItem = menuBar.addItem("Edit", MenuItemType.Popup);
            MenuControl edit = menuBar.createItemPopupMenuChild(editItem);
            edit.addItemAction("Cut", controller.cut);
            edit.addItemAction("Copy", controller.copy);
            edit.addItemAction("Paste", controller.paste);

            MenuItem resourcesItem = menuBar.addItem("Resources", MenuItemType.Popup);
            MenuControl resources = menuBar.createItemPopupMenuChild(resourcesItem);
            resources.addItemAction("Publish", publish);
            resources.addItemAction("Obfuscate Archive", obfuscateArchive);
            resources.addItemAction("Refresh Global Resources", controller.refreshGlobalResources);

            MenuItem sceneItem = menuBar.addItem("Scene", MenuItemType.Popup);
            MenuControl scene = menuBar.createItemPopupMenuChild(sceneItem);
            scene.addItemAction("View Configuration", viewConfiguration);
            scene.addItemAction("View Resources", viewResources);

            MenuItem windowItem = menuBar.addItem("Window", MenuItemType.Popup);
            MenuControl window = menuBar.createItemPopupMenuChild(windowItem);
            showStats = window.addItemAction("Show Stats", changeShowStats);
            showStats.Selected = controller.ShowStats;
            MenuItem layoutItem = window.addItem("Layout", MenuItemType.Popup);
            MenuControl layout = window.createItemPopupMenuChild(layoutItem);
            layout.addItemAction("One Window", controller.createOneWindow);
            layout.addItemAction("Two Window", controller.createTwoWindows);
            layout.addItemAction("Three Window", controller.createThreeWindows);
            layout.addItemAction("Four Window", controller.createFourWindows);

            List<CommandManager> commands = PluginManager.Instance.createDebugCommands();
            if (commands.Count > 0)
            {
                MenuItem debugItem = menuBar.addItem("Debug", MenuItemType.Popup);
                MenuControl debug = menuBar.createItemPopupMenuChild(debugItem);

                foreach (CommandManager commandManager in commands)
                {
                    MenuItem subsystemCommandItem = debug.addItem(commandManager.Name, MenuItemType.Popup);
                    MenuControl subystemCommand = debug.createItemPopupMenuChild(subsystemCommandItem);

                    foreach (EngineCommand command in commandManager.getCommandList())
                    {
                        subystemCommand.addItemAction(command.PrettyName, () => command.execute());
                    }
                }
            }
        }

        public SingleChildLayoutContainer LayoutContainer { get; private set; }

        private void save()
        {
            controller.saveSolution(false);
        }

        private void forceSave()
        {
            controller.saveSolution(true);
        }

        private void obfuscateArchive()
        {
            //obfuscateGUI.ShowDialog(this);
        }

        private void publish()
        {
            //publishGUI.ShowDialog(this);
        }

        private void changeShowStats()
        {
            controller.ShowStats = showStats.Selected = !showStats.Selected;
        }

        private void viewResources()
        {
            controller.ResourceController.viewResources();
        }

        private void viewConfiguration()
        {
            controller.SceneController.editScene();
        }
    }
}

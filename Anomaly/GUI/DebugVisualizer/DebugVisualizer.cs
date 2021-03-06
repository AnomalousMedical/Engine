﻿using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Editor;
using Engine;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Platform;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomaly.GUI
{
    class DebugVisualizer : MDIDialog
    {
        private PluginManager pluginManager;
        private SceneController sceneController;
        private bool firstShow = true;
        private SimScene currentScene;

        private GuiFrameworkUICallback uiCallback;
        private Tree tree;
        private EditInterfaceTreeView editTreeView;

        private PropertiesForm propertiesForm;

        private ObjectEditor objectEditor;

        private Splitter splitter;

        public DebugVisualizer(PluginManager pluginManager, SceneController sceneController)
            : base("Anomaly.GUI.DebugVisualizer.DebugVisualizer.layout")
        {
            this.pluginManager = pluginManager;
            this.sceneController = sceneController;

            uiCallback = new GuiFrameworkUICallback();

            tree = new Tree((ScrollView)window.findWidget("TreeScroller"));
            editTreeView = new EditInterfaceTreeView(tree, uiCallback);

            propertiesForm = new ScrollablePropertiesForm((ScrollView)window.findWidget("TableScroller"), uiCallback);

            objectEditor = new ObjectEditor(editTreeView, propertiesForm, uiCallback);

            this.Resized += DebugVisualizer_Resized;

            currentScene = sceneController.CurrentScene;

            splitter = new Splitter(window.findWidget("Splitter"));
            splitter.Widget1Resized += a => tree.layout();
            splitter.Widget2Resized += a => propertiesForm.layout();
        }

        public override void Dispose()
        {
            objectEditor.Dispose();
            propertiesForm.Dispose();
            editTreeView.Dispose();
            tree.Dispose();
            base.Dispose();
        }

        protected override void onShown(EventArgs args)
        {
            base.onShown(args);
            if(firstShow)
            {
                firstShow = false;
                EditInterface editInterface = new EditInterface("Debug Visualizers");
                foreach(var debugInterface in pluginManager.DebugInterfaces)
                {
                    EditInterface debugEditInterface = new EditInterface(debugInterface.Name);
                    EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
                    propertyInfo.addColumn(new EditablePropertyColumn("Name", true));
                    propertyInfo.addColumn(new EditablePropertyColumn("Value", false));
                    debugEditInterface.setPropertyInfo(propertyInfo);

                    debugEditInterface.addEditableProperty(new CallbackEditableProperty<bool>("Enabled",
                        () => debugInterface.Enabled, v => debugInterface.Enabled = v, canParseBool, bool.Parse));

                    debugEditInterface.addEditableProperty(new CallbackEditableProperty<bool>("Depth Testing",
                        () => debugInterface.DepthTesting, v => debugInterface.DepthTesting = v, canParseBool, bool.Parse));

                    foreach(var entry in debugInterface.Entries)
                    {
                        debugEditInterface.addEditableProperty(new CallbackEditableProperty<bool>(entry.Text,
                            () => entry.Enabled, value => entry.Enabled = value, canParseBool, bool.Parse));
                    }
                    
                    editInterface.addSubInterface(debugEditInterface);
                }

                objectEditor.EditInterface = editInterface;

                createDebugVisualizers();
            }
        }

        protected override void customDeserialize(ConfigSection section, ConfigFile file)
        {
            base.customDeserialize(section, file);
            splitter.SplitterPosition = section.getValue("SplitterPosition", splitter.SplitterPosition);
        }

        protected override void customSerialize(ConfigSection section, ConfigFile file)
        {
            base.customSerialize(section, file);
            section.setValue("SplitterPosition", splitter.SplitterPosition);
        }

        void sceneLoaded(SimScene scene)
        {
            currentScene = scene;
            createDebugVisualizers();
        }

        private void createDebugVisualizers()
        {
            if (!firstShow && currentScene != null)
            {
                foreach (DebugInterface debugInterface in pluginManager.DebugInterfaces)
                {
                    debugInterface.createDebugInterface(pluginManager.RendererPlugin, currentScene.getDefaultSubScene());
                }
            }
        }

        void sceneUnloading(SimScene scene)
        {
            currentScene = null;
            if (!firstShow)
            {
                foreach (DebugInterface debugInterface in pluginManager.DebugInterfaces)
                {
                    debugInterface.destroyDebugInterface(pluginManager.RendererPlugin, scene.getDefaultSubScene());
                }
            }
        }

        void DebugVisualizer_Resized(object sender, EventArgs e)
        {
            splitter.layout();
        }

        bool canParseBool(String str)
        {
            bool bVal;
            return bool.TryParse(str, out bVal);
        }
    }
}

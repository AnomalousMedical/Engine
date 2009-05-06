using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using Editor;
using Engine.Platform;
using Engine;
using System.Drawing;

namespace Anomaly
{
    class SimObjectController
    {
        private AnomalyController controller;
        private Dictionary<String, SelectableSimObject> selectables = new Dictionary<string, SelectableSimObject>();
        private SimObjectManager simObjectManager;
        private SimObjectManagerDefinition simObjectManagerDefiniton;
        private SimObjectPanel panel;
        private SimSubScene subScene;

        public SimObjectController()
        {

        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
            controller.SceneController.OnSceneLoading += SceneController_OnSceneLoading;
            controller.SceneController.OnSceneUnloading += SceneController_OnSceneUnloading;
            controller.SelectionController.OnSelectionChanged += new ObjectSelected(SelectionController_OnSelectionChanged);
        }

        public void setUI(SimObjectPanel panel)
        {
            this.panel = panel;
            panel.EditInterface.setEditInterface(getEditInterface());
            panel.EditInterface.OnEditInterfaceChosen += new EditInterfaceChosen(editInterfaceChosen);
            panel.EditInterface.OnEditInterfaceSelectionEdit += new EditInterfaceSelectionEdit(editInterfaceEdit);
        }

        public SimObjectManagerDefinition getSimObjectManagerDefinition()
        {
            return simObjectManagerDefiniton;
        }

        public void createSimObject(SimObjectDefinition definition)
        {
            SimObjectBase instance = definition.register(subScene);
            controller.SceneController.createSimObjects();

            simObjectManager.addSimObject(instance);
            simObjectManagerDefiniton.addSimObject(definition);
            createSelectable(definition, instance);
        }

        public void destroySimObject(SimObjectDefinition definition)
        {
            simObjectManager.destroySimObject(definition.Name);
            simObjectManagerDefiniton.removeSimObject(definition);

            SelectableSimObject selectable = selectables[definition.Name];
            selectables.Remove(definition.Name);
            removeSelectableEditInterface(selectable);
        }

        public void captureSceneProperties()
        {
            foreach (SelectableSimObject selectable in selectables.Values)
            {
                simObjectManagerDefiniton.removeSimObject(selectable.Definition);
                selectable.captureInstanceProperties();
                simObjectManagerDefiniton.addSimObject(selectable.Definition);
            }
        }

        public void setSceneManagerDefintion(SimObjectManagerDefinition definition)
        {
            clearEditInterfaces();
            selectables.Clear();
            controller.SelectionController.clearSelection();
            simObjectManagerDefiniton = definition;
            foreach (SimObjectDefinition simObject in simObjectManagerDefiniton.getDefinitionIter())
            {
                createSelectable(simObject, null);
            }
        }

        public bool hasSimObject(String name)
        {
            return selectables.ContainsKey(name);
        }

        private void createSelectable(SimObjectDefinition definition, SimObjectBase instance)
        {
            SelectableSimObject selectable = new SelectableSimObject(definition, instance);
            selectables.Add(definition.Name, selectable);
            addSelectableEditInterface(selectable);
        }

        private void SceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            if (simObjectManager != null)
            {
                simObjectManager.Dispose();
            }
        }

        private void SceneController_OnSceneLoading(SceneController controller, SimScene scene)
        {
            this.subScene = scene.getDefaultSubScene();
            simObjectManager = simObjectManagerDefiniton.createSimObjectManager(scene.getDefaultSubScene());
            foreach (SelectableSimObject selectable in selectables.Values)
            {
                String name = selectable.Definition.Name;
                if (simObjectManager.hasSimObject(name))
                {
                    selectable.Instance = simObjectManager.getSimObject(name);
                }
                else
                {
                    selectable.Instance = null;
                }
            }
        }

        void editInterfaceEdit(EditInterfaceViewEvent evt)
        {
            SelectableSimObject selectable = selectableEdits.resolveSourceObject(evt.EditInterface);
            if (selectable != null)
            {
                controller.showObjectEditor(selectable.Definition.getEditInterface());
                simObjectManager.destroySimObject(selectable.Instance.Name);
                SimObjectBase instance = selectable.Definition.register(subScene);
                controller.SceneController.createSimObjects();
                simObjectManager.addSimObject(instance);
                selectable.Instance = instance;
            }
        }

        void editInterfaceChosen(EditInterfaceViewEvent evt)
        {
            SelectableSimObject selectable = selectableEdits.resolveSourceObject(evt.EditInterface);
            if (selectable != null)
            {
                if (controller.EventManager.Keyboard.isModifierDown(Modifier.Ctrl))
                {
                    controller.SelectionController.addSelectedObject(selectable);
                }
                else if (controller.EventManager.Keyboard.isModifierDown(Modifier.Alt))
                {
                    controller.SelectionController.removeSelectedObject(selectable);
                }
                else
                {
                    controller.SelectionController.setSelectedObject(selectable);
                }
            }
            else
            {
                controller.SelectionController.clearSelection();
            }
        }

        void SelectionController_OnSelectionChanged(SelectionChangedArgs args)
        {
            if (editInterface != null)
            {
                foreach (SelectableSimObject selectable in args.ObjectsAdded)
                {
                    EditInterface currentEdit = selectableEdits.getEditInterface(selectable);
                    currentEdit.BackColor = Engine.Color.FromARGB(SystemColors.Highlight.ToArgb());
                    currentEdit.ForeColor = Engine.Color.FromARGB(SystemColors.HighlightText.ToArgb());
                }
                foreach (SelectableSimObject selectable in args.ObjectsRemoved)
                {
                    if (selectableEdits.hasEditInterface(selectable))
                    {
                        EditInterface currentEdit = selectableEdits.getEditInterface(selectable);
                        currentEdit.BackColor = Engine.Color.FromARGB(SystemColors.Window.ToArgb());
                        currentEdit.ForeColor = Engine.Color.FromARGB(SystemColors.WindowText.ToArgb());
                    }
                }
            }
        }

        #region EditInterface

        private EditInterface editInterface;
        private EditInterfaceManager<SelectableSimObject> selectableEdits;
        private EditInterfaceCommand destroyCommand;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface("SimObjects");
                selectableEdits = new EditInterfaceManager<SelectableSimObject>(editInterface);
                destroyCommand = new EditInterfaceCommand("Remove", destroySimObjectCallback);
                foreach (SelectableSimObject selectable in selectables.Values)
                {
                    addSelectableEditInterface(selectable);
                }
            }
            return editInterface;
        }

        private void addSelectableEditInterface(SelectableSimObject selectable)
        {
            if (editInterface != null)
            {
                EditInterface edit = selectable.getEditInterface();
                edit.addCommand(destroyCommand);
                selectableEdits.addSubInterface(selectable, edit);
            }
        }

        private void removeSelectableEditInterface(SelectableSimObject selectable)
        {
            if (editInterface != null)
            {
                selectableEdits.removeSubInterface(selectable);
            }
        }

        private void clearEditInterfaces()
        {
            if (editInterface != null)
            {
                selectableEdits.clearSubInterfaces();
            }
        }

        private void destroySimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            foreach (SelectableSimObject selectable in controller.SelectionController.getSelectedObjects())
            {
                destroySimObject(selectable.Definition);
            }
            controller.SelectionController.clearSelection();
        }

        #endregion EditInterface
    }
}

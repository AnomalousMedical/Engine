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
        private SimScene scene;
        private SimObjectPanel panel;

        public SimObjectController()
        {

        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
            controller.SceneController.OnSceneLoaded += new SceneLoaded(SceneController_OnSceneLoaded);
            controller.SceneController.OnSceneUnloading += new SceneUnloading(SceneController_OnSceneUnloading);
            controller.SelectionController.OnSelectionChanged += new ObjectSelected(SelectionController_OnSelectionChanged);
        }

        public void setUI(SimObjectPanel panel)
        {
            this.panel = panel;
            panel.EditInterface.setEditInterface(getEditInterface());
            panel.EditInterface.OnEditInterfaceSelectionChanged += new Editor.EditInterfaceSelectionChanged(EditInterface_OnEditInterfaceSelectionChanged);
            panel.EditInterface.OnEditInterfaceSelectionEdit += new EditInterfaceSelectionEdit(EditInterface_OnEditInterfaceSelectionEdit);
        }

        public void createSimObject(SimObjectDefinition definition)
        {
            SimObjectBase instance = definition.register(scene.getDefaultSubScene());
            scene.buildScene();

            addSimObject(definition, instance);
        }

        public void destroySimObject(SimObjectDefinition definition)
        {
            simObjectManager.destroySimObject(definition.Name);
            simObjectManagerDefiniton.removeSimObject(definition);

            SelectableSimObject selectable = selectables[definition.Name];
            selectables.Remove(definition.Name);
            removeSelectableEditInterface(selectable);
        }

        public void createEmptyManager()
        {
            simObjectManagerDefiniton = new SimObjectManagerDefinition();
            simObjectManager = simObjectManagerDefiniton.createSimObjectManager(scene.getDefaultSubScene());
        }

        public void createFromDefinition(SimObjectManagerDefinition definition)
        {
            simObjectManagerDefiniton = definition;
            simObjectManager = simObjectManagerDefiniton.createSimObjectManager(scene.getDefaultSubScene());
        }

        private void addSimObject(SimObjectDefinition definition, SimObjectBase instance)
        {
            simObjectManager.addSimObject(instance);
            simObjectManagerDefiniton.addSimObject(definition);

            SelectableSimObject selectable = new SelectableSimObject(definition, instance);
            selectables.Add(definition.Name, selectable);
            addSelectableEditInterface(selectable);
        }

        private void SceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            simObjectManager.Dispose();
        }

        private void SceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            this.scene = scene;
        }

        void EditInterface_OnEditInterfaceSelectionEdit(EditInterfaceViewEvent evt)
        {
            SelectableSimObject selectable = selectableEdits.resolveSourceObject(evt.EditInterface);
            if (selectable != null)
            {
                controller.showObjectEditor(selectable.Definition.getEditInterface());
                simObjectManager.destroySimObject(selectable.Instance.Name);
                SimObjectBase instance = selectable.Definition.register(scene.getDefaultSubScene());
                scene.buildScene();
                simObjectManager.addSimObject(instance);
                selectable.Instance = instance;
            }
        }

        void EditInterface_OnEditInterfaceSelectionChanged(EditInterfaceViewEvent evt)
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
                    EditInterface currentEdit = selectableEdits.getEditInterface(selectable);
                    currentEdit.BackColor = Engine.Color.FromARGB(SystemColors.Window.ToArgb());
                    currentEdit.ForeColor = Engine.Color.FromARGB(SystemColors.WindowText.ToArgb());                    
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

        private void destroySimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            destroySimObject(selectableEdits.resolveSourceObject(callback.getSelectedEditInterface()).Definition);
        }

        #endregion EditInterface
    }
}

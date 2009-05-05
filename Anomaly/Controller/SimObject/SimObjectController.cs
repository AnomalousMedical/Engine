using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using Editor;

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
        }

        public void setUI(SimObjectPanel panel)
        {
            this.panel = panel;
            panel.EditInterface.setEditInterface(getEditInterface());
            panel.EditInterface.OnEditInterfaceSelectionChanged += new Editor.EditInterfaceSelectionChanged(EditInterface_OnEditInterfaceSelectionChanged);
        }

        void EditInterface_OnEditInterfaceSelectionChanged(EditInterfaceViewEvent evt)
        {
            SelectableSimObject selectable = selectableEdits.resolveSourceObject(evt.EditInterface);
            if (selectable != null)
            {
                controller.SelectionController.setSelectedObject(selectable);
            }
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

        #region EditInterface

        private EditInterface editInterface;
        private EditInterfaceManager<SelectableSimObject> selectableEdits;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface("SimObjects");
                selectableEdits = new EditInterfaceManager<SelectableSimObject>(editInterface);
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

        #endregion EditInterface
    }
}

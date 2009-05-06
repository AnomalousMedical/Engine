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
    /// <summary>
    /// This class manages the SimObjects.
    /// </summary>
    class SimObjectController
    {
        #region Fields

        private AnomalyController controller;
        private Dictionary<String, SelectableSimObject> selectables = new Dictionary<string, SelectableSimObject>();
        private SimObjectManager simObjectManager;
        private SimObjectManagerDefinition simObjectManagerDefiniton;
        private SimObjectPanel panel;
        private SimSubScene subScene;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public SimObjectController()
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Initialize function.
        /// </summary>
        /// <param name="controller">The AnomalyController.</param>
        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
            controller.SceneController.OnSceneLoading += SceneController_OnSceneLoading;
            controller.SceneController.OnSceneUnloading += SceneController_OnSceneUnloading;
            controller.SelectionController.OnSelectionChanged += new ObjectSelected(SelectionController_OnSelectionChanged);
        }

        /// <summary>
        /// Set the UI that the SimObjects will appear on.
        /// </summary>
        /// <param name="panel">The SimObjectPanel.</param>
        public void setUI(SimObjectPanel panel)
        {
            this.panel = panel;
            panel.EditInterface.setEditInterface(getEditInterface());
            panel.EditInterface.OnEditInterfaceChosen += new EditInterfaceChosen(editInterfaceChosen);
            panel.EditInterface.OnEditInterfaceSelectionEdit += new EditInterfaceSelectionEdit(editInterfaceEdit);
        }

        /// <summary>
        /// Get the current SimObjectManagerDefinition. This should be treated
        /// as read only.
        /// </summary>
        /// <returns>The SimObjectManagerDefinition for this class.</returns>
        public SimObjectManagerDefinition getSimObjectManagerDefinition()
        {
            return simObjectManagerDefiniton;
        }

        /// <summary>
        /// Create a new SimObject and add it.
        /// </summary>
        /// <param name="definition">The definition to create.</param>
        public void createSimObject(SimObjectDefinition definition)
        {
            SimObjectBase instance = definition.register(subScene);
            controller.SceneController.createSimObjects();

            simObjectManager.addSimObject(instance);
            simObjectManagerDefiniton.addSimObject(definition);
            createSelectable(definition, instance);
        }

        /// <summary>
        /// Destroy and remove the SimObject defined by definition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void destroySimObject(SimObjectDefinition definition)
        {
            simObjectManager.destroySimObject(definition.Name);
            simObjectManagerDefiniton.removeSimObject(definition);

            SelectableSimObject selectable = selectables[definition.Name];
            selectables.Remove(definition.Name);
            removeSelectableEditInterface(selectable);
        }

        /// <summary>
        /// This function will update all definitions with the current state of
        /// their associated SimObjects. It is intended to be called when
        /// changing from dynamic mode to static mode in order to capture the
        /// updates made in dynamic mode.
        /// </summary>
        public void captureSceneProperties()
        {
            foreach (SelectableSimObject selectable in selectables.Values)
            {
                simObjectManagerDefiniton.removeSimObject(selectable.Definition);
                selectable.captureInstanceProperties();
                simObjectManagerDefiniton.addSimObject(selectable.Definition);
            }
        }

        /// <summary>
        /// Set the SimObjectManagerDefintion to be used by this controller.
        /// This will add the definitions to the UI but will not create the
        /// instances. That will happen when the scene is loaded on the
        /// callback.
        /// </summary>
        /// <param name="definition">The SimObjectManagerDefinition to use.</param>
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

        /// <summary>
        /// Determine if this controller has a SimObject named name.
        /// </summary>
        /// <param name="name">The name to check for.</param>
        /// <returns>True if name exists in this controller.</returns>
        public bool hasSimObject(String name)
        {
            return selectables.ContainsKey(name);
        }

        /// <summary>
        /// Helper function to create a selectable.
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="instance"></param>
        private void createSelectable(SimObjectDefinition definition, SimObjectBase instance)
        {
            SelectableSimObject selectable = new SelectableSimObject(definition, instance);
            selectables.Add(definition.Name, selectable);
            addSelectableEditInterface(selectable);
        }

        /// <summary>
        /// Callback for when the scene is unloading. Will clear all SimObject instances.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="scene"></param>
        private void SceneController_OnSceneUnloading(SceneController controller, SimScene scene)
        {
            if (simObjectManager != null)
            {
                simObjectManager.Dispose();
            }
        }

        /// <summary>
        /// Callback for when the scene is loading. Will create all instances
        /// and add them to their selectables.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="scene"></param>
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

        /// <summary>
        /// Callback for when the UI has requested an object be edited. Will
        /// show the UI dialog for editing and recreate the object.
        /// </summary>
        /// <param name="evt"></param>
        public void editInterfaceEdit(EditInterfaceViewEvent evt)
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

        /// <summary>
        /// Callback for when an item is chosen on the UI. Used to select objects.
        /// </summary>
        /// <param name="evt"></param>
        public void editInterfaceChosen(EditInterfaceViewEvent evt)
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

        /// <summary>
        /// Callback for when the seleciton changes. Will highlight the
        /// EditInterfaces for all selected objects.
        /// </summary>
        /// <param name="args"></param>
        public void SelectionController_OnSelectionChanged(SelectionChangedArgs args)
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

        #endregion Functions

        #region EditInterface

        private EditInterface editInterface;
        private EditInterfaceManager<SelectableSimObject> selectableEdits;
        private EditInterfaceCommand destroyCommand;

        /// <summary>
        /// Get the EditInterface for this controller.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Helper function to add a SelectableSimObject EditInterface.
        /// </summary>
        /// <param name="selectable"></param>
        private void addSelectableEditInterface(SelectableSimObject selectable)
        {
            if (editInterface != null)
            {
                EditInterface edit = selectable.getEditInterface();
                edit.addCommand(destroyCommand);
                selectableEdits.addSubInterface(selectable, edit);
            }
        }

        /// <summary>
        /// Helper function to remove an EditInterface.
        /// </summary>
        /// <param name="selectable"></param>
        private void removeSelectableEditInterface(SelectableSimObject selectable)
        {
            if (editInterface != null)
            {
                selectableEdits.removeSubInterface(selectable);
            }
        }

        /// <summary>
        /// Helper function to clear all EditInterfaces.
        /// </summary>
        private void clearEditInterfaces()
        {
            if (editInterface != null)
            {
                selectableEdits.clearSubInterfaces();
            }
        }

        /// <summary>
        /// Callback for when the destroy command is executed. Will erase a SimObject.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="command"></param>
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

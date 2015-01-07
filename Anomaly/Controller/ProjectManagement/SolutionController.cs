using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using Engine.Editing;
using Logging;
using Engine.ObjectManagement;
using Engine.Resources;
using System.Drawing;
using Engine.Platform;
using Anomalous.GuiFramework.Editor;

namespace Anomaly
{
    class SolutionController
    {
        private static readonly ButtonEvent AddSelectable;
        private static readonly ButtonEvent RemoveSelectable;

        static SolutionController()
        {
            AddSelectable = new ButtonEvent(EventLayers.Main);
            AddSelectable.addButton(KeyboardButtonCode.KC_LCONTROL);
            DefaultEvents.registerDefaultEvent(AddSelectable);

            RemoveSelectable = new ButtonEvent(EventLayers.Main);
            RemoveSelectable.addButton(KeyboardButtonCode.KC_LMENU);
            DefaultEvents.registerDefaultEvent(RemoveSelectable);
        }

        private Solution solution;
        private SolutionPanel solutionPanel;
        private IObjectEditorGUI objectEditor;
        private AnomalyController controller;

        private ObjectPlaceholderInterface currentPlaceholder;

        private List<EditInterface> selectedEditInterfaces = new List<EditInterface>();

        public SolutionController(Solution solution, SolutionPanel solutionPanel, AnomalyController controller, IObjectEditorGUI objectEditor)
        {
            this.controller = controller;
            this.solution = solution;
            this.solutionPanel = solutionPanel;
            solutionPanel.setSolution(solution);
            this.objectEditor = objectEditor;
            solutionPanel.InterfaceChosen += new Editor.EditInterfaceEvent(solutionPanel_InterfaceChosen);
            controller.SelectionController.OnSelectionChanged += new ObjectSelected(SelectionController_OnSelectionChanged);
        }

        /// <summary>
        /// The interface that is currently active.
        /// </summary>
        public EditInterface CurrentEditInterface
        {
            get
            {
                return solutionPanel.SelectedEditInterface;
            }
        }

        /// <summary>
        /// The list of all selected interfaces.
        /// </summary>
        public IEnumerable<EditInterface> SelectedEditInterfaces
        {
            get
            {
                //If there are multi selected edit interfaces, grab those.
                if (selectedEditInterfaces.Count > 0)
                {
                    return selectedEditInterfaces;
                }
                //Otherwise only one thing is selected.
                else
                {
                    List<EditInterface> selected = new List<EditInterface>();
                    //Only actually add something to this list if it isnt null.
                    if (CurrentEditInterface != null)
                    {
                        selected.Add(CurrentEditInterface);
                    }
                    return selected;
                }
            }
        }

        void solutionPanel_InterfaceChosen(EditInterfaceViewEvent evt)
        {
            EditInterface editInterface = evt.EditInterface;
            if (editInterface.hasEditableProperties())
            {
                //Determine if the EditInterface is an ObjectPlaceholderInterface
                ObjectPlaceholderInterface objectPlaceholder = editInterface.getEditableProperties().First() as ObjectPlaceholderInterface;
                if (objectPlaceholder != null)
                {
                    showPlaceholder(objectPlaceholder);
                }
                else
                {
                    objectEditor.setEditInterface(editInterface, null, null, null);
                }
            }
            else if (editInterface.canAddRemoveProperties())
            {
                objectEditor.setEditInterface(editInterface, null, null, null);
            }
            else
            {
                objectEditor.clearEditInterface();
            }
            processInstanceSelection(editInterface);
        }        

        private void showPlaceholder(ObjectPlaceholderInterface placeholder)
        {
            Object currentObject = placeholder.getObject();
            EditInterface edit = placeholder.getObjectEditInterface(currentObject);
            objectEditor.setEditInterface(edit, currentObject, placeholder.uiFieldUpdateCallback, placeholder.uiEditingCompletedCallback);
            currentPlaceholder = placeholder;
        }

        private void processInstanceSelection(EditInterface editInterface)
        {
            if (editInterface.hasEditableProperties())
            {
                InstanceFileInterface instanceFile = editInterface.getEditableProperties().First() as InstanceFileInterface;
                if (instanceFile != null)
                {
                    if (AddSelectable.HeldDown)
                    {
                        controller.SelectionController.addSelectedObject(instanceFile);
                    }
                    else if (RemoveSelectable.HeldDown)
                    {
                        controller.SelectionController.removeSelectedObject(instanceFile);
                    }
                    else
                    {
                        controller.SelectionController.setSelectedObject(instanceFile);
                    }
                }
                else
                {
                    controller.SelectionController.clearSelection();
                }
            }
            else
            {
                controller.SelectionController.clearSelection();
            }
        }

        void SelectionController_OnSelectionChanged(SelectionChangedArgs args)
        {
            foreach (InstanceFileInterface selectable in args.ObjectsAdded)
            {
                EditInterface edit = selectable.getEditInterface();
                edit.BackColor = Engine.Color.FromARGB(SystemColors.Highlight.ToArgb());
                edit.ForeColor = Engine.Color.FromARGB(SystemColors.HighlightText.ToArgb());
                selectedEditInterfaces.Add(edit);
            }
            foreach (InstanceFileInterface selectable in args.ObjectsRemoved)
            {
                EditInterface edit = selectable.getEditInterface();
                edit.BackColor = Engine.Color.FromARGB(SystemColors.Window.ToArgb());
                edit.ForeColor = Engine.Color.FromARGB(SystemColors.WindowText.ToArgb());
                selectedEditInterfaces.Remove(edit);
            }
        }
    }
}

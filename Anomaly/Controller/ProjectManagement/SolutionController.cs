using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using Engine.Editing;
using Logging;
using Engine.ObjectManagement;
using Engine.Resources;

namespace Anomaly
{
    class SolutionController
    {
        private Solution solution;
        private SolutionPanel solutionPanel;
        private IObjectEditorGUI objectEditor;

        private ObjectPlaceholderInterface currentPlaceholder;

        public SolutionController(Solution solution, SolutionPanel solutionPanel, IObjectEditorGUI objectEditor)
        {
            this.solution = solution;
            this.solutionPanel = solutionPanel;
            solutionPanel.setSolution(solution);
            this.objectEditor = objectEditor;
            solutionPanel.InterfaceChosen += new Editor.EditInterfaceEvent(solutionPanel_InterfaceChosen);
        }

        void solutionPanel_InterfaceChosen(Editor.EditInterfaceViewEvent evt)
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
        }        

        private void showPlaceholder(ObjectPlaceholderInterface placeholder)
        {
            Object currentObject = placeholder.getObject();
            EditInterface edit = placeholder.getObjectEditInterface(currentObject);
            objectEditor.setEditInterface(edit, currentObject, placeholder.uiFieldUpdateCallback, placeholder.uiEditingCompletedCallback);
            currentPlaceholder = placeholder;
        }
    }
}

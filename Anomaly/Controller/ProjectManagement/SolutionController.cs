using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using Engine.Editing;
using Logging;

namespace Anomaly
{
    class SolutionController
    {
        private Solution solution;
        private SolutionPanel solutionPanel;
        private IObjectEditorGUI objectEditor;

        private EditableFileInterface<Instance> currentInstanceFile;

        public SolutionController(Solution solution, SolutionPanel solutionPanel, IObjectEditorGUI objectEditor)
        {
            this.solution = solution;
            this.solutionPanel = solutionPanel;
            solutionPanel.setSolution(solution);
            this.objectEditor = objectEditor;
            solutionPanel.InterfaceChosen += new Editor.EditInterfaceChosen(solutionPanel_InterfaceChosen);
        }

        void solutionPanel_InterfaceChosen(Editor.EditInterfaceViewEvent evt)
        {
            EditInterface editInterface = evt.EditInterface;
            if (editInterface.hasEditableProperties())
            {
                EditableFileInterface<Instance> instanceFile = editInterface.getEditableProperties().First() as EditableFileInterface<Instance>;
                if (instanceFile != null)
                {
                    showInstance(instanceFile);
                }
                else
                {
                    objectEditor.setEditInterface(editInterface, null, null);
                }
            }
            else if (editInterface.canAddRemoveProperties())
            {
                objectEditor.setEditInterface(editInterface, null, null);
            }
            else
            {
                objectEditor.clearEditInterface();
            }
        }

        private void showInstance(EditableFileInterface<Instance> instanceFile)
        {
            Instance instance = instanceFile.getFileObject();
            if (instance != null)
            {
                objectEditor.setEditInterface(instance.getEditInterface(), instance, instanceUpdatedByUI);
                currentInstanceFile = instanceFile;
            }
            else
            {
                Log.Error("Could not load instance {0}. File is invalid.", instanceFile.Filename);
                objectEditor.clearEditInterface();
            }
        }

        void instanceUpdatedByUI(EditInterface editInterface, object editingObject)
        {
            Instance instance = editingObject as Instance;
            if (instance != null)
            {
                currentInstanceFile.saveObject(instance);
            }
        }
    }
}

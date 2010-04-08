using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using Engine.Editing;

namespace Anomaly
{
    class SolutionController
    {
        private Solution solution;
        private SolutionPanel solutionPanel;
        private IObjectEditorGUI objectEditor;

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
                objectEditor.setEditInterface(editInterface, null, null);
            }
            else
            {
                objectEditor.clearEditInterface();
            }
        }
    }
}

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

        private EditableFileInterface<Instance> currentInstanceFile;
        private EditableFileInterface<SimSceneDefinition> currentSceneFile;
        private EditableFileInterface<ResourceManager> currentResourceFile;

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
                //Determine if the EditInterface is representing one of our files.
                EditableProperty firstProperty = editInterface.getEditableProperties().First();
                EditableFileInterface<Instance> instanceFile = firstProperty as EditableFileInterface<Instance>;
                if (instanceFile != null)
                {
                    showInstance(instanceFile);
                    return;
                }

                EditableFileInterface<SimSceneDefinition> sceneFile = firstProperty as EditableFileInterface<SimSceneDefinition>;
                if (sceneFile != null)
                {
                    showScene(sceneFile);
                    return;
                }

                EditableFileInterface<ResourceManager> resourceFile = firstProperty as EditableFileInterface<ResourceManager>;
                if (resourceFile != null)
                {
                    showResources(resourceFile);
                    return;
                }

                objectEditor.setEditInterface(editInterface, null, null);
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

        private void showResources(EditableFileInterface<ResourceManager> resourceFile)
        {
            ResourceManager resources = resourceFile.getFileObject();
            if (resources != null)
            {
                objectEditor.setEditInterface(resources.getEditInterface(), resources, resourcesUpdatedByUI);
                currentResourceFile = resourceFile;
            }
            else
            {
                Log.Error("Could not load resources {0}. File is invalid.", resourceFile.Filename);
                objectEditor.clearEditInterface();
            }
        }

        private void showScene(EditableFileInterface<SimSceneDefinition> sceneFile)
        {
            SimSceneDefinition scene = sceneFile.getFileObject();
            if (scene != null)
            {
                objectEditor.setEditInterface(scene.getEditInterface(), scene, sceneUpdatedByUI);
                currentSceneFile = sceneFile;
            }
            else
            {
                Log.Error("Could not load scene {0}. File is invalid.", sceneFile.Filename);
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

        void resourcesUpdatedByUI(EditInterface editInterface, object editingObject)
        {
            ResourceManager resources = editingObject as ResourceManager;
            if (resources != null)
            {
                currentResourceFile.saveObject(resources);
            }
        }

        void sceneUpdatedByUI(EditInterface editInterface, object editingObject)
        {
            SimSceneDefinition scene = editingObject as SimSceneDefinition;
            if (scene != null)
            {
                currentSceneFile.saveObject(scene);
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

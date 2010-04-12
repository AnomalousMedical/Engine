using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using System.IO;
using Engine;
using Engine.ObjectManagement;
using Engine.Saving.XMLSaver;
using System.Xml;
using Engine.Resources;

namespace Anomaly
{
    public partial class Solution
    {
        private static XmlSaver xmlSaver = new XmlSaver();

        private String name;
        private Dictionary<String, Project> projects = new Dictionary<string, Project>();

        private PluginSection pluginSection;
        private ResourceSection resourceSection;
        private ConfigFile backingFile;
        private String workingDirectory;
        private SolutionSection solutionSection;
        private ResourceManagerFileInterface resourceFileInterface;

        private Project currentProject;

        private AnomalyController anomalyController;

        public Solution(String solutionFileName)
        {
            name = Path.GetFileNameWithoutExtension(solutionFileName);
            workingDirectory = Path.GetDirectoryName(solutionFileName);
            backingFile = new ConfigFile(solutionFileName);
            backingFile.loadConfigFile();
            pluginSection = new PluginSection(backingFile);
            resourceSection = new ResourceSection(backingFile);
            solutionSection = new SolutionSection(backingFile);
        }

        public void loadExternalFiles(AnomalyController controller)
        {
            this.anomalyController = controller;
            String globalResourcesFile = Path.Combine(workingDirectory, solutionSection.GlobalResourceFile);
            if (!File.Exists(globalResourcesFile))
            {
                ResourceManager globalResources = controller.PluginManager.createEmptyResourceManager();
                using (XmlTextWriter textWriter = new XmlTextWriter(globalResourcesFile, Encoding.Default))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(globalResources, textWriter);
                }
            }

            resourceFileInterface = new ResourceManagerFileInterface("Global Resources", null, globalResourcesFile);

            foreach (String projectFile in Directory.GetFiles(workingDirectory, "*.prj", SearchOption.AllDirectories))
            {
                Project project = new Project(Path.GetFileNameWithoutExtension(projectFile), Path.GetDirectoryName(projectFile));
                addProject(project);
                if (currentProject == null)
                {
                    currentProject = project;
                }
            }
        }

        public void addProject(Project project)
        {
            projects.Add(project.Name, project);
            ProjectWriter.addProject(project);
            onProjectAdded(project);
        }

        public void removeProject(Project project)
        {
            projects.Remove(project.Name);
            onProjectRemoved(project);
        }

        public void createCurrentProject()
        {
            anomalyController.ResourceController.clearResources();
            anomalyController.ResourceController.addResources(resourceFileInterface.getFileObject());
            if (currentProject != null)
            {
                currentProject.buildScene(anomalyController);
            }
            else //Create an empty scene
            {
                SimSceneDefinition sceneDefinition = new SimSceneDefinition();
                anomalyController.SceneController.setSceneDefinition(sceneDefinition);

                SimObjectManagerDefinition simObjectManager = new SimObjectManagerDefinition();
                anomalyController.SimObjectController.setSceneManagerDefintion(simObjectManager);
            }
            anomalyController.ResourceController.applyToScene();
        }

        public void save()
        {
            resourceFileInterface.save();
            foreach (Project project in projects.Values)
            {
                project.save();
            }
        }

        public PluginSection PluginSection
        {
            get
            {
                return pluginSection;
            }
        }

        public ResourceSection ResourceSection
        {
            get
            {
                return resourceSection;
            }
        }

        public String WorkingDirectory
        {
            get
            {
                return workingDirectory;
            }
        }
    }

    partial class Solution
    {
        private EditInterface editInterface;

        private EditInterfaceCommand destroyProject;
        private EditInterfaceCommand setActiveProject;
        private EditInterfaceManager<Project> projectInterfaceManager;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.IconReferenceTag = AnomalyIcons.Solution;

                editInterface.addSubInterface(resourceFileInterface.getEditInterface());

                editInterface.addCommand(new EditInterfaceCommand("Create Project", createProjectCallback));
                projectInterfaceManager = new EditInterfaceManager<Project>(editInterface);
                destroyProject = new EditInterfaceCommand("Remove", destroyProjectCallback);
                setActiveProject = new EditInterfaceCommand("Set Active", setActiveProjectCallback);
                foreach (Project project in projects.Values)
                {
                    onProjectAdded(project);
                }
            }

            return editInterface;
        }

        #region Projects

        private void createProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateProjectCreate);
            if (accept)
            {
                Project project = new Project(name, Path.Combine(workingDirectory, name));
                addProject(project);
            }
        }

        private bool validateProjectCreate(String input, out String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (this.projects.ContainsKey(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        private void destroyProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            removeProject(projectInterfaceManager.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        private void setActiveProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            currentProject.unloadScene(anomalyController);
            currentProject = projectInterfaceManager.resolveSourceObject(callback.getSelectedEditInterface());
            anomalyController.createNewScene();
        }

        private void onProjectAdded(Project project)
        {
            if (editInterface != null)
            {
                EditInterface edit = project.getEditInterface();
                edit.addCommand(setActiveProject);
                edit.addCommand(destroyProject);
                projectInterfaceManager.addSubInterface(project, edit);
            }
        }

        private void onProjectRemoved(Project project)
        {
            if (editInterface != null)
            {
                projectInterfaceManager.removeSubInterface(project);
            }
        }

        #endregion
    }
}

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
using System.Drawing;
using Logging;

namespace Anomaly
{
    public partial class Solution
    {
        private static XmlSaver xmlSaver = new XmlSaver();
        private static Engine.Color ACTIVE_PROJECT_COLOR = new Engine.Color(0.133f, 0.694f, 0.298f);

        private String name;
        private Dictionary<String, Project> projects = new Dictionary<string, Project>();

        private PluginSection pluginSection;
        private ResourceSection resourceSection;
        private ConfigFile engineConfiguration;
        private String workingDirectory;
        private ResourceManagerFileInterface resourceFileInterface;

        private Project currentProject;

        private AnomalyController anomalyController;

        private String solutionFile;
        private SolutionData solutionData;

        public Solution(String solutionFileName)
        {
            this.solutionFile = solutionFileName;
            name = Path.GetFileNameWithoutExtension(solutionFileName);
            workingDirectory = Path.GetDirectoryName(solutionFileName);

            String engineConfigFile = Path.Combine(workingDirectory, name + ".ecfg");
            engineConfiguration = new ConfigFile(engineConfigFile);
            engineConfiguration.loadConfigFile();
            pluginSection = new PluginSection(engineConfiguration);
            resourceSection = new ResourceSection(engineConfiguration);
        }

        public void loadExternalFiles(AnomalyController controller)
        {
            if (File.Exists(solutionFile))
            {
                try
                {
                    using (XmlTextReader textReader = new XmlTextReader(solutionFile))
                    {
                        solutionData = xmlSaver.restoreObject(textReader) as SolutionData;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Could not load solution data file {0} because:\n{1}", solutionFile, e.Message);
                }
            }
            if (solutionData == null)
            {
                solutionData = new SolutionData();
            }

            this.anomalyController = controller;
            String globalResourcesFile = Path.Combine(workingDirectory, "Resources.xml");
            if (!File.Exists(globalResourcesFile))
            {
                ResourceManager globalResources = controller.PluginManager.createScratchResourceManager();
                using (XmlTextWriter textWriter = new XmlTextWriter(globalResourcesFile, Encoding.Unicode))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(globalResources, textWriter);
                }
            }

            resourceFileInterface = new ResourceManagerFileInterface("Global Resources", EngineIcons.Resources, globalResourcesFile);
            anomalyController.ResourceController.GlobalResources.addResources(resourceFileInterface.getFileObject());
            anomalyController.ResourceController.GlobalResources.initializeResources();

            foreach (String projectFile in Directory.GetFiles(workingDirectory, "*.prj", SearchOption.AllDirectories))
            {
                Project project = new Project(this, Path.GetFileNameWithoutExtension(projectFile), Path.GetDirectoryName(projectFile));
                addProject(project);
                if (currentProject == null)
                {
                    currentProject = project;
                }
            }
        }

        public void refreshGlobalResources()
        {
            if (currentProject != null)
            {
                currentProject.unloadScene(anomalyController);
                anomalyController.ResourceController.clearResources();
                anomalyController.ResourceController.applyToScene();
            }
            anomalyController.ResourceController.GlobalResources.changeResourcesToMatch(resourceFileInterface.getFileObject());
            anomalyController.ResourceController.GlobalResources.initializeResources();
            createCurrentProject();
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
            ProjectWriter.removeProject(project);
            onProjectRemoved(project);
        }

        public Project getProject(String name)
        {
            Project project = null;
            projects.TryGetValue(name, out project);
            return project;
        }

        public void createCurrentProject()
        {
            anomalyController.ResourceController.clearResources();
            if (currentProject != null)
            {
                currentProject.showScene(anomalyController);
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

        public void build()
        {
            foreach (Project project in projects.Values)
            {
                project.build();
            }
        }

        public void save()
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(solutionFile, Encoding.Unicode))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(solutionData, xmlWriter);
            }
            resourceFileInterface.save();
            foreach (Project project in projects.Values)
            {
                project.save();
            }
        }

        /// <summary>
        /// Get the resources from all projects.
        /// </summary>
        /// <returns></returns>
        public ResourceManager getAllResources()
        {
            ResourceManager resourceManager = PluginManager.Instance.createScratchResourceManager();
            resourceManager.addResources(GlobalResources);
            foreach (Project project in projects.Values)
            {
                project.getResources(resourceManager);
            }
            return resourceManager;
        }

        public PluginSection PluginSection
        {
            get
            {
                return pluginSection;
            }
        }

        public String ResourceRoot
        {
            get
            {
                return resourceSection.ResourceRoot;
            }
        }

        public IEnumerable<ExternalResource> AdditionalResources
        {
            get
            {
                return solutionData.ExternalResources.ExternalResources;
            }
        }

        public String WorkingDirectory
        {
            get
            {
                return workingDirectory;
            }
        }

        public ResourceManager GlobalResources
        {
            get
            {
                return resourceFileInterface.getFileObject();
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
        }
    }

    partial class Solution
    {
        private EditInterface editInterface;

        private EditInterfaceCommand destroyProject;
        private EditInterfaceCommand setActiveProject;
        private EditInterfaceCommand buildProject;
        private EditInterfaceManager<Project> projectInterfaceManager;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.IconReferenceTag = AnomalyIcons.Solution;

                editInterface.addSubInterface(resourceFileInterface.getEditInterface());
                editInterface.addSubInterface(solutionData.ExternalResources.getEditInterface());

                editInterface.addCommand(new EditInterfaceCommand("Build Solution", buildSolutionCallback));
                editInterface.addCommand(new EditInterfaceCommand("Create Project", createProjectCallback));

                projectInterfaceManager = new EditInterfaceManager<Project>(editInterface);
                destroyProject = new EditInterfaceCommand("Remove", destroyProjectCallback);
                setActiveProject = new EditInterfaceCommand("Set Active", setActiveProjectCallback);
                buildProject = new EditInterfaceCommand("Build Project", buildProjectCallback);
                foreach (Project project in projects.Values)
                {
                    onProjectAdded(project);
                    if (project == currentProject)
                    {
                        EditInterface edit = project.getEditInterface();
                        edit.ForeColor = ACTIVE_PROJECT_COLOR;
                    }
                }
            }

            return editInterface;
        }

        #region Projects

        private void createProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            callback.getInputString("Enter a name.", delegate(String input, ref String errorPrompt)
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

                Project project = new Project(this, input, Path.Combine(workingDirectory, input));
                addProject(project);
                return true;
            });
        }

        private void destroyProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            removeProject(projectInterfaceManager.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        private void setActiveProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            if (currentProject != null)
            {
                currentProject.getEditInterface().ForeColor = Engine.Color.FromARGB(SystemColors.WindowText.ToArgb());
                currentProject.unloadScene(anomalyController);
            }
            currentProject = projectInterfaceManager.resolveSourceObject(callback.getSelectedEditInterface());
            callback.getSelectedEditInterface().ForeColor = ACTIVE_PROJECT_COLOR;
            anomalyController.buildScene();
        }

        private void buildProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            Project project = projectInterfaceManager.resolveSourceObject(callback.getSelectedEditInterface());
            project.build();
        }

        private void onProjectAdded(Project project)
        {
            if (editInterface != null)
            {
                EditInterface edit = project.getEditInterface();
                edit.addCommand(setActiveProject);
                edit.addCommand(buildProject);
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

        private void buildSolutionCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            Log.ImportantInfo("Building solution {0}.", name);
            this.build();
            Log.ImportantInfo("Finished building solution {0}.", name);
        }
    }
}

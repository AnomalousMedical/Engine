using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using System.IO;
using Engine.Resources;
using System.Xml;
using Engine;
using Engine.Saving.XMLSaver;
using Engine.ObjectManagement;
using Logging;

namespace Anomaly
{
    public partial class Project
    {
        private static XmlSaver xmlSaver = new XmlSaver();

        private String name;
        private InstanceGroup instanceGroup;
        private String workingDirectory;
        private bool shown = false;
        private ResourceManagerFileInterface resourceFileInterface;
        private SimSceneFileInterface sceneFileInterface;
        private Solution solution;
        private String projectFile;
        private ProjectData projectData;

        public Project(Solution solution, String name, String workingDirectory)
        {
            this.solution = solution;
            this.name = name;
            this.workingDirectory = workingDirectory;
            String instancesPath = Path.Combine(workingDirectory, "Instances");
            instanceGroup = new InstanceGroup("Instances", instancesPath);
            if (!Directory.Exists(instancesPath))
            {
                InstanceWriter.Instance.addInstanceGroup(instanceGroup);
            }
            else
            {
                InstanceWriter.Instance.scanForFiles(instanceGroup);
            }

            projectFile = Path.Combine(WorkingDirectory, Name + ".prj");
            if (File.Exists(projectFile))
            {
                try
                {
                    using (XmlTextReader textReader = new XmlTextReader(projectFile))
                    {
                        projectData = xmlSaver.restoreObject(textReader) as ProjectData;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Could not load project references file {0} because:\n{1}", projectFile, e.Message);
                }
            }
            if(projectData == null)
            {
                projectData = new ProjectData();
                projectData.SceneFileName = name + ".sim.xml";
            }

            String resourcesFile = Path.Combine(workingDirectory, "Resources.xml");
            if (!File.Exists(resourcesFile))
            {
                ResourceManager resources = PluginManager.Instance.createEmptyResourceManager();
                using (XmlTextWriter textWriter = new XmlTextWriter(resourcesFile, Encoding.Unicode))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(resources, textWriter);
                }
            }

            String sceneDefinitionFile = Path.Combine(workingDirectory, "SceneDefinition.xml");
            if (!File.Exists(sceneDefinitionFile))
            {
                SimSceneDefinition sceneDefinition = new SimSceneDefinition();
                using (XmlTextWriter textWriter = new XmlTextWriter(sceneDefinitionFile, Encoding.Unicode))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(sceneDefinition, textWriter);
                }
            }

            resourceFileInterface = new ResourceManagerFileInterface("Resources", EngineIcons.Resources, resourcesFile);
            sceneFileInterface = new SimSceneFileInterface("Scene Definition", EngineIcons.Scene, sceneDefinitionFile);
        }

        public void showScene(AnomalyController anomalyController)
        {
            if (!shown)
            {
                anomalyController.SceneController.setSceneDefinition(sceneFileInterface.getFileObject());

                SimObjectManagerDefinition simObjectManager = new SimObjectManagerDefinition();
                anomalyController.SimObjectController.setSceneManagerDefintion(simObjectManager);

                commonShowScene(anomalyController);
            }
        }

        private void commonShowScene(AnomalyController anomalyController)
        {
            if (!shown)
            {
                if (editInterface != null)
                {
                    editInterface.IconReferenceTag = AnomalyIcons.ProjectBuilt;
                }

                shown = true;

                anomalyController.ResourceController.addResources(resourceFileInterface.getFileObject());

                instanceGroup.showInstances(anomalyController.SimObjectController);

                foreach (ProjectReference reference in projectData.ProjectReferences.ReferencedProjectNames)
                {
                    Project subProject = solution.getProject(reference.ProjectName);
                    if (subProject != null)
                    {
                        subProject.commonShowScene(anomalyController);
                    }
                    else
                    {
                        Log.Error("Could not find referenced project {0} for project {1}. Project could not be shown.", reference.ProjectName, Name);
                    }
                }
            }
        }

        public void unloadScene(AnomalyController anomalyController)
        {
            if (shown)
            {
                if (editInterface != null)
                {
                    editInterface.IconReferenceTag = AnomalyIcons.Project;
                }

                shown = false;
                instanceGroup.destroyInstances(anomalyController.SimObjectController);

                foreach (ProjectReference reference in projectData.ProjectReferences.ReferencedProjectNames)
                {
                    Project subProject = solution.getProject(reference.ProjectName);
                    if (subProject != null)
                    {
                        subProject.unloadScene(anomalyController);
                    }
                    else
                    {
                        Log.Error("Could not find referenced project {0} for project {1}. Project could not be destroyed.", reference.ProjectName, Name);
                    }
                }
            }
        }

        public void build()
        {
            if (projectData.CreateSceneFile)
            {
                Log.ImportantInfo("Building project {0}", name);
                ScenePackage scenePackage = new ScenePackage();
                scenePackage.ResourceManager = PluginManager.Instance.createEmptyResourceManager();
                scenePackage.ResourceManager.addResources(solution.GlobalResources);
                scenePackage.SceneDefinition = sceneFileInterface.getFileObject();
                scenePackage.SimObjectManagerDefinition = new SimObjectManagerDefinition();
                commonBuild(scenePackage);

                String sceneFileName = Path.GetFullPath(Path.Combine(workingDirectory, projectData.SceneFileName));
                try
                {
                    using (XmlTextWriter textWriter = new XmlTextWriter(sceneFileName, Encoding.Unicode))
                    {
                        textWriter.Formatting = Formatting.Indented;
                        xmlSaver.saveObject(scenePackage, textWriter);
                        Log.ImportantInfo("Finished building project {0} to {1}.", name, sceneFileName);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Could not save project {0} to file {1} because:\n{2}", name, sceneFileName, e.Message);
                }
            }
        }

        public void commonBuild(ScenePackage scenePackage)
        {
            scenePackage.ResourceManager.addResources(resourceFileInterface.getFileObject());
            instanceGroup.buildInstances(scenePackage);

            foreach (ProjectReference reference in projectData.ProjectReferences.ReferencedProjectNames)
            {
                Project refProject = solution.getProject(reference.ProjectName);
                if (refProject != null)
                {
                    Log.ImportantInfo("Building referenced project {0}.", refProject.Name);
                    refProject.commonBuild(scenePackage);
                }
                else
                {
                    Log.Error("Could not find referenced project {0} for project {1}. Project could not be built.", reference.ProjectName, Name);
                }
            }
        }

        internal void getResources(ResourceManager resourceManager)
        {
            resourceManager.addResources(resourceFileInterface.getFileObject());
        }

        public void save()
        {
            using (XmlTextWriter textWriter = new XmlTextWriter(projectFile, Encoding.Unicode))
            {
                textWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(projectData, textWriter);
            }
            sceneFileInterface.save();
            resourceFileInterface.save();
            instanceGroup.save();
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public String WorkingDirectory
        {
            get
            {
                return workingDirectory;
            }
        }

        [Editable]
        public bool CreateSceneFile
        {
            get
            {
                return projectData.CreateSceneFile;
            }
            set
            {
                projectData.CreateSceneFile = value;
            }
        }

        [Editable]
        public String SceneFileName
        {
            get
            {
                return projectData.SceneFileName;
            }
            set
            {
                projectData.SceneFileName = value;
            }
        }
    }

    public partial class Project
    {
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, ReflectedEditInterface.DefaultScanner, name, null);// new EditInterface(name);
                editInterface.IconReferenceTag = AnomalyIcons.Project;

                editInterface.addSubInterface(projectData.ProjectReferences.getEditInterface());
                editInterface.addSubInterface(resourceFileInterface.getEditInterface());
                editInterface.addSubInterface(sceneFileInterface.getEditInterface());

                editInterface.addSubInterface(instanceGroup.getEditInterface());
            }

            return editInterface;
        }
    }
}

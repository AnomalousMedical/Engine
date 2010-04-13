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
        private bool built = false;
        private ResourceManagerFileInterface resourceFileInterface;
        private SimSceneFileInterface sceneFileInterface;
        private ProjectReferenceManager projectReferences;
        private Solution solution;
        private String projectFile;

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
                        projectReferences = xmlSaver.restoreObject(textReader) as ProjectReferenceManager;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Could not load project references file {0} because:\n{1}", projectFile, e.Message);
                }
            }
            if(projectReferences == null)
            {
                projectReferences = new ProjectReferenceManager();
            }

            String resourcesFile = Path.Combine(workingDirectory, "Resources.xml");
            if (!File.Exists(resourcesFile))
            {
                ResourceManager resources = PluginManager.Instance.createEmptyResourceManager();
                using (XmlTextWriter textWriter = new XmlTextWriter(resourcesFile, Encoding.Default))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(resources, textWriter);
                }
            }

            String sceneDefinitionFile = Path.Combine(workingDirectory, "SceneDefinition.xml");
            if (!File.Exists(sceneDefinitionFile))
            {
                SimSceneDefinition sceneDefinition = new SimSceneDefinition();
                using (XmlTextWriter textWriter = new XmlTextWriter(sceneDefinitionFile, Encoding.Default))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(sceneDefinition, textWriter);
                }
            }

            resourceFileInterface = new ResourceManagerFileInterface("Resources", EngineIcons.Resources, resourcesFile);
            sceneFileInterface = new SimSceneFileInterface("Scene Definition", EngineIcons.Scene, sceneDefinitionFile);
        }

        public void buildScene(AnomalyController anomalyController)
        {
            if (!built)
            {
                anomalyController.SceneController.setSceneDefinition(sceneFileInterface.getFileObject());

                SimObjectManagerDefinition simObjectManager = new SimObjectManagerDefinition();
                anomalyController.SimObjectController.setSceneManagerDefintion(simObjectManager);

                commonBuild(anomalyController);
            }
        }

        private void commonBuild(AnomalyController anomalyController)
        {
            if (!built)
            {
                if (editInterface != null)
                {
                    editInterface.IconReferenceTag = AnomalyIcons.ProjectBuilt;
                }

                built = true;

                anomalyController.ResourceController.addResources(resourceFileInterface.getFileObject());

                instanceGroup.buildInstances(anomalyController.SimObjectController);

                foreach (ProjectReference reference in projectReferences.ReferencedProjectNames)
                {
                    Project subProject = solution.getProject(reference.ProjectName);
                    if (subProject != null)
                    {
                        subProject.commonBuild(anomalyController);
                    }
                    else
                    {
                        Log.Error("Could not find referenced project {0} for project {1}. Project could not be built.", reference.ProjectName, Name);
                    }
                }
            }
        }

        public void unloadScene(AnomalyController anomalyController)
        {
            if (built)
            {
                if (editInterface != null)
                {
                    editInterface.IconReferenceTag = AnomalyIcons.Project;
                }

                built = false;
                instanceGroup.destroyInstances(anomalyController.SimObjectController);

                foreach (ProjectReference reference in projectReferences.ReferencedProjectNames)
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

        public void save()
        {
            using (XmlTextWriter textWriter = new XmlTextWriter(projectFile, Encoding.Default))
            {
                textWriter.Formatting = Formatting.Indented;
                xmlSaver.saveObject(projectReferences, textWriter);
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
    }

    public partial class Project
    {
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.IconReferenceTag = AnomalyIcons.Project;

                editInterface.addSubInterface(projectReferences.getEditInterface());
                editInterface.addSubInterface(resourceFileInterface.getEditInterface());
                editInterface.addSubInterface(sceneFileInterface.getEditInterface());

                editInterface.addSubInterface(instanceGroup.getEditInterface());
            }

            return editInterface;
        }
    }
}

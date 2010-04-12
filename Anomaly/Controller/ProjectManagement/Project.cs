﻿using System;
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

namespace Anomaly
{
    public partial class Project
    {
        private static XmlSaver xmlSaver = new XmlSaver();

        private String name;
        private InstanceGroup instanceGroup;
        private String workingDirectory;
        private String resourcesFile;
        private String sceneDefinitionFile;
        private bool built = false;

        public Project(String name, String workingDirectory)
        {
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

            resourcesFile = Path.Combine(workingDirectory, "Resources.xml");
            if (!File.Exists(resourcesFile))
            {
                ResourceManager resources = PluginManager.Instance.createEmptyResourceManager();
                using (XmlTextWriter textWriter = new XmlTextWriter(resourcesFile, Encoding.Default))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(resources, textWriter);
                }
            }

            sceneDefinitionFile = Path.Combine(workingDirectory, "SceneDefinition.xml");
            if (!File.Exists(sceneDefinitionFile))
            {
                SimSceneDefinition sceneDefinition = new SimSceneDefinition();
                using (XmlTextWriter textWriter = new XmlTextWriter(sceneDefinitionFile, Encoding.Default))
                {
                    textWriter.Formatting = Formatting.Indented;
                    xmlSaver.saveObject(sceneDefinition, textWriter);
                }
            }
        }

        public void buildScene(AnomalyController anomalyController)
        {
            built = true;

            anomalyController.ResourceController.addResources(loadResourceManager());
            anomalyController.SceneController.setSceneDefinition(loadSceneDefinition());

            SimObjectManagerDefinition simObjectManager = new SimObjectManagerDefinition();
            anomalyController.SimObjectController.setSceneManagerDefintion(simObjectManager);
            instanceGroup.buildInstances(anomalyController.SimObjectController);
        }

        public void unloadScene(AnomalyController anomalyController)
        {
            built = false;
            instanceGroup.destroyInstances(anomalyController.SimObjectController);
        }

        public void save()
        {
            instanceGroup.save();
        }

        private SimSceneDefinition loadSceneDefinition()
        {
            using (XmlTextReader textReader = new XmlTextReader(sceneDefinitionFile))
            {
                return xmlSaver.restoreObject(textReader) as SimSceneDefinition;
            }
        }

        private ResourceManager loadResourceManager()
        {
            using (XmlTextReader textReader = new XmlTextReader(resourcesFile))
            {
                return xmlSaver.restoreObject(textReader) as ResourceManager;
            }
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
        private ResourceManagerFileInterface resourceFileInterface;
        private SimSceneFileInterface sceneFileInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.IconReferenceTag = AnomalyIcons.Project;

                resourceFileInterface = new ResourceManagerFileInterface("Resources", null, resourcesFile);
                sceneFileInterface = new SimSceneFileInterface("Scene Definition", null, sceneDefinitionFile);

                editInterface.addSubInterface(resourceFileInterface.getEditInterface());
                editInterface.addSubInterface(sceneFileInterface.getEditInterface());

                editInterface.addSubInterface(instanceGroup.getEditInterface());
            }

            return editInterface;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using System.IO;
using Engine.Saving;
using System.Windows.Forms;
using Logging;
using Editor;
using System.Xml;

namespace Anomaly
{
    class InstanceGroup : IDisposable
    {
        private static CopySaver copySaver = new CopySaver();

        private String name;
        private Dictionary<String, InstanceGroup> groups = new Dictionary<string, InstanceGroup>();
        private Dictionary<String, InstanceFileInterface> instanceFiles = new Dictionary<String, InstanceFileInterface>();
        private String path;
        private bool instancesDisplayed = false; //True if instances for this group are currently in the scene.
        private SimObjectController simObjectController;

        private List<InstanceGroup> deletedGroups = new List<InstanceGroup>();
        private List<InstanceFileInterface> deletedInstances = new List<InstanceFileInterface>();

        public InstanceGroup(String name, String path)
        {
            this.name = name;
            this.path = path;
        }

        public void Dispose()
        {
            foreach (InstanceGroup group in groups.Values)
            {
                group.Dispose();
            }

            foreach (InstanceFileInterface fileInterface in instanceFiles.Values)
            {
                fileInterface.Dispose();
            }
        }

        public void addGroup(InstanceGroup group)
        {
            groups.Add(group.name, group);
            if (instancesDisplayed)
            {
                group.buildInstances(simObjectController);
            }
            if (editInterface != null)
            {
                addGroupSubInterface(group);
            }
        }

        public void removeGroup(InstanceGroup group)
        {
            groups.Remove(group.name);
            group.Dispose();
            deletedGroups.Add(group);
            if (editInterface != null)
            {
                groupManager.removeSubInterface(group);
            }
        }

        /// <summary>
        /// Add a instance and save the defintion.
        /// </summary>
        /// <param name="simObject"></param>
        public void addInstanceFile(String instanceName)
        {
            InstanceFileInterface fileInterface = new InstanceFileInterface(instanceName, AnomalyIcons.Instance, InstanceWriter.Instance.getInstanceFileName(this, instanceName));
            if (instancesDisplayed)
            {
                fileInterface.createInstance(simObjectController);
            }
            instanceFiles.Add(instanceName, fileInterface);
            onInstanceFileAdded(fileInterface);
        }

        public void addInstance(Instance instance)
        {
            InstanceFileInterface fileInterface = new InstanceFileInterface(instance.Name, AnomalyIcons.Instance, InstanceWriter.Instance.getInstanceFileName(this, instance.Name), instance);
            if (instancesDisplayed)
            {
                fileInterface.createInstance(simObjectController);
            }
            instanceFiles.Add(instance.Name, fileInterface);
            onInstanceFileAdded(fileInterface);
        }

        public void removeInstanceFile(String instanceName)
        {
            InstanceFileInterface fileInterface = instanceFiles[instanceName];
            fileInterface.Dispose();
            instanceFiles.Remove(instanceName);
            deletedInstances.Add(fileInterface);
            onInstanceFileRemoved(fileInterface);
        }

        public void buildInstances(SimObjectController simObjectController)
        {
            this.simObjectController = simObjectController;
            this.instancesDisplayed = true;

            foreach (InstanceFileInterface instanceFileInterface in instanceFiles.Values)
            {
                instanceFileInterface.createInstance(simObjectController);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.buildInstances(simObjectController);
            }
        }

        public void destroyInstances(SimObjectController simObjectController)
        {
            this.simObjectController = null;
            this.instancesDisplayed = false;

            foreach (InstanceFileInterface instanceFileInterface in instanceFiles.Values)
            {
                instanceFileInterface.destroyInstance(simObjectController);
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.destroyInstances(simObjectController);
            }
        }

        public void save()
        {
            InstanceWriter.Instance.addInstanceGroup(this);
            foreach (InstanceGroup group in deletedGroups)
            {
                InstanceWriter.Instance.removeInstanceGroup(group);
            }
            deletedGroups.Clear();

            foreach (InstanceFileInterface instanceFile in deletedInstances)
            {
                instanceFile.save();
            }
            deletedInstances.Clear();

            foreach (InstanceFileInterface instanceFile in instanceFiles.Values)
            {
                instanceFile.save();
            }

            foreach (InstanceGroup group in groups.Values)
            {
                group.save();
            }
        }

        #region Properties

        public String Name
        {
            get
            {
                return name;
            }
        }

        public String FullPath
        {
            get
            {
                return path;
            }
        }

        #endregion Properties

        #region EditInterface

        private EditInterfaceCommand destroyGroup;
        private EditInterfaceCommand destroySimObject;
        private EditInterfaceManager<InstanceGroup> groupManager;
        private EditInterfaceManager<InstanceFileInterface> instanceFileManager;
        private EditInterface editInterface;
        private EditInterfaceCommand duplicateSimObject;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                destroyGroup = new EditInterfaceCommand("Remove", destroyGroupCallback);
                destroySimObject = new EditInterfaceCommand("Remove", destroySimObjectCallback);
                duplicateSimObject = new EditInterfaceCommand("Duplicate", duplicateSimObjectCallback);
                editInterface = new EditInterface(name);
                editInterface.addCommand(new EditInterfaceCommand("Create Group", createGroupCallback));
                editInterface.addCommand(new EditInterfaceCommand("Create Sim Object", createSimObjectCallback));
                editInterface.addCommand(new EditInterfaceCommand("Import legacy templates", importTemplates));
                editInterface.IconReferenceTag = EditorIcons.Folder;
                groupManager = new EditInterfaceManager<InstanceGroup>(editInterface);
                instanceFileManager = new EditInterfaceManager<InstanceFileInterface>(editInterface);
                foreach (InstanceGroup group in groups.Values)
                {
                    addGroupSubInterface(group);
                }
                foreach (InstanceFileInterface file in instanceFiles.Values)
                {
                    onInstanceFileAdded(file);
                }
            }
            return editInterface;
        }

        private void addGroupSubInterface(InstanceGroup group)
        {
            EditInterface edit = group.getEditInterface();
            edit.addCommand(destroyGroup);
            groupManager.addSubInterface(group, edit);
        }

        private void createGroupCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateGroupCreate);
            if (accept)
            {
                InstanceGroup group = new InstanceGroup(name, Path.Combine(path, name));
                this.addGroup(group);
            }
        }

        private bool validateGroupCreate(String input, out String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (this.groups.ContainsKey(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        private void destroyGroupCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            removeGroup(groupManager.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        private void onInstanceFileAdded(InstanceFileInterface fileInterface)
        {
            if (editInterface != null)
            {
                EditInterface edit = fileInterface.getEditInterface();
                edit.addCommand(destroySimObject);
                edit.addCommand(duplicateSimObject);
                instanceFileManager.addSubInterface(fileInterface, edit);
            }
        }

        private void onInstanceFileRemoved(InstanceFileInterface fileInterface)
        {
            if (editInterface != null)
            {
                instanceFileManager.removeSubInterface(fileInterface);
            }
        }

        private void createSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateSimObjectCreate);
            if (accept)
            {
                SimObjectDefinition simObject = new GenericSimObjectDefinition(name);
                createInstance(simObject);
            }
        }

        private bool validateSimObjectCreate(String input, out String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (this.instanceFiles.ContainsKey(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        private void destroySimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            EditInterface editInterface = callback.getSelectedEditInterface();
            if (editInterface.hasEditableProperties())
            {
                InstanceFileInterface file = editInterface.getEditableProperties().First() as InstanceFileInterface;
                file.Deleted = true;
                InstanceFileInterface instanceFile = instanceFileManager.resolveSourceObject(editInterface);
                removeInstanceFile(instanceFile.Name);
            }
        }

        private void duplicateSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateDuplicateSimObject);
            if (accept)
            {
                InstanceFileInterface instanceFile = callback.getSelectedEditInterface().getEditableProperties().First() as InstanceFileInterface;
                SimObjectDefinition sourceObject = instanceFile.Instance.Definition;
                SimObjectDefinition simObject = copySaver.copyObject(sourceObject) as SimObjectDefinition;
                simObject.Name = name;
                createInstance(simObject);
            }
        }

        private bool validateDuplicateSimObject(String input, out String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (this.instanceFiles.ContainsKey(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        private void createInstance(SimObjectDefinition simObject)
        {
            Instance instance = new Instance(simObject.Name, simObject);
            this.addInstance(instance);
            instanceFiles[instance.Name].Modified = true;
        }

        private void importTemplates(EditUICallback callback, EditInterfaceCommand command)
        {
            String folder;
            bool accept = callback.showFolderBrowserDialog(out folder);
            if (accept)
            {
                doImportTemplates(folder);
            }
        }

        private void doImportTemplates(String directory)
        {
            foreach (String template in Directory.GetFiles(directory, "*.tpl", SearchOption.TopDirectoryOnly))
            {
                createInstance(InstanceWriter.Instance.loadTemplate(template));
            }
            foreach (String groupDir in Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(groupDir);
                if ((dirInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    String name = dirInfo.Name;
                    if (!groups.ContainsKey(groupDir))
                    {
                        InstanceGroup group = new InstanceGroup(name, Path.Combine(path, name));
                        addGroup(group);
                    }
                    groups[name].doImportTemplates(groupDir);
                }
            }
        }

        #endregion
    }
}

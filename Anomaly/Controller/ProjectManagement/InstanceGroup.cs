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
    class InstanceGroup
    {
        private static CopySaver copySaver = new CopySaver();

        private String name;
        private Dictionary<String, InstanceGroup> groups = new Dictionary<string, InstanceGroup>();
        private List<String> instanceFiles = new List<String>();
        private String path;

        public InstanceGroup(String name, String path)
        {
            this.name = name;
            this.path = path;
        }

        public void addGroup(InstanceGroup group)
        {
            groups.Add(group.name, group);
            InstanceWriter.Instance.addInstanceGroup(group);
            if (editInterface != null)
            {
                addGroupSubInterface(group);
            }
        }

        public void removeGroup(InstanceGroup group)
        {
            groups.Remove(group.name);
            InstanceWriter.Instance.removeInstanceGroup(group);
            if (editInterface != null)
            {
                groupManager.removeSubInterface(group);
            }
        }

        /// <summary>
        /// Add a instance and save the defintion.
        /// </summary>
        /// <param name="simObject"></param>
        public void addInstanceFile(String instanceFile)
        {
            instanceFiles.Add(instanceFile);
            onInstanceFileAdded(instanceFile);
        }

        public void removeInstanceFile(String instanceFile)
        {
            instanceFiles.Remove(instanceFile);
            onInstanceFileRemoved(instanceFile);
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
        private EditInterfaceManager<String> instanceFileManager;
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
                instanceFileManager = new EditInterfaceManager<String>(editInterface);
                foreach (InstanceGroup group in groups.Values)
                {
                    addGroupSubInterface(group);
                }
                foreach (String file in instanceFiles)
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

        private void onInstanceFileAdded(String instanceName)
        {
            if (editInterface != null)
            {
                InstanceFileInterface fileInterface = new InstanceFileInterface(instanceName, AnomalyIcons.Instance, InstanceWriter.Instance.getInstanceFileName(this, instanceName));
                EditInterface edit = fileInterface.getEditInterface();
                edit.addCommand(destroySimObject);
                edit.addCommand(duplicateSimObject);
                instanceFileManager.addSubInterface(instanceName, edit);
            }
        }

        private void onInstanceFileRemoved(string instanceFile)
        {
            if (editInterface != null)
            {
                instanceFileManager.removeSubInterface(instanceFile);
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
            if (this.instanceFiles.Contains(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        private void destroySimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String instanceFile = instanceFileManager.resolveSourceObject(callback.getSelectedEditInterface());
            InstanceWriter.Instance.deleteInstance(this, instanceFile);
            removeInstanceFile(instanceFile);
        }

        private void duplicateSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateDuplicateSimObject);
            if (accept)
            {
                InstanceFileInterface instanceFile = callback.getSelectedEditInterface().getEditableProperties().First() as InstanceFileInterface;
                SimObjectDefinition sourceObject = instanceFile.getFileObject().Definition;
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
            if (this.instanceFiles.Contains(input))
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
            if (InstanceWriter.Instance.saveInstance(this, instance))
            {
                this.addInstanceFile(simObject.Name);
            }
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

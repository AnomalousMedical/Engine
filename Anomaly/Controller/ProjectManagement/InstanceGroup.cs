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

namespace Anomaly
{
    class InstanceGroup
    {
        private static CopySaver copySaver = new CopySaver();

        private String name;
        private Dictionary<String, InstanceGroup> groups = new Dictionary<string, InstanceGroup>();
        private Dictionary<String, Instance> instances = new Dictionary<string, Instance>();
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
        public void addSimObject(SimObjectDefinition simObject)
        {
            Instance instance = new Instance(simObject.Name, simObject);
            instances.Add(simObject.Name, instance);
            InstanceWriter.Instance.saveInstance(this, instance);
            if (editInterface != null)
            {
                addSimObjectSubInterface(instance);
            }
        }

        /// <summary>
        /// This function adds a SimObject but does not save the instance. Used
        /// only to reload a instance from a file.
        /// </summary>
        /// <param name="simObject"></param>
        public void addExistingSimObject(SimObjectDefinition simObject)
        {
            Instance instance = new Instance(simObject.Name, simObject);
            instances.Add(simObject.Name, instance);
            if (editInterface != null)
            {
                addSimObjectSubInterface(instance);
            }
        }

        public void removeSimObject(SimObjectDefinition simObject)
        {
            Instance instance = instances[simObject.Name];
            instances.Remove(simObject.Name);
            InstanceWriter.Instance.deleteInstance(this, instance);
            if (editInterface != null)
            {
                instanceManager.removeSubInterface(instance);
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
        private EditInterfaceManager<Instance> instanceManager;
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
                editInterface.IconReferenceTag = EditorIcons.Folder;
                groupManager = new EditInterfaceManager<InstanceGroup>(editInterface);
                instanceManager = new EditInterfaceManager<Instance>(editInterface);
                foreach (InstanceGroup group in groups.Values)
                {
                    addGroupSubInterface(group);
                }
                foreach (Instance definition in instances.Values)
                {
                    addSimObjectSubInterface(definition);
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

        private void addSimObjectSubInterface(Instance instance)
        {
            EditInterface edit = instance.getEditInterface();
            edit.addCommand(destroySimObject);
            edit.addCommand(duplicateSimObject);
            instanceManager.addSubInterface(instance, edit);
        }

        private void createSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateSimObjectCreate);
            if (accept)
            {
                SimObjectDefinition simObject = new GenericSimObjectDefinition(name);
                this.addSimObject(simObject);
            }
        }

        private bool validateSimObjectCreate(String input, out String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (this.instances.ContainsKey(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        private void destroySimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            removeSimObject(instanceManager.resolveSourceObject(callback.getSelectedEditInterface()).Definition);
        }

        private void duplicateSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateDuplicateSimObject);
            if (accept)
            {
                SimObjectDefinition sourceObject = instanceManager.resolveSourceObject(callback.getSelectedEditInterface()).Definition;
                SimObjectDefinition simObject = copySaver.copyObject(sourceObject) as SimObjectDefinition;
                simObject.Name = name;
                this.addSimObject(simObject);
            }
        }

        private bool validateDuplicateSimObject(String input, out String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (this.instances.ContainsKey(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        #endregion
    }
}

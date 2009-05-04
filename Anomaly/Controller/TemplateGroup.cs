using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;

namespace Anomaly
{
    class TemplateGroup
    {
        private String name;
        private Dictionary<String, TemplateGroup> groups = new Dictionary<string,TemplateGroup>();
        private Dictionary<String, SimObjectDefinition> simObjects = new Dictionary<string, SimObjectDefinition>();

        public TemplateGroup(String name)
        {
            this.name = name;
        }

        public void addGroup(TemplateGroup group)
        {
            groups.Add(group.name, group);
            if (editInterface != null)
            {
                addGroupSubInterface(group);
            }
        }

        public void removeGroup(TemplateGroup group)
        {
            groups.Remove(group.name);
            if (editInterface != null)
            {
                groupManager.removeSubInterface(group);
            }
        }

        public void addSimObject(SimObjectDefinition simObject)
        {
            simObjects.Add(simObject.Name, simObject);
            if (editInterface != null)
            {
                addSimObjectSubInterface(simObject);
            }
        }

        public void removeSimObject(SimObjectDefinition simObject)
        {
            simObjects.Remove(simObject.Name);
            if (editInterface != null)
            {
                simObjectDefinitionManager.removeSubInterface(simObject);
            }
        }

        #region EditInterface

        private EditInterfaceCommand destroyGroup;
        private EditInterfaceCommand destroySimObject;
        private EditInterfaceManager<TemplateGroup> groupManager;
        private EditInterfaceManager<SimObjectDefinition> simObjectDefinitionManager;
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                destroyGroup = new EditInterfaceCommand("Remove", destroyGroupCallback);
                destroySimObject = new EditInterfaceCommand("Remove", destroySimObjectCallback);
                editInterface = new EditInterface(name);
                editInterface.addCommand(new EditInterfaceCommand("Create Group", createGroupCallback));
                editInterface.addCommand(new EditInterfaceCommand("Create Sim Object", createSimObjectCallback));
                groupManager = new EditInterfaceManager<TemplateGroup>(editInterface);
                simObjectDefinitionManager = new EditInterfaceManager<SimObjectDefinition>(editInterface);
            }
            return editInterface;
        }

        private void addGroupSubInterface(TemplateGroup group)
        {
            EditInterface edit = group.getEditInterface();
            edit.addCommand(destroyGroup);
            groupManager.addSubInterface(group, edit);
        }

        private void createGroupCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.groups.ContainsKey(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            if (accept)
            {
                TemplateGroup group = new TemplateGroup(name);
                this.addGroup(group);
            }
        }

        private void destroyGroupCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            removeGroup(groupManager.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        private void addSimObjectSubInterface(SimObjectDefinition simObject)
        {
            EditInterface edit = simObject.getEditInterface();
            edit.addCommand(destroySimObject);
            simObjectDefinitionManager.addSubInterface(simObject, edit);
        }

        private void createSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.groups.ContainsKey(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            if (accept)
            {
                SimObjectDefinition simObject = new GenericSimObjectDefinition(name);
                this.addSimObject(simObject);
            }
        }

        private void destroySimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            removeSimObject(simObjectDefinitionManager.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        #endregion
    }
}

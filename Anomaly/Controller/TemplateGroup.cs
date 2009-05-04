using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using System.IO;

namespace Anomaly
{
    class TemplateGroup
    {
        private String name;
        private Dictionary<String, TemplateGroup> groups = new Dictionary<string,TemplateGroup>();
        private Dictionary<String, SimObjectDefinition> simObjects = new Dictionary<string, SimObjectDefinition>();
        private TemplateGroup parentGroup;
        private TemplateWriter templateWriter;

        public TemplateGroup(String name, TemplateWriter templateWriter)
        {
            this.name = name;
            this.templateWriter = templateWriter;
        }

        public void addGroup(TemplateGroup group)
        {
            group.parentGroup = this;
            groups.Add(group.name, group);
            templateWriter.addTemplateGroup(group);
            if (editInterface != null)
            {
                addGroupSubInterface(group);
            }
        }

        public void removeGroup(TemplateGroup group)
        {
            group.parentGroup = null;
            groups.Remove(group.name);
            templateWriter.removeTemplateGroup(group);
            if (editInterface != null)
            {
                groupManager.removeSubInterface(group);
            }
        }

        public void addSimObject(SimObjectDefinition simObject)
        {
            simObjects.Add(simObject.Name, simObject);
            templateWriter.saveTemplate(this, simObject);
            if (editInterface != null)
            {
                addSimObjectSubInterface(simObject);
            }
        }

        public void removeSimObject(SimObjectDefinition simObject)
        {
            simObjects.Remove(simObject.Name);
            templateWriter.deleteTemplate(this, simObject);
            if (editInterface != null)
            {
                simObjectDefinitionManager.removeSubInterface(simObject);
            }
        }

        public void updateTemplate(SimObjectDefinition simObject)
        {
            if (simObjects.ContainsKey(simObject.Name))
            {
                templateWriter.updateTemplate(this, simObject);
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
                foreach (TemplateGroup group in groups.Values)
                {
                    addGroupSubInterface(group);
                }
                foreach (SimObjectDefinition definition in simObjects.Values)
                {
                    addSimObjectSubInterface(definition);
                }
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
                TemplateGroup group = new TemplateGroup(name, templateWriter);
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
            edit.OnEditInterfaceModified += templateModified;
            simObjectDefinitionManager.addSubInterface(simObject, edit);
        }

        void templateModified(EditInterface modified)
        {
            updateTemplate(simObjectDefinitionManager.resolveSourceObject(modified));
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

        public String FullPath
        {
            get
            {
                if (parentGroup != null)
                {
                    return parentGroup.FullPath + Path.DirectorySeparatorChar + name;
                }
                else
                {
                    return name;
                }
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        #endregion
    }
}

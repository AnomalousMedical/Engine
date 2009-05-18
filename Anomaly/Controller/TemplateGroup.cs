using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using System.IO;
using Engine.Saving;

namespace Anomaly
{
    class TemplateGroup
    {
        private static CopySaver copySaver = new CopySaver();

        private String name;
        private Dictionary<String, TemplateGroup> groups = new Dictionary<string,TemplateGroup>();
        private Dictionary<String, Template> templates = new Dictionary<string, Template>();
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
            Template template = new Template(simObject, templateWriter, this);
            templates.Add(simObject.Name, template);
            templateWriter.saveTemplate(this, simObject);
            if (editInterface != null)
            {
                addSimObjectSubInterface(template);
            }
        }

        public void removeSimObject(SimObjectDefinition simObject)
        {
            Template template = templates[simObject.Name];
            templates.Remove(simObject.Name);
            templateWriter.deleteTemplate(this, simObject);
            if (editInterface != null)
            {
                templateManager.removeSubInterface(template);
            }
        }

        public void updateTemplate(SimObjectDefinition simObject)
        {
            if (templates.ContainsKey(simObject.Name))
            {
                templateWriter.updateTemplate(this, simObject);
            }
        }

        #region Properties

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

        #endregion Properties

        #region EditInterface

        private EditInterfaceCommand destroyGroup;
        private EditInterfaceCommand destroySimObject;
        private EditInterfaceManager<TemplateGroup> groupManager;
        private EditInterfaceManager<Template> templateManager;
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
                groupManager = new EditInterfaceManager<TemplateGroup>(editInterface);
                templateManager = new EditInterfaceManager<Template>(editInterface);
                foreach (TemplateGroup group in groups.Values)
                {
                    addGroupSubInterface(group);
                }
                foreach (Template definition in templates.Values)
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

        private void addSimObjectSubInterface(Template template)
        {
            EditInterface edit = template.getEditInterface();
            edit.addCommand(destroySimObject);
            edit.addCommand(duplicateSimObject);
            templateManager.addSubInterface(template, edit);
        }

        private void createSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.templates.ContainsKey(name))
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
            removeSimObject(templateManager.resolveSourceObject(callback.getSelectedEditInterface()).Definition);
        }

        private void duplicateSimObjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name);
            while (accept && this.templates.ContainsKey(name))
            {
                accept = callback.getInputString("That name is already in use. Please provide another.", name, out name);
            }
            if (accept)
            {
                SimObjectDefinition sourceObject = templateManager.resolveSourceObject(callback.getSelectedEditInterface()).Definition;
                SimObjectDefinition simObject = copySaver.copyObject(sourceObject) as SimObjectDefinition;
                simObject.Name = name;
                this.addSimObject(simObject);
            }
        }

        #endregion
    }
}

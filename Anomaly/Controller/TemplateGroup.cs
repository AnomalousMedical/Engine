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
            templateWriter.removeTemplateGroup(group);
            group.parentGroup = null;
            groups.Remove(group.name);
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

        public SimObjectDefinition getTemplateFromPath(String[] elements, int index)
        {
            SimObjectDefinition definition = null;
            //If we are on the last element search for sim objects
            if (index == elements.Length)
            {
                Log.Default.sendMessage("Hit the end of the path before finding a file. The path is malformed", LogLevel.Warning, "Editor");
            }
            else if (index == elements.Length - 1)
            {
                if (templates.ContainsKey(elements[index]))
                {
                    definition = templates[elements[index]].Definition;
                }
                else
                {
                    Log.Default.sendMessage("Could not find template {0} in group {1}.", LogLevel.Error, "Editor", elements[index], FullPath);
                }
            }
            else
            {
                if (groups.ContainsKey(elements[index]))
                {
                    definition = groups[elements[index]].getTemplateFromPath(elements, index + 1);
                }
                else
                {
                    Log.Default.sendMessage("Could not find group {0} in group {1}.", LogLevel.Error, "Editor", elements[index], FullPath);
                }
            }
            return definition;
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
        private EditInterfaceCommand copyName;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                destroyGroup = new EditInterfaceCommand("Remove", destroyGroupCallback);
                destroySimObject = new EditInterfaceCommand("Remove", destroySimObjectCallback);
                duplicateSimObject = new EditInterfaceCommand("Duplicate", duplicateSimObjectCallback);
                copyName = new EditInterfaceCommand("Copy Full Name", copyNameCallback);
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
            edit.addCommand(copyName);
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

        private void copyNameCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            SimObjectDefinition sourceObject = templateManager.resolveSourceObject(callback.getSelectedEditInterface()).Definition;
            Clipboard.SetText(FullPath + Path.DirectorySeparatorChar + sourceObject.Name);
        }

        #endregion
    }
}

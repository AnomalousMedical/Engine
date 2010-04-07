using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;

namespace Anomaly
{
    class Template : EditableProperty
    {
        private EditInterface editInterface;
        private SimObjectDefinition definition;
        private TemplateWriter templateWriter;
        private TemplateGroup group;

        public Template(SimObjectDefinition definition, TemplateWriter writer, TemplateGroup group)
        {
            this.definition = definition;
            this.templateWriter = writer;
            this.group = group;
        }

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(definition.Name);
                editInterface.addEditableProperty(this);
                editInterface.IconReferenceTag = EngineIcons.SimObject;
            }
            return editInterface;
        }

        public void updated()
        {
            templateWriter.updateTemplate(group, definition);
        }

        public SimObjectDefinition Definition
        {
            get
            {
                return definition;
            }
        }

        #region EditableProperty Members

        public string getValue(int column)
        {
            return "";
        }

        public void setValueStr(int column, string value)
        {
            
        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        public Type getPropertyType(int column)
        {
            return typeof(object);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomaly
{
    public abstract class ObjectPlaceholderInterface : EditableProperty
    {
        private EditInterface editInterface;

        public ObjectPlaceholderInterface(String name, Object iconReferenceTag)
        {
            editInterface = new EditInterface(name);
            editInterface.IconReferenceTag = iconReferenceTag;
            editInterface.addEditableProperty(this);
        }

        public abstract object getObject();

        public abstract EditInterface getObjectEditInterface(Object obj);

        public abstract void uiFieldUpdateCallback(EditInterface editInterface, object editingObject);

        public abstract void uiEditingCompletedCallback(EditInterface editInterface, object editingObject);

        public EditInterface getEditInterface()
        {
            return editInterface;
        }

        public String Name
        {
            get
            {
                return editInterface.getName();
            }
        }

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
    }
}

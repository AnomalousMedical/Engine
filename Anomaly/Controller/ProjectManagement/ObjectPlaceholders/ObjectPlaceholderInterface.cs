﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomaly
{
    public abstract class ObjectPlaceholderInterface : EditableProperty
    {
        protected EditInterface editInterface;

        public ObjectPlaceholderInterface(String name, Object iconReferenceTag)
        {
            editInterface = new EditInterface(name);
            editInterface.IconReferenceTag = iconReferenceTag;
            editInterface.addEditableProperty(this);
        }

        public abstract object getObject();

        public abstract EditInterface getObjectEditInterface(Object obj);

        public abstract void uiFieldUpdateCallback(EditInterface editInterface, object editingObject);

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

        public Object getRealValue(int column)
        {
            return "";
        }

        public void setValue(int column, Object value)
        {
            
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

        public bool hasBrowser(int column)
        {
            return false;
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return null;
        }

        public bool readOnly(int column)
        {
            return false;
        }

        /// <summary>
        /// Set this to true to indicate to the ui that this property is advanced.
        /// </summary>
        public bool Advanced { get; set; }
    }
}

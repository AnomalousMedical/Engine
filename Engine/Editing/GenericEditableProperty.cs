using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class GenericEditableProperty<T> : EditableProperty
    {
        private T obj;
        private String name;

        public GenericEditableProperty(String name, T obj)
        {
            this.name = name;
            this.obj = obj;
        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            errorMessage = null;
            return false;
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return null;
        }

        public Type getPropertyType(int column)
        {
            switch (column)
            {
                case 0:
                    return typeof(String);
                case 1:
                    return typeof(T);
            }
            return null;
        }

        public object getRealValue(int column)
        {
            switch (column)
            {
                case 0:
                    return name;
                case 1:
                    return obj;
            }
            return null;
        }

        public string getValue(int column)
        {
            switch (column)
            {
                case 0:
                    return name;
                case 1:
                    if (obj != null)
                    {
                        return obj.ToString();
                    }
                    return null;
            }
            return null;
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        public void setValue(int column, object value)
        {
            switch (column)
            {
                case 0:
                    name = (String)value;
                    break;
                case 1:
                    value = (T)value;
                    break;
            }
        }

        public void setValueStr(int column, string value)
        {
            switch (column)
            {
                case 0:
                    name = value;
                    break;
                case 1:
                    //value = (T)value;
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    public class CallbackEditableProperty<T> : EditableProperty
    {
        private String name;
        private Func<T> getGenericValue;
        private Action<T> setGenericValue;
        private Func<String, bool> canParseStringToValue;
        private Func<String, T> parseStringToValue;

        public CallbackEditableProperty(String name, Func<T> getGenericValue, Action<T> setGenericValue, Func<String, bool> canParseStringToValue, Func<String, T> parseStringToValue)
        {
            this.name = name;
            this.getGenericValue = getGenericValue;
            this.setGenericValue = setGenericValue;
            this.canParseStringToValue = canParseStringToValue;
            this.parseStringToValue = parseStringToValue;
        }

        public bool Advanced
        {
            get
            {
                return false;
            }
        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            if (canParseStringToValue(value))
            {
                errorMessage = null;
                return true;
            }
            errorMessage = "Cannot parse.";
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
                default:
                    return typeof(String);
            }
        }

        public object getRealValue(int column)
        {
            switch (column)
            {
                case 0:
                    return name;
                case 1:
                    return getGenericValue();
                default:
                    return null;
            }
        }

        public string getValue(int column)
        {
            switch (column)
            {
                case 0:
                    return name;
                case 1:
                    return getGenericValue().ToString();
                default:
                    return null;
            }
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        public bool readOnly(int column)
        {
            return column == 0;
        }

        public void setValue(int column, object value)
        {
            if (column == 1)
            {
                setGenericValue((T)value);
            }
        }

        public void setValueStr(int column, string value)
        {
            if (column == 1)
            {
                setGenericValue(parseStringToValue(value));
            }
        }
    }
}

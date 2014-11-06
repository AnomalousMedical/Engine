using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    class ReflectedListItemEditableProperty<T> : EditableProperty
    {
        private EditInterface rowEditInterface;

        public ReflectedListItemEditableProperty(EditInterface rowEditInterface)
        {
            this.rowEditInterface = rowEditInterface;
        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            //These are always reflected edit interfaces, so we use column 1.
            return rowEditInterface.getEditablePropertyAt(column).canParseString(1, value, out errorMessage);
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return rowEditInterface.getEditablePropertyAt(column).getBrowser(1, uiCallback);
        }

        public Type getPropertyType(int column)
        {
            return rowEditInterface.getEditablePropertyAt(column).getPropertyType(1);
        }

        public object getRealValue(int column)
        {
            return rowEditInterface.getEditablePropertyAt(column).getRealValue(1);
        }

        public string getValue(int column)
        {
            return rowEditInterface.getEditablePropertyAt(column).getValue(1);
        }

        public bool hasBrowser(int column)
        {
            return rowEditInterface.getEditablePropertyAt(column).hasBrowser(1);
        }

        public bool readOnly(int column)
        {
            return rowEditInterface.getEditablePropertyAt(column).readOnly(1);
        }

        public void setValue(int column, object value)
        {
            rowEditInterface.getEditablePropertyAt(column).setValue(1, value);
        }

        public void setValueStr(int column, string value)
        {
            rowEditInterface.getEditablePropertyAt(column).setValueStr(1, value);
        }

        /// <summary>
        /// Set this to true to indicate to the ui that this property is advanced.
        /// </summary>
        public bool Advanced { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Editing
{
    class ListItemEditableProperty<T> : EditableProperty
    {
        private int index;
        private ListlikeEditInterface<T> editInterface;
        private Func<EditUICallback, Browser> getBrowserFunc;

        public ListItemEditableProperty(int index, ListlikeEditInterface<T> editInterface, Func<EditUICallback, Browser> getBrowser)
        {
            this.index = index;
            this.editInterface = editInterface;
            this.getBrowserFunc = getBrowser;
        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            return editInterface.canParseString(value, out errorMessage);
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            if(getBrowserFunc != null)
            {
                return getBrowserFunc(uiCallback);
            }
            return null;
        }

        public Type getPropertyType(int column)
        {
            switch (column)
            {
                case 0:
                    return typeof(T);
            }
            return null;
        }

        public object getRealValue(int column)
        {
            switch (column)
            {
                case 0:
                    return editInterface[index];
            }
            return null;
        }

        public string getValue(int column)
        {
            switch (column)
            {
                case 0:
                    T item = editInterface[index];
                    if (item != null)
                    {
                        return item.ToString();
                    }
                    return null;
            }
            return null;
        }

        public bool hasBrowser(int column)
        {
            return getBrowserFunc != null;
        }

        public bool readOnly(int column)
        {
            return column != 0;
        }

        public void setValue(int column, object value)
        {
            switch (column)
            {
                case 0:
                    editInterface[index] = (T)value;
                    break;
            }
        }

        public void setValueStr(int column, string value)
        {
            switch (column)
            {
                case 0:
                    editInterface[index] = editInterface.parseString(value);
                    break;
            }
        }

        /// <summary>
        /// Set this to true to indicate to the ui that this property is advanced.
        /// </summary>
        public bool Advanced { get; set; }

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }
    }
}

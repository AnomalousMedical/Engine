using Engine.Editing;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class BehaviorUpdatePhase : Saveable, EditableProperty
    {
        public BehaviorUpdatePhase()
        {

        }

        public BehaviorUpdatePhase(String name)
        {
            this.Name = name;
        }

        public String Name { get; set; }

        public string getValue(int column)
        {
            return Name;
        }

        public object getRealValue(int column)
        {
            return Name;
        }

        public void setValue(int column, object value)
        {
            Name = (String)value;
        }

        public void setValueStr(int column, string value)
        {
            Name = value;
        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        public Type getPropertyType(int column)
        {
            return typeof(String);
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return null;
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        public bool readOnly(int column)
        {
            return false;
        }

        public bool Advanced
        {
            get
            {
                return false;
            }
        }

        protected BehaviorUpdatePhase(LoadInfo info)
        {
            Name = info.GetString("Name");
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue("Name", Name);
        }
    }
}

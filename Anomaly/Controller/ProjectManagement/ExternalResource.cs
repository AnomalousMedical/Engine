using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;

namespace Anomaly
{
    public class ExternalResource : EditableProperty, Saveable
    {
        public ExternalResource(String path)
        {
            this.Path = path;
        }

        public String Path { get; set; }

        #region EditableProperty Members

        public string getValue(int column)
        {
            return Path;
        }

        public Object getRealValue(int column)
        {
            return Path;
        }

        public void setValue(int column, Object value)
        {
            Path = (String)value;
        }

        public void setValueStr(int column, string value)
        {
            Path = value;
        }

        public bool canParseString(int column, string value, out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public Type getPropertyType(int column)
        {
            return typeof(String);
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return null;
        }

        #endregion

        #region Saveable Members

        private const String PATH = "Path";

        protected ExternalResource(LoadInfo info)
        {
            Path = info.GetString(PATH);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(PATH, Path);
        }

        #endregion
    }
}

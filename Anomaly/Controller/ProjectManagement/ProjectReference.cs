using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;

namespace Anomaly
{
    public class ProjectReference : EditableProperty, Saveable
    {
        public ProjectReference(String projectName)
        {
            this.ProjectName = projectName;
        }

        public String ProjectName { get; set; }

        #region EditableProperty Members

        public string getValue(int column)
        {
            return ProjectName;
        }

        public Object getRealValue(int column)
        {
            return ProjectName;
        }

        public void setValue(int column, Object value)
        {
            ProjectName = (String)value;
        }

        public void setValueStr(int column, string value)
        {
            ProjectName = value;
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

        public Browser getBrowser(int column)
        {
            return null;
        }

        #endregion

        #region Saveable Members

        private const String PROJECT_NAME = "ProjectName";

        protected ProjectReference(LoadInfo info)
        {
            ProjectName = info.GetString(PROJECT_NAME);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(PROJECT_NAME, ProjectName);
        }

        #endregion
    }
}

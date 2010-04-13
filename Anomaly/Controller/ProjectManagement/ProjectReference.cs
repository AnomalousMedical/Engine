using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Anomaly
{
    public class ProjectReference : EditableProperty
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

        #endregion
    }
}

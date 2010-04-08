using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using System.IO;

namespace Anomaly
{
    public partial class Project
    {
        private String name;
        private InstanceGroup instanceGroup;
        private String path;

        public Project(String name, String path)
        {
            this.name = name;
            this.path = path;
            instanceGroup = new InstanceGroup("Instances", Path.Combine(path, "Instances"));
            InstanceWriter.Instance.addInstanceGroup(instanceGroup);
        }

        public String Name
        {
            get
            {
                return name;
            }
        }
    }

    public partial class Project
    {
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.IconReferenceTag = AnomalyIcons.Project;
                editInterface.addSubInterface(instanceGroup.getEditInterface());
            }

            return editInterface;
        }
    }
}

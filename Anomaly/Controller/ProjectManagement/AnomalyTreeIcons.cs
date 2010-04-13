using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using Anomaly.Properties;

namespace Anomaly
{
    enum AnomalyIcons
    {
        Project,
        ProjectBuilt,
        ReferencedProjects,
        Solution,
        Instance,
        InstanceModified,
        InstanceModifiedHidden,
        InstanceHidden,
    }

    class AnomalyTreeIcons
    {
        private AnomalyTreeIcons()
        {

        }

        static public void createIcons()
        {
            EditInterfaceIconCollection.addIcon(AnomalyIcons.Solution, Resources.Solution);
            EditInterfaceIconCollection.addIcon(AnomalyIcons.Project, Resources.Project);
            EditInterfaceIconCollection.addIcon(AnomalyIcons.Instance, Resources.Instance);
            EditInterfaceIconCollection.addIcon(AnomalyIcons.ProjectBuilt, Resources.ProjectBuilt);
            EditInterfaceIconCollection.addIcon(AnomalyIcons.ReferencedProjects, Resources.ReferencedProjects);
            EditInterfaceIconCollection.addIcon(AnomalyIcons.InstanceModified, Resources.InstanceModified);
            EditInterfaceIconCollection.addIcon(AnomalyIcons.InstanceModifiedHidden, Resources.InstanceModifiedHidden);
            EditInterfaceIconCollection.addIcon(AnomalyIcons.InstanceHidden, Resources.InstanceHidden);
        }
    }
}

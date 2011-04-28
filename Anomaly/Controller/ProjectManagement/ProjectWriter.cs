using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Anomaly
{
    class ProjectWriter
    {
        public static void addProject(Project project)
        {
            if (!Directory.Exists(project.WorkingDirectory))
            {
                Directory.CreateDirectory(project.WorkingDirectory);
            }
            String projectFile = Path.Combine(project.WorkingDirectory, project.Name + ".prj");
            if (!File.Exists(projectFile))
            {
                File.Create(projectFile);
            }
        }

        public static void removeProject(Project project)
        {
            DirectoryUtility.ForceDirectoryDelete(project.WorkingDirectory);
        }
    }
}

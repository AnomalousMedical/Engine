using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using System.IO;
using Engine;

namespace Anomaly
{
    public partial class Solution
    {
        private String name;
        private Dictionary<String, Project> projects = new Dictionary<string, Project>();

        private PluginSection pluginSection;
        private ResourceSection resourceSection;
        private ConfigFile backingFile;
        private String workingDirectory;

        public Solution(String projectFileName)
        {
            workingDirectory = Path.GetDirectoryName(projectFileName);
            backingFile = new ConfigFile(projectFileName);
            backingFile.loadConfigFile();
            pluginSection = new PluginSection(backingFile);
            resourceSection = new ResourceSection(backingFile);
        }

        public void addProject(Project project)
        {
            projects.Add(project.Name, project);
            onProjectAdded(project);
        }

        public void removeProject(Project project)
        {
            projects.Remove(project.Name);
            onProjectRemoved(project);
        }

        public PluginSection PluginSection
        {
            get
            {
                return pluginSection;
            }
        }

        public ResourceSection ResourceSection
        {
            get
            {
                return resourceSection;
            }
        }

        public String WorkingDirectory
        {
            get
            {
                return workingDirectory;
            }
        }
    }

    partial class Solution
    {
        private EditInterface editInterface;

        private EditInterfaceCommand destroyProject;
        private EditInterfaceManager<Project> projectInterfaceManager;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface(name);
                editInterface.IconReferenceTag = AnomalyIcons.Solution;

                editInterface.addCommand(new EditInterfaceCommand("Create Project", createProjectCallback));
                projectInterfaceManager = new EditInterfaceManager<Project>(editInterface);
                destroyProject = new EditInterfaceCommand("Remove", destroyProjectCallback);
                foreach (Project project in projects.Values)
                {
                    onProjectAdded(project);
                }
            }

            return editInterface;
        }

        #region Projects

        private void createProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            String name;
            bool accept = callback.getInputString("Enter a name.", out name, validateProjectCreate);
            if (accept)
            {
                Project project = new Project(name, Path.Combine(workingDirectory, name));
                addProject(project);
            }
        }

        private bool validateProjectCreate(String input, out String errorPrompt)
        {
            if (input == null || input == "")
            {
                errorPrompt = "Please enter a non empty name.";
                return false;
            }
            if (this.projects.ContainsKey(input))
            {
                errorPrompt = "That name is already in use. Please provide another.";
                return false;
            }
            errorPrompt = "";
            return true;
        }

        private void destroyProjectCallback(EditUICallback callback, EditInterfaceCommand command)
        {
            removeProject(projectInterfaceManager.resolveSourceObject(callback.getSelectedEditInterface()));
        }

        private void onProjectAdded(Project project)
        {
            if (editInterface != null)
            {
                EditInterface edit = project.getEditInterface();
                edit.addCommand(destroyProject);
                projectInterfaceManager.addSubInterface(project, edit);
            }
        }

        private void onProjectRemoved(Project project)
        {
            if (editInterface != null)
            {
                projectInterfaceManager.removeSubInterface(project);
            }
        }

        #endregion
    }
}

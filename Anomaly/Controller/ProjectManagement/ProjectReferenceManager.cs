using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;

namespace Anomaly
{
    public partial class ProjectReferenceManager
    {
        private List<ProjectReference> referencedProjects = new List<ProjectReference>();

        public ProjectReferenceManager()
        {

        }

        public void addReferencedProject(String projectName)
        {
            ProjectReference dependency = new ProjectReference(projectName);
            referencedProjects.Add(dependency);
            onReferenceAdded(dependency);
        }

        public void removeReferencedProject(String projectName)
        {
            ProjectReference matchingRef = null;
            foreach (ProjectReference reference in referencedProjects)
            {
                if (reference.ProjectName == projectName)
                {
                    matchingRef = reference;
                    break;
                }
            }
            if (matchingRef != null)
            {
                referencedProjects.Remove(matchingRef);
                onReferenceRemoved(matchingRef);
            }
        }

        public IEnumerable<ProjectReference> ReferencedProjectNames
        {
            get
            {
                return referencedProjects;
            }
        }
    }

    public partial class ProjectReferenceManager : Saveable
    {
        private const string PROJECT_REFERENCES = "ProjectReferences";

        protected ProjectReferenceManager(LoadInfo info)
        {
            info.RebuildList<ProjectReference>(PROJECT_REFERENCES, referencedProjects);
        }

        public void getInfo(SaveInfo info)
        {
            info.ExtractList<ProjectReference>(PROJECT_REFERENCES, referencedProjects);
        }
    }

    public partial class ProjectReferenceManager
    {
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface("Referenced Projects", addReferenceCallback, removeReferenceCallback, null);
                editInterface.IconReferenceTag = AnomalyIcons.ReferencedProjects;
                EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
                propertyInfo.addColumn(new EditablePropertyColumn("Project", false));
                editInterface.setPropertyInfo(propertyInfo);
                foreach (ProjectReference project in referencedProjects)
                {
                    editInterface.addEditableProperty(project);
                }
            }
            return editInterface;
        }

        private void addReferenceCallback(EditUICallback callback)
        {
            addReferencedProject("");
        }

        private void removeReferenceCallback(EditUICallback callback, EditableProperty property)
        {
            ProjectReference reference = property as ProjectReference;
            if (reference != null)
            {
                referencedProjects.Remove(reference);
                onReferenceRemoved(reference);
            }
        }

        private void onReferenceAdded(ProjectReference reference)
        {
            if (editInterface != null)
            {
                editInterface.addEditableProperty(reference);
            }
        }

        private void onReferenceRemoved(ProjectReference reference)
        {
            if (editInterface != null)
            {
                editInterface.removeEditableProperty(reference);
            }
        }
    }
}

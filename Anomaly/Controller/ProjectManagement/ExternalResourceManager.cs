using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;

namespace Anomaly
{
    public partial class ExternalResourceManager
    {
        private List<ExternalResource> externalResources = new List<ExternalResource>();

        public ExternalResourceManager()
        {

        }

        public void addExternalResource(String path)
        {
            ExternalResource resource = new ExternalResource(path);
            externalResources.Add(resource);
            onResourceAdded(resource);
        }

        public void removeExternalResource(String path)
        {
            ExternalResource matchingRef = null;
            foreach (ExternalResource reference in externalResources)
            {
                if (reference.Path == path)
                {
                    matchingRef = reference;
                    break;
                }
            }
            if (matchingRef != null)
            {
                externalResources.Remove(matchingRef);
                onResourceRemoved(matchingRef);
            }
        }

        public IEnumerable<ExternalResource> ExternalResources
        {
            get
            {
                return externalResources;
            }
        }
    }

    public partial class ExternalResourceManager : Saveable
    {
        private const string EXTERNAL_RESOURCES = "ExternalResources";

        protected ExternalResourceManager(LoadInfo info)
        {
            info.RebuildList<ExternalResource>(EXTERNAL_RESOURCES, externalResources);
        }

        public void getInfo(SaveInfo info)
        {
            info.ExtractList<ExternalResource>(EXTERNAL_RESOURCES, externalResources);
        }
    }

    public partial class ExternalResourceManager
    {
        private EditInterface editInterface;

        public EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new EditInterface("External Resources", addReferenceCallback, removeReferenceCallback, validate);
                editInterface.IconReferenceTag = AnomalyIcons.ExternalResources;
                EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
                propertyInfo.addColumn(new EditablePropertyColumn("Resource", false));
                editInterface.setPropertyInfo(propertyInfo);
                foreach (ExternalResource resource in externalResources)
                {
                    editInterface.addEditableProperty(resource);
                }
            }
            return editInterface;
        }

        private void addReferenceCallback(EditUICallback callback)
        {
            addExternalResource("");
        }

        private void removeReferenceCallback(EditUICallback callback, EditableProperty property)
        {
            ExternalResource resource = property as ExternalResource;
            if (resource != null)
            {
                externalResources.Remove(resource);
                onResourceRemoved(resource);
            }
        }

        private bool validate(out String message)
        {
            message = null;
            return true;
        }

        private void onResourceAdded(ExternalResource resource)
        {
            if (editInterface != null)
            {
                editInterface.addEditableProperty(resource);
            }
        }

        private void onResourceRemoved(ExternalResource resource)
        {
            if (editInterface != null)
            {
                editInterface.removeEditableProperty(resource);
            }
        }
    }
}

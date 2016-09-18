using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using Engine.Editing;

namespace Anomaly
{
    class ResourceManagerFileInterface : EditableFileInterface<ResourceManager>
    {
        private ResourceManager referenceSubsystems;

        public ResourceManagerFileInterface(String name, Object iconReferenceTag, String filename, ResourceManager referenceSubsystems)
            : base(name, iconReferenceTag, filename)
        {
            this.referenceSubsystems = referenceSubsystems;
        }

        public override EditInterface getObjectEditInterface(object obj)
        {
            ResourceManager resourceManager = obj as ResourceManager;
            if (resourceManager != null)
            {
                resourceManager.ensureHasAllSubsystems(referenceSubsystems);
                return resourceManager.getEditInterface();
            }
            else
            {
                throw new Exception(String.Format("Cannot get edit interface for object {0} because it is not of type ResourceManager", obj.ToString()));
            }
        }
    }
}

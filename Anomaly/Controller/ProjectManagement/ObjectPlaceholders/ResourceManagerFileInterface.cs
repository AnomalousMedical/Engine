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
        public ResourceManagerFileInterface(String name, Object iconReferenceTag, String filename)
            : base(name, iconReferenceTag, filename)
        {

        }

        public override EditInterface getObjectEditInterface(object obj)
        {
            ResourceManager resourceManager = obj as ResourceManager;
            if (resourceManager != null)
            {
                return resourceManager.getEditInterface();
            }
            else
            {
                throw new Exception(String.Format("Cannot get edit interface for object {0} because it is not of type ResourceManager", obj.ToString()));
            }
        }
    }
}

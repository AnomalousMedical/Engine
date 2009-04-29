using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using OgreWrapper;

namespace OgrePlugin
{
    class OgreResourceManager : ResourceListener
    {
        private static OgreResourceManager instance = new OgreResourceManager();

        public static OgreResourceManager Instance { get { return instance; } }

        private OgreResourceGroupManager ogreResourceManager;

        private OgreResourceManager()
        {
            ogreResourceManager = OgreResourceGroupManager.getInstance();
        }

        public void forceResourceRefresh()
        {
            ogreResourceManager.initializeAllResourceGroups();
        }

        public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
        {
            String resourceType;
            switch(resource.Type)
	        {
		        case ResourceType.FileSystem:
			        resourceType = "FileSystem";
			        break;
		        case ResourceType.ZipArchive:
			        resourceType = "Zip";
			        break;
		        default:
			        resourceType = "FileSystem";
			        break;
	        }
            ogreResourceManager.addResourceLocation(resource.FullPath, resourceType, group.Name, resource.Recursive);
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            ogreResourceManager.createResourceGroup(group.Name);
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            ogreResourceManager.destroyResourceGroup(group.Name);
        }

        public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
        {
            ogreResourceManager.removeResourceLocation(resource.FullPath, group.Name);
        }
    }
}

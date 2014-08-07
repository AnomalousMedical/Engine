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

        public void initializeResources(SubsystemResources resources)
        {
            foreach (var group in resources.Groups)
            {
                ogreResourceManager.initializeResourceGroup(group.FullName);
            }
        }

        public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
        {
            ogreResourceManager.addResourceLocation(resource.FullPath, "EngineArchive", group.FullName, resource.Recursive);
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            ogreResourceManager.createResourceGroup(group.FullName);
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            ogreResourceManager.destroyResourceGroup(group.FullName);
        }

        public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
        {
            ogreResourceManager.removeResourceLocation(resource.FullPath, group.FullName);
        }
    }
}

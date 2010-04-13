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
        private List<String> resourceGroupOrder = new List<string>();

        private OgreResourceManager()
        {
            ogreResourceManager = OgreResourceGroupManager.getInstance();
        }

        public void forceResourceRefresh()
        {
            foreach (String group in resourceGroupOrder)
            {
                ogreResourceManager.initializeResourceGroup(group);
            }
        }

        public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
        {
            ogreResourceManager.addResourceLocation(resource.FullPath, "EngineArchive", group.Name, resource.Recursive);
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            ogreResourceManager.createResourceGroup(group.Name);
            resourceGroupOrder.Add(group.Name);
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            ogreResourceManager.destroyResourceGroup(group.Name);
            resourceGroupOrder.Remove(group.Name);
        }

        public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
        {
            ogreResourceManager.removeResourceLocation(resource.FullPath, group.Name);
        }
    }
}

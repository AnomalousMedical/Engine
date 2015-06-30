using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;

namespace OgrePlugin
{
    class OgreResourceManager : ResourceListener
    {
        private static OgreResourceManager instance;

        public static OgreResourceManager Instance { get { return instance; } }

        private OgreResourceGroupManager ogreResourceManager;
        private MaterialParserManager materialParser;

        internal OgreResourceManager(MaterialParserManager materialParser)
        {
            this.materialParser = materialParser;
            ogreResourceManager = OgreResourceGroupManager.getInstance();
            instance = this;
        }

        public void initializeResources(IEnumerable<ResourceGroup> groups)
        {
            materialParser.initializeResources(groups);
            foreach (var group in groups)
            {
                ogreResourceManager.initializeResourceGroup(group.FullName);
            }
        }

        public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
        {
            materialParser.resourceAdded(group, resource);
            ogreResourceManager.addResourceLocation(resource.LocName, resource.ArchiveType, group.FullName, resource.Recursive);
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            materialParser.resourceGroupAdded(group);
            ogreResourceManager.createResourceGroup(group.FullName);
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            materialParser.resourceGroupRemoved(group);
            ogreResourceManager.destroyResourceGroup(group.FullName);
        }

        public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
        {
            materialParser.resourceRemoved(group, resource);
            ogreResourceManager.removeResourceLocation(resource.LocName, group.FullName);
        }
    }
}

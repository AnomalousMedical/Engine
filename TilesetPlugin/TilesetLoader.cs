using Engine.Resources;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesetPlugin
{
    class TilesetLoader : ResourceListener
    {
        private Dictionary<String, TilesetGroup> groups = new Dictionary<string, TilesetGroup>();
        private TilesetManager manager;

        public TilesetLoader(TilesetManager manager)
        {
            this.manager = manager;
        }

        public void initializeResources(IEnumerable<ResourceGroup> groups)
        {
            foreach(var group in groups.Select(g => this.groups[g.FullName]))
            {
                group.initializeResources();
            }
        }

        public void resourceAdded(ResourceGroup group, Resource resource)
        {
            TilesetGroup tsGroup;
            if (groups.TryGetValue(group.FullName, out tsGroup))
            {
                tsGroup.resourceAdded(resource);
            }
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            if (!groups.ContainsKey(group.FullName))
            {
                groups[group.FullName] = new TilesetGroup(manager);
            }
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            TilesetGroup tsGroup;
            if(groups.TryGetValue(group.FullName, out tsGroup))
            {
                tsGroup.removed();
                groups.Remove(group.FullName);
            }
        }

        public void resourceRemoved(ResourceGroup group, Resource resource)
        {
            TilesetGroup tsGroup;
            if (groups.TryGetValue(group.FullName, out tsGroup))
            {
                tsGroup.resourceRemoved(resource);
            }
        }
    }
}

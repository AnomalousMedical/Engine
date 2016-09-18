using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Resources;

namespace TilesetPlugin
{
    class TilesetGroup
    {
        private bool loaded = false;
        private TilesetManager manager;
        private Dictionary<String, TilesetResource> resources = new Dictionary<String, TilesetResource>();

        public TilesetGroup(TilesetManager manager)
        {
            this.manager = manager;
        }

        internal void resourceAdded(Resource resource)
        {
            var tsResource = new TilesetResource(resource.LocName, resource.Recursive);
            if (loaded)
            {
                tsResource.load(manager);
            }
            resources[resource.LocName] = tsResource;
        }

        internal void resourceRemoved(Resource resource)
        {
            if (loaded)
            {
                TilesetResource tsResource;
                if(resources.TryGetValue(resource.LocName, out tsResource))
                {
                    tsResource.unload(manager);
                }
            }
        }

        internal void initializeResources()
        {
            if (!loaded)
            {
                foreach (var resource in resources.Values)
                {
                    resource.load(manager);
                }
                loaded = true;
            }
        }

        internal void removed()
        {
            if (loaded)
            {
                foreach (var resource in resources.Values)
                {
                    resource.unload(manager);
                }
            }
        }
    }
}

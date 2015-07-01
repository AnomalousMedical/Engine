using Engine.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    class MaterialParserGroup
    {
        private List<MaterialLocation> materialLocations = new List<MaterialLocation>();
        private String name;
        private MaterialParserManager parent;

        public MaterialParserGroup(String name, MaterialParserManager parent)
        {
            this.name = name;
            this.parent = parent;
        }

        internal void initializeResources(JsonSerializer serializer)
        {
            foreach(var entry in materialLocations)
            {
                entry.initializeResources(serializer);
            }
        }

        internal void resourceAdded(Engine.Resources.Resource resource)
        {
            if (resource.ArchiveType == "EngineArchive") //Only support engine archives
            {
                materialLocations.Add(new MaterialLocation(resource, this));
            }
        }

        internal void unloadResources()
        {
            foreach (var entry in materialLocations)
            {
                entry.unloadResources();
            }
        }

        internal void resourceRemoved(Engine.Resources.Resource resource)
        {
            for(int i = 0; i < materialLocations.Count; ++i)
            {
                if (materialLocations[i].LocName == resource.LocName)
                {
                    materialLocations[i].unloadResources();
                    materialLocations.RemoveAt(i);
                }
            }
        }

        internal void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            parent.buildMaterial(description, repo);
        }

        internal void destroyMaterial(MaterialPtr material, String builderName)
        {
            parent.destroyMaterial(material, builderName);
        }

        public String Name
        {
            get
            {
                return name;
            }
        }
    }
}

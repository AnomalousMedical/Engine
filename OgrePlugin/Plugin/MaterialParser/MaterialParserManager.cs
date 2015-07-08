using Engine.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public class MaterialParserManager
    {
        private List<MaterialLocation> materialLocations = new List<MaterialLocation>();
        private Dictionary<String, MaterialBuilder> materialBuilders = new Dictionary<string, MaterialBuilder>();

        public MaterialParserManager()
        {

        }

        public void addMaterialBuilder(MaterialBuilder builder)
        {
            materialBuilders.Add(builder.Name, builder);
        }

        public void removeMaterialBuilder(MaterialBuilder builder)
        {
            materialBuilders.Remove(builder.Name);
        }

        public void initializeResources(IEnumerable<ResourceGroup> groups)
        {
            JsonSerializer serializer = new JsonSerializer();
            foreach(var location in materialLocations)
            {
                location.initializeResources(serializer);
            }
        }

        public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
        {
            if (resource.ArchiveType == "EngineArchive") //Only support engine archives
            {
                var location = materialLocations.Where(l => l.LocName == resource.LocName).FirstOrDefault();
                if(location == null)
                {
                    location = new MaterialLocation(resource, this);
                    materialLocations.Add(location);
                }
                location.addGroup(group);
            }
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            //Don't care
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            for (int i = 0; i < materialLocations.Count; ++i)
            {
                var loc = materialLocations[i];
                if (loc.inGroup(group) && loc.removeGroup(group))
                {
                    loc.unloadResources();
                    materialLocations.RemoveAt(i--);
                }
            }
        }

        public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
        {
            for (int i = 0; i < materialLocations.Count; ++i)
            {
                var loc = materialLocations[i];
                if (loc.inGroup(group) && loc.LocName == resource.LocName && loc.removeGroup(group))
                {
                    loc.unloadResources();
                    materialLocations.RemoveAt(i--);
                }
            }
        }

        internal void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            MaterialBuilder builder;
            if (materialBuilders.TryGetValue(description.Builder, out builder))
            {
                builder.buildMaterial(description, repo);
            }
            else
            {
                throw new MaterialParserException("Could not find material builder '{0}' for material '{1}' defined in '{2}'", description.Builder, description.Name, description.SourceFile);
            }
        }

        internal void destroyMaterial(MaterialPtr material, String builderName)
        {
            MaterialBuilder builder;
            if (materialBuilders.TryGetValue(builderName, out builder))
            {
                builder.destroyMaterial(material);
            }
            else
            {
                Logging.Log.Error("Could not find material builder '{0}' for the destruction of material '{1}'. This builder may have been removed too early.", builderName, material.Value.Name);
            }
        }
    }
}

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
        private Dictionary<String, MaterialParserGroup> materialGroups = new Dictionary<string,MaterialParserGroup>();
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
            foreach (var group in groups)
            {
                MaterialParserGroup matGroup;
                if(materialGroups.TryGetValue(group.FullName, out matGroup))
                {
                    matGroup.initializeResources(serializer);
                }
            }
        }

        public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
        {
            MaterialParserGroup matGroup = declareGroup(group);
            matGroup.resourceAdded(resource);
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            declareGroup(group);
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            MaterialParserGroup matGroup;
            if (materialGroups.TryGetValue(group.FullName, out matGroup))
            {
                matGroup.unloadResources();
                materialGroups.Remove(group.FullName);
            }
        }

        public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
        {
            MaterialParserGroup matGroup;
            if (materialGroups.TryGetValue(group.FullName, out matGroup))
            {
                matGroup.resourceRemoved(resource);
                materialGroups.Remove(group.FullName);
            }
        }

        private MaterialParserGroup declareGroup(ResourceGroup group)
        {
            MaterialParserGroup matGroup;
            if (!materialGroups.TryGetValue(group.FullName, out matGroup))
            {
                matGroup = new MaterialParserGroup(group.FullName, this);
                materialGroups.Add(group.FullName, matGroup);
            }
            return matGroup;
        }

        internal MaterialPtr buildMaterial(MaterialDescription description)
        {
            MaterialBuilder builder;
            if (materialBuilders.TryGetValue(description.Builder, out builder))
            {
                return builder.buildMaterial(description);
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

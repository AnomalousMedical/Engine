using Engine.Resources;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.TextureCompiler
{
    class MaterialParserResourceListener : ResourceListener
    {
        TextureCompiler textureCompiler;
        MaterialParserManager materialParser = new MaterialParserManager();
        public MaterialParserResourceListener(String sourceDirectory, String destDirectory, OutputFormats outputFormats, int maxSize)
        {
            textureCompiler = new TextureCompiler(sourceDirectory, destDirectory, outputFormats, maxSize);
            materialParser.addMaterialBuilder(textureCompiler);
        }

        public void loadTextureInfo()
        {
            textureCompiler.loadTextureInfo();
        }

        public void saveTextureInfo()
        {
            textureCompiler.saveTextureInfo();
        }

        public void initializeResources(IEnumerable<ResourceGroup> groups)
        {
            materialParser.initializeResources(groups);
        }

        public void resourceAdded(ResourceGroup group, Engine.Resources.Resource resource)
        {
            materialParser.resourceAdded(group, resource);
        }

        public void resourceGroupAdded(ResourceGroup group)
        {
            materialParser.resourceGroupAdded(group);
        }

        public void resourceGroupRemoved(ResourceGroup group)
        {
            materialParser.resourceGroupRemoved(group);
        }

        public void resourceRemoved(ResourceGroup group, Engine.Resources.Resource resource)
        {
            materialParser.resourceRemoved(group, resource);
        }
    }
}

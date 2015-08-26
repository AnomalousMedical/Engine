using Logging;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.TextureCompiler
{
    class TextureCompiler : MaterialBuilder
    {
        public TextureCompiler(String sourceDirectory, String destDirectory)
        {

        }

        public override void buildMaterial(MaterialDescription description, MaterialRepository repo)
        {
            Log.Info("Compiling textures for material {0}", description.Name);
            if(description.HasNormalMap)
            {
                //Normal maps are pretty much always the same.
            }
        }

        public override void destroyMaterial(MaterialPtr materialPtr)
        {

        }

        public override void initializationComplete()
        {

        }

        public override string Name
        {
            get
            {
                return "VirtualTexture";
            }
        }
    }
}

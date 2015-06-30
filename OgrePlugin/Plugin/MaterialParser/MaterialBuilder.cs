using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public abstract class MaterialBuilder
    {
        public abstract MaterialPtr buildMaterial(MaterialDescription description);

        public abstract void destroyMaterial(MaterialPtr materialPtr);

        public abstract String Name { get; }
    }
}

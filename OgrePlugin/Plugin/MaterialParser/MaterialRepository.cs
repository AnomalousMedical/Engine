using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public interface MaterialRepository
    {
        void addMaterial(MaterialPtr material, MaterialDescription description);
    }
}

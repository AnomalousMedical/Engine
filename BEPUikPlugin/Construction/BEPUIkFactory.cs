using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUIkFactory : SimElementFactory
    {
        private BEPUIkScene scene;

        public BEPUIkFactory(BEPUIkScene scene)
        {
            this.scene = scene;
        }

        public void createProducts()
        {
            
        }

        public void createStaticProducts()
        {
            
        }

        public void linkProducts()
        {
            
        }

        public void clearDefinitions()
        {
            
        }
    }
}

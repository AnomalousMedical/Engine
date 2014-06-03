using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUIkScene : SimElementManager
    {
        private String name;
        private BEPUIkFactory factory;

        public BEPUIkScene(String name)
        {
            this.name = name;
            factory = new BEPUIkFactory(this);
        }

        public void Dispose()
        {

        }

        public SimElementFactory getFactory()
        {
            return factory;
        }

        public Type getSimElementManagerType()
        {
            return typeof(BEPUIkScene);
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            return new BEPUIkSceneDefinition(name);
        }
    }
}

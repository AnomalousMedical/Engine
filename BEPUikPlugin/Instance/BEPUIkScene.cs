using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikScene : SimElementManager
    {
        private String name;
        private BEPUIkFactory factory;

        public BEPUikScene(String name)
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

        public BEPUIkFactory IkFactory
        {
            get
            {
                return factory;
            }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BEPUikScene);
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            return new BEPUikSceneDefinition(name);
        }
    }
}

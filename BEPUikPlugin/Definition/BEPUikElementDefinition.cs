using Engine.ObjectManagement;
using Engine.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public abstract class BEPUikElementDefinition : SimElementDefinition
    {
        public BEPUikElementDefinition(String name)
            : base(name)
        {

        }

        internal abstract void createProduct(SimObjectBase instance, BEPUikScene scene);

        //Saving

        protected BEPUikElementDefinition(LoadInfo info)
            :base(info)
        {

        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }
    }
}

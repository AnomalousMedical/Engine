using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Saving;

namespace BulletPlugin
{
    public abstract class BulletElementDefinition : SimElementDefinition
    {
        public BulletElementDefinition(String name)
            : base(name)
        {

        }

        internal abstract void createProduct(SimObjectBase instance, BulletScene scene);

        internal abstract void createStaticProduct(SimObjectBase instance, BulletScene scene);

        //Saving

        protected BulletElementDefinition(LoadInfo info)
            :base(info)
        {

        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }
    }
}

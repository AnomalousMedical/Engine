using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    public class BulletFactoryEntry
    {
        SimObjectBase instance;
        BulletElementDefinition element;

        public BulletFactoryEntry(SimObjectBase instance, BulletElementDefinition element)
        {
            this.instance = instance;
            this.element = element;
        }

        public void createProduct(BulletScene scene)
        {
            element.createProduct(instance, scene);
        }

        public void createStaticProduct(BulletScene scene)
        {
            element.createStaticProduct(instance, scene);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    class BulletFactoryEntry
    {
        SimObjectBase instance;
        BulletElementDefinition element;

        public BulletFactoryEntry(SimObjectBase instance, BulletElementDefinition element)
        {
            this.instance = instance;
            this.element = element;
        }

        public void createProduct(BulletSceneInternal scene)
        {
            element.createProduct(instance, scene);
        }

        public void createStaticProduct(BulletSceneInternal scene)
        {
            element.createStaticProduct(instance, scene);
        }
    }
}

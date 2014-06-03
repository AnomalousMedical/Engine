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
        private BEPUikScene scene;

        //The stuff in this list can be created without dependence on other objects
        private List<BEPUikFactoryEntry> independentPhaseEntries = new List<BEPUikFactoryEntry>();

        //The stuff in this list needs the elements from the independent phase created first
        private List<BEPUikFactoryEntry> dependentPhaseEntries = new List<BEPUikFactoryEntry>();

        public BEPUIkFactory(BEPUikScene scene)
        {
            this.scene = scene;
        }

        public void createProducts()
        {
            foreach(var entry in independentPhaseEntries)
            {
                entry.createProduct(scene);
            }

            foreach (var entry in dependentPhaseEntries)
            {
                entry.createProduct(scene);
            }
        }

        public void createStaticProducts()
        {
            
        }

        public void linkProducts()
        {
            
        }

        public void clearDefinitions()
        {
            independentPhaseEntries.Clear();
            dependentPhaseEntries.Clear();
        }

        internal void addBone(BEPUikBoneDefinition bone, SimObjectBase instance)
        {
            independentPhaseEntries.Add(new BEPUikFactoryEntry(instance, bone));
        }

        internal void addJoint(BEPUikJointDefinition joint, SimObjectBase instance)
        {
            dependentPhaseEntries.Add(new BEPUikFactoryEntry(instance, joint));
        }

        internal void addControl(BEPUikControlDefinition control, SimObjectBase instance)
        {
            dependentPhaseEntries.Add(new BEPUikFactoryEntry(instance, control));
        }
    }
}

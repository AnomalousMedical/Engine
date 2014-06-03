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

        public BEPUIkFactory(BEPUikScene scene)
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

        internal void addBone(BEPUikBoneDefinition bone, SimObjectBase instance)
        {
            
        }

        internal void addJoint(BEPUikBallSocketJointDefinition joint, SimObjectBase instance)
        {
            
        }

        internal void addControl(BEPUikDragControlDefinition control, SimObjectBase instance)
        {
            
        }
    }
}

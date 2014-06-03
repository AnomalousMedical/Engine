using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    public class BEPUikControl : SimElement
    {
        private BEPUikScene scene;

        public BEPUikControl(BEPUikScene scene, String name, Subscription subscription)
            :base(name, subscription)
        {
            this.scene = scene;
        }

        protected override void Dispose()
        {
            
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            
        }

        protected override void updateTranslationImpl(ref Vector3 translation)
        {
            
        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            
        }

        protected override void updateScaleImpl(ref Vector3 scale)
        {
            
        }

        protected override void setEnabled(bool enabled)
        {
            
        }

        public override SimElementDefinition saveToDefinition()
        {
            return new BEPUikControlDefinition(Name);
        }
    }
}

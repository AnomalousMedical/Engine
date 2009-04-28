using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;

namespace Engine.ObjectManagement
{
    public class InvalidElement : SimElement
    {
        private SimElementDefinition invalidDefinition;

        public InvalidElement(SimElementDefinition element, String message)
            :base(element.Name, Subscription.None)
        {
            invalidDefinition = element;
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
            return invalidDefinition;
        }
    }
}

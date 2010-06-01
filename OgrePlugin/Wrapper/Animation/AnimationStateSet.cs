using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class AnimationStateSet
    {
        internal IntPtr OgreObject { get; private set; }

        public AnimationStateSet(IntPtr animationStateSet)
        {
            this.OgreObject = animationStateSet;
        }

        internal AnimationState getAnimationState(string name)
        {
            throw new NotImplementedException();
        }
    }
}

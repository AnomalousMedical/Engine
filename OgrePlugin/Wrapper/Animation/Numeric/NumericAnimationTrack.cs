using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class NumericAnimationTrack : AnimationTrack
    {
        internal static NumericAnimationTrack createWrapper(IntPtr nativeObject, object[] args)
        {
            return new NumericAnimationTrack(nativeObject, args[0] as Animation);
        }

        protected NumericAnimationTrack(IntPtr animationTrack, Animation parent)
            : base(animationTrack, parent)
        {

        }

        public override KeyFrame getKeyFrame(ushort index)
        {
            throw new NotImplementedException();
        }

        public override float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2)
        {
            throw new NotImplementedException();
        }

        public override float getKeyFramesAtTime(TimeIndex timeIndex, out KeyFrame keyFrame1, out KeyFrame keyFrame2, out ushort firstKeyIndex)
        {
            throw new NotImplementedException();
        }

        public override KeyFrame createKeyFrame(float timePos)
        {
            throw new NotImplementedException();
        }

        public override void removeKeyFrame(ushort index)
        {
            throw new NotImplementedException();
        }

        public override void removeAllKeyFrames()
        {
            throw new NotImplementedException();
        }
    }
}

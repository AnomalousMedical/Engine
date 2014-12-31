using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class TransformKeyFrame : KeyFrame
    {
        internal static TransformKeyFrame createWrapper(IntPtr nativeObject, object[] args)
        {
            return new TransformKeyFrame(nativeObject);
        }

        protected TransformKeyFrame(IntPtr transformKeyFrame)
            :base(transformKeyFrame)
        {

        }
    }
}

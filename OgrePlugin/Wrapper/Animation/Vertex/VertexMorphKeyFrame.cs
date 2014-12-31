using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class VertexMorphKeyFrame : KeyFrame
    {
        internal static VertexMorphKeyFrame createWrapper(IntPtr nativeObject, object[] args)
        {
            return new VertexMorphKeyFrame(nativeObject);
        }

        protected VertexMorphKeyFrame(IntPtr keyFrame)
            :base(keyFrame)
        {

        }
    }
}

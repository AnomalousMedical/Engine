using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class RenderTexture : RenderTarget
    {
        internal RenderTexture(IntPtr nativeObject)
            :base(nativeObject)
        {

        }
    }
}

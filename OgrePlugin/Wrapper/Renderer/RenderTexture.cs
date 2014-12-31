using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgrePlugin
{
    public class RenderTexture : RenderTarget
    {
        internal RenderTexture(IntPtr nativeObject)
            :base(nativeObject)
        {

        }
    }
}

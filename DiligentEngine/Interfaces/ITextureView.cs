using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using Engine;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;

namespace DiligentEngine
{
    /// <summary>
    /// \remarks
    /// To create a texture view, call ITexture::CreateView().
    /// Texture view holds strong references to the texture. The texture
    /// will not be destroyed until all views are released.
    /// The texture view will also keep a strong reference to the texture sampler,
    /// if any is set.
    /// </summary>
    public partial class ITextureView :  IDeviceObject
    {
        public ITextureView(IntPtr objPtr)
            : base(objPtr)
        {

        }


    }
}

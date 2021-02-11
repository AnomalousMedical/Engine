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
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

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
            this._ConstructorCalled();
        }
        partial void _ConstructorCalled();
        /// <summary>
        /// Sets the texture sampler to use for filtering operations
        /// when accessing a texture from shaders. Only
        /// shader resource views can be assigned a sampler.
        /// The view will keep strong reference to the sampler.
        /// </summary>
        public void SetSampler(ISampler pSampler)
        {
            ITextureView_SetSampler(
                this.objPtr
                , pSampler.objPtr
            );
        }
        /// <summary>
        /// Returns the pointer to the referenced texture object.
        /// The method does *NOT* call AddRef() on the returned interface,
        /// so Release() must not be called.
        /// </summary>
        public  ITexture GetTexture()
        {
            var theReturnValue = 
            ITextureView_GetTexture(
                this.objPtr
            );
            return theReturnValue != IntPtr.Zero ? new  ITexture(theReturnValue) : null;
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ITextureView_SetSampler(
            IntPtr objPtr
            , IntPtr pSampler
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ITextureView_GetTexture(
            IntPtr objPtr
        );
    }
}

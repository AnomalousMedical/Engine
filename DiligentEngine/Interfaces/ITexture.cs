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
    /// Texture inteface
    /// </summary>
    public partial class ITexture :  IDeviceObject
    {
        public ITexture(IntPtr objPtr)
            : base(objPtr)
        {

        }
        /// <summary>
        /// Returns the pointer to the default view.
        /// \param [in] ViewType - Type of the requested view. See Diligent::TEXTURE_VIEW_TYPE.
        /// \return Pointer to the interface
        /// 
        /// \note The function does not increase the reference counter for the returned interface, so
        /// Release() must *NOT* be called.
        /// </summary>
        public ITextureView GetDefaultView(TEXTURE_VIEW_TYPE ViewType)
        {
            var theReturnValue = 
            ITexture_GetDefaultView(
                this.objPtr
                , ViewType
            );
            return theReturnValue != IntPtr.Zero ? new ITextureView(theReturnValue) : null;
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ITexture_GetDefaultView(
            IntPtr objPtr
            , TEXTURE_VIEW_TYPE ViewType
        );
    }
}

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
            this._ConstructorCalled();
        }
        partial void _ConstructorCalled();
        /// <summary>
        /// Creates a new texture view
        /// \param [in] ViewDesc - View description. See Diligent::TextureViewDesc for details.
        /// \param [out] ppView - Address of the memory location where the pointer to the view interface will be written to.
        /// 
        /// \remarks To create a shader resource view addressing the entire texture, set only TextureViewDesc::ViewType
        /// member of the ViewDesc parameter to Diligent::TEXTURE_VIEW_SHADER_RESOURCE and leave all other
        /// members in their default values. Using the same method, you can create render target or depth stencil
        /// view addressing the largest mip level.\n
        /// If texture view format is Diligent::TEX_FORMAT_UNKNOWN, the view format will match the texture format.\n
        /// If texture view type is Diligent::TEXTURE_VIEW_UNDEFINED, the type will match the texture type.\n
        /// If the number of mip levels is 0, and the view type is shader resource, the view will address all mip levels.
        /// For other view types it will address one mip level.\n
        /// If the number of slices is 0, all slices from FirstArraySlice or FirstDepthSlice will be referenced by the view.
        /// For non-array textures, the only allowed values for the number of slices are 0 and 1.\n
        /// Texture view will contain strong reference to the texture, so the texture will not be destroyed
        /// until all views are released.\n
        /// The function calls AddRef() for the created interface, so it must be released by
        /// a call to Release() when it is no longer needed.
        /// </summary>
        public AutoPtr<ITextureView> CreateView(TextureViewDesc ViewDesc)
        {
            var theReturnValue = 
            ITexture_CreateView(
                this.objPtr
                , ViewDesc.ViewType
                , ViewDesc.TextureDim
                , ViewDesc.Format
                , ViewDesc.MostDetailedMip
                , ViewDesc.NumMipLevels
                , ViewDesc.FirstArraySlice
                , ViewDesc.NumArraySlices
                , ViewDesc.AccessFlags
                , ViewDesc.Flags
                , ViewDesc.Name
            );
            return theReturnValue != IntPtr.Zero ? new AutoPtr<ITextureView>(new ITextureView(theReturnValue), false) : null;
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
        private static extern IntPtr ITexture_CreateView(
            IntPtr objPtr
            , TEXTURE_VIEW_TYPE ViewDesc_ViewType
            , RESOURCE_DIMENSION ViewDesc_TextureDim
            , TEXTURE_FORMAT ViewDesc_Format
            , Uint32 ViewDesc_MostDetailedMip
            , Uint32 ViewDesc_NumMipLevels
            , Uint32 ViewDesc_FirstArraySlice
            , Uint32 ViewDesc_NumArraySlices
            , UAV_ACCESS_FLAG ViewDesc_AccessFlags
            , TEXTURE_VIEW_FLAGS ViewDesc_Flags
            , String ViewDesc_Name
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ITexture_GetDefaultView(
            IntPtr objPtr
            , TEXTURE_VIEW_TYPE ViewType
        );
    }
}

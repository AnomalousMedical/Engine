using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Engine;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;

namespace DiligentEngine
{
    public partial class IRenderDevice : IObject
    {
        public AutoPtr<IBuffer> CreateBuffer(BufferDesc BuffDesc)
        {
            return new AutoPtr<IBuffer>(new IBuffer(IRenderDevice_CreateBuffer_Null_Data(
                this.objPtr
                , BuffDesc.uiSizeInBytes
                , BuffDesc.BindFlags
                , BuffDesc.Usage
                , BuffDesc.CPUAccessFlags
                , BuffDesc.Mode
                , BuffDesc.ElementByteStride
                , BuffDesc.CommandQueueMask
                , BuffDesc.Name
            )), false);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateBuffer_Null_Data(
            IntPtr objPtr
            , Uint32 BuffDesc_uiSizeInBytes
            , BIND_FLAGS BuffDesc_BindFlags
            , USAGE BuffDesc_Usage
            , CPU_ACCESS_FLAGS BuffDesc_CPUAccessFlags
            , BUFFER_MODE BuffDesc_Mode
            , Uint32 BuffDesc_ElementByteStride
            , Uint64 BuffDesc_CommandQueueMask
            , String BuffDesc_Name
        );

        /// <summary>
        /// Creates a new shader object
        /// \param [in] ShaderCI  - Shader create info, see Diligent::ShaderCreateInfo for details.
        /// \param [out] ppShader - Address of the memory location where the pointer to the
        /// shader interface will be stored.
        /// The function calls AddRef(), so that the new object will contain
        /// one reference.
        /// </summary>
        public AutoPtr<IShader> CreateShader(ShaderCreateInfo ShaderCI, ShaderMacroHelper macros)
        {
            var macrosArray = macros.CreatePassStructArray();
            var theReturnValue =
            IRenderDevice_CreateShader_Macros(
                this.objPtr
                , ShaderCI.FilePath
                , ShaderCI.Source
                , ShaderCI.EntryPoint
                , ShaderCI.UseCombinedTextureSamplers
                , ShaderCI.CombinedSamplerSuffix
                , ShaderCI.Desc.ShaderType
                , ShaderCI.Desc.Name
                , ShaderCI.SourceLanguage
                , ShaderCI.ShaderCompiler
                , ShaderCI.HLSLVersion.Major
                , ShaderCI.HLSLVersion.Minor
                , macrosArray
                , (Uint32)macrosArray.Length
            );
            return new AutoPtr<IShader>(new IShader(theReturnValue), false);
        }
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateShader_Macros(
            IntPtr objPtr
            , String ShaderCI_FilePath
            , String ShaderCI_Source
            , String ShaderCI_EntryPoint
            , [MarshalAs(UnmanagedType.I1)] bool ShaderCI_UseCombinedTextureSamplers
            , String ShaderCI_CombinedSamplerSuffix
            , SHADER_TYPE ShaderCI_Desc_ShaderType
            , String ShaderCI_Desc_Name
            , SHADER_SOURCE_LANGUAGE ShaderCI_SourceLanguage
            , SHADER_COMPILER ShaderCI_ShaderCompiler
            , Uint8 ShaderCI_HLSLVersion_Major
            , Uint8 ShaderCI_HLSLVersion_Minor
            , MacroPassStruct[] macros
            , Uint32 macrosCount
        );

        public NDCAttribs GetDeviceCaps_GetNDCAttribs()
        {
            //This is not fast, does this on the unmanaged side too
            var result = IRenderDevice_GetDeviceCaps_GetNDCAttribs(this.objPtr);
            return new NDCAttribs()
            {
                MinZ = result.MinZ,
                YtoVScale = result.YtoVScale,
                ZtoDepthScale = result.ZtoDepthScale
            };
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern NDCAttribsPassStruct IRenderDevice_GetDeviceCaps_GetNDCAttribs(IntPtr objPtr);

        public Uint32 DeviceProperties_MaxRayTracingRecursionDepth => IRenderDevice_DeviceProperties_MaxRayTracingRecursionDepth(this.objPtr);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Uint32 IRenderDevice_DeviceProperties_MaxRayTracingRecursionDepth(IntPtr objPtr);
    }
}

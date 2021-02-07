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
    public partial class KtxLoader
    {
        public static AutoPtr<ITexture> CreateTextureFromKTX(IntPtr pKTXData, UIntPtr DataSize, TextureLoadInfo TexLoadInfo, IRenderDevice pDevice)
        {
            var theReturnValue = 
            KtxLoader_CreateTextureFromKTX(
                pKTXData
                , DataSize
                , TexLoadInfo.Name
                , TexLoadInfo.Usage
                , TexLoadInfo.BindFlags
                , TexLoadInfo.MipLevels
                , TexLoadInfo.CPUAccessFlags
                , TexLoadInfo.IsSRGB
                , TexLoadInfo.GenerateMips
                , TexLoadInfo.Format
                , pDevice.objPtr
            );
            return new AutoPtr<ITexture>(new ITexture(theReturnValue), false);
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr KtxLoader_CreateTextureFromKTX(
            IntPtr pKTXData
            , UIntPtr DataSize
            , String TexLoadInfo_Name
            , USAGE TexLoadInfo_Usage
            , BIND_FLAGS TexLoadInfo_BindFlags
            , Uint32 TexLoadInfo_MipLevels
            , CPU_ACCESS_FLAGS TexLoadInfo_CPUAccessFlags
            , [MarshalAs(UnmanagedType.I1)]Bool TexLoadInfo_IsSRGB
            , [MarshalAs(UnmanagedType.I1)]Bool TexLoadInfo_GenerateMips
            , TEXTURE_FORMAT TexLoadInfo_Format
            , IntPtr pDevice
        );
    }
}

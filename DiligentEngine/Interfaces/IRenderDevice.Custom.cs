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
    public partial class IRenderDevice :  IObject
    {
        private static BufferData EmptyData = new BufferData();

        public IBuffer CreateBuffer(BufferDesc BuffDesc)
        {
            return new IBuffer(IRenderDevice_CreateBuffer_Null_Data(
                this.objPtr
                , BuffDesc.uiSizeInBytes
                , BuffDesc.BindFlags
                , BuffDesc.Usage
                , BuffDesc.CPUAccessFlags
                , BuffDesc.Mode
                , BuffDesc.ElementByteStride
                , BuffDesc.CommandQueueMask
                , BuffDesc.Name
            ));
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
    }
}

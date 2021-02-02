using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;

namespace DiligentEngine
{
    public partial class BufferDesc : DeviceObjectAttribs
    {
        public BufferDesc(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public Uint32 uiSizeInBytes
        {
            get
            {
                return BufferDesc_Get_uiSizeInBytes(this.objPtr);
            }
            set
            {
                BufferDesc_Set_uiSizeInBytes(this.objPtr, value);
            }
        }
        public BIND_FLAGS BindFlags
        {
            get
            {
                return BufferDesc_Get_BindFlags(this.objPtr);
            }
            set
            {
                BufferDesc_Set_BindFlags(this.objPtr, value);
            }
        }
        public USAGE Usage
        {
            get
            {
                return BufferDesc_Get_Usage(this.objPtr);
            }
            set
            {
                BufferDesc_Set_Usage(this.objPtr, value);
            }
        }
        public CPU_ACCESS_FLAGS CPUAccessFlags
        {
            get
            {
                return BufferDesc_Get_CPUAccessFlags(this.objPtr);
            }
            set
            {
                BufferDesc_Set_CPUAccessFlags(this.objPtr, value);
            }
        }
        public BUFFER_MODE Mode
        {
            get
            {
                return BufferDesc_Get_Mode(this.objPtr);
            }
            set
            {
                BufferDesc_Set_Mode(this.objPtr, value);
            }
        }
        public Uint32 ElementByteStride
        {
            get
            {
                return BufferDesc_Get_ElementByteStride(this.objPtr);
            }
            set
            {
                BufferDesc_Set_ElementByteStride(this.objPtr, value);
            }
        }
        public Uint64 CommandQueueMask
        {
            get
            {
                return BufferDesc_Get_CommandQueueMask(this.objPtr);
            }
            set
            {
                BufferDesc_Set_CommandQueueMask(this.objPtr, value);
            }
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Uint32 BufferDesc_Get_uiSizeInBytes(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BufferDesc_Set_uiSizeInBytes(IntPtr objPtr, Uint32 value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern BIND_FLAGS BufferDesc_Get_BindFlags(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BufferDesc_Set_BindFlags(IntPtr objPtr, BIND_FLAGS value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern USAGE BufferDesc_Get_Usage(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BufferDesc_Set_Usage(IntPtr objPtr, USAGE value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern CPU_ACCESS_FLAGS BufferDesc_Get_CPUAccessFlags(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BufferDesc_Set_CPUAccessFlags(IntPtr objPtr, CPU_ACCESS_FLAGS value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern BUFFER_MODE BufferDesc_Get_Mode(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BufferDesc_Set_Mode(IntPtr objPtr, BUFFER_MODE value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Uint32 BufferDesc_Get_ElementByteStride(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BufferDesc_Set_ElementByteStride(IntPtr objPtr, Uint32 value);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Uint64 BufferDesc_Get_CommandQueueMask(IntPtr objPtr);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void BufferDesc_Set_CommandQueueMask(IntPtr objPtr, Uint64 value);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class ISwapChain : DilligentObject
    {
        public ISwapChain(IntPtr objPtr) : base(objPtr)
        {
        }

        public void Present()
        {
            ISwapChain_Present(objPtr);
        }

        public void Resize(UInt32 width, UInt32 height)
        {
            ISwapChain_Resize(objPtr, width, height);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ISwapChain_Present(IntPtr objPtr);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ISwapChain_Resize(IntPtr objPtr, UInt32 NewWidth, UInt32 NewHeight);
    }
}
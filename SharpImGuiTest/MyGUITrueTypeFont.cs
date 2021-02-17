using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    class MyGUITrueTypeFont : IDisposable
    {
        public const String LibraryName = "MyGUIFontLoader.dll";
        private IntPtr objPtr;

        public MyGUITrueTypeFont(byte[] fontBytes)
        {
            objPtr = MyGUIFontLoader_LoadFont(fontBytes, new UIntPtr((uint)fontBytes.Length));
        }

        public void Dispose()
        {
            MyGUIFontLoader_DestoryFont(objPtr);
        }


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MyGUIFontLoader_LoadFont(byte[] fontBuffer, UIntPtr fontBufferSize);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MyGUIFontLoader_DestoryFont(IntPtr obj);
    }
}

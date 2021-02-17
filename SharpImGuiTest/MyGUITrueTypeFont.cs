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
        [StructLayout(LayoutKind.Sequential)]
        struct CharMapPassStruct
        {
            uint key;
            uint value;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct GlyphInfoPassStruct
        {
            uint key;
            GlyphInfo value;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct FontInfoPassStruct
        {
            public UIntPtr charMapLength;
            public UIntPtr glyphInfoLength;
            public uint substituteCodePoint;
            public GlyphInfo substituteGlyphInfo;
        };

        public const String LibraryName = "MyGUIFontLoader.dll";
        private IntPtr objPtr;

        public unsafe MyGUITrueTypeFont(byte[] fontBytes)
        {
            objPtr = MyGUIFontLoader_LoadFont(fontBytes, new UIntPtr((uint)fontBytes.Length));
            var fontInfo = MyGUIFontLoader_GetFontInfo(objPtr);
            var charMapPass = new CharMapPassStruct[fontInfo.charMapLength.ToUInt64()];
            var glyphInfoPass = new GlyphInfoPassStruct[fontInfo.charMapLength.ToUInt64()];

            fixed (CharMapPassStruct* pCharMap = charMapPass)
            fixed (GlyphInfoPassStruct* pGlyphInfo = glyphInfoPass)
            {
                MyGUIFontLoader_GetArrayInfo(objPtr, new IntPtr(pCharMap), new IntPtr(pGlyphInfo));
            }
        }

        public void Dispose()
        {
            MyGUIFontLoader_DestoryFont(objPtr);
        }


        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MyGUIFontLoader_LoadFont(byte[] fontBuffer, UIntPtr fontBufferSize);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern FontInfoPassStruct MyGUIFontLoader_GetFontInfo(IntPtr font);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MyGUIFontLoader_GetArrayInfo(IntPtr font, IntPtr charMap, IntPtr glyphInfo);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MyGUIFontLoader_DestoryFont(IntPtr obj);
    }
}

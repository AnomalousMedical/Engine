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
            public uint key;
            public uint value;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct GlyphInfoPassStruct
        {
            public uint key;
            public GlyphInfo value;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct FontInfoPassStruct
        {
            public UIntPtr charMapLength;
            public UIntPtr glyphInfoLength;
            public uint substituteCodePoint;
            public GlyphInfo substituteGlyphInfo;
            public IntPtr textureBuffer;
            public UIntPtr textureBufferSize;
        };

        public const String LibraryName = "MyGUIFontLoader.dll";
        private IntPtr objPtr;
        private Dictionary<uint, uint> charMap = new Dictionary<uint, uint>();
        private Dictionary<uint, GlyphInfo> glyphInfo = new Dictionary<uint, GlyphInfo>();
        private uint substituteCodePoint;
        private GlyphInfo substituteCodePointGlyphInfo;
        private IntPtr textureBuffer;
        private UIntPtr textureBufferSize;

        public unsafe MyGUITrueTypeFont(byte[] fontBytes)
        {
            objPtr = MyGUIFontLoader_LoadFont(fontBytes, new UIntPtr((uint)fontBytes.Length));
            var fontInfo = MyGUIFontLoader_GetFontInfo(objPtr);
            this.substituteCodePoint = fontInfo.substituteCodePoint;
            this.substituteCodePointGlyphInfo = fontInfo.substituteGlyphInfo;
            this.textureBuffer = fontInfo.textureBuffer;
            this.textureBufferSize = fontInfo.textureBufferSize;

            var charMapPass = new CharMapPassStruct[fontInfo.charMapLength.ToUInt64()];
            var glyphInfoPass = new GlyphInfoPassStruct[fontInfo.glyphInfoLength.ToUInt64()];

            fixed (CharMapPassStruct* pCharMap = charMapPass)
            fixed (GlyphInfoPassStruct* pGlyphInfo = glyphInfoPass)
            {
                MyGUIFontLoader_GetArrayInfo(objPtr, new IntPtr(pCharMap), new IntPtr(pGlyphInfo));
            }

            foreach(var i in charMapPass)
            {
                charMap.Add(i.key, i.value);
            }

            foreach(var i in glyphInfoPass)
            {
                glyphInfo.Add(i.key, i.value);
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

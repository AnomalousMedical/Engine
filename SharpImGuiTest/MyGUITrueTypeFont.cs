using Engine;
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
        private IntPtr objPtr;
        private Dictionary<char, uint> charMap = new Dictionary<char, uint>();
        private Dictionary<uint, GlyphInfo> glyphInfo = new Dictionary<uint, GlyphInfo>();
        private uint substituteCodePoint;
        private GlyphInfoEntryPassStruct substituteCodePointGlyphInfo;
        private IntPtr textureBuffer;
        private UIntPtr textureBufferSize;
        private int textureBufferWidth;
        private int textureBufferHeight;

        public IntPtr TextureBuffer => textureBuffer;

        public UIntPtr TextureBufferSize => textureBufferSize;

        public int TextureBufferWidth => textureBufferWidth;

        public int TextureBufferHeight => textureBufferHeight;

        public Dictionary<char, uint> CharMap => charMap;

        public Dictionary<uint, GlyphInfo> GlyphInfo => glyphInfo;

        public uint SubstituteCodePoint => substituteCodePoint;

        public GlyphInfoEntryPassStruct SubstituteCodePointGlyphInfo => substituteCodePointGlyphInfo;

        public unsafe MyGUITrueTypeFont(byte[] fontBytes)
        {
            objPtr = MyGUIFontLoader_LoadFont(fontBytes, new UIntPtr((uint)fontBytes.Length));
            var fontInfo = MyGUIFontLoader_GetFontInfo(objPtr);
            this.substituteCodePoint = fontInfo.substituteCodePoint;
            this.substituteCodePointGlyphInfo = fontInfo.substituteGlyphInfo;
            this.textureBuffer = fontInfo.textureBuffer;
            this.textureBufferSize = fontInfo.textureBufferSize;
            this.textureBufferWidth = fontInfo.textureBufferWidth;
            this.textureBufferHeight = fontInfo.textureBufferHeight;

            var charMapPass = new CharMapPassStruct[fontInfo.charMapLength.ToUInt64()];
            var glyphInfoPass = new GlyphInfoPassStruct[fontInfo.glyphInfoLength.ToUInt64()];

            fixed (CharMapPassStruct* pCharMap = charMapPass)
            fixed (GlyphInfoPassStruct* pGlyphInfo = glyphInfoPass)
            {
                MyGUIFontLoader_GetArrayInfo(objPtr, new IntPtr(pCharMap), new IntPtr(pGlyphInfo));
            }

            foreach(var i in charMapPass)
            {
                charMap.Add((char)i.key, i.value); //Typecast only at load time this way
            }

            //Convert these to ref types. That will be better for overall perf frame by frame.
            foreach(var i in glyphInfoPass)
            {
                var src = i.value;
                glyphInfo.Add(i.key, new GlyphInfo()
                {
                    codePoint = src.codePoint,
                    width = src.width,
                    height = src.height,
                    advance = src.advance,
                    bearingX = src.bearingX,
                    bearingY = src.bearingY,
                    uvRect = src.uvRect,
                });
            }
        }

        public void Dispose()
        {
            MyGUIFontLoader_DestoryFont(objPtr);
        }

        public const String LibraryName = "MyGUIFontLoader.dll";
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MyGUIFontLoader_LoadFont(byte[] fontBuffer, UIntPtr fontBufferSize);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern FontInfoPassStruct MyGUIFontLoader_GetFontInfo(IntPtr font);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MyGUIFontLoader_GetArrayInfo(IntPtr font, IntPtr charMap, IntPtr glyphInfo);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MyGUIFontLoader_DestoryFont(IntPtr obj);

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
            public GlyphInfoEntryPassStruct value;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct FontInfoPassStruct
        {
            public UIntPtr charMapLength;
            public UIntPtr glyphInfoLength;
            public uint substituteCodePoint;
            public GlyphInfoEntryPassStruct substituteGlyphInfo;
            public IntPtr textureBuffer;
            public UIntPtr textureBufferSize;
            public int textureBufferWidth;
            public int textureBufferHeight;
        };
    }
}

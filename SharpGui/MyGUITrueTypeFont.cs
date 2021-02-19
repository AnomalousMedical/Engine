using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpGui
{
    [StructLayout(LayoutKind.Sequential)]
    struct MyGUITrueTypeFontDesc
    {
        /// <summary>
        /// Size of the font, in points (there are 72 points per inch).
        /// </summary>
        public float size;

        /// <summary>
        /// Resolution of the font, in pixels per inch.
        /// </summary>
        public uint resolution;

        /// <summary>
        /// Whether or not to anti-alias the font by copying its alpha channel to its luminance channel.
        /// </summary>
        public bool antialias;

        /// <summary>
        /// The width of the "Tab" special character, in pixels.
        /// </summary>
        public float tabWidth;

        /// <summary>
        /// How far up to nudge text rendered in this font, in pixels. May be negative to nudge text down.
        /// </summary>
        public int offsetHeight;

        /// <summary>
        /// The code point to use as a substitute for code points that don't exist in the font.
        /// </summary>
        public uint substituteCodePoint;

        public static MyGUITrueTypeFontDesc CreateDefault(IScaleHelper scaleHelper)
        {
            return new MyGUITrueTypeFontDesc() {
                size = 25,
                resolution = scaleHelper.Scaled(96u),
                antialias = false,
                tabWidth = 8,
                offsetHeight = 0,
                substituteCodePoint = 1073
            };
        }
    }

    class MyGUITrueTypeFont : IDisposable
    {
        private IntPtr objPtr;
        private Dictionary<char, uint> charMap = new Dictionary<char, uint>();
        private Dictionary<uint, GlyphInfo> glyphInfo = new Dictionary<uint, GlyphInfo>();
        private uint substituteCodePoint;
        private GlyphInfo substituteCodePointGlyphInfo;
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

        public GlyphInfo SubstituteCodePointGlyphInfo => substituteCodePointGlyphInfo;

        public unsafe MyGUITrueTypeFont(MyGUITrueTypeFontDesc desc, byte[] fontBytes)
        {
            objPtr = MyGUIFontLoader_LoadFont(ref desc, fontBytes, new UIntPtr((uint)fontBytes.Length));
            var fontInfo = MyGUIFontLoader_GetFontInfo(objPtr);
            this.substituteCodePoint = fontInfo.substituteCodePoint;
            this.substituteCodePointGlyphInfo = fontInfo.substituteGlyphInfo.ToGlyphInfo();
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
                glyphInfo.Add(i.key, src.ToGlyphInfo());
            }
        }

        public void Dispose()
        {
            MyGUIFontLoader_DestoryFont(objPtr);
        }

        public const String LibraryName = "MyGUIFontLoader.dll";
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MyGUIFontLoader_LoadFont(ref MyGUITrueTypeFontDesc fontDesc, byte[] fontBuffer, UIntPtr fontBufferSize);

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

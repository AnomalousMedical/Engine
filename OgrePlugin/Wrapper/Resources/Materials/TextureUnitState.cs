using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public enum TextureAddressingMode
    {
        /// Texture wraps at values over 1.0.
        TAM_WRAP,
        /// Texture mirrors (flips) at joins over 1.0.
        TAM_MIRROR,
        /// Texture clamps at 1.0.
        TAM_CLAMP,
        /// Texture coordinates outside the range [0.0, 1.0] are set to the border colour.
        TAM_BORDER,
        /// Unknown
        TAM_UNKNOWN = 99
    };

    public enum FilterOptions
    {
        None,
        Point,
        Linear,
        Anisotropic
    }

    public class TextureUnitState : IDisposable
    {
        enum FilterType
        {
            Min,
            Mag,
            Mip
        }

        internal static TextureUnitState createWrapper(IntPtr nativeObject, object[] args)
        {
            return new TextureUnitState(nativeObject);
        }

        IntPtr textureUnit;

        private TextureUnitState(IntPtr textureUnit)
        {
            this.textureUnit = textureUnit;
        }

        public void Dispose()
        {
            textureUnit = IntPtr.Zero;
        }

        public String TextureName
        {
            get
            {
                return Marshal.PtrToStringAnsi(TextureUnitState_getTextureName(textureUnit));
            }
            set
            {
                TextureUnitState_setTextureName(textureUnit, value);
            }
        }

        public String Name
        {
            get
            {
                return Marshal.PtrToStringAnsi(TextureUnitState_getName(textureUnit));
            }
        }

        public FilterOptions MinFilter
        {
            get
            {
                return TextureUnitState_getTextureFiltering(textureUnit, FilterType.Min);
            }
            set
            {
                TextureUnitState_setTextureFiltering(textureUnit, FilterType.Min, value);
            }
        }

        public FilterOptions MagFilter
        {
            get
            {
                return TextureUnitState_getTextureFiltering(textureUnit, FilterType.Mag);
            }
            set
            {
                TextureUnitState_setTextureFiltering(textureUnit, FilterType.Mag, value);
            }
        }

        public FilterOptions MipFilter
        {
            get
            {
                return TextureUnitState_getTextureFiltering(textureUnit, FilterType.Mip);
            }
            set
            {
                TextureUnitState_setTextureFiltering(textureUnit, FilterType.Mip, value);
            }
        }

        public TextureAddressingMode AddressModeU
        {
            get
            {
                return TextureUnitState_getTextureAddressingModeU(textureUnit);
            }
            set
            {
                TextureUnitState_setTextureAddressingModeU(textureUnit, value);
            }
        }

        public TextureAddressingMode AddressModeV
        {
            get
            {
                return TextureUnitState_getTextureAddressingModeV(textureUnit);
            }
            set
            {
                TextureUnitState_setTextureAddressingModeV(textureUnit, value);
            }
        }

        public TextureAddressingMode AddressModeW
        {
            get
            {
                return TextureUnitState_getTextureAddressingModeW(textureUnit);
            }
            set
            {
                TextureUnitState_setTextureAddressingModeW(textureUnit, value);
            }
        }

        public void setAdressingMode(TextureAddressingMode u, TextureAddressingMode v, TextureAddressingMode w)
        {
            TextureUnitState_setTextureAddressingModeUVW(textureUnit, u, v, w);
        }

        public void setAdressingMode(TextureAddressingMode tam)
        {
            TextureUnitState_setTextureAddressingMode(textureUnit, tam);
        }

        public void setFilteringOptions(FilterOptions minFilter, FilterOptions magFilter, FilterOptions mipFilter)
        {
            TextureUnitState_setTextureFiltering2(textureUnit, minFilter, magFilter, mipFilter);
        }

        internal IntPtr OgreState
        {
            get
            {
                return textureUnit;
            }
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr TextureUnitState_getTextureName(IntPtr textureUnit);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureName(IntPtr textureUnit, String name);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TextureUnitState_getName(IntPtr textureUnit);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern FilterOptions TextureUnitState_getTextureFiltering(IntPtr textureUnit, FilterType filterType);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureFiltering(IntPtr textureUnit, FilterType filterType, FilterOptions option);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureFiltering2(IntPtr textureUnit, FilterOptions minFilter, FilterOptions magFilter, FilterOptions mipFilter);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern TextureAddressingMode TextureUnitState_getTextureAddressingModeU(IntPtr textureUnit);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern TextureAddressingMode TextureUnitState_getTextureAddressingModeV(IntPtr textureUnit);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern TextureAddressingMode TextureUnitState_getTextureAddressingModeW(IntPtr textureUnit);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureAddressingModeU(IntPtr textureUnit, TextureAddressingMode tam);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureAddressingModeV(IntPtr textureUnit, TextureAddressingMode tam);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureAddressingModeW(IntPtr textureUnit, TextureAddressingMode tam);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureAddressingModeUVW(IntPtr textureUnit, TextureAddressingMode u, TextureAddressingMode v, TextureAddressingMode w);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureUnitState_setTextureAddressingMode(IntPtr textureUnit, TextureAddressingMode tam);

#endregion
    }
}

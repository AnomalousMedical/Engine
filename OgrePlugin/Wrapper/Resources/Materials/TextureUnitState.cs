using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
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

#endregion
    }
}

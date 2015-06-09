using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public class TextureUnitState : IDisposable
    {
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

#endregion
    }
}

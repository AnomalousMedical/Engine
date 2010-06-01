using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
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

        public String getTextureName()
        {
            return Marshal.PtrToStringAnsi(TextureUnitState_getTextureName(textureUnit));
        }

        public void setTextureName(String name)
        {
            TextureUnitState_setTextureName(textureUnit, name);
        }

        internal IntPtr OgreState
        {
            get
            {
                return textureUnit;
            }
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr TextureUnitState_getTextureName(IntPtr textureUnit);

        [DllImport("OgreCWrapper")]
        private static extern void TextureUnitState_setTextureName(IntPtr textureUnit, String name);

#endregion
    }
}

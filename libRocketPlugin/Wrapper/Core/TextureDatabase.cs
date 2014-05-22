using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class TextureDatabase
    {
        public static void ReleaseTextures()
        {
            TextureDatabase_ReleaseTextures();
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TextureDatabase_ReleaseTextures();
        #endregion
    }
}

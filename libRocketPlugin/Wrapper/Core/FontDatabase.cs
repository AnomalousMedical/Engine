using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class FontDatabase
    {
        public static bool LoadFontFace(String fileName)
        {
            return FontDatabase_LoadFontFace(fileName);
        }

        public static bool LoadFontFace(String fileName, String family, Font.Style style, Font.Weight weight)
        {
            return FontDatabase_LoadFontFace_Opt(fileName, family, style, weight);
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool FontDatabase_LoadFontFace(String file_name);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool FontDatabase_LoadFontFace_Opt(String file_name, String family, Font.Style style, Font.Weight weight);

        #endregion
    }
}

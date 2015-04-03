using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libRocketPlugin
{
    internal static class GeometryDatabase
    {
        public static void ReleaseGeometries()
        {
            GeometryDatabase_ReleaseGeometries();
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeometryDatabase_ReleaseGeometries();

        #endregion
    }
}

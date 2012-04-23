using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class Controls
    {
        public static void Initialise()
        {
            Controls_Initialise();
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Controls_Initialise();

        #endregion
    }
}

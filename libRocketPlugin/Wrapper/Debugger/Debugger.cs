using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class Debugger
    {
        public static void Initialise(Context context)
        {
            Debugger_Initialise(context.Ptr);
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Debugger_Initialise(IntPtr context);

        #endregion
    }
}

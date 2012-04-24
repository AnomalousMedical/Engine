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

        public static bool SetContext(Context context)
        {
            return Debugger_SetContext(context.Ptr);
        }

        public static void SetVisible(bool visibility)
        {
            Debugger_SetVisible(visibility);
        }

        public static bool IsVisible()
        {
            return Debugger_IsVisible();
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Debugger_Initialise(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Debugger_SetContext(IntPtr context);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Debugger_SetVisible(bool visibility);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Debugger_IsVisible();

        #endregion
    }
}

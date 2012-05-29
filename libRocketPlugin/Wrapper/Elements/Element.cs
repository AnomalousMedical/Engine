using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class Element : ReferenceCountable
    {
        internal Element(IntPtr ptr)
            : base(ptr)
        {

        }

        public bool Focus()
        {
            return Element_Focus(ptr);
        }

        public void Blur()
        {
            Element_Blur(ptr);
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Element_Focus(IntPtr element);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Element_Blur(IntPtr element);

        #endregion
    }
}

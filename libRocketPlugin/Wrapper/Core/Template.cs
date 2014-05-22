using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class Template : RocketNativeObject
    {
        internal static Template Create(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                return null;
            }
            return new Template(ptr);
        }

        protected Template(IntPtr ptr)
            : base(ptr)
        {

        }

        public String Name
        {
            get
            {
                return Marshal.PtrToStringAnsi(Template_GetName(ptr));
            }
        }

        public String Content
        {
            get
            {
                return Marshal.PtrToStringAnsi(Template_GetContent(ptr));
            }
        }
        
        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Template_GetName(IntPtr rktTemplate);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Template_GetContent(IntPtr rktTemplate);
    }
}

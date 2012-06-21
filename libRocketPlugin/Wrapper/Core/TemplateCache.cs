using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class TemplateCache
    {
        public static void ClearTemplateCache()
        {
            TemplateCache_Shutdown();
            TemplateCache_Initialise();
        }

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TemplateCache_Initialise();

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TemplateCache_Shutdown();
    }
}

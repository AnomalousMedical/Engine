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

        public static Template GetTemplate(String id)
        {
            return Template.Create(TemplateCache_GetTemplate(id));
        }

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TemplateCache_Initialise();

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TemplateCache_Shutdown();

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TemplateCache_GetTemplate(String id);
    }
}

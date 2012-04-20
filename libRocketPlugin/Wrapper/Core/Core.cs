using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class Core
    {
        private static bool Initialise()
        {
            return Core_Initialise();
        }

        private static void Shutdown()
        {
            Core_Shutdown();
        }

        private static void SetSystemInterface(SystemInterface system_interface)
        {
            Core_SetSystemInterface(system_interface.Ptr);
        }

        private static IntPtr GetSystemInterface()
        {
            return Core_GetSystemInterface();
        }

        private static void SetRenderInterface(RenderInterface render_interface)
        {
            Core_SetRenderInterface(render_interface.Ptr);
        }

        private static IntPtr GetRenderInterface()
        {
            return Core_GetRenderInterface();
        }

        private static void SetFileInterface(IntPtr file_interface)
        {
            throw new NotImplementedException();
            //Core_SetFileInterface();
        }

        private static IntPtr GetFileInterface()
        {
            return Core_GetFileInterface();
        }

        private static IntPtr CreateContext(String name, Vector2i dimensions, IntPtr render_interface)
        {
            throw new NotImplementedException();
            //return Core_CreateContext();
        }

        private static IntPtr GetContext(String name)
        {
            throw new NotImplementedException();
            //return Core_GetContext();
        }

        private static IntPtr GetContext(int index)
        {
            throw new NotImplementedException();
            //return Core_GetContext_Index();
        }

        private static int GetNumContexts()
        {
            return Core_GetNumContexts();
        }

        //private static void RegisterPlugin(IntPtr plugin)
        //{
        //    Core_RegisterPlugin();
        //}

        private static void ReleaseCompiledGeometries()
        {
            Core_ReleaseCompiledGeometries();
        }

        private static void ReleaseTextures()
        {
            Core_ReleaseTextures();
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Core_Initialise();

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_Shutdown();

        //[DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        //private static extern String Core_GetVersion();

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_SetSystemInterface(IntPtr system_interface);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetSystemInterface();
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_SetRenderInterface(IntPtr render_interface);
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetRenderInterface();
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_SetFileInterface(IntPtr file_interface);
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetFileInterface();
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_CreateContext(String name, Vector2i dimensions, IntPtr render_interface);
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetContext(String name);
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetContext_Index(int index);
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern int Core_GetNumContexts();
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_RegisterPlugin(IntPtr plugin);
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_ReleaseCompiledGeometries();
        
        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_ReleaseTextures();

        #endregion
    }
}

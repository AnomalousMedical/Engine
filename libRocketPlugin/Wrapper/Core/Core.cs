using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public static class Core
    {
        private static RenderInterface renderInterface;
        private static SystemInterface systemInterface;
        private static FileInterface fileInterface;

        public static bool Initialise()
        {
            return Core_Initialise();
        }

        public static void Shutdown()
        {
            Core_Shutdown();
        }

        public static void SetSystemInterface(SystemInterface systemInterface)
        {
            Core.systemInterface = systemInterface;
            Core_SetSystemInterface(systemInterface.Ptr);
        }

        public static SystemInterface GetSystemInterface()
        {
            return systemInterface;
        }

        public static void SetRenderInterface(RenderInterface renderInterface)
        {
            Core.renderInterface = renderInterface;
            Core_SetRenderInterface(renderInterface.Ptr);
        }

        public static RenderInterface GetRenderInterface()
        {
            return renderInterface;
        }

        public static void SetFileInterface(FileInterface fileInterface)
        {
            Core.fileInterface = fileInterface;
            Core_SetFileInterface(fileInterface.Ptr);
        }

        public static FileInterface GetFileInterface()
        {
            return fileInterface;
        }

        public static Context CreateContext(String name, Vector2i dimensions)
        {
            return ElementManager.getContext(Core_CreateContext(name, dimensions, IntPtr.Zero));
        }

        public static Context CreateContext(String name, Vector2i dimensions, IntPtr renderInterface)
        {
            return ElementManager.getContext(Core_CreateContext(name, dimensions, renderInterface));
        }

        public static Context GetContext(String name)
        {
            return ElementManager.getContext(Core_GetContext(name));
        }

        public static Context GetContext(int index)
        {
            return ElementManager.getContext(Core_GetContext_Index(index));
        }

        public static int GetNumContexts()
        {
            return Core_GetNumContexts();
        }

        //private static void RegisterPlugin(IntPtr plugin)
        //{
        //    Core_RegisterPlugin();
        //}

        public static void ReleaseCompiledGeometries()
        {
            Core_ReleaseCompiledGeometries();
        }

        public static void ReleaseTextures()
        {
            Core_ReleaseTextures();
        }
        
        /// <summary>
        /// libRocket expects its paths to have only / in it so this will fix it up for you.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static String MakeSafePath(String path)
        {
            return path.Replace('\\', '/');
        }

        /// <summary>
        /// Get an enumerator over all contexts.
        /// </summary>
        public static IEnumerable<Context> Contexts
        {
            get
            {
                for(int i = 0; i < GetNumContexts(); ++i)
                {
                    yield return GetContext(i);
                }
            }
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Core_Initialise();

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_Shutdown();

        //[DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        //private static extern String Core_GetVersion();

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_SetSystemInterface(IntPtr system_interface);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetSystemInterface();
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_SetRenderInterface(IntPtr render_interface);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetRenderInterface();
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_SetFileInterface(IntPtr file_interface);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetFileInterface();
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_CreateContext(String name, Vector2i dimensions, IntPtr render_interface);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetContext(String name);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr Core_GetContext_Index(int index);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern int Core_GetNumContexts();
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_RegisterPlugin(IntPtr plugin);
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_ReleaseCompiledGeometries();
        
        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Core_ReleaseTextures();

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OgreWrapper
{
    class GpuProgramManager
    {
        public static GpuProgramManager Instance { get; private set; }

        static GpuProgramManager()
        {
            Instance = new GpuProgramManager(); 
        }

        public bool SaveMicrocodesToCache
        {
            get
            {
                return GpuProgramManager_getSaveMicrocodesToCache();
            }
            set
            {
                GpuProgramManager_setSaveMicrocodesToCache(value);
            }
        }

        public void saveMicrocodeCache(OgreManagedStream stream)
        {
            GpuProgramManager_saveMicrocodeCache(stream.NativeStream);
        }

        public void loadMicrocodeCache(OgreManagedStream stream)
        {
            GpuProgramManager_loadMicrocodeCache(stream.NativeStream);
        }

#region PInvoke
        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void GpuProgramManager_setSaveMicrocodesToCache(bool val);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgramManager_getSaveMicrocodesToCache();

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void GpuProgramManager_saveMicrocodeCache(IntPtr dataStream);

        [DllImport("OgreCWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgramManager_loadMicrocodeCache(IntPtr dataStream);
#endregion
    }
}

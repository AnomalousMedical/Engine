using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OgreWrapper
{
    public class GpuProgramManager
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

        /// <summary>
        /// Save the microcode cache to the given stream. The stream will be controlled
        /// by ogre which will close it when it is done, however, you should still be able
        /// to follow normal using patterns on the stream you pass in.
        /// </summary>
        /// <param name="stream">The stream to save to, must be writable.</param>
        public void saveMicrocodeCache(Stream stream)
        {
            OgreManagedStream managedStream = new OgreManagedStream("MicrocodeSaveStream", stream);
            GpuProgramManager_saveMicrocodeCache(managedStream.NativeStream);
        }

        /// <summary>
        /// Load the microcode cache from the given stream. The stream will be controlled
        /// by ogre which will close it when it is done, however, you should still be able
        /// to follow normal using patterns on the stream you pass in.
        /// </summary>
        /// <param name="stream">The stream to load from, must be readable.</param>
        public void loadMicrocodeCache(Stream stream)
        {
            OgreManagedStream managedStream = new OgreManagedStream("MicrocodeLoadStream", stream);
            GpuProgramManager_loadMicrocodeCache(managedStream.NativeStream);
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

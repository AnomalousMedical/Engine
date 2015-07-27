using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OgrePlugin
{
    public class GpuProgramManager : IDisposable
    {
        public static GpuProgramManager Instance { get; private set; }

#if FULL_AOT_COMPILE
        [Anomalous.Interop.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
        public static void processWrapperObject_AOT(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            Instance.gpuProgramParametersWrappers.processWrapperObject(nativeObject, stackSharedPtr);
        }
#endif

        static GpuProgramManager()
        {
            Instance = new GpuProgramManager(); 
        }

        private SharedPtrCollection<GpuProgramParameters> gpuProgramParametersWrappers = new SharedPtrCollection<GpuProgramParameters>(GpuProgramParameters.createWrapper, GpuProgramParameters_createHeapPtr, GpuProgramParameters_Delete
#if FULL_AOT_COMPILE
            , processWrapperObject_AOT
#endif
);
        public void Dispose()
        {
            gpuProgramParametersWrappers.Dispose();
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

        /// <summary>
        /// Get a wrapper around a pointer to a ogre shared pointer for a gpuprogramparameters object.
        /// </summary>
        /// <param name="gpuProgramParametersSharedPtr"></param>
        /// <returns></returns>
        internal GpuProgramParametersSharedPtr getGpuProgramParametersWrapper(IntPtr gpuProgramParametersSharedPtr)
        {
            return new GpuProgramParametersSharedPtr(gpuProgramParametersWrappers.getObject(gpuProgramParametersSharedPtr));
        }

        internal ProcessWrapperObjectDelegate ProcessWrapperObjectCallback
        {
            get
            {
                return gpuProgramParametersWrappers.ProcessWrapperCallback;
            }
        }

#region PInvoke
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void GpuProgramManager_setSaveMicrocodesToCache(bool val);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool GpuProgramManager_getSaveMicrocodesToCache();

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void GpuProgramManager_saveMicrocodeCache(IntPtr dataStream);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GpuProgramManager_loadMicrocodeCache(IntPtr dataStream);

        //GpuProgramParameters SharedPtr
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr GpuProgramParameters_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void GpuProgramParameters_Delete(IntPtr heapSharedPtr);

#endregion
    }
}

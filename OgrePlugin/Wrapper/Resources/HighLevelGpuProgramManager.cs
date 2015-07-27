using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public enum GpuProgramType
    {
        GPT_VERTEX_PROGRAM,
        GPT_FRAGMENT_PROGRAM,
        GPT_GEOMETRY_PROGRAM,
        GPT_DOMAIN_PROGRAM,
        GPT_HULL_PROGRAM,
        GPT_COMPUTE_PROGRAM
    };

    public class HighLevelGpuProgramManager : IDisposable
    {
        public static HighLevelGpuProgramManager Instance { get; private set; }

#if FULL_AOT_COMPILE
        [Anomalous.Interop.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
        public static void processWrapperObject_AOT(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            Instance.gpuProgramWrappers.processWrapperObject(nativeObject, stackSharedPtr);
        }
#endif

        static HighLevelGpuProgramManager()
        {
            Instance = new HighLevelGpuProgramManager(); 
        }

        private SharedPtrCollection<HighLevelGpuProgram> gpuProgramWrappers = new SharedPtrCollection<HighLevelGpuProgram>(HighLevelGpuProgram.createWrapper, HighLevelGpuProgram_createHeapPtr, HighLevelGpuProgram_Delete
#if FULL_AOT_COMPILE
            , processWrapperObject_AOT
#endif
);
        public HighLevelGpuProgramManager()
        {

        }

        public void Dispose()
        {
            gpuProgramWrappers.Dispose();
        }

        public HighLevelGpuProgramSharedPtr createProgram(String name, String groupName, String language, GpuProgramType programType)
        {
            return getObject(HighLevelGpuProgramManager_createProgram(name, groupName, language, programType, gpuProgramWrappers.ProcessWrapperCallback));
        }

        public HighLevelGpuProgramSharedPtr getByName(String name)
        {
            return getObject(HighLevelGpuProgramManager_getByName1(name, gpuProgramWrappers.ProcessWrapperCallback));
        }

        public HighLevelGpuProgramSharedPtr getByName(String name, String groupName)
        {
            return getObject(HighLevelGpuProgramManager_getByName2(name, groupName, gpuProgramWrappers.ProcessWrapperCallback));
        }

        internal HighLevelGpuProgramSharedPtr getObject(IntPtr nativeGpuProgram)
        {
            return new HighLevelGpuProgramSharedPtr(gpuProgramWrappers.getObject(nativeGpuProgram));
        }

#region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr HighLevelGpuProgramManager_createProgram(String name, String group, String language, GpuProgramType gptype, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr HighLevelGpuProgramManager_getByName1(String name, ProcessWrapperObjectDelegate processWrapper);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr HighLevelGpuProgramManager_getByName2(String name, String group, ProcessWrapperObjectDelegate processWrapper);
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr HighLevelGpuProgram_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void HighLevelGpuProgram_Delete(IntPtr heapSharedPtr);

#endregion
    }
}

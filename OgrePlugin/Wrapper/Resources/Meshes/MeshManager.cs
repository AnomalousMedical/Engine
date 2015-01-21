using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class MeshManager : IDisposable
    {
        static MeshManager instance = new MeshManager();

#if FULL_AOT_COMPILE
        [MonoTouch.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
        public static void processWrapperObject_AOT(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            instance.meshPtrCollection.processWrapperObject(nativeObject, stackSharedPtr);
        }
#endif

        public static MeshManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Mesh> meshPtrCollection;

        private MeshManager()
        {
            meshPtrCollection = new SharedPtrCollection<Mesh>(Mesh.createWrapper, MeshPtr_createHeapPtr, MeshPtr_Delete
            #if FULL_AOT_COMPILE
            , processWrapperObject_AOT
            #endif
            );
        }

        public void Dispose()
        {
            meshPtrCollection.Dispose();
        }

        internal MeshPtr getObject(IntPtr nativeMaterial)
        {
            return new MeshPtr(meshPtrCollection.getObject(nativeMaterial));
        }

        internal ProcessWrapperObjectDelegate ProcessWrapperObjectCallback
        {
            get
            {
                return meshPtrCollection.ProcessWrapperCallback;
            }
        }

#region PInvoke

        //MeshPtr
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MeshPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MeshPtr_Delete(IntPtr heapSharedPtr);

#endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class MeshManager : IDisposable
    {
        static MeshManager instance = new MeshManager();

#if FULL_AOT_COMPILE
        [Anomalous.Interop.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
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
            PerformanceMonitor.addValueProvider("Ogre Mesh Memory Usage", () => Prettify.GetSizeReadable(MemoryUsage));
            meshPtrCollection = new SharedPtrCollection<Mesh>(Mesh.createWrapper, MeshPtr_createHeapPtr, MeshPtr_Delete
            #if FULL_AOT_COMPILE
            , processWrapperObject_AOT
            #endif
            );
        }

        public void Dispose()
        {
            PerformanceMonitor.removeValueProvider("Ogre Mesh Memory Usage");
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

        public long MemoryUsage
        {
            get
            {
                return MeshManager_getMemoryUsage().ToInt64();
            }
        }

        #region PInvoke

        //MeshPtr
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr MeshPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void MeshPtr_Delete(IntPtr heapSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MeshManager_getMemoryUsage();

        #endregion
    }
}

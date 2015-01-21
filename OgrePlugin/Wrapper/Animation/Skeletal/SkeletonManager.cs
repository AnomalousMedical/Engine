using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class SkeletonManager : IDisposable
    {
        static SkeletonManager instance = new SkeletonManager();

#if FULL_AOT_COMPILE
        [MonoTouch.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
        public static void processWrapperObject_AOT(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            instance.skeletonPtrCollection.processWrapperObject(nativeObject, stackSharedPtr);
        }
#endif

        public static SkeletonManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Skeleton> skeletonPtrCollection;

        private SkeletonManager()
        {
            skeletonPtrCollection = new SharedPtrCollection<Skeleton>(Skeleton.createWrapper, SkeletonPtr_createHeapPtr, SkeletonPtr_Delete
#if FULL_AOT_COMPILE
            , processWrapperObject_AOT
#endif
            );
        }

        public void Dispose()
        {
            skeletonPtrCollection.Dispose();
        }

        internal SkeletonPtr getObject(IntPtr nativeMaterial)
        {
            return new SkeletonPtr(skeletonPtrCollection.getObject(nativeMaterial));
        }

        internal ProcessWrapperObjectDelegate ProcessWrapperObjectCallback
        {
            get
            {
                return skeletonPtrCollection.ProcessWrapperCallback;
            }
        }

        #region PInvoke

        //MeshPtr
        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SkeletonPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void SkeletonPtr_Delete(IntPtr heapSharedPtr);

        #endregion
    }
}

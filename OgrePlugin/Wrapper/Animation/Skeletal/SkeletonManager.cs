using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class SkeletonManager : IDisposable
    {
        static SkeletonManager instance = new SkeletonManager();

        public static SkeletonManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Skeleton> skeletonPtrCollection = new SharedPtrCollection<Skeleton>(Skeleton.createWrapper, SkeletonPtr_createHeapPtr, SkeletonPtr_Delete);

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
        [DllImport("OgreCWrapper")]
        private static extern IntPtr SkeletonPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport("OgreCWrapper")]
        private static extern void SkeletonPtr_Delete(IntPtr heapSharedPtr);

        #endregion
    }
}

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

        public static MeshManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Mesh> meshPtrCollection = new SharedPtrCollection<Mesh>(Mesh.createWrapper, MeshPtr_createHeapPtr, MeshPtr_Delete);

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

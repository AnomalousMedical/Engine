using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class MaterialManager : IDisposable
    {
        static MaterialManager instance = new MaterialManager();

        public static MaterialManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Material> materialPtrCollection = new SharedPtrCollection<Material>(Material.createWrapper, MaterialPtr_createHeapPtr, MaterialPtr_Delete);

        public MaterialManager()
        {
            
        }

        public void Dispose()
        {
            materialPtrCollection.Dispose();
        }

        public MaterialPtr getByName(String name)
        {
            return getObject(MaterialManager_getByName(name, ProcessWrapperObjectCallback));
        }

        public bool resourceExists(String name)
        {
            return MaterialManager_resourceExists(name);
        }

        public String getActiveScheme()
        {
            return Marshal.PtrToStringAnsi(MaterialManager_getActiveScheme());
        }

        public void setActiveScheme(String name)
        {
            MaterialManager_setActiveScheme(name);
        }

        internal MaterialPtr getObject(IntPtr nativeMaterial)
        {
            return new MaterialPtr(materialPtrCollection.getObject(nativeMaterial));
        }

        internal ProcessWrapperObjectDelegate ProcessWrapperObjectCallback
        {
            get
            {
                return materialPtrCollection.ProcessWrapperCallback;
            }
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern IntPtr MaterialManager_getByName(String name, ProcessWrapperObjectDelegate processWrapperCallback);

        [DllImport("OgreCWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MaterialManager_resourceExists(String name);

        [DllImport("OgreCWrapper")]
        private static extern IntPtr MaterialManager_getActiveScheme();

        [DllImport("OgreCWrapper")]
        private static extern void MaterialManager_setActiveScheme(String name);

        //MaterialPtr
        [DllImport("OgreCWrapper")]
        private static extern IntPtr MaterialPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport("OgreCWrapper")]
        private static extern void MaterialPtr_Delete(IntPtr heapSharedPtr);

#endregion
    }
}


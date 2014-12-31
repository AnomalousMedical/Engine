using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public abstract class OgreManagedArchiveFactory : IDisposable
    {
        CreateInstanceDelegate createInstanceCallback;
        DestroyInstanceDelegate destroyInstanceCallback;

        private Dictionary<IntPtr, OgreManagedArchive> archives = new Dictionary<IntPtr, OgreManagedArchive>(); //keep handles around in this list

        private IntPtr nativeFactory;

        internal IntPtr NativeFactory
        {
            get
            {
                return nativeFactory;
            }
        }

        public OgreManagedArchiveFactory(String archType)
        {
            createInstanceCallback = new CreateInstanceDelegate(createInstance);
            destroyInstanceCallback = new DestroyInstanceDelegate(destroyInstance);

            nativeFactory = OgreManagedArchiveFactory_Create(archType, createInstanceCallback, destroyInstanceCallback);
        }

        public virtual void Dispose()
        {
            OgreManagedArchiveFactory_Delete(nativeFactory);

            createInstanceCallback = null;
            destroyInstanceCallback = null;
        }

	    IntPtr createInstance(String name)
        {
            OgreManagedArchive archive = doCreateInstance(name);
            archives.Add(archive.NativeArchive, archive);
            return archive.NativeArchive;
        }

        protected abstract OgreManagedArchive doCreateInstance(String name);
        
        void destroyInstance(IntPtr arch)
        {
            OgreManagedArchive archive = archives[arch];
            archive.Dispose();
            archives.Remove(arch);
        }

#region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr CreateInstanceDelegate(String name);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void DestroyInstanceDelegate(IntPtr arch);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedArchiveFactory_Create(String archType, CreateInstanceDelegate createInstanceCallback, DestroyInstanceDelegate destroyInstanceCallback);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreManagedArchiveFactory_Delete(IntPtr archive);

#endregion
    }
}

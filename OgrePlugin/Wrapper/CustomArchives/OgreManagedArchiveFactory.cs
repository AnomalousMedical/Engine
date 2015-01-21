using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Anomalous.Interop;

namespace OgrePlugin
{
    public abstract class OgreManagedArchiveFactory : IDisposable
    {
        private Dictionary<IntPtr, OgreManagedArchive> archives = new Dictionary<IntPtr, OgreManagedArchive>(); //keep handles around in this list

        private IntPtr nativeFactory;
        private CallbackHandler callbackHandler;

        internal IntPtr NativeFactory
        {
            get
            {
                return nativeFactory;
            }
        }

        public OgreManagedArchiveFactory(String archType)
        {
            callbackHandler = new CallbackHandler();
            nativeFactory = callbackHandler.createNative(this, archType);
        }

        public virtual void Dispose()
        {
            OgreManagedArchiveFactory_Delete(nativeFactory);
            callbackHandler.Dispose();
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

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedArchiveFactory_Create(String archType, NativeFunc_String_StrongIntPtr createInstanceCallback, NativeAction_StrongIntPtr destroyInstanceCallback
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreManagedArchiveFactory_Delete(IntPtr archive);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static NativeFunc_String_StrongIntPtr createInstanceCallback;
            private static NativeAction_StrongIntPtr destroyInstanceCallback;

            static CallbackHandler()
            {
                createInstanceCallback = new NativeFunc_String_StrongIntPtr(CreateInstanceStatic);
                destroyInstanceCallback = new NativeAction_StrongIntPtr(DestroyInstanceStatic);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeFunc_String_StrongIntPtr))]
            static IntPtr CreateInstanceStatic(String name, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedArchiveFactory).createInstance(name);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction_StrongIntPtr))]
            static void DestroyInstanceStatic(IntPtr arch, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreManagedArchiveFactory).destroyInstance(arch);
            }

            private GCHandle handle;

            public CallbackHandler()
            {
                
            }

            public void Dispose()
            {
                handle.Free();
            }

            public IntPtr createNative(OgreManagedArchiveFactory obj, String archType)
            {
                handle = GCHandle.Alloc(obj);
                return OgreManagedArchiveFactory_Create(archType, createInstanceCallback, destroyInstanceCallback, GCHandle.ToIntPtr(handle));
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            NativeFunc_String_StrongIntPtr createInstanceCallback;
            NativeAction_StrongIntPtr destroyInstanceCallback;

            public CallbackHandler()
            {
                
            }

            public void Dispose()
            {

            }

            public IntPtr createNative(OgreManagedArchiveFactory obj, String archType)
            {
                createInstanceCallback = new NativeFunc_String_StrongIntPtr(obj.createInstance);
                destroyInstanceCallback = new NativeAction_StrongIntPtr(obj.destroyInstance);

                return OgreManagedArchiveFactory_Create(archType, createInstanceCallback, destroyInstanceCallback);
            }
        }
#endif

#endregion
    }
}

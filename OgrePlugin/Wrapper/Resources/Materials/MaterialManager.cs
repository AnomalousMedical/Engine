using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class MaterialManager : IDisposable
    {
        public const String DefaultSchemeName = "Default";

        public delegate void HandleSchemeNotFoundDelegate(ushort schemeIndex, String schemeName, Material originalMaterial, ushort lodIndex, out Technique technique);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr HandleSchemeNotFoundCb(ushort schemeIndex, String schemeName, IntPtr originalMaterial, ushort lodIndex, IntPtr rend);

        static MaterialManager instance = new MaterialManager();

#if FULL_AOT_COMPILE
        [Anomalous.Interop.MonoPInvokeCallback(typeof(ProcessWrapperObjectDelegate))]
        public static void processWrapperObject_AOT(IntPtr nativeObject, IntPtr stackSharedPtr)
        {
            instance.materialPtrCollection.processWrapperObject(nativeObject, stackSharedPtr);
        }
#endif

        public static MaterialManager getInstance()
        {
            return instance;
        }

        private SharedPtrCollection<Material> materialPtrCollection;
        private HandleSchemeNotFoundCb handleSchemeNotFoundCb;
        private IntPtr materialManagerListener = IntPtr.Zero;

        private MaterialManager()
        {
            materialPtrCollection = new SharedPtrCollection<Material>(Material.createWrapper, MaterialPtr_createHeapPtr, MaterialPtr_Delete
#if FULL_AOT_COMPILE
                , processWrapperObject_AOT
#endif
);
        }

        public void Dispose()
        {
            materialPtrCollection.Dispose();
        }

        public MaterialPtr getByName(String name)
        {
            return getObject(MaterialManager_getByName(name, ProcessWrapperObjectCallback));
        }

        public MaterialPtr create(String name, String group, bool isManual, ManagedManualResourceLoader loader)
        {
            return getObject(MaterialManager_create(name, group, isManual, loader != null ? loader.Ptr : IntPtr.Zero, ProcessWrapperObjectCallback));
        }

        public void remove(String name)
        {
            MaterialManager_removeName(name);
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

        /// <summary>
        /// Get an iterator over all materials, note that you are getting direct material objects not
        /// MaterialPtrs, so you cannot store the returned materials, if you need the returned materials
        /// call getByName to retrieve them. This helps avoid creating a wrapper for every single material
        /// in the enum. Also note that this iterator itself needs unmanaged resources to work, use it in a
        /// using block or make use of foreach to iterate over it or you will leak the native iterator.
        /// </summary>
        public IEnumerable<Material> Iterator
        {
            get
            {
                IntPtr iter = MaterialManager_beginResourceIterator();
                try
                {
                    while(MaterialManager_resourceIteratorHasMoreElements(iter))
                    {
                        yield return instance.materialPtrCollection.getTemporaryObject(MaterialManager_resourceIteratorNext(iter), ptr => Material.createWrapper(ptr));
                    }
                }
                finally
                {
                    MaterialManager_endResourceIterator(iter);
                }
            }
        }

        private event HandleSchemeNotFoundDelegate mHandleSchemeNotFound;
        public event HandleSchemeNotFoundDelegate HandleSchemeNotFound
        {
            add
            {
                if (mHandleSchemeNotFound == null)
                {
                    //Create material listener
                    handleSchemeNotFoundCb = new HandleSchemeNotFoundCb(fireSchemeNotFound);
                    materialManagerListener = NativeMaterialListener_create(handleSchemeNotFoundCb);
                }
                mHandleSchemeNotFound += value;
            }
            remove
            {
                mHandleSchemeNotFound -= value;
                if (mHandleSchemeNotFound == null)
                {
                    //Destroy material listener
                    handleSchemeNotFoundCb = null;
                    NativeMaterialListener_delete(materialManagerListener);
                    materialManagerListener = IntPtr.Zero;
                }
            }

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

        IntPtr fireSchemeNotFound(ushort schemeIndex, String schemeName, IntPtr originalMaterial, ushort lodIndex, IntPtr rend)
        {
            Technique technique;
            Material material = instance.materialPtrCollection.getTemporaryObject(originalMaterial, ptr => Material.createWrapper(ptr));
            foreach (HandleSchemeNotFoundDelegate target in mHandleSchemeNotFound.GetInvocationList())
            {
                target(schemeIndex, schemeName, material, lodIndex, out technique);
                if (technique != null)
                {
                    return technique.Ptr;
                }
            }
            return IntPtr.Zero;
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialManager_getByName(String name, ProcessWrapperObjectDelegate processWrapperCallback);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialManager_create(String name, String group, bool isManual, IntPtr loader, ProcessWrapperObjectDelegate processWrapperCallback);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MaterialManager_removeName(String name);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MaterialManager_resourceExists(String name);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialManager_getActiveScheme();

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MaterialManager_setActiveScheme(String name);

        //Iterator
        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialManager_beginResourceIterator();

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialManager_resourceIteratorNext(IntPtr iter);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MaterialManager_resourceIteratorHasMoreElements(IntPtr iter);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MaterialManager_endResourceIterator(IntPtr iter);

        //MaterialPtr
        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialPtr_createHeapPtr(IntPtr stackSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MaterialPtr_Delete(IntPtr heapSharedPtr);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr NativeMaterialListener_create(HandleSchemeNotFoundCb schemeNotFoundDelegate);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeMaterialListener_delete(IntPtr listener);

        #endregion
    }
}


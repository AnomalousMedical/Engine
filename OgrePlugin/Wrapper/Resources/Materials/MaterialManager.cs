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
        public delegate Technique HandleSchemeNotFoundDelegate(ushort schemeIndex, String schemeName, Material originalMaterial, ushort lodIndex);

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

        private HandleSchemeNotFoundDelegate mHandleSchemeNotFound;
        public HandleSchemeNotFoundDelegate HandleSchemeNotFound
        {
            get
            {
                return mHandleSchemeNotFound;
            }
            set
            {
                mHandleSchemeNotFound = value;
                if (mHandleSchemeNotFound == null && materialManagerListener != IntPtr.Zero)
                {
                    //Destroy material listener
                    handleSchemeNotFoundCb = null;
                    NativeMaterialListener_delete(materialManagerListener);
                    materialManagerListener = IntPtr.Zero;
                }
                else if (mHandleSchemeNotFound != null && materialManagerListener == IntPtr.Zero)
                {
                    //Create material listener
                    handleSchemeNotFoundCb = new HandleSchemeNotFoundCb(fireSchemeNotFound);
                    materialManagerListener = NativeMaterialListener_create(handleSchemeNotFoundCb);
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
            Technique technique = mHandleSchemeNotFound(schemeIndex, schemeName, instance.materialPtrCollection.getTemporaryObject(originalMaterial, ptr => Material.createWrapper(ptr)), lodIndex);
            return technique != null ? technique.Ptr : IntPtr.Zero;
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialManager_getByName(String name, ProcessWrapperObjectDelegate processWrapperCallback);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool MaterialManager_resourceExists(String name);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr MaterialManager_getActiveScheme();

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void MaterialManager_setActiveScheme(String name);

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


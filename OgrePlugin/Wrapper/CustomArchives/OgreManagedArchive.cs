using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace OgrePlugin
{
    public abstract class OgreManagedArchive : IDisposable
    {
        protected IntPtr nativeArchive;
        protected String name;
        private CallbackHandler callbackHandler;

        internal IntPtr NativeArchive
        {
            get
            {
                return nativeArchive;
            }
        }

        protected OgreManagedArchive(String name, String archType)
        {
            this.name = name;
            callbackHandler = new CallbackHandler();

            nativeArchive = callbackHandler.create(this, name, archType);
        }

        public virtual void Dispose()
        {
            OgreManagedArchive_Delete(nativeArchive);
            callbackHandler.Dispose();
        }

        protected internal abstract void load();

        protected internal abstract void unload();

        IntPtr open(String filename, bool readOnly)
        {
            OgreManagedStream managedStream = new OgreManagedStream(filename, doOpen(filename));
            return managedStream.NativeStream;
        }

        protected internal abstract Stream doOpen(String filename);

        //List

        IntPtr list(bool recursive, bool dirs)
        {
            IntPtr ogreStringVector = OgreManagedArchive_createOgreStringVector();
            doList(recursive, dirs, ogreStringVector);
            return ogreStringVector;
        }

        protected internal abstract void doList(bool recursive, bool dirs, IntPtr ogreStringVector);

        //List File Info

        IntPtr listFileInfo(bool recursive, bool dirs)
        {
            IntPtr ogreFileList = OgreManagedArchive_createOgreFileInfoList();
            doListFileInfo(recursive, dirs, ogreFileList, nativeArchive);
            return ogreFileList;
        }

        protected internal abstract void doListFileInfo(bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive);

        //Find

        IntPtr find(String pattern, bool recursive, bool dirs)
        {
            IntPtr ogreStringVector = OgreManagedArchive_createOgreStringVector();
            dofind(pattern, recursive, dirs, ogreStringVector);
            return ogreStringVector;
        }

        protected internal abstract void dofind(String pattern, bool recursive, bool dirs, IntPtr ogreStringVector);

        //Find File Info

        IntPtr findFileInfo(String pattern, bool recursive, bool dirs)
        {
            IntPtr ogreFileList = OgreManagedArchive_createOgreFileInfoList();
            dofindFileInfo(pattern, recursive, dirs, ogreFileList, nativeArchive);
            return ogreFileList;
        }

        protected internal abstract void dofindFileInfo(String pattern, bool recursive, bool dirs, IntPtr ogreFileList, IntPtr archive);

        protected internal abstract bool exists(String filename);

#region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void LoadDelegate(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void UnloadDelegate(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr OpenDelegate(String filename, [MarshalAs(UnmanagedType.I1)] bool readOnly
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr ListDelegate([MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr ListFileInfoDelegate([MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr FindDelegate(String pattern, [MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr FindFileInfoDelegate(String pattern, [MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool ExistsDelegate(String filename
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedArchive_Create(String name, String archType, LoadDelegate loadCallback, UnloadDelegate unloadCallback, OpenDelegate openCallback, ListDelegate listCallback, ListFileInfoDelegate listFileInfoCallback, FindDelegate findCallback, FindFileInfoDelegate findFileInfoCallback, ExistsDelegate existsCallback
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void OgreManagedArchive_Delete(IntPtr archive);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedArchive_createOgreStringVector();

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        protected static extern void OgreStringVector_push_back(IntPtr stringVector, String value);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedArchive_createOgreFileInfoList();

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        protected static extern void OgreFileInfoList_push_back(IntPtr fileList, IntPtr archive, IntPtr compressedSize, IntPtr uncompressedSize, String baseName, String filename, String path);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            static LoadDelegate loadCallback;
            static UnloadDelegate unloadCallback;
            static OpenDelegate openCallback;
            static ListDelegate listCallback;
            static ListFileInfoDelegate listFileInfoCallback;
            static FindDelegate findCallback;
            static FindFileInfoDelegate findFileInfoCallback;
            static ExistsDelegate existsCallback;

            static CallbackHandler()
            {
                loadCallback = new LoadDelegate(load);
                unloadCallback = new UnloadDelegate(unload);
                openCallback = new OpenDelegate(open);
                listCallback = new ListDelegate(list);
                listFileInfoCallback = new ListFileInfoDelegate(listFileInfo);
                findCallback = new FindDelegate(find);
                findFileInfoCallback = new FindFileInfoDelegate(findFileInfo);
                existsCallback = new ExistsDelegate(exists);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(LoadDelegate))]
            private static void load(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreManagedArchive).load();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(UnloadDelegate))]
            private static void unload(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as OgreManagedArchive).unload();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(OpenDelegate))]
            private static IntPtr open(String filename, bool readOnly, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedArchive).open(filename, readOnly);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ListDelegate))]
            private static IntPtr list(bool recursive, bool dirs, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedArchive).list(recursive, dirs);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ListFileInfoDelegate))]
            private static IntPtr listFileInfo(bool recursive, bool dirs, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedArchive).listFileInfo(recursive, dirs);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(FindDelegate))]
            private static IntPtr find(String pattern, bool recursive, bool dirs, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedArchive).find(pattern, recursive, dirs);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(FindFileInfoDelegate))]
            private static IntPtr findFileInfo(String pattern, bool recursive, bool dirs, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedArchive).findFileInfo(pattern, recursive, dirs);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ExistsDelegate))]
            private static bool exists(String filename, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as OgreManagedArchive).exists(filename);
            }

            private GCHandle gcHandle;

            public IntPtr create(OgreManagedArchive obj, String name, String archType)
            {
                gcHandle = GCHandle.Alloc(obj);
                return OgreManagedArchive_Create(name, archType, loadCallback, unloadCallback, openCallback, listCallback, listFileInfoCallback, findCallback, findFileInfoCallback, existsCallback, GCHandle.ToIntPtr(gcHandle));
            }

            public void Dispose()
            {
                gcHandle.Free();

                loadCallback = null;
                unloadCallback = null;
                openCallback = null;
                listCallback = null;
                listFileInfoCallback = null;
                findCallback = null;
                findFileInfoCallback = null;
                existsCallback = null;
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            LoadDelegate loadCallback;
            UnloadDelegate unloadCallback;
            OpenDelegate openCallback;
            ListDelegate listCallback;
            ListFileInfoDelegate listFileInfoCallback;
            FindDelegate findCallback;
            FindFileInfoDelegate findFileInfoCallback;
            ExistsDelegate existsCallback;

            public IntPtr create(OgreManagedArchive obj, String name, String archType)
            {
                loadCallback = new LoadDelegate(obj.load);
                unloadCallback = new UnloadDelegate(obj.unload);
                openCallback = new OpenDelegate(obj.open);
                listCallback = new ListDelegate(obj.list);
                listFileInfoCallback = new ListFileInfoDelegate(obj.listFileInfo);
                findCallback = new FindDelegate(obj.find);
                findFileInfoCallback = new FindFileInfoDelegate(obj.findFileInfo);
                existsCallback = new ExistsDelegate(obj.exists);

                return OgreManagedArchive_Create(name, archType, loadCallback, unloadCallback, openCallback, listCallback, listFileInfoCallback, findCallback, findFileInfoCallback, existsCallback);
            }

            public void Dispose()
            {
                loadCallback = null;
                unloadCallback = null;
                openCallback = null;
                listCallback = null;
                listFileInfoCallback = null;
                findCallback = null;
                findFileInfoCallback = null;
                existsCallback = null;
            }
        }
#endif

#endregion
    }
}
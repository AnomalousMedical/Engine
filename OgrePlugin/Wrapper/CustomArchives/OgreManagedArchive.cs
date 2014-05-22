using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace OgreWrapper
{
    public abstract class OgreManagedArchive : IDisposable
    {
        LoadDelegate loadCallback;
        UnloadDelegate unloadCallback;
        OpenDelegate openCallback;
        ListDelegate listCallback;
        ListFileInfoDelegate listFileInfoCallback;
        FindDelegate findCallback;
        FindFileInfoDelegate findFileInfoCallback;
        ExistsDelegate existsCallback;

        protected IntPtr nativeArchive;
        protected String name;

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

            loadCallback = new LoadDelegate(load);
            unloadCallback = new UnloadDelegate(unload);
            openCallback = new OpenDelegate(open);
            listCallback = new ListDelegate(list);
            listFileInfoCallback = new ListFileInfoDelegate(listFileInfo);
            findCallback = new FindDelegate(find);
            findFileInfoCallback = new FindFileInfoDelegate(findFileInfo);
            existsCallback = new ExistsDelegate(exists);

            nativeArchive = OgreManagedArchive_Create(name, archType, loadCallback, unloadCallback, openCallback, listCallback, listFileInfoCallback, findCallback, findFileInfoCallback, existsCallback);
        }

        public virtual void Dispose()
        {
            OgreManagedArchive_Delete(nativeArchive);

            loadCallback = null;
            unloadCallback = null;
            openCallback = null;
            listCallback = null;
            listFileInfoCallback = null;
            findCallback = null;
            findFileInfoCallback = null;
            existsCallback = null;
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
        delegate void LoadDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void UnloadDelegate();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr OpenDelegate(String filename, [MarshalAs(UnmanagedType.I1)] bool readOnly);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr ListDelegate([MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr ListFileInfoDelegate([MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr FindDelegate(String pattern, [MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr FindFileInfoDelegate(String pattern, [MarshalAs(UnmanagedType.I1)] bool recursive, [MarshalAs(UnmanagedType.I1)] bool dirs);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool ExistsDelegate(String filename);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr OgreManagedArchive_Create(String name, String archType, LoadDelegate loadCallback, UnloadDelegate unloadCallback, OpenDelegate openCallback, ListDelegate listCallback, ListFileInfoDelegate listFileInfoCallback, FindDelegate findCallback, FindFileInfoDelegate findFileInfoCallback, ExistsDelegate existsCallback);

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

#endregion
    }
}
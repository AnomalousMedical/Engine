using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Threads;

namespace Anomalous.OSPlatform
{
    public class FileSaveDialog
    {
        public delegate void ResultCallback(NativeDialogResult result, String path);

        public FileSaveDialog(NativeOSWindow parent = null, String message = "", String defaultDir = "", String defaultFile = "", String wildcard = "")
        {
            Parent = parent;
            Message = message;
            DefaultDir = defaultDir;
            DefaultFile = defaultFile;
            Wildcard = wildcard;
        }

        /// <summary>
        /// May or may not block the main thread depending on os. Assume it does
        /// not block and handle all results in the callback.
        /// </summary>
        /// <param name="callback">Called when the dialog is done showing with the results.</param>
        /// <returns></returns>
        public void showModal(ResultCallback callback)
        {
            FileSaveDialogResults results = new FileSaveDialogResults(callback);
            results.showNativeDialogModal(Parent, Message, DefaultDir, DefaultFile, Wildcard);
        }

        public NativeOSWindow Parent { get; set; }

        public String Message { get; set; }

        public String DefaultDir { get; set; }

        public String DefaultFile { get; set; }

        public String Wildcard { get; set; }

        class FileSaveDialogResults : IDisposable
        {
            CallbackHandler callbackHandler;
            ResultCallback showModalCallback;
            GCHandle handle;

            public FileSaveDialogResults(ResultCallback callback)
            {
                this.showModalCallback = callback;
                callbackHandler = new CallbackHandler();
            }

            public void Dispose()
            {
                handle.Free();
            }

            public void showNativeDialogModal(NativeOSWindow parent, String message, String defaultDir, String defaultFile, String wildcard)
            {
                handle = GCHandle.Alloc(this, GCHandleType.Normal);
                IntPtr parentPtr = parent != null ? parent._NativePtr : IntPtr.Zero;
                callbackHandler.showModal(this, parentPtr, message, defaultDir, defaultFile, wildcard);
            }

            private void getResults(NativeDialogResult result, IntPtr filePtr)
            {
                String managedFileString = Marshal.PtrToStringUni(filePtr);
                ThreadManager.invoke(() =>
                {
                    try
                    {
                        this.showModalCallback(result, managedFileString);
                    }
                    finally
                    {
                        this.Dispose();
                    }
                });
            }

            #region PInvoke

            [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void FileSaveDialog_showModal(IntPtr parent, [MarshalAs(UnmanagedType.LPWStr)] String message, [MarshalAs(UnmanagedType.LPWStr)] String defaultDir, [MarshalAs(UnmanagedType.LPWStr)] String defaultFile, [MarshalAs(UnmanagedType.LPWStr)] String wildcard, FileSaveDialogResultCallback resultCallback
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            private delegate void FileSaveDialogResultCallback(NativeDialogResult result, IntPtr file
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
            class CallbackHandler
            {
                static FileSaveDialogResultCallback resultCb;

                static CallbackHandler()
                {
                    resultCb = getResults;
                }

                [MonoTouch.MonoPInvokeCallback(typeof(FileSaveDialogResultCallback))]
                private static void getResults(NativeDialogResult result, IntPtr file, IntPtr instanceHandle)
                {
                    GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                    (handle.Target as FileSaveDialogResults).getResults(result, file);
                }

                public void showModal(FileSaveDialogResults obj, IntPtr parentPtr, String message, String defaultDir, String defaultFile, String wildcard)
                {
                    FileSaveDialog_showModal(parentPtr, message, defaultDir, defaultFile, wildcard, resultCb, GCHandle.ToIntPtr(obj.handle));
                }
            }
#else
            class CallbackHandler
            {
                DirDialogResultCallback resultCb;

                public void showModal(FileSaveDialogResults obj, IntPtr parentPtr, String message, String defaultDir, String defaultFile, String wildcard)
                {
                    resultCb = obj.getResults;
                    FileSaveDialog_showModal(parentPtr, message, defaultDir, defaultFile, wildcard, resultCb);
                }
            }
#endif

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Engine.Threads;

namespace Anomalous.OSPlatform
{
    public class ColorDialog
    {
        public delegate void ResultCallback(NativeDialogResult result, Color color);

        public ColorDialog(NativeOSWindow parent = null)
        {
            Parent = parent;
        }

        /// <summary>
        /// May or may not block the main thread depending on os. Assume it does
        /// not block and handle all results in the callback.
        /// </summary>
        /// <param name="callback">Called when the dialog is done showing with the results.</param>
        /// <returns></returns>
        public void showModal(ResultCallback callback)
        {
            ColorDialogResults results = new ColorDialogResults(callback);
            results.showNativeDialogModal(Parent, Color);
        }

        public NativeOSWindow Parent { get; set; }

        public Color Color { get; set; }

        class ColorDialogResults : IDisposable
        {
            ResultCallback showModalCallback;
            GCHandle handle;
            CallbackHandler callbackHandler;

            public ColorDialogResults(ResultCallback callback)
            {
                this.showModalCallback = callback;
                callbackHandler = new CallbackHandler();
            }

            public void Dispose()
            {
                handle.Free();
            }

            public void showNativeDialogModal(NativeOSWindow parent, Color color)
            {
                handle = GCHandle.Alloc(this, GCHandleType.Normal);
                IntPtr parentPtr = parent != null ? parent._NativePtr : IntPtr.Zero;
                callbackHandler.showModal(this, parentPtr, color);
            }

            private void getResults(NativeDialogResult result, Color color)
            {
                ThreadManager.invoke(() =>
                {
                    try
                    {
                        this.showModalCallback(result, color);
                    }
                    finally
                    {
                        this.Dispose();
                    }
                });
            }

            #region PInvoke

            [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
            private static extern void ColorDialog_showModal(IntPtr parent, Color color, ColorDialogResultCallback resultCallback
#if FULL_AOT_COMPILE
            , IntPtr instanceHandle
#endif
);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            private delegate void ColorDialogResultCallback(NativeDialogResult result, Color color
#if FULL_AOT_COMPILE
            , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
            class CallbackHandler
            {
                static ColorDialogResultCallback resultCb;

                static CallbackHandler()
                {
                    resultCb = getResults;
                }

                [MonoTouch.MonoPInvokeCallback(typeof(ColorDialogResultCallback))]
                private static void getResults(NativeDialogResult result, Color color, IntPtr instanceHandle)
                {
                    GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                    (handle.Target as ColorDialogResults).getResults(result, color);
                }

                public void showModal(ColorDialogResults obj, IntPtr parentPtr, Color color)
                {
                    ColorDialog_showModal(parentPtr, color, resultCb, GCHandle.ToIntPtr(obj.handle));
                }
            }
#else
            class CallbackHandler
            {
                ColorDialogResultCallback resultCb;

                public void showModal(ColorDialogResults obj, IntPtr parentPtr, Color color)
                {
                    resultCb = obj.getResults;
                    ColorDialog_showModal(parentPtr, color, resultCb);
                }
            }
#endif

            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Anomalous.Interop;

namespace Anomalous.OSPlatform
{
    public abstract partial class App : IDisposable
    {
        IntPtr appPtr;
        CallbackHandler callbackHandler;

        protected bool restartOnShutdown = false;
        protected bool restartAsAdmin = false;

        public App()
        {
            appPtr = App_create();
            callbackHandler = new CallbackHandler(this);
        }

        public void Dispose()
        {
            App_delete(appPtr);
            appPtr = IntPtr.Zero;
            callbackHandler.Dispose();
        }

        public void run()
        {
            App_run(appPtr);
        }

        public void exit()
        {
            App_exit(appPtr);
        }

        public void restart(bool asAdmin)
        {
            exit();
            restartOnShutdown = true;
            restartAsAdmin = asAdmin;
        }

        public abstract bool OnInit();

        public abstract int OnExit();

        public abstract void OnIdle();

        public String Title { get; set; }

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr App_create();

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void App_registerDelegates(IntPtr app, NativeFunc_Bool onInitCB, NativeFunc_Int onExitCB, NativeAction onIdleCB
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void App_delete(IntPtr app);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void App_run(IntPtr app);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void App_exit(IntPtr app);
    }

    partial class App
    {
#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static NativeFunc_Bool onInitCB;
            private static NativeFunc_Int onExitCB;
            private static NativeAction onIdleCB;

            static CallbackHandler()
            {
                onInitCB = new NativeFunc_Bool(OnInitStatic);
                onExitCB = new NativeFunc_Int(OnExitStatic);
                onIdleCB = new NativeAction(OnIdleStatic);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeFunc_Bool))]
            static bool OnInitStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as App).OnInit();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeFunc_Int))]
            static int OnExitStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as App).OnExit();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(NativeAction))]
            static void OnIdleStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as App).OnIdle();
            }

            private GCHandle handle;

            public CallbackHandler(App app)
            {
                handle = GCHandle.Alloc(app);
                App_registerDelegates(app.appPtr, onInitCB, onExitCB, onIdleCB, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private NativeFunc_Bool onInitCB;
            private NativeFunc_Int onExitCB;
            private NativeAction onIdleCB;

            public CallbackHandler(App app)
            {
                onInitCB = new NativeFunc_Bool(app.OnInit);
                onExitCB = new NativeFunc_Int(app.OnExit);
                onIdleCB = new NativeAction(app.OnIdle);

                App_registerDelegates(app.appPtr, onInitCB, onExitCB, onIdleCB);
            }

            public void Dispose()
            {
                
            }
        }
#endif
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Anomalous.Interop;
using System.Diagnostics;
using Engine;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Anomalous.OSPlatform
{
    public abstract partial class App : IDisposable
    {
        private IntPtr appPtr;
        private CallbackHandler callbackHandler;
        private bool restartOnShutdown = false;
        private bool restartAsAdmin = false;
        private String restartArgs = null;
        private IServiceCollection services = new ServiceCollection();
        private PluginManager pluginManager;

        public App()
        {
            appPtr = App_create();
            callbackHandler = new CallbackHandler(this);
            pluginManager = new PluginManager(services);
        }

        public abstract void Dispose();

        /// <summary>
        /// Dispose the global scope. Call this before destroying any the main window.
        /// </summary>
        public void DisposeGlobalScope()
        {
            pluginManager.Dispose();
        }

        /// <summary>
        /// Finish up the app dispose here. Call after destroying the Window.
        /// </summary>
        public void FinalDispose()
        {
            App_delete(appPtr);
            appPtr = IntPtr.Zero;
            callbackHandler.Dispose();
            if(restartOnShutdown)
            {
                try
                {
                    ProcessStartInfo startInfo;
                    if (restartAsAdmin)
                    {
                        startInfo = RuntimePlatformInfo.RestartAdminProcInfo;
                    }
                    else
                    {
                        startInfo = RuntimePlatformInfo.RestartProcInfo;
                    }
                    startInfo.Arguments = restartArgs;
                    Process.Start(startInfo);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public void Run()
        {
            App_run(appPtr);
        }

        public void Exit()
        {
            App_exit(appPtr);
        }

        /// <summary>
        /// Exit the app and set it up to restart when this App instance is disposed.
        /// </summary>
        /// <param name="asAdmin"></param>
        public void Restart(bool asAdmin, String args = null)
        {
            Exit();
            restartOnShutdown = true;
            restartAsAdmin = asAdmin;
            restartArgs = args;
        }

        /// <summary>
        /// Call this to cancel a restart request, only really makes sense in the OnExit callback,
        /// can be used to cancel app restarts for any reson.
        /// </summary>
        public void CancelRestart()
        {
            restartOnShutdown = false;
        }

        public bool OnInitCb()
        {
            var result = this.OnInit(services, pluginManager);

            if (!result)
            {
                return result;
            }

            pluginManager.InitializePlugins();

            result = OnLink(pluginManager.GlobalScope);

            return result;
        }

        public abstract bool OnInit(IServiceCollection services, PluginManager pluginManager);

        public abstract bool OnLink(IServiceScope globalScope);

        public abstract int OnExit();

        public abstract void OnIdle();

        /// <summary>
        /// On a mobile os apps are moved to the background and foreground. When they are in the
        /// background they can be killed at any time, so we must be able to respond to those events
        /// to save state. Note that this event only fires on mobile frameworks that support "backgrounding"
        /// this does not fire for minimized apps on a desktop, since that is more of a window not an app thing.
        /// </summary>
        public virtual void OnMovedToBackground()
        {

        }

        /// <summary>
        /// On a mobile os apps are moved to the background and foreground. This is called when an app moves back to the foreground.
        /// Note that this event only fires on mobile frameworks that support "backgrounding"
        /// this does not fire for maximized apps on a desktop, since that is more of a window not an app thing.
        /// </summary>
        public virtual void OnMovedToForeground()
        {

        }

        public String Title { get; set; }

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr App_create();

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void App_registerDelegates(IntPtr app, OnInitDelegate onInitCB, OnExitDelegate onExitCB, NativeAction onIdleCB, NativeAction onMovedToBackgroundCB, NativeAction onMovedToForegroundCB
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool OnInitDelegate(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int OnExitDelegate(
#if FULL_AOT_COMPILE
    IntPtr instanceHandle
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
            private static OnInitDelegate onInitCB;
            private static OnExitDelegate onExitCB;
            private static NativeAction onIdleCB;
            private static NativeAction onMovedToBackgroundCB;
            private static NativeAction onMovedToForegroundCB;

            static CallbackHandler()
            {
                onInitCB = new OnInitDelegate(OnInitStatic);
                onExitCB = new OnExitDelegate(OnExitStatic);
                onIdleCB = new NativeAction(OnIdleStatic);
                onMovedToBackgroundCB = new NativeAction(OnMovedToBackgroundStatic);
                onMovedToForegroundCB = new NativeAction(OnMovedToForegroundStatic);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(OnInitDelegate))]
            static bool OnInitStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as App).OnInit();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(OnExitDelegate))]
            static int OnExitStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as App).OnExit();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void OnIdleStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as App).OnIdle();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void OnMovedToBackgroundStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as App).OnMovedToBackground();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void OnMovedToForegroundStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as App).OnMovedToForeground();
            }

            private GCHandle handle;

            public CallbackHandler(App app)
            {
                handle = GCHandle.Alloc(app);
                App_registerDelegates(app.appPtr, onInitCB, onExitCB, onIdleCB, onMovedToBackgroundCB, onMovedToForegroundCB, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            private OnInitDelegate onInitCB;
            private OnExitDelegate onExitCB;
            private NativeAction onIdleCB;
            private NativeAction onMovedToBackgroundCB;
            private NativeAction onMovedToForegroundCB;

            public CallbackHandler(App app)
            {
                onInitCB = new OnInitDelegate(app.OnInitCb);
                onExitCB = new OnExitDelegate(app.OnExit);
                onIdleCB = new NativeAction(app.OnIdle);
                onMovedToBackgroundCB = new NativeAction(app.OnMovedToBackground);
                onMovedToForegroundCB = new NativeAction(app.OnMovedToForeground);

                App_registerDelegates(app.appPtr, onInitCB, onExitCB, onIdleCB, onMovedToBackgroundCB, onMovedToForegroundCB);
            }

            public void Dispose()
            {
                
            }
        }
#endif
    }
}

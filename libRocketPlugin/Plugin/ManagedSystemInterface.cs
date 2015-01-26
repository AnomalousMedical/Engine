using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Logging;
using Engine.Platform;
using Engine;

namespace libRocketPlugin
{
    public class ManagedSystemInterface : SystemInterface
    {
        private CallbackHandler callbackHandler;

        public ManagedSystemInterface()
        {
            callbackHandler = new CallbackHandler();
            systemInterfacePtr = callbackHandler.create(this);
        }

        public override void Dispose()
        {
            ManagedSystemInterface_Delete(systemInterfacePtr);
            callbackHandler.Dispose();
        }

        public float GetElapsedTime()
        {
            return Timer.ElapsedTime * 1e-6f;
        }

        public void LogMessage(LogType type, String message)
        {
            switch (type)
            {
                case LogType.LT_ALWAYS:
                    Log.ImportantInfo(message);
                    break;
                case LogType.LT_ERROR:
                    Log.Error(message);
                    break;
                case LogType.LT_ASSERT:
                    Log.Error(message);
                    break;
                case LogType.LT_WARNING:
                    Log.Warning(message);
                    break;
                case LogType.LT_INFO:
                    Log.Info(message);
                    break;
                case LogType.LT_DEBUG:
                    Log.Debug(message);
                    break;
                case LogType.LT_MAX:
                    Log.Info(message);
                    break;
            }
        }

        public void AddRootPath(String rootPath)
        {
            ManagedSystemInterface_AddRootPath(systemInterfacePtr, fixRootPath(rootPath));
        }

        public void RemoveRootPath(String rootPath)
        {
            ManagedSystemInterface_RemoveRootPath(systemInterfacePtr, fixRootPath(rootPath));
        }

        private String fixRootPath(String rootPath)
        {
            rootPath = RocketInterface.createValidFileUrl(rootPath);
            if (rootPath.EndsWith("/"))
            {
                rootPath = rootPath.Substring(0, rootPath.Length - 1);
            }
            return rootPath;
        }

        public UpdateTimer Timer { get; set; }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate float GetElapsedTimeDelegate(
#if FULL_AOT_COMPILE
        IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void LogMessageDelegate(LogType type, String message
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ManagedSystemInterface_Create(GetElapsedTimeDelegate etDelegate, LogMessageDelegate logDelegate
#if FULL_AOT_COMPILE
, IntPtr instanceHandle
#endif
);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedSystemInterface_Delete(IntPtr systemInterface);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedSystemInterface_AddRootPath(IntPtr systemInterface, String rootPath);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ManagedSystemInterface_RemoveRootPath(IntPtr systemInterface, String rootPath);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static GetElapsedTimeDelegate etDelegate;
            private static LogMessageDelegate logDelegate;

            static CallbackHandler()
            {
                etDelegate = new GetElapsedTimeDelegate(GetElapsedTime);
                logDelegate = new LogMessageDelegate(LogMessage);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(GetElapsedTimeDelegate))]
            private static float GetElapsedTime(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                return (handle.Target as ManagedSystemInterface).GetElapsedTime();
            }

            [MonoTouch.MonoPInvokeCallback(typeof(LogMessageDelegate))]
            private static void LogMessage(LogType type, string message, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedSystemInterface).LogMessage(type, message);
            }

            private GCHandle handle;

            public IntPtr create(ManagedSystemInterface obj)
            {
                handle = GCHandle.Alloc(obj);
                return ManagedSystemInterface_Create(etDelegate, logDelegate, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            GetElapsedTimeDelegate etDelegate;
            LogMessageDelegate logDelegate;

            public IntPtr create(ManagedSystemInterface obj)
            {
                etDelegate = new GetElapsedTimeDelegate(obj.GetElapsedTime);
                logDelegate = new LogMessageDelegate(obj.LogMessage);

                return ManagedSystemInterface_Create(etDelegate, logDelegate);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

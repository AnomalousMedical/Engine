//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Logging;
//using System.Runtime.InteropServices;

//namespace Anomalous.OSPlatform
//{
//    class ManagedLogListener : IDisposable
//    {
//        private IntPtr managedLogListener;
//        private CallbackHandler callbackHandler;

//        public ManagedLogListener()
//        {
//            callbackHandler = new CallbackHandler();
//            managedLogListener = callbackHandler.create(this);
//            NativeLog_addLogListener(managedLogListener);
//        }

//        public void Dispose()
//        {
//            ManagedLogListener_destroy(managedLogListener);
//            callbackHandler.Dispose();
//        }

//        private void logMessage(IntPtr message, LogLevel logLevel, IntPtr subsystem)
//        {
//            Log.Default.sendMessage(Marshal.PtrToStringAnsi(message), logLevel, Marshal.PtrToStringAnsi(subsystem));
//        }

//        #region PInvoke

//        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
//        delegate void LogMessageDelegate(IntPtr message, LogLevel logLevel, IntPtr subsystem
//#if FULL_AOT_COMPILE
//, IntPtr instanceHandle
//#endif
//);

//        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
//        private static extern IntPtr ManagedLogListener_create(LogMessageDelegate logDelegate
//#if FULL_AOT_COMPILE
//, IntPtr instanceHandle
//#endif
//);

//        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
//        private static extern void ManagedLogListener_destroy(IntPtr managedLogListener);

//        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
//        private static extern void NativeLog_addLogListener(IntPtr logListener);

//#if FULL_AOT_COMPILE
//        class CallbackHandler : IDisposable
//        {
//            private static LogMessageDelegate logCB;

//            static CallbackHandler()
//            {
//                logCB = logMessage;
//            }

//            [Anomalous.Interop.MonoPInvokeCallback(typeof(LogMessageDelegate))]
//            private static void logMessage(IntPtr message, LogLevel logLevel, IntPtr subsystem, IntPtr instanceHandle)
//            {
//                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
//                (handle.Target as ManagedLogListener).logMessage(message, logLevel, subsystem);
//            }

//            private GCHandle handle;

//            public IntPtr create(ManagedLogListener obj)
//            {
//                handle = GCHandle.Alloc(obj);
//                return ManagedLogListener_create(logCB, GCHandle.ToIntPtr(handle));
//            }

//            public void Dispose()
//            {
//                handle.Free();
//            }
//        }
//#else
//        class CallbackHandler : IDisposable
//        {
//            private LogMessageDelegate logCB;

//            public IntPtr create(ManagedLogListener obj)
//            {
//                logCB = obj.logMessage;
//                return ManagedLogListener_create(logCB);
//            }

//            public void Dispose()
//            {

//            }
//        }
//#endif

//        #endregion
//    }
//}

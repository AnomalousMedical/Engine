using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Anomalous.Interop
{
    /// <summary>
    /// This class provides a callback mechanism for getting strings from native
    /// classes that are freed when the native function returns. By using this
    /// class you can set the value using a callback before the native string is
    /// freed. Note that this could cause threading issues if multiple threads
    /// try to access strings at once, but it is up to the user to determine how
    /// this should be handed. This class is also disposable, so if you are creating
    /// a non static instance you must make sure to dispose it or you will leak GCHandles
    /// in AOT mode. Instances are also reusable, so it is valid to create these as static
    /// fields and the GCHandle leak won't matter since they are living for the lifetime
    /// of the program anyway.
    /// </summary>
    /// <remarks>
    /// The following prototype is in the NativeDelegates.h file. It supports AOT
    /// mode through the handle and this should always be provided in the P/Invoke 
    /// signature.
    /// <code>
    /// typedef void(*StringRetrieverCallback)(const char* value, void* handle)
    /// </code>
    /// </remarks>
    public class UnicodeStringRetriever : IDisposable
    {
        private String currentString;
        private bool gotString = false;
        private CallbackHandler callbackHandler;

        public UnicodeStringRetriever()
        {
            callbackHandler = new CallbackHandler(this);
        }

        public void Dispose()
        {
            callbackHandler.Dispose();
        }

        public String retrieveString()
        {
            String str = currentString;
            currentString = null;
            return str;
        }

        public bool GotString
        {
            get
            {
                return gotString;
            }
        }

        public Callback StringCallback
        {
            get
            {
                gotString = false;
                return callbackHandler.StringCallback;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return callbackHandler.Handle;
            }
        }

        private void setNativeString(IntPtr value)
        {
            gotString = true;
            currentString = Marshal.PtrToStringUni(value);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Callback(IntPtr value, IntPtr instanceHandle);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            private static Callback callback;

            static CallbackHandler()
            {
                callback = new Callback(setNativeString);
            }

            [MonoTouch.MonoPInvokeCallback(typeof(Callback))]
            private static void setNativeString(IntPtr value, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as UnicodeStringRetriever).setNativeString(value);
            }

            private GCHandle handle;

            public CallbackHandler(UnicodeStringRetriever obj)
            {
                handle = GCHandle.Alloc(obj);
            }

            public void Dispose()
            {
                handle.Free();
            }

            public Callback StringCallback
            {
                get
                {
                    return callback;
                }
            }

            public IntPtr Handle
            {
                get
                {
                    return GCHandle.ToIntPtr(handle);
                }
            }
        }
#else 
        class CallbackHandler : IDisposable
        {
            private Callback callback;

            public CallbackHandler(UnicodeStringRetriever obj)
            {
                callback = new Callback(obj.setNativeString);
            }

            public void Dispose()
            {
                
            }

            public Callback StringCallback
            {
                get
                {
                    return callback;
                }
            }

            public IntPtr Handle
            {
                get
                {
                    return IntPtr.Zero;
                }
            }
        }
#endif
    }
}
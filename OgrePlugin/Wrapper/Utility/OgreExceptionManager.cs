using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Anomalous.Interop;

namespace OgrePlugin
{
    //This class handles exceptions from ogre and can refire them as managed exceptions.
    class OgreExceptionManager
    {
        static OgreException exception = null;
        static ExceptionFoundCallback exceptionFoundCallback = exceptionFound;

        /// <summary>
        /// Cannot use static constructor, because it will not fire until the
        /// first time the class is referenced. This method will be called by
        /// the root wrapper's static constructor.
        /// </summary>
        public static void initializeOgreExceptionManager()
        {
            OgreExceptionManager_setCallback(exceptionFoundCallback);
        }

        private OgreExceptionManager()
        {

        }

        /// <summary>
        /// This method will fire an exception if one is stored. If you are
        /// calling a method that expects an exception to be loaded you must
        /// call this function.
        /// </summary>
        public static void fireAnyException()
        {
            try
            {
                if (exception != null)
                {
                    throw exception;
                }
            }
            finally
            {
                exception = null;
            }
        }

        /// <summary>
        /// Callback from native code to put an exception into this class.
        /// </summary>
        /// <param name="fullMessage">The full message from ogre.</param>
        [Anomalous.Interop.MonoPInvokeCallback(typeof(ExceptionFoundCallback))]
        private static void exceptionFound(IntPtr fullMessage)
        {
            exception = new OgreException(Marshal.PtrToStringAnsi(fullMessage));
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreExceptionManager_setCallback(ExceptionFoundCallback exCb);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ExceptionFoundCallback(IntPtr str0); //No special aot version needed.

        #endregion
    }
}

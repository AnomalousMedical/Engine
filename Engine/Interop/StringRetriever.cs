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
    /// this should be handed.
    /// </summary>
    /// <remarks>
    /// The following prototype is in the NativeDelegates.h file. It supports AOT
    /// mode in that file.
    /// <code>
    /// typedef void (*StringRetrieverCallback)(String value);
    /// </code>
    /// </remarks>
    public class StringRetriever
    {
        private Callback callback;
        private String currentString;
        private bool gotString = false;

        public StringRetriever()
        {
            callback = new Callback(setNativeString);
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
                return callback;
            }
        }

        private void setNativeString(String value)
        {
            gotString = true;
            currentString = value;
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Callback(String value);

        #endregion
    }
}
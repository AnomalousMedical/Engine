using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Engine
{
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

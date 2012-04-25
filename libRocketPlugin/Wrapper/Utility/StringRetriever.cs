using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    class StringRetriever
    {
        private Callback callback;
        private String currentString;
        private bool gotString = false;

        public StringRetriever()
        {
            callback = new Callback(setNativeString);
        }

        public void reset()
        {
            gotString = false;
        }

        public String CurrentString
        {
            get
            {
                return currentString;
            }
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

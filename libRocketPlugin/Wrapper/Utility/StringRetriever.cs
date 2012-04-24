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

        public StringRetriever()
        {
            callback = new Callback(setNativeString);
        }

        private void setNativeString(String value)
        {
            CurrentString = value;
        }

        public String CurrentString { get; private set; }

        public Callback StringCallback
        {
            get
            {
                return callback;
            }
        }

        #region PInvoke

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Callback(String value);

        #endregion
    }
}

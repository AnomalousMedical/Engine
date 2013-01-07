using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace libRocketPlugin
{
    public abstract class SystemInterface : IDisposable
    {
        protected IntPtr systemInterfacePtr;

        public abstract void Dispose();

        public String JoinPath(String documentPath, String path)
        {
            StringRetriever sr = new StringRetriever();
            SystemInterface_JoinPath(Ptr, documentPath, path, sr.StringCallback);
            return sr.retrieveString();
        }

        internal IntPtr Ptr
        {
            get
            {
                return systemInterfacePtr;
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SystemInterface_JoinPath(IntPtr systemInterface, String documentPath, String path, StringRetriever.Callback stringCallback);

        #endregion
    }
}

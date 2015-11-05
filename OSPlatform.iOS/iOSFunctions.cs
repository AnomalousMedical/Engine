using Anomalous.Interop;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.iOS
{
    static class iOSFunctions
    {
#if STATIC_LINK
		internal const String LibraryName = "__Internal";
#else
        internal const String LibraryName = "OSHelper";
#endif

        public static String LocalUserDocumentsFolder
        {
            get
            {
                using (StringRetriever sr = new StringRetriever())
                {
                    iOSPlatformConfig_getLocalUserDocumentsFolder(sr.StringCallback, sr.Handle);
                    return sr.retrieveString();
                }
            }
        }

        public static String LocalDataFolder
        {
            get
            {
                using (StringRetriever sr = new StringRetriever())
                {
                    iOSPlatformConfig_getLocalDataFolder(sr.StringCallback, sr.Handle);
                    return sr.retrieveString();
                }
            }
        }

        public static String LocalPrivateDataFolder
        {
            get
            {
                using (StringRetriever sr = new StringRetriever())
                {
                    iOSPlatformConfig_getLocalPrivateDataFolder(sr.StringCallback, sr.Handle);
                    return sr.retrieveString();
                }
            }
        }

        #region PInvoke

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern void iOSPlatformConfig_getLocalUserDocumentsFolder(StringRetriever.Callback retrieve, IntPtr handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern void iOSPlatformConfig_getLocalDataFolder(StringRetriever.Callback retrieve, IntPtr handle);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern void iOSPlatformConfig_getLocalPrivateDataFolder(StringRetriever.Callback retrieve, IntPtr handle);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Anomalous.OSPlatform.Mac
{
    public class MacRuntimePlatformInfo : RuntimePlatformInfo
    {
        private const String LibraryName = "OSHelper";

        public static void Initialize()
        {
            new MacRuntimePlatformInfo();
        }

        /// <summary>
        /// If your app is going to crash call this function before disposing the app, this will deal
        /// with the fact that the stack did not unwind correctly and prevent the app from crashing again
        /// after it closes.
        /// </summary>
        public static void AlertCrashing()
        {
            CocoaApp_alertCrashing();
        }

        protected MacRuntimePlatformInfo()
        {

        }

        protected override String LocalUserDocumentsFolderImpl
        {
            get
            {
                return MacOSXFunctions.LocalUserDocumentsFolder;
            }
        }

        protected override String LocalDataFolderImpl
        {
            get
            {
                return MacOSXFunctions.LocalDataFolder;
            }
        }

        protected override String LocalPrivateDataFolderImpl
        {
            get
            {
                return MacOSXFunctions.LocalPrivateDataFolder;
            }
        }

        protected override string ExecutablePathImpl
        {
            get
            {
                String[] args = Environment.GetCommandLineArgs();
                if (args.Length > 0)
                {
                    return Path.GetDirectoryName(args[0]);
                }
                else
                {
                    return Path.GetFullPath(".");
                }
            }
        }

        protected override bool ShowMoreColorsImpl
        {
            get
            {
                return true;
            }
        }

        protected override ProcessStartInfo RestartProcInfoImpl
        {
            get
            {
                String appBundle = Path.GetFullPath("../../");
                if (appBundle.Length > 1)
                {
                    appBundle = appBundle.Substring(0, appBundle.Length - 1);
                }
                ProcessStartInfo startInfo = new ProcessStartInfo("open", String.Format("-a '{0}' -n", appBundle));
                startInfo.UseShellExecute = false;
                return startInfo;
            }
        }

        protected override ProcessStartInfo RestartAdminProcInfoImpl
        {
            get
            {
                return RestartProcInfoImpl;
            }
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void CocoaApp_alertCrashing();
    }
}

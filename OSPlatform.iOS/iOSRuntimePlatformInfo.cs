﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.iOS
{
    public class iOSRuntimePlatformInfo : RuntimePlatformInfo
    {
        public static void Initialize()
        {
            new iOSRuntimePlatformInfo();
        }

        protected override String LocalUserDocumentsFolderImpl
        {
            get
            {
                return iOSFunctions.LocalUserDocumentsFolder;
            }
        }

        protected override String LocalDataFolderImpl
        {
            get
            {
                return iOSFunctions.LocalDataFolder;
            }
        }

        protected override String LocalPrivateDataFolderImpl
        {
            get
            {
                return iOSFunctions.LocalPrivateDataFolder;
            }
        }

        protected override string ExecutablePathImpl
        {
            get
            {
                return Path.GetFullPath(".");
            }
        }

        protected override bool ShowMoreColorsImpl
        {
            get
            {
                return false;
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
    }
}

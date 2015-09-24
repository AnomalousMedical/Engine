﻿using Android.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.Android
{
    class AndroidRuntimePlatformInfo : RuntimePlatformInfo
    {
        private AndroidActivity activity;

        public AndroidRuntimePlatformInfo(AndroidActivity activity)
        {
            this.activity = activity;
        }

        protected override string LocalUserDocumentsFolderImpl
        {
            get
            {
                return Path.Combine(activity.BaseContext.GetExternalFilesDir(null).Path, "UserData");
            }
        }

        protected override string LocalDataFolderImpl
        {
            get
            {
                return Path.Combine(activity.BaseContext.GetExternalFilesDir(null).Path, "LocalData");
            }
        }

        protected override string LocalPrivateDataFolderImpl
        {
            get
            {
                return Path.Combine(activity.BaseContext.GetExternalFilesDir(null).Path, "PrivateData");
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
                //ANROID_FIXLATER: probably not right
                String[] args = Environment.GetCommandLineArgs();
                return new ProcessStartInfo(args[0]);
            }
        }

        protected override ProcessStartInfo RestartAdminProcInfoImpl
        {
            get
            {
                //ANROID_FIXLATER: probably not right
                return RestartProcInfo;
            }
        }
    }
}

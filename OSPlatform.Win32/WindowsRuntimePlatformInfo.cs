﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.Win32
{
    public class WindowsRuntimePlatformInfo : RuntimePlatformInfo
    {
        const String LibraryName = "OSHelper";

        public static void Initialize()
        {
            new WindowsRuntimePlatformInfo();

            //Make sure the paths are setup correctly on windows to find 32/64 bit binaries.
            try
            {
                //Check bitness and determine path 
                String executionPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                String nativeLibPath;
                if (Environment.Is64BitProcess)
                {
                    nativeLibPath = Path.Combine(executionPath, "x64");
                }
                else
                {
                    nativeLibPath = Path.Combine(executionPath, "x86");
                }
                if (Directory.Exists(nativeLibPath))
                {
                    RuntimePlatformInfo.addPath(nativeLibPath);
                }
            }
            catch (Exception) { }

            //Find tabtip, this has a lot of combos since we have to run the 64 bit version on 64 bit oses even if the current process is 32 bit
            String tabTipLoc = null;
            if(Environment.Is64BitOperatingSystem)
            {
                tabTipLoc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), "Microsoft Shared\\ink\\TabTip.exe");
                if (!Environment.Is64BitProcess)
                {
                    //Hacky, but should help find the 64 bit tabtip when running as 32 bit on a 64 bit os
                    tabTipLoc = tabTipLoc.Replace(" (x86)", "");
                }
            }
            else
            {
                tabTipLoc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), "Microsoft Shared\\ink\\TabTip32.exe");
            }

            if (File.Exists(tabTipLoc))
            {
                Win32Window_setKeyboardPathAndWindow(tabTipLoc, "IPTip_Main_Window");
            }
        }

        protected WindowsRuntimePlatformInfo()
        {

        }

        protected override String LocalUserDocumentsFolderImpl
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
        }

        protected override String LocalDataFolderImpl
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            }
        }

        protected override String LocalPrivateDataFolderImpl
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
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
                String[] args = Environment.GetCommandLineArgs();
                return new ProcessStartInfo(args[0]);
            }
        }

        protected override ProcessStartInfo RestartAdminProcInfoImpl
        {
            get
            {
                var startInfo = RestartProcInfoImpl;
                startInfo.Verb = "runas";
                startInfo.UseShellExecute = true;

                return startInfo;
            }
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Win32Window_setKeyboardPathAndWindow([MarshalAs(UnmanagedType.LPWStr)] String keyboardPath, [MarshalAs(UnmanagedType.LPWStr)] String windowName);
    }
}
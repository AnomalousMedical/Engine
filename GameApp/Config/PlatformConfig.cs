using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Anomalous.OSPlatform;
using Engine.Shim;

namespace Anomalous.GameApp.Config
{
    public abstract class PlatformConfig
    {
        private static PlatformConfig currentConfig;

        static PlatformConfig()
        {
            OsId = SystemInfo.RuntimeOS;
            switch (OsId)
            {
                case RuntimeOperatingSystem.Windows:
                    currentConfig = new WindowsPlatformConfig();
                    break;
                case RuntimeOperatingSystem.Mac:
                    currentConfig = new MacPlatformConfig();
                    break;
                case RuntimeOperatingSystem.Android:
                    currentConfig = new AndroidPlatformConfig();
                    break;
                default:
                    throw new Exception("Could not find platform configuration.");
            }
        }

        public static String formatTitle(String windowText, String subText)
        {
            return currentConfig.formatTitleImpl(windowText, subText);
        }

        public static TouchType TouchType
        {
            get
            {
                return currentConfig.TouchTypeImpl;
            }
        }

        public static String ThemeFile
        {
            get
            {
                return currentConfig.ThemeFileImpl;
            }
        }

        public static bool AllowFullscreen
        {
            get
            {
                return currentConfig.AllowFullscreenImpl;
            }
        }

        public static MouseButtonCode DefaultCameraMouseButton
        {
            get
            {
                return currentConfig.DefaultCameraMouseButtonImpl;
            }
        }

        public static bool AllowCloneWindows
        {
            get
            {
                return currentConfig.AllowCloneWindowsImpl;
            }
        }

        public static bool CloseMainWindowOnShutdown
        {
            get
            {
                return currentConfig.CloseMainWindowOnShutdownImpl;
            }
        }

        public static KeyboardButtonCode PanKey
        {
            get
            {
                return currentConfig.PanKeyImpl;
            }
        }

        public static String OverrideFileLocation
        {
            get
            {
                return currentConfig.OverrideFileLocationImpl;
            }
        }

        public static ProcessStartInfo RestartProcInfo
        {
            get
            {
                return currentConfig.RestartProcInfoImpl;
            }
        }

        public static ProcessStartInfo RestartAdminProcInfo
        {
            get
            {
                return currentConfig.RestartAdminProcInfoImpl;
            }
        }

        public static RuntimeOperatingSystem OsId { get; private set; }

        public static bool DefaultEnableMultitouch
        {
            get
            {
                return currentConfig.DefaultEnableMultitouchImpl;
            }
        }

        public static bool HasCustomSSLValidation
        {
            get
            {
                return currentConfig.HasCustomSSLValidationImpl;
            }
        }

        public static bool ForwardTouchAsMouse
        {
            get
            {
                return currentConfig.ForwardTouchAsMouseImpl;
            }
        }

        /// <summary>
        /// A platform forced fps cap, if this is set it will always be used.
        /// </summary>
        public static int? FpsCap
        {
            get
            {
                return currentConfig.FpsCapImpl;
            }
        }

        /// <summary>
        /// This function moves the configuration files for a specific os if they need to move.
        /// We can remove this at some point in the future when we no longer need to check if files need
        /// to be moved.
        /// </summary>
        public static void MoveConfigurationIfNeeded()
        {
            currentConfig.moveConfigurationIfNeededImpl();
        }

        //Subclass
        protected abstract String formatTitleImpl(String windowText, String subText);

        protected abstract TouchType TouchTypeImpl { get; }

        protected abstract String ThemeFileImpl { get; }

        protected abstract bool AllowFullscreenImpl { get; }

        protected abstract MouseButtonCode DefaultCameraMouseButtonImpl { get; }

        protected abstract bool AllowCloneWindowsImpl { get; }

        protected abstract bool CloseMainWindowOnShutdownImpl { get; }

        protected abstract KeyboardButtonCode PanKeyImpl { get; }

        protected abstract String OverrideFileLocationImpl { get; }

        protected abstract ProcessStartInfo RestartProcInfoImpl { get; }

        protected abstract bool DefaultEnableMultitouchImpl { get; }

        protected abstract bool HasCustomSSLValidationImpl { get; }

        protected abstract ProcessStartInfo RestartAdminProcInfoImpl { get; }

        protected abstract void moveConfigurationIfNeededImpl();

        protected abstract bool ForwardTouchAsMouseImpl { get; }

        public virtual int? FpsCapImpl { get { return null; } }
    }
}

using Engine.Platform;
using Engine.Platform.Input;
using Engine.Shim;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform
{
    /// <summary>
    /// Provides platform specific runtime info.
    /// </summary>
    public abstract class RuntimePlatformInfo
    {
        public static GamepadHardware CreateGamepadHardware(Gamepad pad)
        {
            return Instance.CreateGamepadHardwareImpl(pad);
        }

        /// <summary>
        /// A folder that user documents and settings can be put into.
        /// </summary>
        public static String LocalUserDocumentsFolder
        {
            get
            {
                return Instance.LocalUserDocumentsFolderImpl;
            }
        }

        /// <summary>
        /// A non roaming folder that larger data files (like plugins and program downloads) can be put into.
        /// </summary>
        public static String LocalDataFolder
        {
            get
            {
                return Instance.LocalDataFolderImpl;
            }
        }

        /// <summary>
        /// A non roaming folder that data that should be kept private (like license files) can be put into.
        /// </summary>
        public static String LocalPrivateDataFolder
        {
            get
            {
                return Instance.LocalPrivateDataFolderImpl;
            }
        }

        /// <summary>
        /// The path to the main executable.
        /// </summary>
        public static String ExecutablePath
        {
            get
            {
                return Instance.ExecutablePathImpl;
            }
        }

        public static bool ShowMoreColors
        {
            get
            {
                return Instance.ShowMoreColorsImpl;
            }
        }

        /// <summary>
        /// This will be true if the platform info has been properly setup.
        /// </summary>
        internal static bool IsValid
        {
            get
            {
                return Instance != null;
            }
        }
        public static ProcessStartInfo RestartAdminProcInfo
        {
            get
            {
                return Instance.RestartAdminProcInfoImpl;
            }
        }

        public static ProcessStartInfo RestartProcInfo
        {
            get
            {
                return Instance.RestartProcInfoImpl;
            }
        }

        /// <summary>
        /// Add a path to the environment for this platform.
        /// </summary>
        /// <remarks>
        /// Right now it looks like this function is ok on all oses, so it is not virtualized at this time.
        /// </remarks>
        /// <param name="path"></param>
        public static void addPath(String path)
        {
            //Make sure this folder has not already been added to the path.
            String currentPathSetting = Environment.GetEnvironmentVariable("PATH");
            if (!String.IsNullOrEmpty(path) && !currentPathSetting.Contains(path))
            {
                Environment.SetEnvironmentVariable("PATH", String.Format("{0};{1}", path, currentPathSetting));
            }
        }

        /// <summary>
        /// The instance of this platform info. Will be created when the subclass is instantiated.
        /// </summary>
        protected static RuntimePlatformInfo Instance { get; set; }

        protected RuntimePlatformInfo()
        {
            if (Instance != null)
            {
                throw new Exception("Can only create one instance of the RuntimePlatformInfo class.");
            }
            Instance = this;
        }

        protected virtual GamepadHardware CreateGamepadHardwareImpl(Gamepad pad)
        {
            return new NullGamepadHardware(pad);
        }

        /// <summary>
        /// A folder that user documents and settings can be put into.
        /// </summary>
        protected abstract String LocalUserDocumentsFolderImpl { get; }

        /// <summary>
        /// A non roaming folder that larger data files (like plugins and program downloads) can be put into.
        /// </summary>
        protected abstract String LocalDataFolderImpl { get; }

        /// <summary>
        /// A non roaming folder that data that should be kept private (like license files) can be put into.
        /// </summary>
        protected abstract String LocalPrivateDataFolderImpl { get; }

        /// <summary>
        /// The path to the main executable.
        /// </summary>
        protected abstract string ExecutablePathImpl { get; }

        protected abstract bool ShowMoreColorsImpl { get; }

        protected abstract ProcessStartInfo RestartProcInfoImpl { get; }

        protected abstract ProcessStartInfo RestartAdminProcInfoImpl { get; }
    }
}

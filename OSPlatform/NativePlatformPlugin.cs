using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Logging;
using System.IO;
using System.Reflection;

namespace Anomalous.OSPlatform
{
    public class NativePlatformPlugin : PluginInterface
    {
#if STATIC_LINK
		internal const String LibraryName = "__Internal";
#else
        internal const String LibraryName = "OSHelper";
#endif

        private ManagedLogListener managedLogListener = new ManagedLogListener();

        public static NativePlatformPlugin Instance { get; private set; }

        public static void StaticInitialize()
        {
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
                    addPath(nativeLibPath);
                }
            }
            catch (Exception) { }

            switch (SystemInfo.RuntimeOS)
            {
                case RuntimeOperatingSystem.Windows:
                    new WindowsRuntimePlatformInfo();
                    break;
                case RuntimeOperatingSystem.Mac:
                    new MacRuntimePlatformInfo();
                    break;
                case RuntimeOperatingSystem.iOS:
                    new iOSRuntimePlatformInfo();
                    break;
                case RuntimeOperatingSystem.Android:
                    //This is handled by the platform specific plugin for android.
                    break;
                default:
                    throw new Exception("Could not find platform configuration.");
            }
        }

        /// <summary>
        /// Add a path to the platform's library search path.
        /// </summary>
        /// <param name="path"></param>
        public static void addPath(String path)
        {
            //Make sure this folder has not already been added to the path.
            String currentPathSetting = Environment.GetEnvironmentVariable("PATH");
            if (!String.IsNullOrEmpty(path) && !currentPathSetting.Contains(path))
            {
                Environment.SetEnvironmentVariable("PATH", String.Format("{0};{1}", currentPathSetting, path));
            }
        }

        public NativePlatformPlugin()
        {
            if (RuntimePlatformInfo.IsValid)
            {
                if (Instance == null)
                {
                    Instance = this;
                }
                else
                {
                    throw new Exception("Can only create NativePlatformPlugin one time.");
                }
            }
            else
            {
                throw new Exception("Invalid configuration for NativePlatformPlugin. Please call StaticInitialize as early as possibly in your client program.");
            }
        }

        public void Dispose()
        {
            managedLogListener.Dispose();
        }

        public void initialize(PluginManager pluginManager)
        {

        }

        public void link(PluginManager pluginManager)
        {

        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {

        }

        public string Name
        {
            get
            {
                return "NativePlatform";
            }
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {

        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }
    }
}

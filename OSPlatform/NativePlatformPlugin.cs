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

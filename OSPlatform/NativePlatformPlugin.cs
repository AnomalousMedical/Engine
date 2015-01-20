﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Logging;

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
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Can only create NativePlatformPlugin one time.");
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

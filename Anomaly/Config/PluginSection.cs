using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    public class PluginSection
    {
        private ConfigSection pluginSection;
        private ConfigIterator pluginIterator;

        public PluginSection(ConfigFile configFile)
        {
            pluginSection = configFile.createOrRetrieveConfigSection("Plugins");
            pluginIterator = new ConfigIterator(pluginSection, "Plugin");
        }

        public ConfigIterator PluginIterator
        {
            get
            {
                return pluginIterator;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    class PluginSection
    {
        private ConfigSection pluginSection;
        private int currentPlugin = 0;

        public PluginSection(ConfigFile configFile)
        {
            pluginSection = configFile.createOrRetrieveConfigSection("Plugins");
        }

        public void resetPluginIterator()
        {
            currentPlugin = 0;
        }

        public String nextPlugin()
        {
            return pluginSection.getValue("Plugin" + currentPlugin++, "");
        }

        public bool hasNext()
        {
            return pluginSection.hasValue("Plugin" + currentPlugin);
        }
    }
}

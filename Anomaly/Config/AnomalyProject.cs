using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.IO;

namespace Anomaly
{
    class AnomalyProject
    {
        private PluginSection pluginSection;
        private ResourceSection resourceSection;
        private ConfigFile backingFile;
        private String workingDirectory;

        public AnomalyProject(String projectFileName)
        {
            workingDirectory = Path.GetDirectoryName(projectFileName);
            backingFile = new ConfigFile(projectFileName);
            backingFile.loadConfigFile();
            pluginSection = new PluginSection(backingFile);
            resourceSection = new ResourceSection(backingFile);
        }

        public PluginSection PluginSection
        {
            get
            {
                return pluginSection;
            }
        }

        public ResourceSection ResourceSection
        {
            get
            {
                return resourceSection;
            }
        }

        public String WorkingDirectory
        {
            get
            {
                return workingDirectory;
            }
        }
    }
}

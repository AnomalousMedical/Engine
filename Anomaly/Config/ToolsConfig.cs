using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    class ToolsConfig
    {
        private ConfigSection toolsSection;

        public ToolsConfig(ConfigFile file)
        {
            this.toolsSection = file.createOrRetrieveConfigSection("Tools");
        }

        public String SevenZipExecutable
        {
            get
            {
                return toolsSection.getValue("7ZipExecutable", "C:\\Program Files\\7-Zip\\7za.exe");
            }
        }
    }
}

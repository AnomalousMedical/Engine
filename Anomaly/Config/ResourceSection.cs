using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    class ResourceSection
    {
        private ConfigSection resourceSection;

        public ResourceSection(ConfigFile configFile)
        {
            resourceSection = configFile.createOrRetrieveConfigSection("Resources");
        }

        public String ResourceRoot
        {
            get
            {
                return resourceSection.getValue("Root", ".");
            }
            set
            {
                resourceSection.setValue("Root", value);
            }
        }
    }
}

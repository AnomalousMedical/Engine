using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    public class ResourceSection
    {
        private ConfigSection resourceSection;
        private ConfigIterator additionalResources;

        public ResourceSection(ConfigFile configFile)
        {
            resourceSection = configFile.createOrRetrieveConfigSection("Resources");
            additionalResources = new ConfigIterator(resourceSection, "External");
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

        public ConfigIterator AdditionalResources
        {
            get
            {
                return additionalResources;
            }
        }
    }
}

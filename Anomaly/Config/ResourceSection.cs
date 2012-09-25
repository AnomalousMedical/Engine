using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.IO;

namespace Anomaly
{
    public class ResourceSection
    {
        private ConfigSection resourceSection;
        private String relativeRootDirectory;

        public ResourceSection(ConfigFile configFile)
        {
            relativeRootDirectory = Path.GetDirectoryName(configFile.BackingFile);
            resourceSection = configFile.createOrRetrieveConfigSection("Resources");
        }

        public String ResourceRoot
        {
            get
            {
                String resourceRoot = resourceSection.getValue("Root", ".");
                if (!Path.IsPathRooted(resourceRoot))
                {
                    resourceRoot = Path.Combine(relativeRootDirectory, resourceRoot);
                    resourceRoot = Path.GetFullPath(resourceRoot);
                }
                return resourceRoot;
            }
            set
            {
                resourceSection.setValue("Root", value);
            }
        }
    }
}

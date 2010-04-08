using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    public class SolutionSection
    {
        private ConfigSection solutionSection;

        public SolutionSection(ConfigFile configFile)
        {
            solutionSection = configFile.createOrRetrieveConfigSection("Solution");
        }

        public String EmptySceneFile
        {
            get
            {
                return solutionSection.getValue("EmptySceneFile", "Empty.xml");
            }
            set
            {
                solutionSection.setValue("EmptySceneFile", value);
            }
        }

        public String GlobalResourceFile
        {
            get
            {
                return solutionSection.getValue("GlobalResourcesFile", "Resources.xml");
            }
            set
            {
                solutionSection.setValue("GlobalResourcesFile", value);
            }
        }
    }
}

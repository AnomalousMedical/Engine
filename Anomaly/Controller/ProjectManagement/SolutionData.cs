using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;

namespace Anomaly
{
    partial class SolutionData
    {
        private ExternalResourceManager externalResources;

        public SolutionData()
        {
            externalResources = new ExternalResourceManager();
        }

        public ExternalResourceManager ExternalResources
        {
            get
            {
                return externalResources;
            }
        }
    }

    partial class SolutionData : Saveable
    {
        private const String EXTERNAL_RESOURCES = "ExternalResources";

        protected SolutionData(LoadInfo info)
        {
            externalResources = info.GetValue<ExternalResourceManager>(EXTERNAL_RESOURCES);
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue(EXTERNAL_RESOURCES, externalResources);
        }
    }
}

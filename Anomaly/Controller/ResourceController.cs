using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using System.IO;
using System.Xml;
using Engine.Saving.XMLSaver;

namespace Anomaly
{
    class ResourceController
    {
        private AnomalyController controller;
        ResourceManager secondaryResources;

        public ResourceController()
        {

        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
        }

        public void editResources()
        {
            controller.showObjectEditor(secondaryResources.getEditInterface());

            XmlTextWriter resourceWriter = new XmlTextWriter(AnomalyConfig.DocRoot + "/resources.xml", Encoding.Unicode);
            resourceWriter.Formatting = Formatting.Indented;
            XmlSaver resourceSaver = new XmlSaver();
            resourceSaver.saveObject(secondaryResources, resourceWriter);
            resourceWriter.Close();

            controller.PluginManager.PrimaryResourceManager.changeResourcesToMatch(secondaryResources);
            controller.PluginManager.PrimaryResourceManager.forceResourceRefresh();
        }

        public void setupResources()
        {
            if (!File.Exists(AnomalyConfig.DocRoot + "/resources.xml"))
            {
                secondaryResources = controller.PluginManager.createSecondaryResourceManager();

                editResources();
            }
            else
            {
                XmlTextReader resourceReader = new XmlTextReader(AnomalyConfig.DocRoot + "/resources.xml");
                XmlSaver xmlSaver = new XmlSaver();
                secondaryResources = xmlSaver.restoreObject(resourceReader) as ResourceManager;
                resourceReader.Close();

                controller.PluginManager.PrimaryResourceManager.changeResourcesToMatch(secondaryResources);
                controller.PluginManager.PrimaryResourceManager.forceResourceRefresh();
            }
        }
    }
}

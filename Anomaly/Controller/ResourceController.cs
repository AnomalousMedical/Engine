using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;
using System.IO;
using System.Xml;
using Engine.Saving.XMLSaver;
using Engine;

namespace Anomaly
{
    public class ResourceController
    {
        private AnomalyController controller;
        ResourceManager secondaryResources;

        public ResourceController()
        {
            
        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
            secondaryResources = controller.PluginManager.createEmptyResourceManager();
        }

        public void viewResources()
        {
            controller.showObjectEditor(secondaryResources.getEditInterface());

            //XmlTextWriter resourceWriter = new XmlTextWriter(AnomalyConfig.DocRoot + "/resources.xml", Encoding.Unicode);
            //resourceWriter.Formatting = Formatting.Indented;
            //XmlSaver resourceSaver = new XmlSaver();
            //resourceSaver.saveObject(secondaryResources, resourceWriter);
            //resourceWriter.Close();

            //controller.PluginManager.PrimaryResourceManager.changeResourcesToMatch(secondaryResources);
            //controller.PluginManager.PrimaryResourceManager.forceResourceRefresh();
        }

        public void clearResources()
        {
            secondaryResources = controller.PluginManager.createEmptyResourceManager();
        }

        public void addResources(ResourceManager toAdd)
        {
            secondaryResources.addResources(toAdd);
        }

        public void applyToScene()
        {
            controller.PluginManager.SceneResourceManager.changeResourcesToMatch(secondaryResources);
            controller.PluginManager.SceneResourceManager.forceResourceRefresh();
        }

        public ResourceManager getResourceManager()
        {
            return secondaryResources;
        }
    }
}

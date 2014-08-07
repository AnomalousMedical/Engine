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
        private ResourceManager secondaryResources;
        private ResourceManager sceneResources;
        private ResourceManager globalResources;

        public ResourceController(AnomalyController controller)
        {
            this.controller = controller;
            secondaryResources = controller.PluginManager.createScratchResourceManager();
            sceneResources = controller.PluginManager.createLiveResourceManager();
            globalResources = controller.PluginManager.createLiveResourceManager();
        }

        public void viewResources()
        {
            controller.showObjectEditor(secondaryResources.getEditInterface());
        }

        public void clearResources()
        {
            secondaryResources = controller.PluginManager.createScratchResourceManager();
        }

        public void addResources(ResourceManager toAdd)
        {
            secondaryResources.addResources(toAdd);
        }

        public void applyToScene()
        {
            sceneResources.changeResourcesToMatch(secondaryResources);
            sceneResources.initializeResources();
        }

        public ResourceManager getResourceManager()
        {
            return secondaryResources;
        }

        public ResourceManager GlobalResources
        {
            get
            {
                return globalResources;
            }
        }
    }
}

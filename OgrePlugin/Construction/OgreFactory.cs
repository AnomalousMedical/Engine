using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    class OgreFactory : SimElementFactory
    {
        private LinkedList<OgreFactoryEntry> sceneNodes = new LinkedList<OgreFactoryEntry>();
        private OgreSceneManager targetScene;

        public OgreFactory(OgreSceneManager targetScene)
        {
            this.targetScene = targetScene;
        }

        public void addSceneNodeDefinition(SimObject instance, SceneNodeDefinition definition)
        {
            sceneNodes.AddLast(new OgreFactoryEntry(instance, definition));
        }

        public void createProducts()
        {
            foreach (OgreFactoryEntry entry in sceneNodes)
            {
                entry.createProduct(targetScene);
            }
        }

        public void createStaticProducts()
        {
            foreach (OgreFactoryEntry entry in sceneNodes)
            {
                entry.createStaticProduct(targetScene);
            }
        }

        public void linkProducts()
        {
            
        }

        public void clearDefinitions()
        {
            sceneNodes.Clear();
        }
    }
}

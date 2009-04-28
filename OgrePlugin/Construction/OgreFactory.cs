using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    /// <summary>
    /// This is a SimElementFactory for building Ogre stuff.
    /// </summary>
    class OgreFactory : SimElementFactory
    {
        private LinkedList<OgreFactoryEntry> sceneNodes = new LinkedList<OgreFactoryEntry>();
        private OgreSceneManager targetScene;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetScene">The OgreSceneManager that will recieve the objects built by this factory.</param>
        public OgreFactory(OgreSceneManager targetScene)
        {
            this.targetScene = targetScene;
        }

        /// <summary>
        /// Add a SceneNodeDefinition.
        /// </summary>
        /// <param name="instance">The SimObject that will get the product built by definition.</param>
        /// <param name="definition">The definition to build.</param>
        public void addSceneNodeDefinition(SimObjectBase instance, SceneNodeDefinition definition)
        {
            sceneNodes.AddLast(new OgreFactoryEntry(instance, definition));
        }

        /// <summary>
        /// Build all products currently registered.
        /// </summary>
        public void createProducts()
        {
            foreach (OgreFactoryEntry entry in sceneNodes)
            {
                entry.createProduct(targetScene);
            }
        }

        /// <summary>
        /// Create all registered static products.
        /// </summary>
        public void createStaticProducts()
        {
            foreach (OgreFactoryEntry entry in sceneNodes)
            {
                entry.createProduct(targetScene);
            }
        }

        /// <summary>
        /// Do any linkup steps.
        /// </summary>
        public void linkProducts()
        {
            
        }

        /// <summary>
        /// Clear the definitions to build.
        /// </summary>
        public void clearDefinitions()
        {
            sceneNodes.Clear();
        }
    }
}

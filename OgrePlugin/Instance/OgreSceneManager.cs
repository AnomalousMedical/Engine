using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Logging;

namespace OgrePlugin
{
    /// <summary>
    /// This is a SceneManager for Ogre scenes. It contains all the objects
    /// created for a scene with their Identifiers.
    /// </summary>
    public class OgreSceneManager : SimElementManager
    {
        private Root ogreRoot;
        private SceneManager scene;
        private String name;
        private OgreFactory factory;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <param name="scene">Ogre's SceneManager for the scene.</param>
        public OgreSceneManager(String name, SceneManager scene)
        {
            this.ogreRoot = Root.getSingleton();
            this.scene = scene;
            this.name = name;
            this.factory = new OgreFactory(this);
        }

        #region Functions

        /// <summary>
        /// Get the factory used to build objects.
        /// </summary>
        /// <returns>The SimElementFactory.</returns>
        public SimElementFactory getFactory()
        {
            return factory;
        }

        /// <summary>
        /// Get the factory as an OgreFactory. This is the same value returned
        /// by getFactory, but it does not need to be typecast.
        /// </summary>
        /// <returns>The OgreFactory.</returns>
        internal OgreFactory getOgreFactory()
        {
            return factory;
        }

        /// <summary>
        /// Get the type of SimElementManager this is.
        /// </summary>
        /// <returns>This type.</returns>
        public Type getSimElementManagerType()
        {
            return this.GetType();
        }

        /// <summary>
        /// Get the name.
        /// </summary>
        /// <returns>The name.</returns>
        public string getName()
        {
            return name;
        }

        /// <summary>
        /// Create a definition from this OgreSceneManager.
        /// </summary>
        /// <returns>A new SimElementManagerDefinition.</returns>
        public SimElementManagerDefinition createDefinition()
        {
            OgreSceneManagerDefinition definition = new OgreSceneManagerDefinition(name);
            if (scene != null)
            {
                definition.ShadowTechnique = scene.getShadowTechnique();
            }
            return definition;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            Root.getSingleton().destroySceneManager(scene);
        }

        public void setScene(SimScene simScene)
        {

        }

        #endregion Functions

        #region Properties

        public SceneManager SceneManager
        {
            get
            {
                return scene;
            }
        }

        #endregion Properites
    }
}

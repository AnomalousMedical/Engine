using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
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
        private Dictionary<Identifier, Entity> entities = new Dictionary<Identifier, Entity>();
        private Dictionary<Identifier, SceneNode> sceneNodes = new Dictionary<Identifier, SceneNode>();
        private Dictionary<Identifier, Camera> cameras = new Dictionary<Identifier, Camera>();

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
            return definition;
        }

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            Root.getSingleton().destroySceneManager(scene);
        }

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="name">The name of the entity to create.</param>
        /// <param name="definition">The definition of the entity to create.</param>
        /// <returns>A new Entity.</returns>
        internal Entity createEntity(Identifier name, EntityDefinition definition)
        {
            Entity entity = scene.createEntity(name.FullName, definition.MeshName);
            entities.Add(name, entity);
            return entity;
        }

        /// <summary>
        /// Destroy the Entity identified by Identifier.
        /// </summary>
        /// <param name="identifier">The Identifier of the entity to destroy.</param>
        internal void destroyEntity(Identifier identifier)
        {
            if (entities.ContainsKey(identifier))
            {
                Entity entity = entities[identifier];
                entities.Remove(identifier);
                scene.destroyEntity(entity);
            }
            else
            {
                Log.Default.sendMessage("Attempted to remove an entity named {0} that does not exist in the scene {1}.", LogLevel.Warning, OgreInterface.PluginName, identifier.FullName, name);
            }
        }

        /// <summary>
        /// Create a scene node.
        /// </summary>
        /// <param name="name">The Identifier of the scene node.</param>
        /// <returns>A new SceneNode.</returns>
        internal SceneNode createSceneNode(Identifier name)
        {
            SceneNode node = scene.createSceneNode(name.FullName);
            sceneNodes.Add(name, node);
            return node;
        }

        /// <summary>
        /// Destroy the SceneNode specified by Identifier.
        /// </summary>
        /// <param name="identifier">The scene node to destroy.</param>
        internal void destroySceneNode(Identifier identifier)
        {
            if (sceneNodes.ContainsKey(identifier))
            {
                SceneNode sceneNode = sceneNodes[identifier];
                sceneNodes.Remove(identifier);
                scene.destroySceneNode(sceneNode);
            }
            else
            {
                Log.Default.sendMessage("Attempted to remove a scene node named {0} that does not exist in the scene {1}.", LogLevel.Warning, OgreInterface.PluginName, identifier.FullName, name);
            }
        }

        /// <summary>
        /// Create a camera.
        /// </summary>
        /// <param name="name">The Identifier of the camera.</param>
        /// <returns>A new Camera.</returns>
        internal Camera createCamera(Identifier name)
        {
            Camera camera = scene.createCamera(name.FullName);
            cameras.Add(name, camera);
            return camera;
        }

        /// <summary>
        /// Destroy a given camera.
        /// </summary>
        /// <param name="name">The Identifier of the camera to destroy.</param>
        internal void destroyCamera(Identifier name)
        {
            if (cameras.ContainsKey(name))
            {
                Camera camera = cameras[name];
                cameras.Remove(name);
                scene.destroyCamera(camera);
            }
            else
            {
                Log.Default.sendMessage("Attempted to remove a camera named {0} that does not exist in the scene {1}.", LogLevel.Warning, OgreInterface.PluginName, name.FullName, this.name);
            }
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

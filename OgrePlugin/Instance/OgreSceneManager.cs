using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OgreWrapper;
using Engine.ObjectManagement;
using Logging;

namespace OgrePlugin
{
    public class OgreSceneManager : SimElementManager
    {
        private Root ogreRoot;
        private SceneManager scene;
        private String name;
        private OgreFactory factory;
        private Dictionary<Identifier, Entity> entities = new Dictionary<Identifier, Entity>();
        private Dictionary<Identifier, SceneNode> sceneNodes = new Dictionary<Identifier, SceneNode>();
        private Dictionary<Identifier, Camera> cameras = new Dictionary<Identifier, Camera>();

        public OgreSceneManager(String name, SceneManager scene)
        {
            this.ogreRoot = Root.getSingleton();
            this.scene = scene;
            this.name = name;
            this.factory = new OgreFactory(this);
        }

        #region Functions

        public SimElementFactory getFactory()
        {
            return factory;
        }

        internal OgreFactory getOgreFactory()
        {
            return factory;
        }

        public Type getSimElementManagerType()
        {
            return this.GetType();
        }

        public string getName()
        {
            return name;
        }

        public SimElementManagerDefinition createDefinition()
        {
            OgreSceneManagerDefinition definition = new OgreSceneManagerDefinition(name);
            return definition;
        }

        public void Dispose()
        {
            Root.getSingleton().destroySceneManager(scene);
        }

        internal Entity createEntity(Identifier name, EntityDefinition definition)
        {
            Entity entity = scene.createEntity(name.FullName, definition.MeshName);
            entities.Add(name, entity);
            return entity;
        }

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

        internal SceneNode createSceneNode(Identifier name)
        {
            SceneNode node = scene.createSceneNode(name.FullName);
            sceneNodes.Add(name, node);
            return node;
        }

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

        internal Camera createCamera(Identifier name)
        {
            Camera camera = scene.createCamera(name.FullName);
            cameras.Add(name, camera);
            return camera;
        }

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

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

        internal SceneNode createSceneNode(Identifier name, SceneNodeDefinition definition)
        {
            SceneNode node = scene.createSceneNode(definition.Name);
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
                Log.Default.sendMessage("Attempted to remove an scene node named {0} that does not exist in the scene {1}.", LogLevel.Warning, OgreInterface.PluginName, identifier.FullName, name);
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

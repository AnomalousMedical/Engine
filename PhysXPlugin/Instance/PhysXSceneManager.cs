using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using Logging;

namespace PhysXPlugin
{
    /// <summary>
    /// This class manages a single PhysX scene.
    /// </summary>
    public class PhysXSceneManager : SimElementManager
    {
        #region Fields

        private PhysScene scene;
        private PhysSDK physSDK;
        private PhysFactory factory;
        private Dictionary<Identifier, PhysActor> actors = new Dictionary<Identifier, PhysActor>();
        private String name;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <param name="scene">The scene to manage.</param>
        /// <param name="physSDK">The PhysSDK that created the scene.</param>
        internal PhysXSceneManager(String name, PhysScene scene, PhysSDK physSDK)
        {
            this.scene = scene;
            this.physSDK = physSDK;
            this.factory = new PhysFactory(this);
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            physSDK.releaseScene(scene);
        }

        /// <summary>
        /// Get the factory that builds SimElements.
        /// </summary>
        /// <returns>The factory.</returns>
        public SimElementFactory getFactory()
        {
            return factory;
        }

        /// <summary>
        /// This will return the type the SimElementManager wishes to report
        /// itself as. Usually this will be the type of the class itself,
        /// however, it is possible to specify a superclass if desired. This
        /// will be the type reported to the SimSubScene.
        /// </summary>
        /// <returns></returns>
        public Type getSimElementManagerType()
        {
            return this.GetType();
        }

        /// <summary>
        /// Get the factory as a PhysFactory. This is the same factory as
        /// getFactory(), but as the subclass.
        /// </summary>
        /// <returns>The factory for this scene as a PhysFactory.</returns>
        internal PhysFactory getPhysFactory()
        {
            return factory;
        }

        /// <summary>
        /// Get the name.
        /// </summary>
        /// <returns>The name.</returns>
        public String getName()
        {
            return name;
        }

        /// <summary>
        /// Create a definition for this SimElementManager.
        /// </summary>
        /// <returns>A new SimElementManager for this definition.</returns>
        public SimElementManagerDefinition createDefinition()
        {
            PhysXSceneManagerDefinition definition = new PhysXSceneManagerDefinition(name);
            scene.saveToDesc(definition.SceneDesc);
            return definition;
        }

        /// <summary>
        /// Create a new PhysActor. It must be destroyed using destroyPhysActor
        /// or it will not be released from PhysX properly.
        /// </summary>
        /// <param name="name">The name of the PhysActor.</param>
        /// <param name="actorDesc">The description to build the actor with.</param>
        /// <returns>The newly created PhysActor or null if an error occured.</returns>
        public PhysActor createPhysActor(Identifier name, PhysActorDesc actorDesc)
        {
            if (!actors.ContainsKey(name))
            {
                PhysActor actor = scene.createActor(actorDesc);
                actors.Add(name, actor);
                return actor;
            }
            else
            {
                Log.Default.sendMessage("Attempted to create an actor named {0} that already exists. No changes made.", LogLevel.Warning, PhysXInterface.PluginName, name);
                return null;
            }
        }

        /// <summary>
        /// Destroy the actor specified by name. Must be called for every actor
        /// that is created.
        /// </summary>
        /// <param name="name">The name of the actor to destroy.</param>
        public void destroyPhysActor(Identifier name)
        {
            if (actors.ContainsKey(name))
            {
                scene.releaseActor(actors[name]);
                actors.Remove(name);
            }
            else
            {
                Log.Default.sendMessage("Attempted to erase an actor named {0} that does not exist. No changes made.", LogLevel.Warning, PhysXInterface.PluginName, name);
            }
        }

        /// <summary>
        /// Get the PhysActor specified by name.
        /// </summary>
        /// <param name="name">The name of the PhysActor.</param>
        /// <returns>The matching PhysActor or null if it was not found.</returns>
        public PhysActor getPhysActor(Identifier name)
        {
            PhysActor actor;
            actors.TryGetValue(name, out actor);
            return actor;
        }

        #endregion Functions
    }
}

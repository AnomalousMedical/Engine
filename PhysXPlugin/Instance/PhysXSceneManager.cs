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

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scene">The scene to manage.</param>
        internal PhysXSceneManager(PhysScene scene, PhysSDK physSDK, PhysFactory factory)
        {
            this.scene = scene;
            this.physSDK = physSDK;
            this.factory = factory;
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
        /// Set this manager as the target object that constructed objects will
        /// be placed in by the factory.
        /// </summary>
        public void setAsConstructionTarget()
        {
            factory.setTargetManager(this);
        }

        /// <summary>
        /// Make this manager no longer the target object for constructed
        /// objects.
        /// </summary>
        public void unsetAsConstructionTarget()
        {
            factory.unsetTargetManager();
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

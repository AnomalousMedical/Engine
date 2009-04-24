using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using Logging;
using Engine.ObjectManagement;
using Engine.Platform;

namespace PhysXPlugin
{
    /// <summary>
    /// This class manages a single PhysX scene.
    /// </summary>
    public class PhysXSceneManager : SimElementManager, UpdateListener
    {
        #region Fields

        private PhysScene scene;
        private PhysSDK physSDK;
        private PhysFactory factory;
        private Dictionary<Identifier, PhysActorElement> actors = new Dictionary<Identifier, PhysActorElement>();
        private Dictionary<Identifier, PhysJointElement> joints = new Dictionary<Identifier, PhysJointElement>();
        private String name;
        private UpdateTimer mainTimer;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <param name="scene">The scene to manage.</param>
        /// <param name="physSDK">The PhysSDK that created the scene.</param>
        internal PhysXSceneManager(String name, PhysScene scene, PhysSDK physSDK, UpdateTimer mainTimer)
        {
            this.scene = scene;
            this.physSDK = physSDK;
            this.factory = new PhysFactory(this);
            this.name = name;
            this.mainTimer = mainTimer;
            mainTimer.addFixedUpdateListener(this);
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Dispose function.
        /// </summary>
        public void Dispose()
        {
            mainTimer.removeFixedUpdateListener(this);
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
        internal PhysActorElement createPhysActor(Identifier name, PhysActorDefinition actorDesc)
        {
            if (!actors.ContainsKey(name))
            {
                PhysActor actor = scene.createActor(actorDesc.ActorDesc);
                PhysActorElement element = new PhysActorElement(actor, this, name, actorDesc.Subscription);
                actors.Add(name, element);
                return element;
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
        internal void destroyPhysActor(Identifier name)
        {
            if (actors.ContainsKey(name))
            {
                scene.releaseActor(actors[name].Actor);
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
        internal PhysActorElement getPhysActor(Identifier name)
        {
            PhysActorElement actor;
            actors.TryGetValue(name, out actor);
            return actor;
        }

        internal PhysJointElement createJoint(Identifier name, PhysJointDefinition jointDef)
        {
            PhysJoint joint = scene.createJoint(jointDef.JointDesc);
            PhysJointElement element = jointDef.createElement(name, joint, this);
            joints.Add(name, element);
            return element;
        }

        internal void destroyJoint(Identifier jointId)
        {
            scene.releaseJoint(joints[jointId].Joint);
            joints.Remove(jointId);
        }

        public void sendUpdate(Clock clock)
        {
            scene.stepSimulation(clock.Seconds);
        }

        public void loopStarting()
        {

        }

        public void exceededMaxDelta()
        {

        }

        #endregion Functions
    }
}

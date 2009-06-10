﻿using System;
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
    public class PhysXSceneManager : SimElementManager, UpdateListener, PhysContactReport
    {
        #region Fields

        private PhysScene scene;
        private PhysSDK physSDK;
        private PhysFactory factory;
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
            scene.setContactReport(this);
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
            scene.startActorGroupPairIter();
            while (scene.hasNextActorGroupPair())
            {
                PhysActorGroupPair pair = scene.getNextActorGroupPair();
                definition.addActorGroupPair(pair.Group0, pair.Group1, pair.Flags);
            }
            return definition;
        }

        /// <summary>
        /// Create a new PhysActor. It must be destroyed using destroyPhysActor
        /// or it will not be released from PhysX properly.
        /// </summary>
        /// <param name="name">The name of the PhysActor.</param>
        /// <param name="actorDesc">The description to build the actor with.</param>
        /// <returns>The newly created PhysActor or null if an error occured.</returns>
        internal PhysActorElement createPhysActor(PhysActorDefinition actorDesc)
        {
            PhysActor actor = scene.createActor(actorDesc.ActorDesc);
            PhysActorElement element = new PhysActorElement(actor, this, actorDesc.Name, actorDesc.Subscription);
            actor.setUserData(element);
            return element;
        }

        /// <summary>
        /// Destroy the actor specified by name. Must be called for every actor
        /// that is created.
        /// </summary>
        /// <param name="name">The name of the actor to destroy.</param>
        internal void destroyPhysActor(PhysActorElement actor)
        {
            actor.Actor.setUserData(null);
            scene.releaseActor(actor.Actor);
        }

        internal PhysJointElement createJoint(PhysJointDefinition jointDef)
        {
            PhysJoint joint = scene.createJoint(jointDef.JointDesc);
            PhysJointElement element = jointDef.createElement(joint, this);
            return element;
        }

        internal void destroyJoint(PhysJointElement joint)
        {
            scene.releaseJoint(joint.Joint);
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

        #region Properties

        internal PhysScene PhysScene
        {
            get
            {
                return scene;
            }
        }

        #endregion Properties

        #region PhysContactReport Members

        public void onContactNotify(PhysContactPair pair, ContactPairFlag events)
        {
            PhysActorElement actor0 = null;
            PhysActorElement actor1 = null;
            if (!pair.isActorDeleted(0))
            {
                actor0 = pair.getActor(0).getUserData() as PhysActorElement;
            }
            if (!pair.isActorDeleted(1))
            {
                actor1 = pair.getActor(1).getUserData() as PhysActorElement;
            }
            if (actor0 != null)
            {
                actor0.fireContactEvent(actor1, actor0, pair.getContactIterator(), events);
            }
            if (actor1 != null)
            {
                actor1.fireContactEvent(actor0, actor1, pair.getContactIterator(), events);
            }
        }

        #endregion
    }
}

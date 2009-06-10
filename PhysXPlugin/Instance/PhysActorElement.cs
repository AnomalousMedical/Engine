using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using Engine.ObjectManagement;

namespace PhysXPlugin
{
    /// <summary>
    /// This class is a SimElement for PhysActors so they can interact with
    /// other SimObject pieces.
    /// </summary>
    /// <seealso cref="T:Engine.SimElement"/>
    /// <seealso cref="T:PhysXWrapper.ActiveTransformCallback"/>
    public class PhysActorElement : SimElement, ActiveTransformCallback
    {
        #region Fields

        private PhysActor actor;
        private PhysXSceneManager scene;
        private bool changeKinematicStatus;
        private bool changeDisableCollisionStatus;
        private String shapeName;
        private List<PhysContactListener> listeners = new List<PhysContactListener>(); 

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Instance of PhysXPlugin.Instance.PhysActorElement
        /// </summary>
        /// <param name="actor">The actor to track.</param>
        /// <param name="scene">The scene the actor belongs to.</param>
        /// <param name="name">The name of this Element.</param>
        /// <param name="subscription">The subscription of events this Element listens to.</param>
        internal PhysActorElement(PhysActor actor, PhysXSceneManager scene, String name, Subscription subscription)
            : base(name, subscription)
        {
            this.actor = actor;
            this.scene = scene;
            this.shapeName = null;
            actor.setActiveTransformCallback(this);
            changeKinematicStatus = actor.isDynamic() && !actor.readBodyFlag(BodyFlag.NX_BF_KINEMATIC);
            changeDisableCollisionStatus = !actor.readActorFlag(ActorFlag.NX_AF_DISABLE_COLLISION);
            setEnabled(false);
        }

        #endregion

        #region Functions

        /// <summary>
        /// Dispose function. Cleans up any objects that cannot be garbage collected.
        /// </summary>
        protected override void Dispose()
        {
            scene.destroyPhysActor(this);
        }

        /// <summary>
        /// This function will update the position of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            actor.setGlobalPosition(translation);
            actor.setGlobalOrientationQuat(rotation);
        }

        /// <summary>
        /// This function will update the translation of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        protected override void updateTranslationImpl(ref Vector3 translation)
        {
            actor.setGlobalPosition(translation);
        }

        /// <summary>
        /// This function will update the rotation of the SimElement.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            actor.setGlobalOrientationQuat(rotation);
        }

        /// <summary>
        /// Not implemented for PhysActors they do not support scaling.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        protected override void updateScaleImpl(ref Vector3 scale)
        {
            
        }

        /// <summary>
        /// This function will enable or disable the SimElement. What this
        /// means is subsystem dependent and may not reduce the processing of
        /// the object very much.
        /// </summary>
        /// <param name="enabled">True to enable the object. False to disable the object.</param>
        protected override void setEnabled(bool enabled)
        {
            if (enabled)
            {
                if (changeDisableCollisionStatus)
                {
                    actor.clearActorFlag(ActorFlag.NX_AF_DISABLE_COLLISION);
                }
                if (changeKinematicStatus)
                {
                    actor.clearBodyFlag(BodyFlag.NX_BF_KINEMATIC);
                }
            }
            else
            {
                if (changeDisableCollisionStatus)
                {
                    actor.raiseActorFlag(ActorFlag.NX_AF_DISABLE_COLLISION);
                }
                if (changeKinematicStatus)
                {
                    actor.raiseBodyFlag(BodyFlag.NX_BF_KINEMATIC);
                }
            }
        }

        /// <summary>
        /// Save a SimElementDefinition from this SimElement.
        /// </summary>
        /// <returns>A new SimElementDefinition for this SimElement.</returns>
        public override SimElementDefinition saveToDefinition()
        {
            PhysActorDefinition actorDef = new PhysActorDefinition(Name, shapeName, actor);
            //fix flags that the mediator uses.
            if (changeKinematicStatus)
            {
                actorDef.BodyFlags &= ~BodyFlag.NX_BF_KINEMATIC;
            }
            if (changeDisableCollisionStatus)
            {
                actorDef.Flags &= ~ActorFlag.NX_AF_DISABLE_COLLISION;
            }

            //fix state variables that may be saved that cause illegal actors
            //If we have a density it is illegal to also define mass and mass space intertia
            //so disable them.
            if (actorDef.Density > 0.0f)
            {
                actorDef.Mass = 0;
                actorDef.MassSpaceIntertia = Vector3.Zero;
            }
            return actorDef;
        }

        /// <summary>
        /// This function is called when the actor moves. It will dispatch the
        /// event to the rest of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        public void firePositionUpdate(ref Vector3 translation, ref Quaternion rotation)
        {
            updatePosition(ref translation, ref rotation);
        }

        /// <summary>
        /// Add a PhysContactListener.
        /// </summary>
        /// <param name="listener">The listener to add.</param>
        public void addContactListener(PhysContactListener listener)
        {
            listeners.Add(listener);
        }

        /// <summary>
        /// Remove a PhysContactListener.
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void removeContactListener(PhysContactListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Fire the contact event.
        /// </summary>
        /// <param name="contactWith">The item being contacted with. This will be null if the PhysActorElement being collided with is deleted.</param>
        /// <param name="myself">The owner item contacting the other item. This will be null if the PhysActorElement is deleted.</param>
        /// <param name="contacts">The contact iterator with the contact point information.</param>
        /// <param name="contactType">The type of contact.</param>
        internal void fireContactEvent(PhysActorElement contactWith, PhysActorElement myself, ContactIterator contacts, ContactPairFlag contactType)
        {
            foreach (PhysContactListener listener in listeners)
            {
                listener.onContact(contactWith, myself, contacts, contactType);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// This is the name of the shape used to create this actor. If this is
        /// defined the shapes on the actor will not be saved and instead it
        /// will reference the external shape.
        /// </summary>
        public String ShapeName
        {
            get
            {
                return shapeName;
            }
            internal set
            {
                shapeName = value;
            }
        }

        public PhysActor Actor
        {
            get
            {
                return actor;
            }
        }

        #endregion
    }
}

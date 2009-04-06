using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using EngineMath;

namespace PhysXPlugin
{
    /// <summary>
    /// This class is a SimComponent for PhysActors so they can interact with
    /// other SimObject pieces.
    /// </summary>
    /// <seealso cref="T:Engine.SimComponent"/>
    /// <seealso cref="T:PhysXWrapper.ActiveTransformCallback"/>
    class PhysActorComponent : SimComponent, ActiveTransformCallback
    {
        #region Fields

        private PhysActor actor;
        private PhysXSceneManager scene;
        private bool changeKinematicStatus;
        private bool changeDisableCollisionStatus;
        private Identifier actorId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new Instance of PhysXPlugin.Instance.PhysActorComponent
        /// </summary>
        /// <param name="actor">The actor to track.</param>
        /// <param name="scene">The scene the actor belongs to.</param>
        /// <param name="name">The name of this component.</param>
        /// <param name="subscription">The subscription of events this component listens to.</param>
        public PhysActorComponent(PhysActor actor, PhysXSceneManager scene, Identifier name, Subscription subscription)
            : base(name.ComponentName, subscription)
        {
            this.actor = actor;
            this.scene = scene;
            this.actorId = name;
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
        public override void Dispose()
        {
            scene.destroyPhysActor(actorId);
        }

        /// <summary>
        /// This function will update the position of the SimComponent.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        public override void updatePosition(ref EngineMath.Vector3 translation, ref EngineMath.Quaternion rotation)
        {
            actor.setGlobalPosition(translation);
            actor.setGlobalOrientationQuat(rotation);
        }

        /// <summary>
        /// This function will update the translation of the SimComponent.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        public override void updateTranslation(ref EngineMath.Vector3 translation)
        {
            actor.setGlobalPosition(translation);
        }

        /// <summary>
        /// This function will update the rotation of the SimComponent.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        public override void updateRotation(ref EngineMath.Quaternion rotation)
        {
            actor.setGlobalOrientationQuat(rotation);
        }

        /// <summary>
        /// Not implemented for PhysActors they do not support scaling.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        public override void updateScale(ref EngineMath.Vector3 scale)
        {
            
        }

        /// <summary>
        /// This function will enable or disable the SimComponent. What this
        /// means is subsystem dependent and may not reduce the processing of
        /// the object very much.
        /// </summary>
        /// <param name="enabled">True to enable the object. False to disable the object.</param>
        public override void setEnabled(bool enabled)
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
        /// This function is called when the actor moves. It will dispatch the
        /// event to the rest of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        public void firePositionUpdate(ref Vector3 translation, ref Quaternion rotation)
        {
            SimObject.updatePosition(ref translation, ref rotation, this);
        }

        #endregion
    }
}

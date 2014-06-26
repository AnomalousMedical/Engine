using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Attributes;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This enum sets what updates a element will listen to.
    /// All element must support the enable and disable events, however.
    /// </summary>
    public enum Subscription
    {
        /// <summary>
        /// Recieve no updates.
        /// </summary>
        None = 0,
        /// <summary>
        /// Subscribe to position updates.
        /// </summary>
        PositionUpdate = 1 << 0,
        /// <summary>
        /// Subscribe to scale updates.
        /// </summary>
        ScaleUpdate = 1 << 1,
        /// <summary>
        /// Recieve all updates.
        /// </summary>
        All = PositionUpdate | ScaleUpdate,
    };

    /// <summary>
    /// This is a subsystem specific part of a SimObject. The SimObject will
    /// handle updating the common state between all of these objects, but the
    /// exact behavior they implement is unknown at this level. These objects
    /// could be graphics, physics, behaviors or anything else that needs to
    /// maintain or update state with the SimObject. Subclasses of this in
    /// plugins should be public so that programs using the subclasses can
    /// access them. However, the engine core itself does not need to have any
    /// knowledge of the individual SimElements.
    /// </summary>
    [DoNotCopy]
    [DoNotSave]
    public abstract class SimElement
    {
        private SimObjectBase simObject;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the SimElement.</param>
        /// <param name="subscription">The subscription of events to listen to.</param>
        public SimElement(String name, Subscription subscription)
        {
            this.Name = name;
            this.Subscription = subscription;
        }

        /// <summary>
        /// This is an internal function so behaviors can abuse this interface a little bit and provide an empty constructor.
        /// </summary>
        /// <param name="name">The name of the SimElement.</param>
        /// <param name="subscription">The subscription of events to listen to.</param>
        internal void _behaviorSetAttributes(String name, Subscription subscription)
        {
            this.Name = name;
            this.Subscription = subscription;
        }

        /// <summary>
        /// Cleans up any objects that cannot be garbage collected.
        /// </summary>
        internal void DoDispose()
        {
            Dispose();
        }

        /// <summary>
        /// Dispose function called on cleanup. This is hidden by being invoked
        /// by the internal member Destroy().
        /// </summary>
        protected abstract void Dispose();

        /// <summary>
        /// This function will update the position of the entire SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        public void updatePosition(ref Vector3 translation, ref Quaternion rotation)
        {
            simObject.updatePosition(ref translation, ref rotation, this);
        }

        /// <summary>
        /// This function will update the position of the SimElement. It is set
        /// as internal to prevent other libraries from directly modifying a
        /// SimElement, since this is not allowed. This will call the
        /// corresponding protected function so subclasses can provide their own
        /// motion update implementation.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        internal void alertUpdatePosition(ref Vector3 translation, ref Quaternion rotation)
        {
            updatePositionImpl(ref translation, ref rotation);
        }

        /// <summary>
        /// This function will update the position of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        protected abstract void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation);

        /// <summary>
        /// This function will update the translation of the entire SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        public void updateTranslation(ref Vector3 translation)
        {
            simObject.updateTranslation(ref translation, this);
        }

        /// <summary>
        /// This function will update the translation of the entire SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        public void updateTranslation(Vector3 translation)
        {
            simObject.updateTranslation(ref translation, this);
        }

        /// <summary>
        /// This function will update the translation of the SimElement. It is
        /// set as internal to prevent other libraries from directly modifying a
        /// SimElement, since this is not allowed. This will call the
        /// corresponding protected function so subclasses can provide their own
        /// motion update implementation.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        internal void alertUpdateTranslation(ref Vector3 translation)
        {
            updateTranslationImpl(ref translation);
        }

        /// <summary>
        /// This function will update the translation of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        protected abstract void updateTranslationImpl(ref Vector3 translation);

        /// <summary>
        /// This function will update the rotation of the entire SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        public void updateRotation(ref Quaternion rotation)
        {
            simObject.updateRotation(ref rotation, this);
        }

        /// <summary>
        /// This function will update the rotation of the entire SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        public void updateRotation(Quaternion rotation)
        {
            simObject.updateRotation(ref rotation, this);
        }

        /// <summary>
        /// This function will update the rotation of the SimElement. It is set
        /// as internal to prevent other libraries from directly modifying a
        /// SimElement, since this is not allowed. This will call the
        /// corresponding protected function so subclasses can provide their own
        /// motion update implementation.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        internal void alertUpdateRotation(ref Quaternion rotation)
        {
            updateRotationImpl(ref rotation);
        }

        /// <summary>
        /// This function will update the rotation of the SimElement.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        protected abstract void updateRotationImpl(ref Quaternion rotation);

        /// <summary>
        /// This function will update the scale of the entire SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        public void updateScale(ref Vector3 scale)
        {
            simObject.updateScale(ref scale, this);
        }

        /// <summary>
        /// This function will update the scale of the entire SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        public void updateScale(Vector3 scale)
        {
            simObject.updateScale(ref scale, this);
        }

        /// <summary>
        /// This function will update the scale of the SimElement. It is set as
        /// internal to prevent other libraries from directly modifying a
        /// SimElement, since this is not allowed. This will call the
        /// corresponding protected function so subclasses can provide their own
        /// motion update implementation.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        internal void alertUpdateScale(ref Vector3 scale)
        {
            updateScaleImpl(ref scale);
        }

        /// <summary>
        /// This function will update the scale of the SimElement.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        protected abstract void updateScaleImpl(ref Vector3 scale);

        /// <summary>
        /// This function will enable or disable the SimElement. What this means
        /// is subsystem dependent and may not reduce the processing of the
        /// object very much. This is also internal to hide it from classes
        /// using SimElements.
        /// </summary>
        /// <param name="enabled">True to enable the object. False to disable the object.</param>
        internal void fireSetEnabled(bool enabled)
        {
            setEnabled(enabled);
        }

        /// <summary>
        /// This function will enable or disable the SimElement. What this
        /// means is subsystem dependent and may not reduce the processing of
        /// the object very much.
        /// </summary>
        /// <param name="enabled">True to enable the object. False to disable the object.</param>
        protected abstract void setEnabled(bool enabled);

        /// <summary>
        /// Save a SimElementDefinition from this SimElement.
        /// </summary>
        /// <returns>A new SimElementDefinition for this SimElement.</returns>
        public abstract SimElementDefinition saveToDefinition();

        /// <summary>
        /// Internal function to set the SimObject for this element.
        /// </summary>
        /// <param name="simObject">The SimObject to set.</param>
        internal void setSimObject(SimObjectBase simObject)
        {
            this.simObject = simObject;
            this.setEnabled(simObject != null && simObject.Enabled);
        }

        /// <summary>
        /// Get the name of this SimElement.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Get/Set the subscription of events this element listens to.
        /// </summary>
        public Subscription Subscription { get; private set; }

        /// <summary>
        /// The SimObject that owns this SimElement.
        /// </summary>
        public SimObject Owner
        {
            get
            {
                return simObject;
            }
        }
    }
}

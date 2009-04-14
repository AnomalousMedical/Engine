using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine;

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
    /// maintain or update state with the SimObject.
    /// </summary>
    public abstract class SimElement : IDisposable
    {
        #region Fields

        private SimObject simObject;

        #endregion Fields

        #region Constructors

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

        #endregion

        #region Functions

        /// <summary>
        /// Dispose function. Cleans up any objects that cannot be garbage collected.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// This function will update the position of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        public abstract void updatePosition(ref Vector3 translation, ref Quaternion rotation);

        /// <summary>
        /// This function will update the translation of the SimElement.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        public abstract void updateTranslation(ref Vector3 translation);

        /// <summary>
        /// This function will update the rotation of the SimElement.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        public abstract void updateRotation(ref Quaternion rotation);

        /// <summary>
        /// This function will update the scale of the SimElement.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        public abstract void updateScale(ref Vector3 scale);

        /// <summary>
        /// This function will enable or disable the SimElement. What this
        /// means is subsystem dependent and may not reduce the processing of
        /// the object very much.
        /// </summary>
        /// <param name="enabled">True to enable the object. False to disable the object.</param>
        public abstract void setEnabled(bool enabled);

        /// <summary>
        /// Save a SimElementDefinition from this SimElement.
        /// </summary>
        /// <returns>A new SimElementDefinition for this SimElement.</returns>
        public abstract SimElementDefinition saveToDefinition();

        #endregion

        #region Properties

        /// <summary>
        /// Get the name of this SimElement.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Get/Set the subscription of events this element listens to.
        /// </summary>
        public Subscription Subscription { get; set; }

        /// <summary>
        /// The SimObject for this SimElement.
        /// </summary>
        public SimObject SimObject
        {
            get
            {
                return simObject;
            }
            internal set
            {
                simObject = value;
                this.setEnabled(simObject.Enabled);
            }
        }

        #endregion
    }
}

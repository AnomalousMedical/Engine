using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine;

namespace Engine
{
    /// <summary>
    /// This enum sets what updates a component will listen to.
    /// All component must support the enable and disable events, however.
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
    public abstract class SimComponent : IDisposable
    {
        public abstract void Dispose();

        public abstract String Name { get; }

        public Subscription Subscription { get; set; }

        public abstract void updatePosition(ref Vector3 translation, ref Quaternion rotation);

        public abstract void updateTranslation(ref Vector3 translation);

        public abstract void updateRotation(ref Quaternion rotation);

        public abstract void updateScale(ref Vector3 scale);

        public abstract void setEnabled(bool enabled);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine.ObjectManagement;

namespace Engine
{
    /// <summary>
    /// This enum sets what updates a component will listen to.
    /// All component must support the enable and disable events, however.
    /// </summary>
    public enum Subscription
    {
        None = 0,
        PositionUpdate = (1 << 0),  //Subscribe to position updates
        All = PositionUpdate,
    };

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

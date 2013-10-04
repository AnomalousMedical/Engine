using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public interface ISimElement
    {
        /// <summary>
        /// This function will update the position of the entire SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        void updatePosition(ref Vector3 translation, ref Quaternion rotation);

        /// <summary>
        /// This function will update the translation of the entire SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        void updateTranslation(ref Vector3 translation);

        /// <summary>
        /// This function will update the translation of the entire SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        void updateTranslation(Vector3 translation);

        /// <summary>
        /// This function will update the rotation of the entire SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        void updateRotation(ref Quaternion rotation);

        /// <summary>
        /// This function will update the rotation of the entire SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        void updateRotation(Quaternion rotation);

        /// <summary>
        /// This function will update the scale of the entire SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        void updateScale(ref Vector3 scale);

        /// <summary>
        /// This function will update the scale of the entire SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        void updateScale(Vector3 scale);

        /// <summary>
        /// Save a SimElementDefinition from this SimElement.
        /// </summary>
        /// <returns>A new SimElementDefinition for this SimElement.</returns>
        SimElementDefinition saveToDefinition();

        /// <summary>
        /// Get the name of this SimElement.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Get/Set the subscription of events this element listens to.
        /// </summary>
        Subscription Subscription { get; }

        /// <summary>
        /// The SimObject that owns this SimElement.
        /// </summary>
        SimObject Owner { get; }
    }
}

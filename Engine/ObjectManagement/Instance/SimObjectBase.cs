using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is the class new SimObjects should be extended from as it provides
    /// the entire interface.
    /// </summary>
    public abstract class SimObjectBase : SimObject, IDisposable
    {
        #region Functions

        /// <summary>
        /// Dispose function. Destroys subsystem objects, which may or may not
        /// be unmanaged. Do not call this function directly. The
        /// SimObjectManager will take care of it.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Add a SimElement to this SimObject.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public abstract void addElement(SimElement element);

        /// <summary>
        /// Add an element that will take this sim object as an owner, but will
        /// not show up in the sim object itself. This means that the .Owner
        /// property of the element will report this sim object, but the object
        /// will not be accessible through this SimObject and will not be
        /// managed in any way by the SimObject.
        /// </summary>
        /// <param name="element">The element to add as a weak element.</param>
        public void addWeakElement(SimElement element)
        {
            element.setSimObject(this);
        }

        /// <summary>
        /// Remove a SimElement as a weak element setting its sim object back to null.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public void removeWeakElement(SimElement element)
        {
            element.setSimObject(null);
        }

        /// <summary>
        /// Update the position of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updatePosition(ref Vector3 translation, ref Quaternion rotation, SimElement trigger);

        /// <summary>
        /// Update the translation of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updateTranslation(ref Vector3 translation, SimElement trigger);

        /// <summary>
        /// Update the rotation of the SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updateRotation(ref Quaternion rotation, SimElement trigger);

        /// <summary>
        /// Update the scale of the SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        public abstract void updateScale(ref Vector3 scale, SimElement trigger);

        #endregion Functions
    }
}

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
        /// Dispose function. Destroys subsystem objects, which may or may not be unmanaged.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Add a SimElement to this SimObject.
        /// </summary>
        /// <param name="element">The element to add.</param>
        public abstract void addElement(SimElement element);

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

        /// <summary>
        /// Set the SimObject as enabled or disabled. The subsystems will
        /// determine the exact status that that their objects will go into when
        /// this is activated. However, this mode can be changed as quickly as
        /// possible.
        /// </summary>
        /// <param name="enabled">True to enable the SimObject, false to disable it.</param>
        public abstract void setEnabled(bool enabled);

        /// <summary>
        /// Save this SimObject to a SimObjectDefinition.
        /// </summary>
        /// <param name="definitionName">The name to give the SimObjectDefinition.</param>
        /// <returns>A new SimObjectDefinition for this SimObject.</returns>
        public abstract SimObjectDefinition saveToDefinition(String definitionName);

        #endregion Functions
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// A SimObject is a mediator between various SimElement instances. This
    /// allows a SimObject to represent any kind of object in a 3d scene
    /// utilizing whatever subsystems are needed for it to do work. For example
    /// it could be composed of a mesh from a renderer and a rigid body from a
    /// physics engine allowing it to move and be rendered. The updates will be
    /// fired appropriatly between these subsystems to keep everything in sync.
    /// </summary>
    /// <seealso cref="T:System.IDisposable"/>
    public interface SimObject : IDisposable
    {
        #region Functions

        /// <summary>
        /// Add a SimElement to this SimObject.
        /// </summary>
        /// <param name="element">The element to add.</param>
        void addElement(SimElement element);

        /// <summary>
        /// Update the position of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        void updatePosition(ref Vector3 translation, ref Quaternion rotation, SimElement trigger);

        /// <summary>
        /// Update the translation of the SimObject.
        /// </summary>
        /// <param name="translation">The translation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        void updateTranslation(ref Vector3 translation, SimElement trigger);

        /// <summary>
        /// Update the rotation of the SimObject.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        void updateRotation(ref Quaternion rotation, SimElement trigger);

        /// <summary>
        /// Update the scale of the SimObject.
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        /// <param name="trigger">The object that triggered the update. Can be null.</param>
        void updateScale(ref Vector3 scale, SimElement trigger);

        /// <summary>
        /// Set the SimObject as enabled or disabled. The subsystems will
        /// determine the exact status that that their objects will go into when
        /// this is activated. However, this mode can be changed as quickly as
        /// possible.
        /// </summary>
        /// <param name="enabled">True to enable the SimObject, false to disable it.</param>
        void setEnabled(bool enabled);

        /// <summary>
        /// Save this SimObject to a SimObjectDefinition.
        /// </summary>
        /// <param name="definitionName">The name to give the SimObjectDefinition.</param>
        /// <returns>A new SimObjectDefinition for this SimObject.</returns>
        SimObjectDefinition saveToDefinition(String definitionName);

        /// <summary>
        /// Save the instance specific information for this SimObject.
        /// </summary>
        /// <returns>A new SimObjectInstanceDefinition.</returns>
        SimObjectInstanceDefinition saveInstanceDefinition();

        /// <summary>
        /// Get a particular SimElement from the SimObject. This will return
        /// null if the element cannot be found. This method could potentially
        /// be fairly slow so it is best to cache the values returned from this
        /// function somehow.
        /// </summary>
        /// <param name="name">The name of the SimElement to retrieve.</param>
        /// <returns>The SimElement specified by name or null if it cannot be found.</returns>
        SimElement getElement(String name);

        #endregion Functions

        #region Properties

        /// <summary>
        /// Get the name of this SimObject.
        /// </summary>
        String Name
        {
            get;
        }

        /// <summary>
        /// Get the enabled status of this SimObject.
        /// </summary>
        bool Enabled
        {
            get;
        }

        /// <summary>
        /// Get the translation of this SimObject.
        /// </summary>
        Vector3 Translation
        {
            get;
        }

        /// <summary>
        /// Get the rotation of the SimObject.
        /// </summary>
        Quaternion Rotation
        {
            get;
        }

        /// <summary>
        /// Get the scale of the SimObject.
        /// </summary>
        Vector3 Scale
        {
            get;
        }

        #endregion Properites
    }
}

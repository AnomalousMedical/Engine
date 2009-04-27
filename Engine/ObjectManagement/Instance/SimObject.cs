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
    /// This top level SimObject interface does not allow access to all
    /// functions in a SimObject, however, it provides all the access that are
    /// needed by individual SimElements. The missing functionality is filled in
    /// with protected functions in the SimElement class.
    /// </summary>
    /// <remarks>
    /// New SimObject types should implement the SimObjectBase interface instead
    /// of this one so they provide the entire required interface.
    /// </remarks>
    public interface SimObject
    {
        #region Functions

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

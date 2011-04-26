using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This class defines an instance of a sim object.
    /// </summary>
    public interface SimObjectDefinition : Saveable
    {
        #region Functions

        /// <summary>
        /// Register with factories to build this definition into the given SimObject.
        /// </summary>
        /// <param name="subScene">The SimSubScene to bulid the SimObject into.</param>
        /// <returns>A new SimObjectBase with the newly created SimObject.</returns>
        SimObjectBase register(SimSubScene subScene);

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        EditInterface getEditInterface();

        #endregion Functions

        #region Properties

        /// <summary>
        /// The instance name of this SimObject.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// The initial rotation of the sim object.
        /// </summary>
        Quaternion Rotation { get; set; }

        /// <summary>
        /// The initial translation of the sim object.
        /// </summary>
        Vector3 Translation { get; set; }

        /// <summary>
        /// The initial scale of the object.
        /// </summary>
        Vector3 Scale { get; set; }

        /// <summary>
        /// True if the object is enabled, false if it is disabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// This function is used to paste a SimElementDefinition into this SimObjectDefinition.
        /// </summary>
        /// <param name="simElement"></param>
        void pasteElement(SimElementDefinition simElement);

        #endregion Properties
    }
}

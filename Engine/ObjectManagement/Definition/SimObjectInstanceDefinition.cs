using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine.Saving;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is the definition for a single instance of a SimObject. This may or
    /// may not have a 1-1 relationship with a SimObjectDefinition as those can
    /// be used to create multiple instances that start out the same.
    /// </summary>
    public interface SimObjectInstanceDefinition : Saveable
    {
        SimObject createSimObject();

        /// <summary>
        /// The name of the instance of the sim object.
        /// </summary>
        String Name { get; }

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
        /// The name of the defintion to load for the sim object.
        /// </summary>
        String DefinitionName { get; set; }

        /// <summary>
        /// True if the object is enabled, false if it is disabled.
        /// </summary>
        bool Enabled { get; set; }
    }
}

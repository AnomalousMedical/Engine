using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This is an interface to a SimObject that can have things added and removed from it.
    /// </summary>
    public interface CompositeSimObjectDefinition
    {
        /// <summary>
        /// Add a SimElementDefinition. A element should only be added to
        /// one defintion.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        void addElement(SimElementDefinition definition);

        /// <summary>
        /// Remove a SimElementDefinition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        void removeElement(SimElementDefinition definition);
    }
}

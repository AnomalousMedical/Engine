using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    /// <summary>
    /// This interface is a definition class for SimElementManagers.
    /// </summary>
    public interface SimElementManagerDefinition
    {
        /// <summary>
        /// Get an EditInterface.
        /// </summary>
        /// <returns>An EditInterface for the definition or null if there is not interface.</returns>
        EditInterface getEditInterface();

        /// <summary>
        /// Create the SimElementManager this definition defines and return it.
        /// This may not be safe to call more than once per definition.
        /// </summary>
        /// <returns>The SimElementManager this definition is designed to create.</returns>
        SimElementManager getSimElementManager();

        /// <summary>
        /// The name of the definition and the SimElementManager it defines.
        /// </summary>
        String Name { get; }
    }
}

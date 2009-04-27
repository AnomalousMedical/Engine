using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Saving;
using Engine.Editing;

namespace Engine
{
    /// <summary>
    /// This is the interface for classes that provide data about behaviors.
    /// </summary>
    public interface BehaviorData : Saveable
    {
        /// <summary>
        /// Get an EditInterface for the Behavior.
        /// </summary>
        /// <returns>The EditInterface for the behavior.</returns>
        EditInterface getEditInterface();

        /// <summary>
        /// Create a new instance of the Behavior provided by this data.
        /// </summary>
        /// <returns>A new Behavior for the given data.</returns>
        Behavior createNewInstance();
    }
}

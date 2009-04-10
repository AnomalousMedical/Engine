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
    public interface SimElementManagerDefinition : IDisposable
    {
        /// <summary>
        /// Get an EditInterface.
        /// </summary>
        /// <param name="destroyCommand">A DestroyEditInterfaceCommand that will destroy the SimElementManagerDefinition.</param>
        /// <returns>An EditInterface for the definition or null if there is not interface.</returns>
        EditInterface getEditInterface();

        /// <summary>
        /// Create the SimElementManager this definition defines and return it.
        /// This may not be safe to call more than once per definition.
        /// </summary>
        /// <returns>The SimElementManager this definition is designed to create.</returns>
        SimElementManager createSimElementManager();

        /// <summary>
        /// The name of the definition and the SimElementManager it defines.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// This will return the type the SimElementManager wishes to report
        /// itself as. Usually this will be the type of the class itself,
        /// however, it is possible to specify a superclass if desired. This
        /// will be the type reported to the SimSubScene. This should be the
        /// value returned by the SimElementManager this definition creates.
        /// </summary>
        /// <returns></returns>
        Type getSimElementManagerType();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This interface defines the common functions for a class that maintains
    /// objects created by a subsystem.
    /// </summary>
    public interface SimElementManager : IDisposable
    {
        /// <summary>
        /// Get the factory that builds SimElements.
        /// </summary>
        /// <returns>The factory.</returns>
        SimElementFactory getFactory();

        /// <summary>
        /// This will return the type the SimElementManager wishes to report
        /// itself as. Usually this will be the type of the class itself,
        /// however, it is possible to specify a superclass if desired. This
        /// will be the type reported to the SimSubScene.
        /// </summary>
        /// <returns></returns>
        Type getSimElementManagerType();

        /// <summary>
        /// Get the name.
        /// </summary>
        /// <returns>The name.</returns>
        String getName();

        /// <summary>
        /// Create a definition for this SimElementManager.
        /// </summary>
        /// <returns>A new SimElementManager for this definition.</returns>
        SimElementManagerDefinition createDefinition();

        /// <summary>
        /// Set the scene this SimElementManager is using
        /// </summary>
        /// <param name="simScene">The scene</param>
        void setScene(SimScene simScene);
    }
}

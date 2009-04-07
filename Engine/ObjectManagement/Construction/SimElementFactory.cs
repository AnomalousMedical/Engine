using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This interface describes the basic functions needed by factories to
    /// build subsystem specific elements.
    /// </summary>
    public interface SimElementFactory
    {
        /// <summary>
        /// Create all products for normal operation currently registered for
        /// construction in this factory.
        /// </summary>
        void createProducts();

        /// <summary>
        /// Create all products for static mode operation currently registered
        /// for construction in this factory.
        /// </summary>
        void createStaticProducts();

        /// <summary>
        /// This function will be called when all subsystems have created their
        /// products. At this time it is safe to discover objects present in
        /// other subsystems.
        /// </summary>
        void linkProducts();

        /// <summary>
        /// This function will clear all definitions in the factory. It will be
        /// called after a construction run has completed by executing
        /// createProducts or createStaticProducts.
        /// </summary>
        void clearDefinitions();
    }
}

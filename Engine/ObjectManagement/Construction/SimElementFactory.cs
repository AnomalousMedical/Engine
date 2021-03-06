﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.ObjectManagement
{
    /// <summary>
    /// This interface describes the basic functions needed by factories to
    /// build subsystem specific elements.
    /// </summary>
    public interface SimElementFactory
    {
        /// <summary>
        /// Create all products for normal operation currently registered for
        /// construction in this factory. This function can be an enumerator that
        /// returns SceneBuildStatus objects (pooling this is desired, make one per
        /// function call) that will allow something loading the scene to follow
        /// the status of the load.
        /// </summary>
        IEnumerable<SceneBuildStatus> createProducts(SceneBuildOptions options);

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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.ObjectManagement;
using Engine.Saving;

namespace PhysXPlugin
{
    /// <summary>
    /// This is an abstract base class for all elements built by the PhysXPlugin
    /// </summary>
    public abstract class PhysElementDefinition : SimElementDefinition
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        internal PhysElementDefinition(String name)
            :base(name)
        {
            
        }

        protected PhysElementDefinition(LoadInfo info)
            : base(info)
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Create a new element normally as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the element to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal abstract void createProduct(SimObjectBase instance, PhysXSceneManager scene);

        /// <summary>
        /// Create a new element staticly as a part of scene and add it to instance.
        /// </summary>
        /// <param name="instance">The SimObject to add the element to.</param>
        /// <param name="scene">The PhysSceneManager to create the product with.</param>
        internal abstract void createStaticProduct(SimObjectBase instance, PhysXSceneManager scene);

        #endregion Functions
    }
}

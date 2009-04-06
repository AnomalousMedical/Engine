using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine.Editing;

namespace Engine
{
    public abstract class SimComponentDefinition
    {   
        #region Fields

        private String name;
        protected SimObjectDefinition simObjectDef;
        protected Subscription subscription = Subscription.All;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the name of the component.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        public SimComponentDefinition(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Register this component with its factory so it can be built.
        /// </summary>
        /// <param name="instance">The SimObject that will get the newly created component.</param>
        public abstract void register(SimObject instance);

        /// <summary>
        /// Get an EditInterface for the SimComponentDefinition so it can be
        /// modified.
        /// </summary>
        /// <returns>The EditInterface for this SimComponentDefinition.</returns>
        public abstract EditInterface getEditInterface();

        /// <summary>
        /// Set the SimObjectDefinition for this component.
        /// </summary>
        /// <param name="simObjectDef">The definition to set.</param>
        internal void setSimObjectDefinition(SimObjectDefinition simObjectDef)
        {
            this.simObjectDef = simObjectDef;
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// Get the name of this SimComponent.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
        }

        #endregion Properties
    }
}

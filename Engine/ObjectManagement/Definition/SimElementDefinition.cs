using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;
using Engine.Editing;

namespace Engine.ObjectManagement
{
    public abstract class SimElementDefinition
    {   
        #region Fields

        private String name;
        protected SimObjectDefinition simObjectDef;
        protected Subscription subscription = Subscription.All;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the name of the element.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public SimElementDefinition(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Register this element with its factory so it can be built.
        /// </summary>
        /// <param name="subscene">The SimSubScene that will get the built product.</param>
        /// <param name="instance">The SimObject that will get the newly created element.</param>
        public abstract void register(SimSubScene subscene, SimObject instance);

        /// <summary>
        /// Get an EditInterface for the SimElementDefinition so it can be
        /// modified.
        /// </summary>
        /// <returns>The EditInterface for this SimElementDefinition.</returns>
        public abstract EditInterface getEditInterface();

        /// <summary>
        /// Set the SimObjectDefinition for this element.
        /// </summary>
        /// <param name="simObjectDef">The definition to set.</param>
        internal void setSimObjectDefinition(SimObjectDefinition simObjectDef)
        {
            this.simObjectDef = simObjectDef;
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// Get the name of this SimElement.
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

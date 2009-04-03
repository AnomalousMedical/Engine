using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;

namespace Engine
{
    /// <summary>
    /// This class defines an instance of a sim object.
    /// </summary>
    public class SimObjectDefinition
    {
        #region Fields

        private LinkedList<SimComponentDefinition> definitions = new LinkedList<SimComponentDefinition>();
        private String name;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor. Takes the name of the definition.
        /// </summary>
        /// <param name="instanceName">The name of the definition.</param>
        public SimObjectDefinition(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Add a SimComponentDefinition. A component should only be added to
        /// one defintion.
        /// </summary>
        /// <param name="definition">The definition to add.</param>
        public void addSimComponentDefinition(SimComponentDefinition definition)
        {
            definition.setSimObjectDefinition(this);
            definitions.AddLast(definition);
        }

        /// <summary>
        /// Remove a SimComponentDefinition.
        /// </summary>
        /// <param name="definition">The definition to remove.</param>
        public void removeSimComponentDefinition(SimComponentDefinition definition)
        {
            definition.setSimObjectDefinition(null);
            definitions.Remove(definition);
        }

        /// <summary>
        /// Register with factories to build this definition into the given SimObject.
        /// </summary>
        /// <param name="instance">The SimObject that will get the built components.</param>
        public void register(SimObject instance)
        {
            foreach (SimComponentDefinition definition in definitions)
            {
                definition.register(instance);
            }
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The instance name of this SimObject.
        /// </summary>
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        #endregion Properties
    }
}

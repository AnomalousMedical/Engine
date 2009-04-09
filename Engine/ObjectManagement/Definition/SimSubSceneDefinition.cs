using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;

namespace Engine
{
    /// <summary>
    /// This is a definition for a SimSubScene.
    /// </summary>
    public class SimSubSceneDefinition
    {
        #region Fields

        private Dictionary<Type, SimSubSceneBinding> bindings = new Dictionary<Type, SimSubSceneBinding>();
        private String name;
        private EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
        private SimSubSceneEditInterface editInterface = null;

        #endregion Fields

        #region Constructors

        public SimSubSceneDefinition(String name)
        {
            this.name = name;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Determine if the given SimElementManager type is already contained
        /// in this SimSubSceneDefinition. SimSubScenes can only have one of
        /// each type of SimElementManager in them.
        /// </summary>
        /// <param name="type">The type to check for.</param>
        /// <returns>True if this definition already contains a SimElementManagerDefinition of the given type.</returns>
        public bool hasTypeBindings(Type type)
        {
            return bindings.ContainsKey(type);
        }

        /// <summary>
        /// Get the bindings to SimComponentManagers.
        /// </summary>
        /// <returns>An enumerable over all SimComponentManager bindings.</returns>
        public IEnumerable<SimSubSceneBinding> getBindings()
        {
            return bindings.Values;
        }

        /// <summary>
        /// Get the EditInterface.
        /// </summary>
        /// <returns>The EditInterface.</returns>
        public SimSubSceneEditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = new SimSubSceneEditInterface(this);
            }
            return editInterface;
        }

        #endregion Functions

        #region Properties

        /// <summary>
        /// The name of the SubScene.
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

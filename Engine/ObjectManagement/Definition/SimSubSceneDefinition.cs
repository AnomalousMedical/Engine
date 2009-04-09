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

        private Dictionary<Type, String> bindings = new Dictionary<Type, String>();
        private String name;
        private EditablePropertyInfo propertyInfo = new EditablePropertyInfo();
        private SimSubSceneEditInterface editInterface = null;
        private SimSceneDefinition scene;

        #endregion Fields

        #region Constructors

        public SimSubSceneDefinition(String name, SimSceneDefinition scene)
        {
            this.name = name;
            this.scene = scene;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Get the SimElementManagerDefinition indicated by name. This will
        /// return null if there is no defintion with the given name.
        /// </summary>
        /// <param name="name">The name to test for.</param>
        /// <returns>The matching definition or null if it cannot be found.</returns>
        public SimElementManagerDefinition getSimElementManager(String name)
        {
            return scene.getSimElementManagerDefinition(name);
        }

        /// <summary>
        /// Determine if the given SimElementManager type is already contained
        /// in this SimSubSceneDefinition. SimSubScenes can only have one of
        /// each type of SimElementManager in them.
        /// </summary>
        /// <param name="type">The type to check for.</param>
        /// <returns>True if this definition already contains a SimElementManagerDefinition of the given type.</returns>
        public bool hasTypeBindings(SimElementManagerDefinition type)
        {
            return bindings.ContainsKey(type.GetType());
        }

        /// <summary>
        /// Add a binding.
        /// </summary>
        /// <param name="toBind">The SimElementManagerDefinition to bind to this SimSubScene</param>
        public void addBinding(SimElementManagerDefinition toBind)
        {
            bindings.Add(toBind.GetType(), toBind.Name);
        }

        /// <summary>
        /// Remove a binding.
        /// </summary>
        /// <param name="toBind">The SimElementManagerDefinition to remove from this SimSubScene</param>
        public void removeBinding(SimElementManagerDefinition toBind)
        {
            bindings.Remove(toBind.GetType());
        }

        /// <summary>
        /// Get the bindings to SimComponentManagers.
        /// </summary>
        /// <returns>An enumerable over all SimComponentManager bindings.</returns>
        public IEnumerable<String> getBindings()
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

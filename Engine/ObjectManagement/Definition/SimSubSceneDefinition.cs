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
    public class SimSubSceneDefinition : EditInterface
    {
        #region Fields

        private Dictionary<Type, String> definedTypes = new Dictionary<Type, string>();
        private String name;

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
        public bool hasTypeDefinition(Type type)
        {
            return definedTypes.ContainsKey(type);
        }

        /// <summary>
        /// Determine if the a given SimElementManager name is already defined.
        /// </summary>
        /// <param name="name">The name of the SimElementManager.</param>
        /// <returns>True if the name is already defined as part of this class.</returns>
        public bool hasNamedDefinition(String name)
        {
            return definedTypes.ContainsValue(name);
        }

        #region EditInterface Members

        public string getName()
        {
            return name + " - SubScene";
        }

        public bool hasEditableProperties()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EditableProperty> getEditableProperties()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the EditablePropertyInfo for this interface that determines
        /// the layout of a single property. This can be null if
        /// hasEditableProperties is false.
        /// </summary>
        /// <returns>The EditablePropertyInfo for this interface.</returns>
        public EditablePropertyInfo getPropertyInfo()
        {
            throw new NotImplementedException();
        }

        public bool hasSubEditInterfaces()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EditInterface> getSubEditInterfaces()
        {
            throw new NotImplementedException();
        }

        public bool hasCreateSubObjectCommands()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CreateEditInterfaceCommand> getCreateSubObjectCommands()
        {
            throw new NotImplementedException();
        }

        public bool hasDestroyObjectCommand()
        {
            throw new NotImplementedException();
        }

        public DestroyEditInterfaceCommand getDestroyObjectCommand()
        {
            throw new NotImplementedException();
        }

        public object getCommandTargetObject()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine if this interface can create properties.
        /// </summary>
        /// <returns>True if this interface can create properties.</returns>
        public bool hasCreatePropertyCommand()
        {
            return false;
        }

        /// <summary>
        /// Get the command that creates new properties.
        /// </summary>
        /// <returns>A CreateEditablePropertyCommand to create properties or null if it does not have one.</returns>
        public CreateEditablePropertyCommand getCreatePropertyCommand()
        {
            return null;
        }

        /// <summary>
        /// Determine if this interface can destroy properties.
        /// </summary>
        /// <returns>True if this interface can destroy properties.</returns>
        public bool hasDestroyPropertyCommand()
        {
            return false;
        }

        /// <summary>
        /// Get the command that destroys properties.
        /// </summary>
        /// <returns>A DestroyEditablePropertyCommand to destroy properties or null if it does not have one.</returns>
        public DestroyEditablePropertyCommand getDestroyPropertyCommand()
        {
            return null;
        }

        #endregion

        #endregion Functions

    }
}

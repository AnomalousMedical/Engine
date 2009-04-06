using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This is a base class for properties that use reflection to get/set their
    /// values.
    /// </summary>
    class ReflectedEditableProperty : EditableProperty
    {
        #region Fields

        private String name;
        private ReflectedVariable variable;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the wrapped variable.</param>
        /// <param name="variable">The ReflectedVariable to be edited.</param>
        public ReflectedEditableProperty(String name, ReflectedVariable variable)
        {
            this.name = name;
            this.variable = variable;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Get the name of this property.
        /// </summary>
        /// <returns>The name of this property.</returns>
        public string getName()
        {
            return name;
        }

        /// <summary>
        /// Get the value of this property.
        /// </summary>
        /// <returns>The value of this property.</returns>
        public Object getValue()
        {
            return variable.getValue();
        }

        /// <summary>
        /// Set the value of this property.
        /// </summary>
        /// <param name="value">The value to set. Must be the correct type.</param>
        public void setValue(object value)
        {
            variable.setValue(value);
        }

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        /// <param name="value">The value as a string to set.</param>
        public void setValueStr(string value)
        {
            variable.setValueString(value);
        }

        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="value">The value to try to parse.</param>
        /// <returns>True if the string can be parsed.</returns>
        public bool canParseString(string value)
        {
            return variable.canParseString(value);
        }

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <returns>The Type of the object this property will set.</returns>
        public Type getPropertyType()
        {
            return variable.getVariableType();
        }

        #endregion
    }
}

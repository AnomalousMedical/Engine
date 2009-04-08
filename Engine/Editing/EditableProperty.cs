using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    /// <summary>
    /// This interface allows a single property of an EditInterface to be changed.
    /// </summary>
    public interface EditableProperty
    {
        /// <summary>
        /// Get the name of this property.
        /// </summary>
        /// <returns>The name of this property.</returns>
        String getName();

        /// <summary>
        /// Get the value of this property.
        /// </summary>
        /// <returns>The value of this property.</returns>
        Object getValue();

        /// <summary>
        /// Set the value of this property.
        /// </summary>
        /// <param name="value">The value to set. Must be the correct type.</param>
        void setValue(Object value);

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        void setValueStr(String value);

        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="value">The value to try to parse.</param>
        /// <param name="errorMessage">An error message if the function returns false.</param>
        /// <returns>True if the string can be parsed.</returns>
        bool canParseString(String value, out String errorMessage);

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <returns>The Type of the object this property will set.</returns>
        Type getPropertyType();
    }
}

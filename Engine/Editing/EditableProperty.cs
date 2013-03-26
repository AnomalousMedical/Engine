using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    /// <summary>
    /// This interface allows a single property of an EditInterface to be
    /// changed. A property can define as many columns as it needs to define
    /// itself.
    /// </summary>
    public interface EditableProperty
    {
        /// <summary>
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        String getValue(int column);

        /// <summary>
        /// Get the actual value for a given column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <returns></returns>
        Object getRealValue(int column);

        void setValue(int column, Object value);

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        void setValueStr(int column, String value);

        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="value">The value to try to parse.</param>
        /// <param name="errorMessage">An error message if the function returns false.</param>
        /// <returns>True if the string can be parsed.</returns>
        bool canParseString(int column, String value, out String errorMessage);

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <returns>The Type of the object this property will set.</returns>
        Type getPropertyType(int column);

        /// <summary>
        /// Get a browser for a column for display. Note that this may be
        /// created when this function is called, so it could be slow and should
        /// only be called when you actually need a browser. If you need to
        /// check call hasbrowser.
        /// </summary>
        /// <param name="column">The column number to get the browser for.</param>
        /// <returns>The browser object for this column.</returns>
        Browser getBrowser(int column, EditUICallback uiCallback);

        /// <summary>
        /// Returns true if the specified column has a browser.
        /// </summary>
        /// <param name="column">The column to check.</param>
        /// <returns>True if there is a browser in this column.</returns>
        bool hasBrowser(int column);

        /// <summary>
        /// Set this to true to indicate to the ui that this property is advanced.
        /// </summary>
        bool Advanced { get; }
    }
}

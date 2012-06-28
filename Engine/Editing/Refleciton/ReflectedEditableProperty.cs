using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This class uses reflection to get/set the value of a simple type.
    /// </summary>
    public class ReflectedEditableProperty : EditableProperty
    {
        #region Static

        private const int NAME_COL = 0;
        private const int VALUE_COL = 1;

        #endregion Static

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
        /// Get the value for a given column.
        /// </summary>
        /// <param name="column">The column to get the value for.</param>
        /// <returns></returns>
        public String getValue(int column)
        {
            switch(column)
            {
                case NAME_COL:
                    return name;
                case VALUE_COL:
                    Object value = variable.getValue();
                    if (value != null)
                    {
                        return value.ToString();
                    }
                    else
                    {
                        return null;
                    }
                default:
                    throw new NotImplementedException("Should not get here");
            }
        }

        public Object getRealValue(int column)
        {
            switch (column)
            {
                case NAME_COL:
                    return name;
                case VALUE_COL:
                    Object value = variable.getValue();
                    if (value != null)
                    {
                        return value;
                    }
                    else
                    {
                        return null;
                    }
                default:
                    throw new NotImplementedException("Should not get here");
            }
        }

        /// <summary>
        /// Set the value of this property.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="value">The value to set. Must be the correct type.</param>
        public void setValue(int column, object value)
        {
            if (column == VALUE_COL)
            {
                variable.setValue(value);
            }
        }

        /// <summary>
        /// Set the value of this property from a string.
        /// </summary>
        /// <param name="value">The value as a string to set.</param>
        public void setValueStr(int column, string value)
        {
            if (column == VALUE_COL)
            {
                variable.setValueString(value);
            }
        }


        /// <summary>
        /// Determine if the given string is in the correct format for this
        /// property to parse.
        /// </summary>
        /// <param name="column">The column to test.</param>
        /// <param name="value">The value to try to parse.</param>
        /// <param name="errorMessage">An error message if the function returns false.</param>
        /// <returns>True if the string can be parsed.</returns>
        public bool canParseString(int column, string value, out String errorMessage)
        {
            if (column == VALUE_COL)
            {
                if (variable.canParseString(value))
                {
                    errorMessage = null;
                    return true;
                }
                else
                {
                    errorMessage = String.Format("Cannot parse the value to a {0}", variable.getVariableType().Name);
                    return false;
                }
            }
            else
            {
                throw new NotImplementedException("Should not get here");
            }
        }

        /// <summary>
        /// Get the type of this property's target object.
        /// </summary>
        /// <returns>The Type of the object this property will set.</returns>
        public Type getPropertyType(int column)
        {
            switch (column)
            {
                case NAME_COL:
                    return typeof(String);
                case VALUE_COL:
                    return variable.getVariableType();
                default:
                    throw new NotImplementedException("Should not get here");
            }
        }

        public bool hasBrowser(int column)
        {
            return false;
        }

        public Browser getBrowser(int column, EditUICallback uiCallback)
        {
            return null;
        }

        #endregion
    }
}

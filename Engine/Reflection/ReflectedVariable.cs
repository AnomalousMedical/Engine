using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    /// <summary>
    /// This abstract class is a supertype for types that can be set or
    /// retrieved directly.
    /// </summary>
    public abstract class ReflectedVariable
    {
        #region Fields

        protected MemberWrapper propertyInfo;
        private object instance;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        /// <param name="visibility">The visibility of the variable.</param>
        public ReflectedVariable(MemberWrapper propInfo, Object instance)
        {
            this.propertyInfo = propInfo;
            this.instance = instance;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Determine if the given object is a valid value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>True if value can be set as the value of this variable.</returns>
        public virtual bool canSetValue(Object value)
        {
            return value.GetType() == propertyInfo.getWrappedType();
        }

        /// <summary>
        /// Get the value as an object.
        /// </summary>
        /// <returns>The value as an object.</returns>
        public object getValue()
        {
            return propertyInfo.getValue(instance, null);
        }

        /// <summary>
        /// Set the value as an object.
        /// </summary>
        /// <param name="value">An object with the value, must be of the correct type.</param>
        public void setValue(object value)
        {
            propertyInfo.setValue(instance, value, null);
        }

        /// <summary>
        /// Determines if value can be parsed to the underlying type.
        /// </summary>
        /// <returns>True if the String can be parsed correctly.</returns>
        public abstract bool canParseString(String value);

        /// <summary>
        /// Get the value of this type as a string.
        /// </summary>
        /// <returns>The value of this type as a string.</returns>
        public virtual String getValueString()
        {
            return getValue().ToString();
        }

        /// <summary>
        /// Set the value of this type as a string.
        /// </summary>
        /// <param name="value">The string to set as the value.</param>
        public abstract void setValueString(String value);

        #endregion Functions
    }
}

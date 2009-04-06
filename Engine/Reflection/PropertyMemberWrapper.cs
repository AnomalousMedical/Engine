using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Engine.Reflection
{
    /// <summary>
    /// This is a wrapper class for PropertyInfo
    /// </summary>
    public class PropertyMemberWrapper : MemberWrapper
    {
        #region Fields

        private PropertyInfo propInfo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propInfo">The property info to wrap.</param>
        public PropertyMemberWrapper(PropertyInfo propInfo)
        {
            this.propInfo = propInfo;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Return the value of the wrapped member.
        /// </summary>
        /// <param name="instance">The instance to get the value out of.</param>
        /// <param name="index">An optional index.</param>
        /// <returns>The value contained in instance's wrapped member.</returns>
        public object getValue(object instance, object[] index)
        {
            if (instance != null)
            {
                return propInfo.GetValue(instance, index);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set the value of the wrapped member.
        /// </summary>
        /// <param name="instance">The instance to set the value on.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="index">An optional index.</param>
        public void setValue(object instance, object value, object[] index)
        {
            propInfo.SetValue(instance, value, index);
        }

        /// <summary>
        /// Returns the type of the wrapped member.
        /// </summary>
        /// <returns>The type of the wrapped member.</returns>
        public Type getWrappedType()
        {
            return propInfo.PropertyType;
        }

        /// <summary>
        /// Get the name of the wrapped variable.
        /// </summary>
        /// <returns></returns>
        public String getWrappedName()
        {
            return propInfo.Name;
        }

        /// <summary>
        /// Get the custom attributes of this member.
        /// </summary>
        /// <param name="type">The type of attributes to get.</param>
        /// <param name="inherit">True to search inheritance chain for attributes.</param>
        /// <returns>An array of matching attributes.</returns>
        public object[] getCustomAttributes(Type type, bool inherit)
        {
            return propInfo.GetCustomAttributes(type, inherit);
        }

        /// <summary>
        /// Get the custom attributes of this member.
        /// </summary>
        /// <param name="inherit">True to search inheritance chain for attributes.</param>
        /// <returns>An array of matching attributes.</returns>
        public object[] getCustomAttributes(bool inherit)
        {
            return propInfo.GetCustomAttributes(inherit);
        }

        #endregion Functions
    }
}

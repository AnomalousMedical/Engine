using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Engine.Reflection
{
    /// <summary>
    /// This is a wrapper class for FieldInfo
    /// </summary>
    public struct FieldMemberWrapper : MemberWrapper
    {
        #region Fields

        private FieldInfo fieldInfo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fieldInfo">The field info to use.</param>
        public FieldMemberWrapper(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Get the value from the field.
        /// </summary>
        /// <param name="instance">The instance to get the value from.</param>
        /// <param name="index">The index of the value.</param>
        /// <returns>The value inside of instance.</returns>
        public object getValue(object instance, object[] index)
        {
            if (instance != null)
            {
                return fieldInfo.GetValue(instance);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set the value on instace.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="index">The index of the value.</param>
        public void setValue(object instance, object value, object[] index)
        {
            fieldInfo.SetValue(instance, value);
        }

        /// <summary>
        /// Get the type of the wrapped field.
        /// </summary>
        /// <returns>The type of the field.</returns>
        public Type getWrappedType()
        {
            return fieldInfo.FieldType;
        }

        /// <summary>
        /// Get the name of the wrapped field.
        /// </summary>
        /// <returns></returns>
        public String getWrappedName()
        {
            return fieldInfo.Name;
        }

        /// <summary>
        /// Get the custom attributes of this member.
        /// </summary>
        /// <param name="type">The type of attributes to get.</param>
        /// <param name="inherit">True to search inheritance chain for attributes.</param>
        /// <returns>An array of matching attributes.</returns>
        public IEnumerable<Attribute> getCustomAttributes(Type type, bool inherit)
        {
            return (IEnumerable<Attribute>)fieldInfo.GetCustomAttributes(type, inherit);
        }

        /// <summary>
        /// Get the custom attributes of this member.
        /// </summary>
        /// <param name="inherit">True to search inheritance chain for attributes.</param>
        /// <returns>An array of matching attributes.</returns>
        public IEnumerable<Attribute> getCustomAttributes(bool inherit)
        {
            return (IEnumerable<Attribute>)fieldInfo.GetCustomAttributes(inherit);
        }

        /// <summary>
        /// Determine if the MemberWrapper can write to its variable. Will
        /// return true if it can.
        /// </summary>
        /// <returns>True if the MemberWrapper can write to its variable.</returns>
        public bool canWrite()
        {
            return true;
        }

        /// <summary>
        /// Determine if the MemberWrapper can read from its variable. Will
        /// return true if it can.
        /// </summary>
        /// <returns>True if the MemberWrapper can read from its variable.</returns>
        public bool canRead()
        {
            return true;
        }
        
        #endregion Functions
    }
}

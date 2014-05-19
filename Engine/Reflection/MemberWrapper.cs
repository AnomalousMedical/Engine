using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    /// <summary>
    /// This interface fills in a hole in the reflection member classes where there is
    /// no getValue/setValue functions on PropertyInfo and MemberInfo.  This interface
    /// allows for these to be viewed the same way.
    /// </summary>
    public interface MemberWrapper
    {
        /// <summary>
        /// Return the value of the wrapped member.
        /// </summary>
        /// <param name="instance">The instance to get the value out of.</param>
        /// <param name="index">An optional index.</param>
        /// <returns>The value contained in instance's wrapped member.</returns>
        object getValue(object instance, object[] index);

        /// <summary>
        /// Set the value of the wrapped member.
        /// </summary>
        /// <param name="instance">The instance to set the value on.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="index">An optional index.</param>
        void setValue(object instance, object value, object[] index);

        /// <summary>
        /// Returns the type of the wrapped member.
        /// </summary>
        /// <returns>The type of the wrapped member.</returns>
        Type getWrappedType();

        /// <summary>
        /// Get the name of the wrapped variable.
        /// </summary>
        /// <returns></returns>
        String getWrappedName();

        /// <summary>
        /// Get the custom attributes of this member.
        /// </summary>
        /// <param name="type">The type of attributes to get.</param>
        /// <param name="inherit">True to search inheritance chain for attributes.</param>
        /// <returns>An array of matching attributes.</returns>
        IEnumerable<Attribute> getCustomAttributes(Type type, bool inherit);

        /// <summary>
        /// Get the custom attributes of this member.
        /// </summary>
        /// <param name="inherit">True to search inheritance chain for attributes.</param>
        /// <returns>An array of matching attributes.</returns>
        IEnumerable<Attribute> getCustomAttributes(bool inherit);

        /// <summary>
        /// Determine if the MemberWrapper can write to its variable. Will
        /// return true if it can.
        /// </summary>
        /// <returns>True if the MemberWrapper can write to its variable.</returns>
        bool canWrite();

        /// <summary>
        /// Determine if the MemberWrapper can read from its variable. Will
        /// return true if it can.
        /// </summary>
        /// <returns>True if the MemberWrapper can read from its variable.</returns>
        bool canRead();
    }
}

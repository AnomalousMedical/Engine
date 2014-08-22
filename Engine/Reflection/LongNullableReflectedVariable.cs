using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    class LongNullableReflectedVariable : LongReflectedVariable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        public LongNullableReflectedVariable(MemberWrapper propInfo, Object instance)
            : base(propInfo, instance)
        {

        }

        /// <summary>
        /// Determines if value can be parsed to the underlying type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the String can be parsed correctly.</returns>
        public override bool canParseString(string value)
        {
            return String.IsNullOrEmpty(value) || base.canParseString(value);
        }

        /// <summary>
        /// Set the value of this type as a string.
        /// </summary>
        /// <param name="value">The string to set as the value.</param>
        public override void setValueString(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                setValue(null);
            }
            else
            {
                setValue(new long?(NumberParser.ParseLong(value)));
            }
        }
    }
}

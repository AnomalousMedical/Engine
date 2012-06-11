using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    class Vector2ReflectedVariable : ReflectedVariable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        public Vector2ReflectedVariable(MemberWrapper propInfo, Object instance)
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
            float test;
            return Vector2.parseString(value, out test, out test);
        }

        /// <summary>
        /// Set the value of this type as a string.
        /// </summary>
        /// <param name="value">The string to set as the value.</param>
        public override void setValueString(string value)
        {
            setValue(new Vector2(value));
        }
    }
}

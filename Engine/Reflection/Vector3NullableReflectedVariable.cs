using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    class Vector3NullableReflectedVariable : ReflectedVariable
    {
        private static char[] SEPS = { ',' };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        public Vector3NullableReflectedVariable(MemberWrapper propInfo, Object instance)
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
            if (String.IsNullOrEmpty(value))
            {
                return true;
            }
            String[] vals = value.Split(SEPS);
            if (vals.Length == 3)
            {
                float test;
                return NumberParser.TryParse(vals[0], out test) &&
                    NumberParser.TryParse(vals[1], out test) &&
                    NumberParser.TryParse(vals[2], out test);
            }
            else
            {
                return false;
            }
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
                setValue(new Vector3(value));
            }
        }
    }
}

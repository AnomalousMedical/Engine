using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    class IntReflectedVariable : ReflectedVariable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        public IntReflectedVariable(MemberWrapper propInfo, Object instance)
            : base(propInfo, instance)
        {

        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Determines if value can be parsed to the underlying type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the String can be parsed correctly.</returns>
        public override bool canParseString(string value)
        {
            int test;
            return NumberParser.TryParse(value, out test);
        }

        /// <summary>
        /// Set the value of this type as a string.
        /// </summary>
        /// <param name="value">The string to set as the value.</param>
        public override void setValueString(string value)
        {
            setValue(NumberParser.ParseInt(value));
        }

        #endregion Functions
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace Engine.Reflection
{
    class IdentifierReflectedVariable : ReflectedVariable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        public IdentifierReflectedVariable(MemberWrapper propInfo, Object instance)
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
            return value.Contains("InstanceName=") && value.Contains("BaseName=");
        }

        /// <summary>
        /// Set the value of this type as a string.
        /// </summary>
        /// <param name="value">The string to set as the value.</param>
        public override void setValueString(string value)
        {
            Identifier ident = getValue() as Identifier;
            if (ident == null)
            {
                ident = new Identifier();
                setValue(ident);
            }
            ident.FromString(value);
        }

        #endregion Functions
    }
}

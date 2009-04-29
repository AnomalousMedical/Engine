﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Reflection
{
    class ColorReflectedVariable : ReflectedVariable
    {
        #region Static

        private static char[] SEPS = { ',' };

        #endregion Static

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propInfo">The MemberWrapper to use.</param>
        /// <param name="instance">The object this variable belongs to.</param>
        public ColorReflectedVariable(MemberWrapper propInfo, Object instance)
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
            String[] vals = value.Split(SEPS);
            if (vals.Length == 4)
            {
                float test;
                return float.TryParse(vals[0], out test) &&
                    float.TryParse(vals[1], out test) &&
                    float.TryParse(vals[2], out test) &&
                    float.TryParse(vals[3], out test);
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
            setValue(new Color(value));
        }

        #endregion Functions
    }
}

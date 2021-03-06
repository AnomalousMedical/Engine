﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    /// <summary>
    /// This attribute should be used on any field or property that should be
    /// picked up and used by the ReflectedEditInterface when it is creating the
    /// automatic interface.
    /// 
    /// This version can have min and max defined. It uses float to store the
    /// number, if this type is not acceptible you will have to create a new
    /// attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EditableMinMaxAttribute : EditableAttribute
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">The type of field this variable wraps if it represents something else. Can be used to show more advanced editors.</param>
        public EditableMinMaxAttribute(double minValue, double maxValue, double increment, String info = "")
            :base(info)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Increment = increment;
        }

        public override EditableProperty createEditableProperty(MemberWrapper memberWrapper, Object target)
        {
            return new ReflectedMinMaxEditableProperty(PrettyName != null ? PrettyName : memberWrapper.getWrappedName(), ReflectedVariable.createVariable(memberWrapper, target), MinValue, MaxValue, Increment)
            {
                Advanced = Advanced,
            };
        }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public double Increment { get; set; }
    }
}

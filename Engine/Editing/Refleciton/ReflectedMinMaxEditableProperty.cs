using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    public class ReflectedMinMaxEditableProperty : ReflectedEditableProperty
    {
        public ReflectedMinMaxEditableProperty(String name, ReflectedVariable variable, float minValue, float maxValue, float increment)
            :base(name, variable)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Increment = increment;
        }

        public float MinValue { get; set; }

        public float MaxValue { get; set; }

        public float Increment { get; set; }
    }
}

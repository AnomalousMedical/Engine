using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Reflection;

namespace Engine.Editing
{
    public class ReflectedMinMaxEditableProperty : ReflectedEditableProperty
    {
        public ReflectedMinMaxEditableProperty(String name, ReflectedVariable variable, DoubleBackedNumber minValue, DoubleBackedNumber maxValue, DoubleBackedNumber increment)
            :base(name, variable)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Increment = increment;
        }

        public DoubleBackedNumber MinValue { get; private set; }

        public DoubleBackedNumber MaxValue { get; private set; }

        public DoubleBackedNumber Increment { get; private set; }
    }
}

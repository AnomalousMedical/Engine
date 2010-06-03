using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Enum, Inherited = true, AllowMultiple = false)]
    public sealed class SingleEnumAttribute : Attribute
    {
        
    }
}

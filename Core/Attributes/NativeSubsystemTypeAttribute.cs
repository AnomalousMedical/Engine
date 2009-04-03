using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class NativeSubsystemTypeAttribute : Attribute
    {

    }
}

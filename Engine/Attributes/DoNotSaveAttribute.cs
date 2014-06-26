using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public sealed class DoNotSaveAttribute : Attribute
    {

    }
}

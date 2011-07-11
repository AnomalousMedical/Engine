using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This attribute can be used to give a pretty name to something.
    /// </summary>
    public class PrettyNameAttribute : Attribute
    {
        public PrettyNameAttribute(String name)
        {
            this.Name = name;
        }

        public String Name { get; private set; }
    }
}

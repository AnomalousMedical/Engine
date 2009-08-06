using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A collection of one or more shapes that describe the collision for an object.
    /// </summary>
    public abstract class ShapeCollection : IDisposable
    {
        public ShapeCollection(String name)
        {
            Name = name;
        }

        public abstract void Dispose();

        public ShapeLocation SourceLocation { get; internal set; }

        public String Name { get; private set; }

        public abstract int Count { get; }
    }
}

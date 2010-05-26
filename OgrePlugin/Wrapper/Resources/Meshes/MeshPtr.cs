using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class MeshPtr : IDisposable
    {
        public void Dispose()
        {

        }

        public Mesh Value { get; set; }
    }
}

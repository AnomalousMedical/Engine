using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class VertexData : IDisposable
    {
        IntPtr vertexData;

        internal VertexData(IntPtr vertexData)
        {
            this.vertexData = vertexData;
        }

        public void Dispose()
        {
            vertexData = IntPtr.Zero;
        }
    }
}

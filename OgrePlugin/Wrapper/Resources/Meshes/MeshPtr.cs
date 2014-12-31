using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class MeshPtr : IDisposable
    {
        private SharedPtr<Mesh> sharedPtr;

        internal MeshPtr(SharedPtr<Mesh> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public Mesh Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }
    }
}

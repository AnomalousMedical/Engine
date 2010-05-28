using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class MaterialPtr : IDisposable
    {
        private SharedPtr<Material> sharedPtr;

        internal MaterialPtr(SharedPtr<Material> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public Material Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }
    }
}

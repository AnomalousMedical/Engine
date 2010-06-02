using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class TexturePtr : IDisposable
    {
        private SharedPtr<Texture> sharedPtr;

        internal TexturePtr(SharedPtr<Texture> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public Texture Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }
    }
}

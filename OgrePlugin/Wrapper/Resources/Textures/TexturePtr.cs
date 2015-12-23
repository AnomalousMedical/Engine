using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgrePlugin
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
            if (sharedPtr != null)
            {
                sharedPtr.Dispose();
            }
        }

        public Texture Value
        {
            get
            {
                if (sharedPtr != null)
                {
                    return sharedPtr.Value;
                }
                return null;
            }
        }

        /// <summary>
        /// If you need to pass the actual SharedPtr instance, pass this one that has been allocated on the
        /// heap.
        /// </summary>
        public IntPtr HeapSharedPtr
        {
            get
            {
                return sharedPtr.HeapSharedPtr;
            }
        }
    }
}

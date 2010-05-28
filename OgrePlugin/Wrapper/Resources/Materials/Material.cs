using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class Material : IDisposable
    {
        private IntPtr material;

        internal static Material createWrapper(IntPtr material)
        {
            return new Material(material);
        }

        private Material(IntPtr material)
        {
            this.material = material;
        }

        public void Dispose()
        {
            material = IntPtr.Zero;
        }
    }
}

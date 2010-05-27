using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class MeshManager : IDisposable
    {
        static MaterialManager instance = new MaterialManager();

        public static MaterialManager getInstance()
        {
            return instance;
        }

        public void Dispose()
        {

        }

        internal MeshPtr getPtr(IntPtr ogreMeshPtr)
        {
            throw new NotImplementedException();
        }
    }
}

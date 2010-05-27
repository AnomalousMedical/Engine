using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class SkeletonManager : IDisposable
    {
        static SkeletonManager instance = new SkeletonManager();

        public static SkeletonManager getInstance()
        {
            return instance;
        }

        public void Dispose()
        {

        }
    }
}

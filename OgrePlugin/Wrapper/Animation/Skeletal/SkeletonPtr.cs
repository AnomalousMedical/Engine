using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class SkeletonPtr : IDisposable
    {
        private SharedPtr<Skeleton> sharedPtr;

        internal SkeletonPtr(SharedPtr<Skeleton> sharedPtr)
        {
            this.sharedPtr = sharedPtr;
        }

        public void Dispose()
        {
            sharedPtr.Dispose();
        }

        public Skeleton Value
        {
            get
            {
                return sharedPtr.Value;
            }
        }
    }
}

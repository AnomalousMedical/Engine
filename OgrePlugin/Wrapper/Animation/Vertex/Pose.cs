using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OgreWrapper
{
    public class Pose : IDisposable
    {
        internal static Pose createWrapper(IntPtr pose, object[] args)
        {
            return new Pose(pose);
        }

        IntPtr pose;

        private Pose(IntPtr pose)
        {
            this.pose = pose;
        }

        public void Dispose()
        {
            pose = IntPtr.Zero;
        }
    }
}

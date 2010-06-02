using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public class KeyFrame : IDisposable
    {
        protected IntPtr keyFrame;

        protected KeyFrame(IntPtr keyFrame)
        {
            this.keyFrame = keyFrame;
        }

        public virtual void Dispose()
        {
            keyFrame = IntPtr.Zero;
        }

        /// <summary>
        /// Gets the time of this keyframe in the animation sequence.
        /// </summary>
        /// <returns>The time of this frame in the sequence.</returns>
        float getTime()
        {
            return KeyFrame_getTime(keyFrame);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern float KeyFrame_getTime(IntPtr keyFrame);

#endregion
    }
}

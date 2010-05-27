using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgreWrapper
{
    [NativeSubsystemType]
    public class RenderWindow : RenderTarget
    {
        public RenderWindow(IntPtr renderWindow)
            :base(renderWindow)
        {

        }

        /// <summary>
        /// Call this function if the render window moves or is resized.
        /// </summary>
        public void windowMovedOrResized()
        {
            RenderWindow_windowMovedOrResized(renderTarget);
        }

        #region PInvoke
        
        [DllImport("OgreCWrapper")]
        private static extern void RenderWindow_windowMovedOrResized(IntPtr renderWindow);
        
        #endregion
    }
}

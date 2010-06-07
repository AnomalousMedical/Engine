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
        private String windowHandleString;
        private GiveWindowStringDelegate GiveWindowStringCallback;

        public RenderWindow(IntPtr renderWindow)
            :base(renderWindow)
        {
            if (renderWindow != IntPtr.Zero)
            {
                GiveWindowStringCallback = new GiveWindowStringDelegate(getWindowString);
                RenderWindow_getWindowHandleStr(renderWindow, GiveWindowStringCallback);
                GiveWindowStringCallback = null;
            }
        }

        /// <summary>
        /// Call this function if the render window moves or is resized.
        /// </summary>
        public void windowMovedOrResized()
        {
            RenderWindow_windowMovedOrResized(renderTarget);
        }

        public String getWindowHandleStr()
        {
            return windowHandleString;
        }

        private void getWindowString(String value)
        {
            this.windowHandleString = value;
        }

        #region PInvoke
        
        [DllImport("OgreCWrapper")]
        private static extern void RenderWindow_windowMovedOrResized(IntPtr renderWindow);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GiveWindowStringDelegate(String handleStr);

        [DllImport("OgreCWrapper")]
        private static extern void RenderWindow_getWindowHandleStr(IntPtr renderWindow, GiveWindowStringDelegate giveWindowString);
        
        #endregion
    }
}

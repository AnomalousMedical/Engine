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
            
        }

        public void destroy()
        {
            RenderWindow_destroy(renderTarget);
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
            if (windowHandleString == null && OgreRenderTarget != IntPtr.Zero)
            {
                GiveWindowStringCallback = new GiveWindowStringDelegate(getWindowString);
                //This is pretty tricky, the pointer is technically from the RenderTarget class, 
                //but we know its a RenderWindow so its safe to pass here
                RenderWindow_getWindowHandleStr(OgreRenderTarget, GiveWindowStringCallback);
                GiveWindowStringCallback = null;
            }
            return windowHandleString;
        }

        public void setFullscreen(bool fullscreen, uint width, uint height)
        {
            RenderWindow_setFullscreen(renderTarget, fullscreen, width, height);
        }

        public bool DeactivateOnFocusChange
        {
            get
            {
                return RenderWindow_isDeactivatedOnFocusChange(renderTarget);
            }
            set
            {
                RenderWindow_setDeactivatedOnFocusChange(renderTarget, value);
            }
        }

        private void getWindowString(String value)
        {
            this.windowHandleString = value;
        }

        public bool Visible
        {
            get
            {
                return RenderWindow_isVisible(renderTarget);
            }
            set
            {
                RenderWindow_setVisible(renderTarget, value);
            }
        }

        #region PInvoke
        
        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderWindow_destroy(IntPtr renderWindow);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderWindow_windowMovedOrResized(IntPtr renderWindow);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GiveWindowStringDelegate(String handleStr);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderWindow_getWindowHandleStr(IntPtr renderWindow, GiveWindowStringDelegate giveWindowString);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderWindow_setFullscreen(IntPtr renderWindow, bool fullscreen, uint width, uint height);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderWindow_isDeactivatedOnFocusChange(IntPtr renderWindow);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderWindow_setDeactivatedOnFocusChange(IntPtr renderWindow, bool deactivate);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderWindow_isVisible(IntPtr renderWindow);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderWindow_setVisible(IntPtr renderWindow, bool visible);
        
        #endregion
    }
}

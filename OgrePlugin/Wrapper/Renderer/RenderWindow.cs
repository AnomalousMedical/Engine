using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;
using Engine;
using Anomalous.Interop;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class RenderWindow : RenderTarget
    {
        private static StringRetriever stringRetriever = new StringRetriever();

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

        /// <summary>
        /// This is called when the OSWindow that holds this RenderWindow needs to create internal resources.
        /// This is an extension in our engine, but some operating systems need to respond to this.
        /// </summary>
        /// <param name="osWindowHandle">The OSWindow's handle.</param>
        internal void createInternalResources(IntPtr osWindowHandle)
        {
            RenderWindow_createInternalResources(renderTarget, osWindowHandle);
        }

        /// <summary>
        /// This is called when the OSWindow that holds this RenderWindow needs to destroy internal resources.
        /// This is an extension in our engine, but some operating systems need to respond to this.
        /// </summary>
        /// <param name="osWindowHandle">The OSWindow's handle.</param>
        internal void destroyInternalResources(IntPtr osWindowHandle)
        {
            RenderWindow_destroyInternalResources(renderTarget, osWindowHandle);
        }

        public String getWindowHandleStr()
        {
            if (OgreRenderTarget != IntPtr.Zero)
            {
                //This is pretty tricky, the pointer is technically from the RenderTarget class, 
                //but we know its a RenderWindow so its safe to pass here
                RenderWindow_getWindowHandleStr(OgreRenderTarget, stringRetriever.StringCallback, stringRetriever.Handle);
                return stringRetriever.retrieveString();
            }
            return null;
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

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderWindow_getWindowHandleStr(IntPtr renderWindow, StringRetriever.Callback stringCb, IntPtr handle);

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

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_createInternalResources(IntPtr renderWindow, IntPtr osWindowHandle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_destroyInternalResources(IntPtr renderWindow, IntPtr osWindowHandle);
        
        #endregion
    }
}

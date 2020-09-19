using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;
using Engine;
using Anomalous.Interop;

namespace OgreNextPlugin
{
    [NativeSubsystemType]
    public class RenderWindow : IDisposable
    {
        internal static RenderWindow createWrapper(IntPtr nativePtr, Object[] args)
        {
            return new RenderWindow(nativePtr);
        }

        private static StringRetriever stringRetriever = new StringRetriever();
        private readonly IntPtr renderTarget;

        public RenderWindow(IntPtr renderWindow)
        {
            this.renderTarget = renderWindow;
        }

        public void Dispose()
        {
            //does nothing, might not need destory either.
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
            RenderWindow_getWindowHandleStr(renderTarget, stringRetriever.StringCallback, stringRetriever.Handle);
            return stringRetriever.retrieveString();
        }

        /// <summary>
        /// Requests to toggle between fullscreen and windowed mode.
        /// </summary>
        /// <remarks>
        /// Use wantsToGoFullscreen & wantsToGoWindowed to know what you've requested.
        /// Same remarks as requestResolution apply:
        ///        If we request to go fullscreen, wantsToGoFullscreen will return true.
        ///        But if get word from OS saying we stay in windowed mode,
        ///        wantsToGoFullscreen will start returning false.
        /// </remarks>
        /// <param name="goFullscreen"></param>
        /// <param name="borderless"></param>
        /// <param name="monitorIdx"></param>
        /// <param name="widthPt">New width. Leave 0 if you don't care.</param>
        /// <param name="heightPt">New height. Leave 0 if you don't care.</param>
        /// <param name="frequencyNumerator">New frequency (fullscreen only). Leave 0 if you don't care.</param>
        /// <param name="frequencyDenominator">New frequency (fullscreen only). Leave 0 if you don't care.</param>
        public void requestFullscreenSwitch(bool goFullscreen, bool borderless, UInt32 monitorIdx,
            UInt32 widthPt = 0, UInt32 heightPt = 0,
            UInt32 frequencyNumerator = 0, UInt32 frequencyDenominator = 0)
        {
            RenderWindow_requestFullscreenSwitch(renderTarget, goFullscreen, borderless, monitorIdx,
            widthPt, heightPt,
            frequencyNumerator, frequencyDenominator);
        }

        public bool Visible
        {
            get
            {
                return RenderWindow_isVisible(renderTarget);
            }
        }

        private TextureGpu textureGpu;
        public TextureGpu Texture
        {
            get
            {
                if (textureGpu == null)
                {
                    textureGpu = new TextureGpu(RenderWindow_getTexture(renderTarget));
                }

                return textureGpu;
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_destroy(IntPtr renderWindow);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_windowMovedOrResized(IntPtr renderWindow);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_getWindowHandleStr(IntPtr renderWindow, StringRetriever.Callback stringCb, IntPtr handle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_requestFullscreenSwitch(IntPtr renderWindow,
            bool goFullscreen, bool borderless, UInt32 monitorIdx,
            UInt32 widthPt, UInt32 heightPt,
            UInt32 frequencyNumerator, UInt32 frequencyDenominator);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderWindow_isVisible(IntPtr renderWindow);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_createInternalResources(IntPtr renderWindow, IntPtr osWindowHandle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderWindow_destroyInternalResources(IntPtr renderWindow, IntPtr osWindowHandle);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr RenderWindow_getTexture(IntPtr renderWindow);

        #endregion
    }
}

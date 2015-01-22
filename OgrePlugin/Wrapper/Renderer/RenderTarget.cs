using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Attributes;
using Engine;

namespace OgrePlugin
{
    enum RenderTargetType
    {
	    RenderWindow,
	    RenderTexture,
	    MultiRenderTarget
    };

    [NativeSubsystemType]
    public abstract class RenderTarget : IDisposable
    {
        public enum FrameBuffer
        {
	        FB_FRONT,
	        FB_BACK,
	        FB_AUTO
        };

        internal static RenderTarget createWrapper(IntPtr nativePtr, object[] args)
        {
            RenderTargetType type = (RenderTargetType)args[0];
	        switch(type)
	        {
	        case RenderTargetType.RenderWindow:
                return new RenderWindow(nativePtr);
	        case RenderTargetType.RenderTexture:
                return new RenderTexture(nativePtr);
	        case RenderTargetType.MultiRenderTarget:
		        throw new NotImplementedException();
	        default:
		        throw new NotImplementedException();
	        }
        }

        protected IntPtr renderTarget;
        private List<Viewport> viewports = new List<Viewport>();
        private RenderTargetListener renderTargetListener;

        public RenderTarget(IntPtr renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public void Dispose()
        {
            checkDestroyListener(true);
            foreach (Viewport vp in viewports)
            {
                ViewportManager.destroyViewport(vp);
            }
            viewports.Clear();
            renderTarget = IntPtr.Zero;
        }

        public IntPtr OgreRenderTarget
        {
            get
            {
                return renderTarget;
            }
        }

        /// <summary>
        /// Returns the name of this render target.
        /// </summary>
        /// <returns>The name of the render target.</returns>
        public String getName()
        {
            return Marshal.PtrToStringAnsi(RenderTarget_getName(renderTarget));
        }

        /// <summary>
        /// Add a viewport to the rendering target.
        /// </summary>
        /// <remarks>
        /// A viewport is the rectangle into which rendering output is sent. This
        /// method adds a viewport to the render target, rendering from the supplied
        /// camera. The rest of the parameters are only required if you wish to add
        /// more than one viewport to a single rendering target. Note that size
        /// information passed to this method is passed as a parametric, i.e. it is
        /// relative rather than absolute. This is to allow viewports to
        /// automatically resize along with the target.
        /// </remarks>
        /// <param name="camera">The camera to use for the viewport.</param>
        /// <returns>A new viewport.</returns>
        public Viewport addViewport(Camera camera)
        {
            Viewport vp = ViewportManager.getViewport(RenderTarget_addViewport(renderTarget, camera.OgreObject));
            viewports.Add(vp);
            return vp;
        }

        /// <summary>
        /// Add a viewport to the rendering target.
        /// </summary>
        /// <remarks>
        /// A viewport is the rectangle into which rendering output is sent. This
        /// method adds a viewport to the render target, rendering from the supplied
        /// camera. The rest of the parameters are only required if you wish to add
        /// more than one viewport to a single rendering target. Note that size
        /// information passed to this method is passed as a parametric, i.e. it is
        /// relative rather than absolute. This is to allow viewports to
        /// automatically resize along with the target.
        /// </remarks>
        /// <param name="camera">The camera to use for the viewport.</param>
        /// <param name="zOrder">The relative order of the viewport with others on the target (allows overlapping viewports i.e. picture-in-picture). Higher ZOrders are on top of lower ones. The actual number is irrelevant, only the relative ZOrder matters (you can leave gaps in the numbering).</param>
        /// <param name="left">The relative position of the left of the viewport on the target, as a value between 0 and 1. </param>
        /// <param name="top">The relative position of the top of the viewport on the target, as a value between 0 and 1. </param>
        /// <param name="width">The relative width of the viewport on the target, as a value between 0 and 1. </param>
        /// <param name="height">The relative height of the viewport on the target, as a value between 0 and 1.</param>
        /// <returns>A new viewport.</returns>
        public Viewport addViewport(Camera camera, int zOrder, float left, float top, float width, float height)
        {
            Viewport vp = ViewportManager.getViewport(RenderTarget_addViewportExt(renderTarget, camera.OgreObject, zOrder, left, top, width, height));
            viewports.Add(vp);
            return vp;
        }

        /// <summary>
        /// This will destroy the passed viewport.
        /// </summary>
        public void destroyViewport(Viewport viewport)
        {
            viewports.Remove(viewport);
            RenderTarget_destroyViewport(renderTarget, ViewportManager.destroyViewport(viewport));
        }

        /// <summary>
        /// Get the width of the RenderTarget.
        /// </summary>
        /// <returns>The width.</returns>
        public uint getWidth()
        {
            return RenderTarget_getWidth(renderTarget);
        }

        /// <summary>
        /// Get the height of the RenderTarget.
        /// </summary>
        /// <returns>The height.</returns>
        public uint getHeight()
        {
            return RenderTarget_getHeight(renderTarget);
        }

        /// <summary>
        /// Get the color depth of the RenderTarget.
        /// </summary>
        /// <returns>The color depth.</returns>
        public uint getColorDepth()
        {
            return RenderTarget_getColorDepth(renderTarget);
        }

        /// <summary>
        /// Tells the target to update it's contents.
        /// 
        /// If OGRE is not running in an automatic rendering loop (started using
        /// Root::startRendering), the user of the library is responsible for asking
        /// each render target to refresh. This is the method used to do this. It
        /// automatically re-renders the contents of the target using whatever
        /// cameras have been pointed at it (using Camera::setRenderTarget). 
        /// 
        /// This allows OGRE to be used in multi-windowed utilities and for contents
        /// to be refreshed only when required, rather than constantly as with the
        /// automatic rendering loop. 
        /// </summary>
        public void update()
        {
            RenderTarget_update(renderTarget);
        }

        /// <summary>
        /// Tells the target to update it's contents.
        /// 
        /// If OGRE is not running in an automatic rendering loop (started using
        /// Root::startRendering), the user of the library is responsible for asking
        /// each render target to refresh. This is the method used to do this. It
        /// automatically re-renders the contents of the target using whatever
        /// cameras have been pointed at it (using Camera::setRenderTarget). 
        /// 
        /// This allows OGRE to be used in multi-windowed utilities and for contents
        /// to be refreshed only when required, rather than constantly as with the
        /// automatic rendering loop. 
        /// </summary>
        /// <param name="swapBuffers">For targets that support double-buffering, if set to true, the target will immediately swap it's buffers after update. Otherwise, the buffers are not swapped, and you have to call swapBuffers yourself sometime later. You might want to do this on some rendersystems which pause for queued rendering commands to complete before accepting swap buffers calls - so you could do other CPU tasks whilst the queued commands complete. Or, you might do this if you want custom control over your windows, such as for externally created windows.</param>
        public void update(bool swapBuffers)
        {
            RenderTarget_updateSwap(renderTarget, swapBuffers);
        }

        /// <summary>
        /// Swaps the frame buffers to display the next frame.
        /// 
        /// For targets that are double-buffered so that no 'in-progress' versions
        /// of the scene are displayed during rendering. Once rendering has
        /// completed (to an off-screen version of the window) the buffers are
        /// swapped to display the new frame.
        /// </summary>
        public void swapBuffers()
        {
            RenderTarget_swapBuffers(renderTarget);
        }

        /// <summary>
        /// Get the number of viewports attached to this target. 
        /// </summary>
        /// <returns>The number of viewports.</returns>
        public ushort getNumViewports()
        {
            return RenderTarget_getNumViewports(renderTarget);
        }

        /// <summary>
        /// Get the viewport identified by name.
        /// </summary>
        /// <param name="index">The index of the viewport to get.</param>
        /// <returns>The viewport identified by name or null if it is not found.</returns>
        public Viewport getViewport(ushort index)
        {
            return ViewportManager.getViewport(RenderTarget_getViewport(renderTarget, index));
        }

        /// <summary>
        /// Individual stats access - gets the number of frames per second (FPS) based on the last frame rendered. 
        /// </summary>
        /// <returns>The last fps.</returns>
        public float getLastFPS()
        {
            return RenderTarget_getLastFPS(renderTarget);
        }

        /// <summary>
        /// Individual stats access - gets the average frames per second (FPS) since call to Root::startRendering. 
        /// </summary>
        /// <returns>The average fps.</returns>
        public float getAverageFPS()
        {
            return RenderTarget_getAverageFPS(renderTarget);
        }

        /// <summary>
        /// Individual stats access - gets the best frames per second (FPS) since call to Root::startRendering. 
        /// </summary>
        /// <returns>The best fps.</returns>
        public float getBestFPS()
        {
            return RenderTarget_getBestFPS(renderTarget);
        }

        /// <summary>
        /// Individual stats access - gets the worst frames per second (FPS) since call to Root::startRendering. 
        /// </summary>
        /// <returns>The worst fps.</returns>
        public float getWorstFPS()
        {
            return RenderTarget_getWorstFPS(renderTarget);
        }

        /// <summary>
        /// Individual stats access - gets the best frame time. 
        /// </summary>
        /// <returns>The best frame time.</returns>
        public float getBestFrameTime()
        {
            return RenderTarget_getBestFrameTime(renderTarget);
        }

        /// <summary>
        /// Individual stats access - gets the worst frame time. 
        /// </summary>
        /// <returns>The worst frame time.</returns>
        public float getWorstFrameTime()
        {
            return RenderTarget_getWorstFrameTime(renderTarget);
        }

        /// <summary>
        /// Resets saved frame-rate statistices. 
        /// </summary>
        public void resetStatistics()
        {
            RenderTarget_resetStatistics(renderTarget);
        }

        /// <summary>
        /// Gets a custom (maybe platform-specific) attribute.
        /// <para>
        /// This is a nasty way of satisfying any API's need to see
        /// platform-specific details. It horrid, but D3D needs this kind of info.
        /// At least it's abstracted.
        /// </para>
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="pData">Pointer to memory of the right kind of structure to receive the info.</param>
        public unsafe void getCustomAttribute(String name, void* pData)
        {
            RenderTarget_getCustomAttribute(renderTarget, name, pData);
        }

        /// <summary>
        /// Sets the priority of this render target in relation to the others.
        /// 
        /// 
        /// This can be used in order to schedule render target updates. Lower
        /// priorities will be rendered first. Note that the priority must be set at
        /// the time the render target is attached to the render system, changes
        /// afterwards will not affect the ordering. 
        /// </summary>
        /// <param name="priority">The priority to set.</param>
        public void setPriority(byte priority)
        {
            RenderTarget_setPriority(renderTarget, priority);
        }

        /// <summary>
        /// Gets the priority of a render target. 
        /// </summary>
        /// <returns>The priority.</returns>
        public byte getPriority()
        {
            return RenderTarget_getPriority(renderTarget);
        }

        /// <summary>
        /// Used to retrieve the active state of the render target. 
        /// </summary>
        /// <returns>True if the viewport is active, false if disabled.</returns>
        public bool isActive()
        {
            return RenderTarget_isActive(renderTarget);
        }

        /// <summary>
        /// Used to set the active state of the render target. 
        /// </summary>
        /// <param name="active">True to activate false to deactivate.</param>
        public void setActive(bool active)
        {
            RenderTarget_setActive(renderTarget, active);
        }

        /// <summary>
        /// Sets whether this target should be automatically updated if Ogre's
        /// rendering loop or Root::_updateAllRenderTargets is being used.
        /// 
        /// By default, if you use Ogre's own rendering loop (Root::startRendering)
        /// or call Root::_updateAllRenderTargets, all render targets are updated
        /// automatically. This method allows you to control that behaviour, if for
        /// example you have a render target which you only want to update
        /// periodically. 
        /// </summary>
        /// <param name="autoUpdate">If true, the render target is updated during the automatic render loop or when Root::_updateAllRenderTargets is called. If false, the target is only updated when its update() method is called explicitly. </param>
        public void setAutoUpdated(bool autoUpdate)
        {
            RenderTarget_setAutoUpdated(renderTarget, autoUpdate);
        }

        /// <summary>
        /// Gets whether this target is automatically updated if Ogre's rendering
        /// loop or Root::_updateAllRenderTargets is being used. 
        /// </summary>
        /// <returns>True if the target is autoupdated, false if it is not.</returns>
        public bool isAutoUpdated()
        {
            return RenderTarget_isAutoUpdated(renderTarget);
        }

        /// <summary>
        /// Copies the current contents of the render target to a pixelbox.
        /// <para>
        /// See suggestPixelFormat for a tip as to the best pixel format to extract
        /// into, although you can use whatever format you like and the results will
        /// be converted. 
        /// </para>
        /// </summary>
        /// <param name="dest">The PixelBox to write the results to.</param>
        public void copyContentsToMemory(PixelBox dest)
        {
            RenderTarget_copyContentsToMemory(renderTarget, dest.OgreBox);
        }

        /// <summary>
        /// Copies the current contents of the render target to a pixelbox.
        /// <para>
        /// See suggestPixelFormat for a tip as to the best pixel format to extract
        /// into, although you can use whatever format you like and the results will
        /// be converted. 
        /// </para>
        /// </summary>
        /// <param name="dest">The PixelBox to write the results to.</param>
        /// <param name="buffer">The frame buffer to copy the contents from.</param>
        public void copyContentsToMemory(PixelBox dest, FrameBuffer buffer)
        {
            RenderTarget_copyContentsToMemoryBuffer(renderTarget, dest.OgreBox, buffer);
        }

        /// <summary>
        /// Suggests a pixel format to use for extracting the data in this target,
        /// when calling copyContentsToMemory.
        /// </summary>
        /// <returns>The reccomended pixel format.</returns>
        public PixelFormat suggestPixelFormat()
        {
            return RenderTarget_suggestPixelFormat(renderTarget);
        }

        /// <summary>
        /// Writes the current contents of the render target to the named file. 
        /// </summary>
        /// <param name="filename">The file to write the contents to.</param>
        public void writeContentsToFile(String filename)
        {
            RenderTarget_writeContentsToFile(renderTarget, filename);
        }

        /// <summary>
        /// Writes the current contents of the render target to the (PREFIX)(time-stamp)(SUFFIX) file. 
        /// </summary>
        /// <param name="filenamePrefix"></param>
        /// <param name="filenameSuffix"></param>
        public String writeContentsToTimestampedFile(String filenamePrefix, String filenameSuffix)
        {
            return Marshal.PtrToStringAnsi(RenderTarget_writeContentsToTimestampedFile(renderTarget, filenamePrefix, filenameSuffix));
        }

        /// <summary>
        /// Determine if this render target requires texture flipping.
        /// </summary>
        /// <returns>True if texture flipping is required, false if not.</returns>
        public bool requiresTextureFlipping()
        {
            return RenderTarget_requiresTextureFlipping(renderTarget);
        }

        /// <summary>
        /// Gets the number of triangles rendered in the last update() call. 
        /// </summary>
        /// <returns>The number of triangles.</returns>
        public uint getTriangleCount()
        {
            return RenderTarget_getTriangleCount(renderTarget);
        }

        /// <summary>
        /// Gets the number of batches rendered in the last update() call. 
        /// </summary>
        /// <returns>The number of batches.</returns>
        public uint getBatchCount()
        {
            return RenderTarget_getBatchCount(renderTarget);
        }

        /// <summary>
        /// Indicates whether this target is the primary window.
        /// 
        /// The primary window is special in that it is destroyed when ogre is shut
        /// down, and cannot be destroyed directly. This is the case because it
        /// holds the context for vertex, index buffers and textures. 
        /// </summary>
        /// <returns>True if this is the primary target.</returns>
        public bool isPrimary()
        {
            return RenderTarget_isPrimary(renderTarget);
        }

        /// <summary>
        /// Indicates whether on rendering, linear colour space is converted to sRGB
        /// gamma colour space.
        /// 
        /// This is the exact opposite conversion of what is indicated by
        /// Texture::isHardwareGammaEnabled, and can only be enabled on creation of
        /// the render target. For render windows, it's enabled through the 'gamma'
        /// creation misc parameter. For textures, it is enabled through the hwGamma
        /// parameter to the create call. 
        /// </summary>
        /// <returns>True if enabled.</returns>
        public bool isHardwareGammaEnabled()
        {
            return RenderTarget_isHardwareGammaEnabled(renderTarget);
        }

        /// <summary>
        /// Indicates whether multisampling is performed on rendering and at what level. 
        /// </summary>
        /// <returns>The level of FSAA.</returns>
        public uint getFSAA()
        {
            return RenderTarget_getFSAA(renderTarget);
        }

        public event Action PreRenderTargetUpdate
        {
            add
            {
                checkListenerCreated();
                renderTargetListener.PreRenderTargetUpdate += value;
            }
            remove
            {
                if (renderTargetListener != null)
                {
                    renderTargetListener.PreRenderTargetUpdate -= value;
                    checkDestroyListener();
                }
            }
        }

        public event Action PostRenderTargetUpdate
        {
            add
            {
                checkListenerCreated();
                renderTargetListener.PostRenderTargetUpdate += value;
            }
            remove
            {
                if (renderTargetListener != null)
                {
                    renderTargetListener.PostRenderTargetUpdate -= value;
                    checkDestroyListener();
                }
            }
        }

        public event Action PreViewportUpdate
        {
            add
            {
                checkListenerCreated();
                renderTargetListener.PreViewportUpdate += value;
            }
            remove
            {
                if (renderTargetListener != null)
                {
                    renderTargetListener.PreViewportUpdate -= value;
                    checkDestroyListener();
                }
            }
        }

        public event Action PostViewportUpdate
        {
            add
            {
                checkListenerCreated();
                renderTargetListener.PostViewportUpdate += value;
            }
            remove
            {
                if (renderTargetListener != null)
                {
                    renderTargetListener.PostViewportUpdate -= value;
                    checkDestroyListener();
                }
            }
        }

        public event Action ViewportAdded
        {
            add
            {
                checkListenerCreated();
                renderTargetListener.ViewportAdded += value;
            }
            remove
            {
                if (renderTargetListener != null)
                {
                    renderTargetListener.ViewportAdded -= value;
                    checkDestroyListener();
                }
            }
        }

        public event Action ViewportRemoved
        {
            add
            {
                checkListenerCreated();
                renderTargetListener.ViewportRemoved += value;
            }
            remove
            {
                if (renderTargetListener != null)
                {
                    renderTargetListener.ViewportRemoved -= value;
                    checkDestroyListener();
                }
            }
        }

        private void checkListenerCreated()
        {
            if (renderTargetListener == null)
            {
                renderTargetListener = new RenderTargetListener();
                RenderTarget_addListener(renderTarget, renderTargetListener.Ptr);
            }
        }

        private void checkDestroyListener(bool forceDelete = false)
        {
            if (renderTargetListener != null && (forceDelete || !renderTargetListener.HasSubscribers))
            {
                RenderTarget_removeListener(renderTarget, renderTargetListener.Ptr);
                renderTargetListener.Dispose();
                renderTargetListener = null;
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderTarget_getName(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderTarget_addViewport(IntPtr renderTarget, IntPtr camera);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderTarget_addViewportExt(IntPtr renderTarget, IntPtr camera, int zOrder, float left, float top, float width, float height);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_destroyViewport(IntPtr renderTarget, IntPtr viewport);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint RenderTarget_getWidth(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint RenderTarget_getHeight(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint RenderTarget_getColorDepth(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_update(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_updateSwap(IntPtr renderTarget, bool swapBuffers);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_swapBuffers(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern ushort RenderTarget_getNumViewports(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderTarget_getViewport(IntPtr renderTarget, ushort index);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float RenderTarget_getLastFPS(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float RenderTarget_getAverageFPS(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float RenderTarget_getBestFPS(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float RenderTarget_getWorstFPS(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float RenderTarget_getBestFrameTime(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float RenderTarget_getWorstFrameTime(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_resetStatistics(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static unsafe extern void RenderTarget_getCustomAttribute(IntPtr renderTarget, String name, void* pData);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_setPriority(IntPtr renderTarget, byte priority);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern byte RenderTarget_getPriority(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderTarget_isActive(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_setActive(IntPtr renderTarget, bool active);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_setAutoUpdated(IntPtr renderTarget, bool autoUpdate);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderTarget_isAutoUpdated(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_copyContentsToMemory(IntPtr renderTarget, IntPtr dest);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_copyContentsToMemoryBuffer(IntPtr renderTarget, IntPtr dest, FrameBuffer buffer);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern PixelFormat RenderTarget_suggestPixelFormat(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_writeContentsToFile(IntPtr renderTarget, String filename);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderTarget_writeContentsToTimestampedFile(IntPtr renderTarget, String filenamePrefix, String filenameSuffix);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderTarget_requiresTextureFlipping(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint RenderTarget_getTriangleCount(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint RenderTarget_getBatchCount(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderTarget_isPrimary(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool RenderTarget_isHardwareGammaEnabled(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint RenderTarget_getFSAA(IntPtr renderTarget);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_addListener(IntPtr renderTarget, IntPtr listener);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void RenderTarget_removeListener(IntPtr renderTarget, IntPtr listener);

        #endregion 
    }
}

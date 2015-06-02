using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Runtime.InteropServices;
using Engine;
using Anomalous.Interop;

namespace Anomalous.OSPlatform
{
    public class NativeOSWindow : OSWindow, IDisposable
    {
        private String title;
        private IntPtr nativeWindow;
        private CallbackHandler callbackHandler;

        public event OSWindowEvent Activated;
        public event OSWindowEvent Deactivated;
        public event OSWindowEvent Disposed;
        private bool activated = true;

        public NativeOSWindow(String title, IntVector2 position, IntSize2 size)
            :this(null, title, position, size, false)
        {
            
        }

        public NativeOSWindow(NativeOSWindow parent, String title, IntVector2 position, IntSize2 size)
            :this(parent, title, position, size, false)
        {

        }

        public NativeOSWindow(NativeOSWindow parent, String title, IntVector2 position, IntSize2 size, bool floatOnParent)
        {
            this.title = title;

            IntPtr parentPtr = IntPtr.Zero;
            if (parent != null)
            {
                parentPtr = parent._NativePtr;
            }

            nativeWindow = NativeOSWindow_create(parentPtr, title, position.x, position.y, size.Width, size.Height, floatOnParent);
            callbackHandler = new CallbackHandler(this);
        }

        public void Dispose()
        {
            if (nativeWindow != IntPtr.Zero)
            {
                if (Disposed != null)
                {
                    Disposed.Invoke(this);
                }
                NativeOSWindow_destroy(nativeWindow);
                callbackHandler.Dispose();
            }
        }

        /// <summary>
        /// Callback from the native class when it is deleted. This WILL be
        /// called if Dispose deletes the class.
        /// </summary>
        private void delete()
        {
            disposed();
            nativeWindow = IntPtr.Zero;
        }

        protected virtual void disposed()
        {

        }

        public void setSize(int width, int height)
        {
            NativeOSWindow_setSize(nativeWindow, width, height);
        }

        public void show()
        {
            NativeOSWindow_show(nativeWindow);
        }

        public void close()
        {
            NativeOSWindow_close(nativeWindow);
        }

        public void setCursor(CursorType cursor)
        {
            NativeOSWindow_setCursor(nativeWindow, cursor);
        }

        public void toggleFullscreen()
        {
            NativeOSWindow_toggleFullscreen(nativeWindow);
        }

		public OnscreenKeyboardMode KeyboardMode
		{
			get 
			{
                return NativeOSWindow_getOnscreenKeyboardMode(nativeWindow);
			}
			set 
			{
				NativeOSWindow_setOnscreenKeyboardMode(nativeWindow, value);
			}
		}

        public String Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                NativeOSWindow_setTitle(nativeWindow, title);
            }
        }

        public bool Maximized
        {
            get
            {
                return NativeOSWindow_getMaximized(nativeWindow);
            }
            set
            {
                NativeOSWindow_setMaximized(nativeWindow, value);
            }
        }

        public bool ExclusiveFullscreen
        {
            get
            {
                return NativeOSWindow_getExclusiveFullscreen(nativeWindow);
            }
            set
            {
                NativeOSWindow_setExclusiveFullscreen(nativeWindow, value);
            }
        }

        public bool Active
        {
            get
            {
                return nativeWindow != IntPtr.Zero;
            }
        }

        public float WindowScaling
        {
            get
            {
                return NativeOSWindow_getWindowScaling(nativeWindow);
            }
        }

        #region OSWindow Members

        public bool Focused
        {
            get { return true; }
        }

        public IntPtr WindowHandle
        {
            get { return NativeOSWindow_getHandle(nativeWindow); }
        }

        public int WindowHeight
        {
            get { return NativeOSWindow_getHeight(nativeWindow); }
        }

        public int WindowWidth
        {
            get { return NativeOSWindow_getWidth(nativeWindow); }
        }

        public event OSWindowEvent Moved;

        public event OSWindowEvent Resized;

        public event OSWindowEvent Closing;

        public event OSWindowEvent Closed;

        public event OSWindowEvent FocusChanged;

        public event OSWindowResourceEvent CreateInternalResources;

        public event OSWindowResourceEvent DestroyInternalResources;

        #endregion

        internal IntPtr _NativePtr
        {
            get
            {
                return nativeWindow;
            }
        }

        private void resize()
        {
            if (Resized != null)
            {
                Resized.Invoke(this);
            }
        }

        private void closing()
        {
            if(Closing != null)
            {
                Closing.Invoke(this);
            }
        }

        private void closed()
        {
            if (Closed != null)
            {
                Closed.Invoke(this);
            }
        }

        private void activate(bool active)
        {
            if (active != this.activated)
            {
                this.activated = active;
                if (activated)
                {
                    if (Activated != null)
                    {
                        Activated.Invoke(this);
                    }
                }
                else
                {
                    if (Deactivated != null)
                    {
                        Deactivated.Invoke(this);
                    }
                }
                if (FocusChanged != null)
                {
                    FocusChanged.Invoke(this);
                }
            }
        }

        private void createInternalResources(InternalResourceType resourceType)
        {
            if (CreateInternalResources != null)
            {
                CreateInternalResources.Invoke(this, resourceType);
            }
        }

        private void destroyInternalResources(InternalResourceType resourceType)
        {
            if (DestroyInternalResources != null)
            {
                DestroyInternalResources.Invoke(this, resourceType);
            }
        }

        #region PInvoke

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NativeOSWindow_create(IntPtr parent, [MarshalAs(UnmanagedType.LPWStr)] String caption, int x, int y, int width, int height, bool floatOnParent);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_destroy(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_setTitle(IntPtr nativeWindow, [MarshalAs(UnmanagedType.LPWStr)] String title);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_setSize(IntPtr nativeWindow, int width, int height);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern int NativeOSWindow_getWidth(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern int NativeOSWindow_getHeight(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NativeOSWindow_getHandle(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_show(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_close(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_setMaximized(IntPtr nativeWindow, bool maximize);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool NativeOSWindow_getMaximized(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_setCursor(IntPtr nativeWindow, CursorType cursor);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern float NativeOSWindow_getWindowScaling(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_setExclusiveFullscreen(IntPtr nativeWindow, bool exclusiveFullscreen);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool NativeOSWindow_getExclusiveFullscreen(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_toggleFullscreen(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_setOnscreenKeyboardMode(IntPtr nativeWindow, OnscreenKeyboardMode mode);

		[DllImport(NativePlatformPlugin.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern OnscreenKeyboardMode NativeOSWindow_getOnscreenKeyboardMode(IntPtr nativeWindow);

        [DllImport(NativePlatformPlugin.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeOSWindow_setCallbacks(IntPtr nativeWindow, NativeAction deleteCB, NativeAction sizedCB, NativeAction closingCB, NativeAction closedCB, ActivateCB activateCB, ModifyResourcesCB createInternalResourcesCB, ModifyResourcesCB destroyInternalResourcesCB
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ActivateCB(bool arg0
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ModifyResourcesCB(InternalResourceType resourceType
#if FULL_AOT_COMPILE
    , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            static NativeAction deleteCB;
            static NativeAction sizedCB;
            static NativeAction closingCB;
            static NativeAction closedCB;
            static ActivateCB activateCB;
            static ModifyResourcesCB createInternalResourcesCB;
            static ModifyResourcesCB destroyInternalResourcesCB;

            static CallbackHandler()
            {
                deleteCB = new NativeAction(DeleteStatic);
                sizedCB = new NativeAction(ResizeStatic);
                closingCB = new NativeAction(ClosingStatic);
                closedCB = new NativeAction(ClosedStatic);
                activateCB = new ActivateCB(ActivateStatic);
                createInternalResourcesCB = new NativeAction(CreateInternalResourcesStatic);
                destroyInternalResourcesCB = new NativeAction(DestroyInternalResourcesStatic);
            }

            GCHandle handle;

            public CallbackHandler(NativeOSWindow window)
            {
                handle = GCHandle.Alloc(window);
                NativeOSWindow_setCallbacks(window._NativePtr, deleteCB, sizedCB, closedCB, closedCB, activateCB, createInternalResourcesCB, destroyInternalResourcesCB, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void DeleteStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeOSWindow).delete();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void ResizeStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeOSWindow).resize();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void ClosingStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeOSWindow).closing();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(NativeAction))]
            static void ClosedStatic(IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeOSWindow).closed();
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ActivateCB))]
            static void ActivateStatic(bool active, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeOSWindow).activate(active);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ModifyResourcesCB))]
            static void CreateInternalResourcesStatic(InternalResourceType resourceType, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeOSWindow).createInternalResources(resourceType);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(ModifyResourcesCB))]
            static void DestroyInternalResourcesStatic(InternalResourceType resourceType, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as NativeOSWindow).destroyInternalResources(resourceType);
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            NativeAction deleteCB;
            NativeAction sizedCB;
            NativeAction closingCB;
            NativeAction closedCB;
            ActivateCB activateCB;
            ModifyResourcesCB createInternalResourcesCB;
            ModifyResourcesCB destroyInternalResourcesCB;

            public CallbackHandler(NativeOSWindow window)
            {
                deleteCB = new NativeAction(window.delete);
                sizedCB = new NativeAction(window.resize);
                closingCB = new NativeAction(window.closing);
                closedCB = new NativeAction(window.closed);
                activateCB = new ActivateCB(window.activate);
                createInternalResourcesCB = new ModifyResourcesCB(window.createInternalResources);
                destroyInternalResourcesCB = new ModifyResourcesCB(window.destroyInternalResources);

                NativeOSWindow_setCallbacks(window._NativePtr, deleteCB, sizedCB, closedCB, closedCB, activateCB, createInternalResourcesCB, destroyInternalResourcesCB);
            }

            public void Dispose()
            {

            }
        }
#endif

        #endregion
    }
}

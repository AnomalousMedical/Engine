using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    class ManagedSceneListener : IDisposable
    {
        List<SceneListener> sceneListeners = new List<SceneListener>();

        private IntPtr nativeSceneListener;
        private CallbackHandler callbackHandler;
        private SceneManager sceneManager;

        public ManagedSceneListener(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            callbackHandler = new CallbackHandler();
            nativeSceneListener = callbackHandler.create(this);
        }

        public void Dispose()
        {
            NativeSceneListener_Delete(nativeSceneListener);
            callbackHandler.Dispose();
        }

        public void addSceneListener(SceneListener listener)
        {
            sceneListeners.Add(listener);
        }

        public void removeSceneListener(SceneListener listener)
        {
            sceneListeners.Remove(listener);
        }

        public int getNumListeners()
        {
            return sceneListeners.Count;
        }

        public IntPtr NativeSceneListener
        {
            get
            {
                return nativeSceneListener;
            }
        }

        /// <summary>
        /// Called prior to searching for visible objects in this SceneManager.
        /// Note that the render queue at this stage will be full of the last render's contents 
        /// and will be cleared after this method is called.
        /// </summary>
        /// <param name="sceneManager">The scene manager raising the event.</param>
        /// <param name="irs">The stage of illumination being dealt with. IRS_NONE for a regular 
        /// render, IRS_RENDER_TO_TEXTURE for a shadow caster render.</param>
        /// <param name="viewport">The viewport being updated.</param>
        void preFindVisibleObjects(IntPtr sceneManager, SceneManager.IlluminationRenderStage irs, IntPtr viewport)
        {
            Viewport vp = ViewportManager.getViewportNoCreate(viewport);
            foreach (SceneListener listener in sceneListeners)
            {
                listener.preFindVisibleObjects(this.sceneManager, irs, vp);
            }
        }

        /// <summary>
        /// Called after searching for visible objects in this SceneManager.
        /// Note that the render queue at this stage will be full of the current scenes contents, 
        /// ready for rendering. You may manually add renderables to this queue if you wish.
        /// </summary>
        /// <param name="sceneManager">The SceneManager instance raising this event.</param>
        /// <param name="irs">The stage of illumination being dealt with. IRS_NONE for a regular 
        /// render, IRS_RENDER_TO_TEXTURE for a shadow caster render.</param>
        /// <param name="viewport">The viewport being updated.</param>
        void postFindVisibleObjects(IntPtr sceneManager, SceneManager.IlluminationRenderStage irs, IntPtr viewport)
        {
            Viewport vp = ViewportManager.getViewportNoCreate(viewport);
            foreach (SceneListener listener in sceneListeners)
            {
                listener.postFindVisibleObjects(this.sceneManager, irs, vp);
            }
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr NativeSceneListener_Create(FindVisibleCallback preFind, FindVisibleCallback postFind
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void NativeSceneListener_Delete(IntPtr nativeSceneListener);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FindVisibleCallback(IntPtr sceneManager, SceneManager.IlluminationRenderStage irs, IntPtr camera
#if FULL_AOT_COMPILE
        , IntPtr instanceHandle
#endif
);

#if FULL_AOT_COMPILE
        class CallbackHandler : IDisposable
        {
            static FindVisibleCallback preFind;
            static FindVisibleCallback postFind;

            static CallbackHandler()
            {
                preFind = new FindVisibleCallback(preFindStatic);
                postFind = new FindVisibleCallback(postFindStatic);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(FindVisibleCallback))]
            static void preFindStatic(IntPtr sceneManager, SceneManager.IlluminationRenderStage irs, IntPtr viewport, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedSceneListener).preFindVisibleObjects(sceneManager, irs, viewport);
            }

            [Anomalous.Interop.MonoPInvokeCallback(typeof(FindVisibleCallback))]
            static void postFindStatic(IntPtr sceneManager, SceneManager.IlluminationRenderStage irs, IntPtr viewport, IntPtr instanceHandle)
            {
                GCHandle handle = GCHandle.FromIntPtr(instanceHandle);
                (handle.Target as ManagedSceneListener).postFindVisibleObjects(sceneManager, irs, viewport);
            }

            private GCHandle handle;

            public IntPtr create(ManagedSceneListener obj)
            {
                handle = GCHandle.Alloc(obj);
                return NativeSceneListener_Create(preFind, postFind, GCHandle.ToIntPtr(handle));
            }

            public void Dispose()
            {
                handle.Free();
            }
        }
#else
        class CallbackHandler : IDisposable
        {
            FindVisibleCallback preFind;
            FindVisibleCallback postFind;

            public IntPtr create(ManagedSceneListener obj)
            {
                preFind = new FindVisibleCallback(obj.preFindVisibleObjects);
                postFind = new FindVisibleCallback(obj.postFindVisibleObjects);
                return NativeSceneListener_Create(preFind, postFind);
            }

            public void Dispose()
            {
                
            }
        }
#endif

        #endregion
    }
}

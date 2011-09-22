using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    class ManagedSceneListener : IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FindVisibleCallback(IntPtr sceneManager, SceneManager.IlluminationRenderStage irs, IntPtr camera);

        List<SceneListener> sceneListeners = new List<SceneListener>();

        IntPtr nativeSceneListener;
        SceneManager sceneManager;
        FindVisibleCallback preFind;
        FindVisibleCallback postFind;

        public ManagedSceneListener(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            preFind = new FindVisibleCallback(preFindVisibleObjects);
            postFind = new FindVisibleCallback(postFindVisibleObjects);
            nativeSceneListener = NativeSceneListener_Create(preFind, postFind);
        }

        public void Dispose()
        {
            preFind = null;
            postFind = null;
            NativeSceneListener_Delete(nativeSceneListener);
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
            foreach(SceneListener listener in sceneListeners)
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
        
        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr NativeSceneListener_Create(FindVisibleCallback preFind, FindVisibleCallback postFind);

        [DllImport("OgreCWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void NativeSceneListener_Delete(IntPtr nativeSceneListener);

#endregion
    }
}

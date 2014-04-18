using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine.ObjectManagement;

namespace Engine.Renderer
{
    /// <summary>
    /// This is an interface to a window that is drawn on by the renderer.
    /// </summary>
    public interface RendererWindow
    {
        /// <summary>
        /// Create a SceneView with a camera that looks at a given SimSubScene.
        /// </summary>
        /// <param name="subScene">The subscene to look at with the camera.</param>
        /// <param name="name">The name of the camera. Must be unique.</param>
        /// <param name="positon">The position of the camera.</param>
        /// <param name="lookAt">The look at point of the camera.</param>
        /// <returns>A new SceneView object that can be used to manipulate the camera.</returns>
        SceneView createSceneView(SimSubScene subScene, String name, Vector3 positon, Vector3 lookAt);

        /// <summary>
        /// Create a scene view with a camera that looks at a given SimSubScene.
        /// </summary>
        /// <param name="subScene">The subscene to look at with the camera.</param>
        /// <param name="name">The name of the camera. Must be unique.</param>
        /// <param name="positon">The position of the camera.</param>
        /// <param name="lookAt">The look at point of the camera.</param>
        /// <param name="zIndex">The z index to use for the scene view.</param>
        /// <returns>A new SceneView object that can be used to manipulate the camera.</returns>
        SceneView createSceneView(SimSubScene subScene, String name, Vector3 positon, Vector3 lookAt, int zIndex);

        /// <summary>
        /// Destroy a given camera.
        /// </summary>
        /// <param name="view">The view to destroy.</param>
        void destroySceneView(SceneView view);

        /// <summary>
        /// Enable or disable this RendererWindow. If it is disabled it will not
        /// be updated, which will save an entire render of one window. Very
        /// useful in multi window situations for a large performance gain.
        /// </summary>
        /// <param name="enabled">True to enable the RenderWindow, false to disable.</param>
        void setEnabled(bool enabled);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using EngineMath;
using Engine.ObjectManagement;

namespace Engine.Renderer
{
    /// <summary>
    /// This is an interface to a window that is drawn on by the renderer.
    /// </summary>
    public interface RendererWindow
    {
        /// <summary>
        /// The OSWindow that is the rendering target.
        /// </summary>
        OSWindow Handle { get; }

        /// <summary>
        /// Create a camera that looks at a given SimSubScene.
        /// </summary>
        /// <param name="subScene">The subscene to look at with the camera.</param>
        /// <param name="name">The name of the camera. Must be unique.</param>
        /// <param name="positon">The position of the camera.</param>
        /// <param name="lookAt">The look at point of the camera.</param>
        /// <returns>A new CameraControl object that can be used to manipulate the camera.</returns>
        CameraControl createCamera(SimSubScene subScene, String name, Vector3 positon, Vector3 lookAt);

        /// <summary>
        /// Destroy a given camera.
        /// </summary>
        /// <param name="camera">The camera to destroy.</param>
        void destroyCamera(CameraControl camera);
    }
}

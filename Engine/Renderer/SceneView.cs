using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// An enum describing the RenderingModes for a camera.
    /// </summary>
    public enum RenderingMode
    {
        Points,
        Wireframe,
        Solid,
    }

    /// <summary>
    /// An event for SceneViews.
    /// </summary>
    /// <param name="sceneView">The scene view that fired the event.</param>
    public delegate void SceneViewEvent(SceneView sceneView);

    /// <summary>
    /// This class is the interface for a camera in the Renderer plugin.
    /// </summary>
    public interface SceneView
    {
        /// <summary>
        /// Called before the SceneView is rendered.
        /// </summary>
        event SceneViewEvent RenderingStarted;

        /// <summary>
        /// Called after the SceneView has finished rendering.
        /// </summary>
        event SceneViewEvent RenderingEnded;

        /// <summary>
        /// This is called when any SceneView is finding its visible objects.
        /// You can check the CurrentlyRendering property if you need to change
        /// an object's visibility based on whether or not this SceneView is the
        /// current one rendering.
        /// </summary>
        event SceneViewEvent FindVisibleObjects;

        /// <summary>
        /// Add a light that follows the camera around. This will only create
        /// one light.
        /// </summary>
        void addLight();

        /// <summary>
        /// Set the near clip distance of the camera.
        /// </summary>
        /// <param name="distance">The distance to set.</param>
        void setNearClipDistance(float distance);

        /// <summary>
        /// Set the far clip distance of the camera.
        /// </summary>
        /// <param name="distance">The distance to set.</param>
        void setFarClipDistance(float distance);

        /// <summary>
        /// Remove the light from the camera.
        /// </summary>
        void removeLight();

        /// <summary>
        /// Turn the light on and off. Only does something if a light has been
        /// added.
        /// </summary>
        /// <param name="enabled">True to enable the light.</param>
        void setLightEnabled(bool enabled);

        /// <summary>
        /// The current translation of the camera.
        /// </summary>
        Vector3 Translation { get; set; }

        /// <summary>
        /// The current direction the camera is facing.
        /// </summary>
        Vector3 Direction { get; }

        /// <summary>
        /// The current orientation of the camera.
        /// </summary>
        Quaternion Orientation { get; }

        /// <summary>
        /// The last set look at point of the camera. Setting this will cause
        /// the camera to look at the new location.
        /// </summary>
        Vector3 LookAt { get; set; }

        /// <summary>
        /// The background color used when this camera is drawing.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Set to true to clear the view every frame.
        /// </summary>
        bool ClearEveryFrame { get; set; }

        /// <summary>
        /// Get a ray that goes from the camera into the 3d space.
        /// </summary>
        /// <param name="x">The x value on the camera's 2d surface.</param>
        /// <param name="y">The y value on the camera's 2d surface.</param>
        /// <returns>A Ray3 with the ray.</returns>
        Ray3 getCameraToViewportRay(float x, float y);

        /// <summary>
        /// Change the RenderingMode of the camera.
        /// </summary>
        /// <param name="mode">The RenderingMode to set.</param>
        void setRenderingMode(RenderingMode mode);

        /// <summary>
        /// Force this camera to redraw immediately.
        /// </summary>
        /// <param name="swapBuffers">True to swap the back buffer to the front buffer.</param>
        void update(bool swapBuffers);

        /// <summary>
        /// The View matrix of the camera.
        /// </summary>
        Matrix4x4 ViewMatrix { get; }

        /// <summary>
        /// The Projection matrix of the camera.
        /// </summary>
        Matrix4x4 ProjectionMatrix { get; }

        /// <summary>
        /// Get the real render width of this view.
        /// </summary>
        int RenderWidth{ get; }

        /// <summary>
        /// Get the real render height of this view.
        /// </summary>
        int RenderHeight{ get; }

        /// <summary>
        /// This will be true if the SceneView is currently rendering.
        /// </summary>
        bool CurrentlyRendering { get; }

        /// <summary>
        /// Set the dimensions of this SceneView using relative numbers between 0 and 1.
        /// </summary>
        /// <param name="left">The left position of the view.</param>
        /// <param name="top">The top position of the view.</param>
        /// <param name="width">The width of the view.</param>
        /// <param name="height">The height of the view.</param>
        void setDimensions(float left, float top, float width, float height);
    }
}

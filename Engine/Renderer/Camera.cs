using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// This class is the interface for a camera in the Renderer plugin.
    /// </summary>
    public interface CameraControl
    {
        /// <summary>
        /// Add a light that follows the camera around. This will only create
        /// one light.
        /// </summary>
        void addLight();

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
        /// The last set look at point of the camera. Setting this will cause
        /// the camera to look at the new location.
        /// </summary>
        Vector3 LookAt { get; set; }

        /// <summary>
        /// The background color used when this camera is drawing.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Get a ray that goes from the camera into the 3d space.
        /// </summary>
        /// <param name="x">The x value on the camera's 2d surface.</param>
        /// <param name="y">The y value on the camera's 2d surface.</param>
        /// <returns>A Ray3 with the ray.</returns>
        Ray3 getCameraToViewportRay(float x, float y);
    }
}

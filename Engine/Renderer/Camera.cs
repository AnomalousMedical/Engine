using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineMath;

namespace Engine
{
    /// <summary>
    /// This class is the interface for a camera in the Renderer plugin.
    /// </summary>
    public interface CameraControl
    {
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
    }
}

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
        Vector3 Translation { get; set; }

        Vector3 LookAt { get; set; }

        Color BackgroundColor { get; set; }
    }
}

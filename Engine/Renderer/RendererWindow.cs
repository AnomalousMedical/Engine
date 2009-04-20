using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using EngineMath;
using Engine.ObjectManagement;

namespace Engine.Renderer
{
    public interface RendererWindow
    {
        OSWindow Handle { get; }

        CameraControl createCamera(SimSubScene subScene, String name, Vector3 positon, Vector3 lookAt);

        void destroyCamera(CameraControl camera);
    }
}

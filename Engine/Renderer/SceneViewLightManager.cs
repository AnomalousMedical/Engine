using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// This class will automatically create a light and move it around the scene as the scene views render.
    /// </summary>
    public interface SceneViewLightManager : IDisposable
    {
        void sceneLoaded(SimScene scene);

        void sceneUnloading(SimScene scene);
    }
}

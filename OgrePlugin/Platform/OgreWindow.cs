using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.Platform;
using OgreWrapper;
using Engine;
using Engine.ObjectManagement;
using Logging;

namespace OgrePlugin
{
    /// <summary>
    /// A RendererWindow for Ogre.
    /// </summary>
    abstract class OgreWindow : RendererWindow, IDisposable
    {
        private OSWindow handle;

        public OgreWindow(OSWindow handle)
        {
            this.handle = handle;
        }

        public OSWindow Handle
        {
            get
            {
                return handle;
            }
        }

        public CameraControl createCamera(SimSubScene subScene, String name, Vector3 positon, Vector3 lookAt)
        {
            if (subScene.hasSimElementManagerType(typeof(OgreSceneManager)))
            {
                OgreSceneManager sceneManager = subScene.getSimElementManager<OgreSceneManager>();
                OgreCameraControl camControl = new OgreCameraControl(name, sceneManager, OgreRenderWindow);
                camControl.Translation = positon;
                camControl.LookAt = lookAt;
                return camControl;
            }
            else
            {
                Log.Default.sendMessage("Cannot create a camera in the subscene {0} named {1} because the subscene has no OgreSceneManager.", LogLevel.Warning, OgreInterface.PluginName, subScene.Name, name);
                return null;
            }
        }

        public void destroyCamera(CameraControl camera)
        {
            OgreCameraControl ogreCam = camera as OgreCameraControl;
            ogreCam.Dispose();
        }

        public abstract void Dispose();

        public abstract RenderWindow OgreRenderWindow
        {
             get;
        }
    }
}

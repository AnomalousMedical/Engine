using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.Platform;
using Engine;
using Engine.ObjectManagement;
using Logging;

namespace OgrePlugin
{
    /// <summary>
    /// A RendererWindow for Ogre.
    /// </summary>
    public abstract class OgreWindow : RendererWindow, IDisposable
    {
        public OgreWindow()
        {

        }

        public SceneView createSceneView(SimSubScene subScene, String name, Vector3 position, Vector3 lookAt)
        {
            return createSceneView(subScene, name, position, lookAt, OgreRenderTarget.getNumViewports());
        }

        public SceneView createSceneView(SimSubScene subScene, String name, Vector3 positon, Vector3 lookAt, int zIndex)
        {
            if (subScene.hasSimElementManagerType(typeof(OgreSceneManager)))
            {
                OgreSceneManager sceneManager = subScene.getSimElementManager<OgreSceneManager>();
                OgreSceneView camControl = new OgreSceneView(name, sceneManager, OgreRenderTarget, zIndex);
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

        public void destroySceneView(SceneView camera)
        {
            OgreSceneView ogreCam = camera as OgreSceneView;
            ogreCam.Dispose();
        }

        /// <summary>
        /// Enable or disable this RendererWindow. If it is disabled it will not
        /// be updated, which will save an entire render of one window. Very
        /// useful in multi window situations for a large performance gain.
        /// </summary>
        /// <param name="enabled">True to enable the RenderWindow, false to disable.</param>
        public void setEnabled(bool enabled)
        {
            OgreRenderTarget.setActive(enabled);
        }

        public void copyContentsToMemory(PixelBox pixelBox, RenderTarget.FrameBuffer buffer)
        {
            OgreRenderTarget.copyContentsToMemory(pixelBox, buffer);
        }

        public abstract void Dispose();

        public abstract RenderTarget OgreRenderTarget
        {
             get;
        }
    }
}

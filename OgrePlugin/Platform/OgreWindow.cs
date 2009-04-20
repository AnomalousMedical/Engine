using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.Platform;
using OgreWrapper;
using EngineMath;
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
        private const String CAMERA_RESERVED_NAME = "__AutoCreatedCamera";
        private const String CAMERA_NODE_RESERVED_NAME = "__AutoCreatedCameraNode";

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
                Identifier cameraId = new Identifier(name, CAMERA_RESERVED_NAME);
                Camera camera = sceneManager.createCamera(cameraId);
                camera.setAutoAspectRatio(true);
                camera.setNearClipDistance(0.1f);
                camera.setFOVy(10.0f);
                Identifier nodeId = new Identifier(name, CAMERA_NODE_RESERVED_NAME);
                SceneNode node = sceneManager.createSceneNode(nodeId);
                node.attachObject(camera);
                Viewport vp = OgreRenderWindow.createViewport(camera, name);
                OgreCameraControl camControl = new OgreCameraControl(node, camera, nodeId, cameraId, sceneManager, vp);
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
            OgreRenderWindow.destroyViewport(ogreCam.Viewport);
            ogreCam.Dispose();
        }

        public abstract void Dispose();

        public abstract RenderWindow OgreRenderWindow
        {
             get;
        }
    }
}

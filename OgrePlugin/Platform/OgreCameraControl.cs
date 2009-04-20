using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using EngineMath;
using OgreWrapper;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    /// <summary>
    /// A CameraControl class for ogre cameras.
    /// </summary>
    class OgreCameraControl : CameraControl, IDisposable
    {
        private SceneNode node;
        private Camera camera;
        private Identifier nodeId;
        private Identifier cameraId;
        private OgreSceneManager sceneManager;
        private Vector3 lookAt;
        private Viewport viewport;

        public OgreCameraControl(SceneNode node, Camera camera, Identifier nodeId, Identifier cameraId, OgreSceneManager sceneManager, Viewport vp)
        {
            this.node = node;
            this.camera = camera;
            this.nodeId = nodeId;
            this.cameraId = cameraId;
            this.sceneManager = sceneManager;
            this.viewport = vp;
        }

        public Vector3 Translation
        {
            get
            {
                return node.getDerivedPosition();
            }
            set
            {
                node.setPosition(value);
            }
        }

        public Vector3 LookAt
        {
            get
            {
                return lookAt;
            }
            set
            {
                lookAt = value;
                camera.lookAt(lookAt);
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return viewport.getBackgroundColor();
            }
            set
            {
                viewport.setBackgroundColor(value);
            }
        }

        public Viewport Viewport
        {
            get
            {
                return viewport;
            }
        }

        public void Dispose()
        {
            node.detachObject(camera);
            sceneManager.destroyCamera(cameraId);
            sceneManager.destroySceneNode(nodeId);
        }
    }
}

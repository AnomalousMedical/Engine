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
        private const String CAMERA_RESERVED_NAME = "__AutoCreatedCamera";
        private const String CAMERA_NODE_RESERVED_NAME = "__AutoCreatedCameraNode";
        private const String LIGHT_RESERVED_NAME = "__AutoCreatedLight";

        private SceneNode node;
        private Camera camera;
        private Identifier nodeId;
        private Identifier cameraId;
        private Identifier lightId;
        private OgreSceneManager sceneManager;
        private Vector3 lookAt;
        private Viewport viewport;
        private Light light = null;
        private String name;
        private RenderWindow renderWindow;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the camera control.</param>
        /// <param name="sceneManager">The scene manager to build the camera into.</param>
        /// <param name="renderWindow">The renderwindow to create a viewport on.</param>
        public OgreCameraControl(String name, OgreSceneManager sceneManager, RenderWindow renderWindow)
        {
            this.name = name;
            this.sceneManager = sceneManager;
            this.renderWindow = renderWindow;
            cameraId = new Identifier(name, CAMERA_RESERVED_NAME);
            camera = sceneManager.createCamera(cameraId);
            camera.setAutoAspectRatio(true);
            camera.setNearClipDistance(0.1f);
            camera.setFOVy(10.0f);
            nodeId = new Identifier(name, CAMERA_NODE_RESERVED_NAME);
            node = sceneManager.createSceneNode(nodeId);
            node.attachObject(camera);
            viewport = renderWindow.addViewport(camera);
        }

        /// <summary>
        /// Add a light that follows the camera around. This will only create
        /// one light.
        /// </summary>
        public void addLight()
        {
            if (light == null)
            {
                lightId = new Identifier(nodeId.ElementName, LIGHT_RESERVED_NAME);
                light = sceneManager.createLight(lightId);
                node.attachObject(light);
            }
        }

        /// <summary>
        /// Remove the light from the camera.
        /// </summary>
        public void removeLight()
        {
            if (light != null)
            {
                node.detachObject(light);
                sceneManager.destroyLight(lightId);
                light = null;
            }
        }

        /// <summary>
        /// Turn the light on and off. Only does something if a light has been
        /// added.
        /// </summary>
        /// <param name="enabled">True to enable the light.</param>
        public void setLightEnabled(bool enabled)
        {
            light.setVisible(enabled);
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
            removeLight();
            renderWindow.destroyViewport(viewport);
            node.detachObject(camera);
            sceneManager.destroyCamera(cameraId);
            sceneManager.destroySceneNode(nodeId);
        }
    }
}

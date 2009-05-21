using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgreWrapper;
using Engine.ObjectManagement;

namespace OgrePlugin
{
    /// <summary>
    /// A CameraControl class for ogre cameras.
    /// </summary>
    class OgreCameraControl : CameraControl, IDisposable, SceneListener
    {
        private const String CAMERA_RESERVED_NAME = "__AutoCreatedCamera";
        private const String CAMERA_NODE_RESERVED_NAME = "__AutoCreatedCameraNode";
        private const String LIGHT_RESERVED_NAME = "__AutoCreatedLight";
        private const String LIGHT_NODE_NAME = "__AutoCreatedLightNode";

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
        private StatsOverlay statsOverlay;
        private bool showStats = false;
        private Vector3 reallyFarAway = new Vector3(10000.0f, 10000.0f, 10000.0f);

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
            camera = sceneManager.SceneManager.createCamera(cameraId.FullName);
            camera.setAutoAspectRatio(true);
            camera.setNearClipDistance(0.1f);
            camera.setFOVy(10.0f);
            nodeId = new Identifier(name, CAMERA_NODE_RESERVED_NAME);
            node = sceneManager.SceneManager.createSceneNode(nodeId.FullName);
            node.attachObject(camera);
            viewport = renderWindow.addViewport(camera);

            if (OgreInterface.FoundOgreCore)
            {
                statsOverlay = new StatsOverlay(name);
                statsOverlay.createOverlays();
            }
            sceneManager.SceneManager.addSceneListener(this);
        }

        /// <summary>
        /// Add a light that follows the camera around. This will only create
        /// one light.
        /// </summary>
        public void addLight()
        {
            if (light == null)
            {
                lightId = new Identifier(nodeId.SimObjectName, LIGHT_RESERVED_NAME);
                light = sceneManager.SceneManager.createLight(lightId.FullName);
                light.setRenderQueueGroup(byte.MaxValue);
                light.setPosition(reallyFarAway);
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
                sceneManager.SceneManager.destroyLight(light);
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

        /// <summary>
        /// Get a ray that goes from the camera into the 3d space.
        /// </summary>
        /// <param name="x">The x value on the camera's 2d surface.</param>
        /// <param name="y">The y value on the camera's 2d surface.</param>
        /// <returns>A Ray3 with the ray.</returns>
        public Ray3 getCameraToViewportRay(float x, float y)
        {
            return camera.getCameraToViewportRay(x, y);
        }

        /// <summary>
        /// Show the scene stats in the window drawn by this camera.
        /// </summary>
        /// <param name="showStats">True to show the scene stats.</param>
        public void showSceneStats(bool showStats)
        {
            if (showStats != this.showStats && statsOverlay != null)
            {
                statsOverlay.setVisible(showStats);
            }
            this.showStats = showStats;
        }

        /// <summary>
        /// Change the RenderingMode of the camera.
        /// </summary>
        /// <param name="mode">The RenderingMode to set.</param>
        public void setRenderingMode(RenderingMode mode)
        {
            switch (mode)
            {
                case RenderingMode.Points:
                    camera.setPolygonMode(PolygonMode.PM_POINTS);
                    break;
                case RenderingMode.Wireframe:
                    camera.setPolygonMode(PolygonMode.PM_WIREFRAME);
                    break;
                case RenderingMode.Solid:
                    camera.setPolygonMode(PolygonMode.PM_SOLID);
                    break;
            }
        }

        public void Dispose()
        {
            sceneManager.SceneManager.removeSceneListener(this);
            if (statsOverlay != null)
            {
                statsOverlay.destroyOverlays();
            }
            removeLight();
            renderWindow.destroyViewport(viewport);
            node.detachObject(camera);
            sceneManager.SceneManager.destroyCamera(camera);
            sceneManager.SceneManager.destroySceneNode(node);
        }

        public void postFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Camera camera)
        {
            
        }

        public void preFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Camera camera)
        {
            if (showStats && statsOverlay != null)
            {
                statsOverlay.setStats(renderWindow);
                statsOverlay.setVisible(showStats && this.camera == camera);
            }
            if (light != null)
            {
                if (this.camera == camera)
                {
                    light.setPosition(node.getDerivedPosition());
                }
                else
                {
                    light.setPosition(reallyFarAway);
                }
            }
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
    }
}

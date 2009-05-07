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
        private static CameraLightManager lightManager = new CameraLightManager();

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
            statsOverlay = new StatsOverlay(name);
            statsOverlay.createOverlays();
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
                light = sceneManager.createLight(lightId);
                light.setRenderQueueGroup(byte.MaxValue);
                node.attachObject(light);
                lightManager.addLight(sceneManager.SceneManager, camera, light);
            }
        }

        /// <summary>
        /// Remove the light from the camera.
        /// </summary>
        public void removeLight()
        {
            if (light != null)
            {
                lightManager.removeLight(sceneManager.SceneManager, light);
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
            if (showStats != this.showStats)
            {
                statsOverlay.setVisible(showStats);
            }
            this.showStats = showStats;
        }

        public void Dispose()
        {
            sceneManager.SceneManager.removeSceneListener(this);
            statsOverlay.destroyOverlays();
            removeLight();
            renderWindow.destroyViewport(viewport);
            node.detachObject(camera);
            sceneManager.destroyCamera(cameraId);
            sceneManager.destroySceneNode(nodeId);
        }

        public void postFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Camera camera)
        {
            //statsOverlay.setVisible(false);
        }

        public void preFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Camera camera)
        {
            if (showStats)
            {
                statsOverlay.setStats(renderWindow);
                statsOverlay.setVisible(showStats && this.camera == camera);
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

        /// <summary>
        /// This class is a hack to get around the fact that ogre will render the
        /// light for the previous camera even if it is disabled in the
        /// SceneListener callbacks. This will maintain an ordered list of the
        /// lights by creation/camera and do the approprate enabling and disabling.
        /// This should only be used by the OgreCameraControl.
        /// </summary>
        class CameraLightManager : SceneListener
        {
            class LightCameraPair
            {
                public Camera camera;
                public Light light;

                public LightCameraPair(Camera camera, Light light)
                {
                    this.camera = camera;
                    this.light = light;
                }
            }

            private Dictionary<SceneManager, List<LightCameraPair>> lights = new Dictionary<SceneManager, List<LightCameraPair>>();

            /// <summary>
            /// Add a light to be managed. This assumes the light will only be added one time.
            /// </summary>
            /// <param name="sceneManager"></param>
            /// <param name="camera"></param>
            /// <param name="light"></param>
            public void addLight(SceneManager sceneManager, Camera camera, Light light)
            {
                if (!lights.ContainsKey(sceneManager))
                {
                    lights.Add(sceneManager, new List<LightCameraPair>());
                    sceneManager.addSceneListener(this);
                }
                lights[sceneManager].Add(new LightCameraPair(camera, light));
            }

            /// <summary>
            /// Remove a light. Call when the light is destroyed.
            /// </summary>
            /// <param name="sceneManager"></param>
            /// <param name="light"></param>
            public void removeLight(SceneManager sceneManager, Light light)
            {
                List<LightCameraPair> pairs = lights[sceneManager];
                LightCameraPair matchingPair = null;
                foreach (LightCameraPair pair in pairs)
                {
                    if (pair.light == light)
                    {
                        matchingPair = pair;
                        break;
                    }
                }
                if (matchingPair != null)
                {
                    pairs.Remove(matchingPair);
                    if (pairs.Count == 0)
                    {
                        sceneManager.removeSceneListener(this);
                        lights.Remove(sceneManager);
                    }
                }
            }

            #region SceneListener Members

            public void postFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Camera camera)
            {
                List<LightCameraPair> windows = lights[sceneManager];
                //The idea here is to enable the light for the next camera that will render
                //so scan backwards through the list and see if the previous camera is the one
                //being rendered.
                for (int i = windows.Count - 1; i >= 0; --i)
                {
                    if (i - 1 >= 0)
                    {
                        windows[i].light.setVisible(windows[i - 1].camera == camera);
                    }
                    else
                    {
                        windows[i].light.setVisible(windows[windows.Count - 1].camera == camera);
                    }
                }
            }

            public void preFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Camera camera)
            {

            }

            #endregion
        }
    }
}

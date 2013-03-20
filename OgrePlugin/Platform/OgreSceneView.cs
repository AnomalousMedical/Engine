using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgreWrapper;
using Engine.ObjectManagement;
using Logging;

namespace OgrePlugin
{
    /// <summary>
    /// A CameraControl class for ogre cameras.
    /// </summary>
    public class OgreSceneView : SceneView, IDisposable, SceneListener, RenderQueueListener
    {
        public event SceneViewEvent RenderingStarted;
        public event SceneViewEvent RenderingEnded;
        public event SceneViewEvent FindVisibleObjects;

        private const String CAMERA_RESERVED_NAME = "__AutoCreatedCamera";
        private const String CAMERA_NODE_RESERVED_NAME = "__AutoCreatedCameraNode";
        private const String LIGHT_RESERVED_NAME = "__AutoCreatedLight";
        private const String LIGHT_NODE_NAME = "__AutoCreatedLightNode";

        private SceneNode node;
        private Camera camera;
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
        public OgreSceneView(String name, OgreSceneManager sceneManager, RenderWindow renderWindow)
        {
            this.name = name;
            this.sceneManager = sceneManager;
            this.renderWindow = renderWindow;
            camera = sceneManager.SceneManager.createCamera(name + CAMERA_RESERVED_NAME);
            camera.setNearClipDistance(1.0f);
            camera.setAutoAspectRatio(true);
            camera.setFOVy(new Degree(10.0f));
            node = sceneManager.SceneManager.createSceneNode(name + CAMERA_NODE_RESERVED_NAME);
            node.attachObject(camera);
            viewport = renderWindow.addViewport(camera, renderWindow.getNumViewports(), 0, 0, 1, 1);
            sceneManager.SceneManager.getRootSceneNode().addChild(node);

            statsOverlay = new StatsOverlay(name);
            statsOverlay.createOverlays();
            sceneManager.SceneManager.addSceneListener(this);
            sceneManager.SceneManager.addRenderQueueListener(this);

            Root.getSingleton().FrameRenderingQueued += OgreCameraControl_FrameRenderingQueued;
        }

        public void Dispose()
        {
            Root.getSingleton().FrameRenderingQueued -= OgreCameraControl_FrameRenderingQueued;
            sceneManager.SceneManager.removeSceneListener(this);
            sceneManager.SceneManager.removeRenderQueueListener(this);
            if (statsOverlay != null)
            {
                statsOverlay.destroyOverlays();
            }
            sceneManager.SceneManager.getRootSceneNode().removeChild(node);
            removeLight();
            renderWindow.destroyViewport(viewport);
            node.detachObject(camera);
            sceneManager.SceneManager.destroyCamera(camera);
            sceneManager.SceneManager.destroySceneNode(node);
        }

        /// <summary>
        /// Add a light that follows the camera around. This will only create
        /// one light.
        /// </summary>
        public void addLight()
        {
            if (light == null)
            {
                light = sceneManager.SceneManager.createLight(name + LIGHT_RESERVED_NAME);
                light.setRenderQueueGroup(byte.MaxValue);
                light.setPosition(reallyFarAway);
            }
        }

        /// <summary>
        /// Set the near clip distance of the camera.
        /// </summary>
        /// <param name="distance">The distance to set.</param>
        public void setNearClipDistance(float distance)
        {
            camera.setNearClipDistance(distance);
        }

        /// <summary>
        /// Set the far clip distance of the camera.
        /// </summary>
        /// <param name="distance">The distance to set.</param>
        public void setFarClipDistance(float distance)
        {
            camera.setFarClipDistance(distance);
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

        public void moveSceneStats(Vector2 position)
        {
            statsOverlay.StatsPosition = position;
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

        /// <summary>
        /// Force this camera to redraw immediately.
        /// </summary>
        public void update(bool swapBuffers)
        {
            renderWindow.update(swapBuffers);
        }

        public PixelFormat suggestPixelFormat()
        {
            return renderWindow.suggestPixelFormat();
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

        public Vector3 Direction
        {
            get
            {
                return camera.getRealDirection();
            }
        }

        public Quaternion Orientation
        {
            get
            {
                return camera.getRealOrientation();
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
                //node.lookAt(lookAt, Node.TransformSpace.TS_WORLD);
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

        public bool ClearEveryFrame
        {
            get
            {
                return viewport.getClearEveryFrame();
            }
            set
            {
                viewport.setClearEveryFrame(value);
            }
        }

        public Viewport Viewport
        {
            get
            {
                return viewport;
            }
        }

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        public Matrix4x4 ViewMatrix
        {
            get { return camera.getViewMatrix(); }
        }

        public Matrix4x4 ProjectionMatrix
        {
            get { return camera.getProjectionMatrix(); }
        }

        public int RenderWidth
        {
            get
            {
                return viewport.getActualWidth();
            }
        }

        public int RenderHeight
        {
            get
            {
                return viewport.getActualHeight();
            }
        }

        public bool CurrentlyRendering { get; private set; }

        public void setDimensions(float left, float top, float width, float height)
        {
            viewport.setDimensions(left, top, width, height);
        }

        void OgreCameraControl_FrameRenderingQueued(FrameEvent frameEvent)
        {
            statsOverlay.setStats(renderWindow);
        }

        private void fireRenderingStarted()
        {
            if (showStats && statsOverlay != null)
            {
                statsOverlay.setVisible(true);
            }
            if (RenderingStarted != null)
            {
                RenderingStarted.Invoke(this);
            }
        }

        private void fireRenderingEnded()
        {
            if (showStats && statsOverlay != null)
            {
                statsOverlay.setVisible(false);
            }
            if (RenderingEnded != null)
            {
                RenderingEnded.Invoke(this);
            }
        }

        private void fireFindVisibleObjects()
        {
            if (light != null)
            {
                if (CurrentlyRendering)
                {
                    light.setPosition(node.getDerivedPosition());
                }
                else
                {
                    light.setPosition(reallyFarAway);
                }
            }
            if (FindVisibleObjects != null)
            {
                FindVisibleObjects.Invoke(this);
            }
        }

        #region SceneListener and RenderQueueListener

        public void postFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Viewport viewport)
        {
            
        }

        public void preFindVisibleObjects(SceneManager sceneManager, SceneManager.IlluminationRenderStage irs, Viewport viewport)
        {
            CurrentlyRendering = this.viewport == viewport;
            if (CurrentlyRendering)
            {
                fireRenderingStarted();
            }
            fireFindVisibleObjects();
        }

        public void preRenderQueues()
        {
            
        }

        public void postRenderQueues()
        {
            if (sceneManager.SceneManager.getCurrentViewport() == viewport)
            {
                fireRenderingEnded();
            }
        }

        public void renderQueueStarted(byte queueGroupId, string invocation, ref bool skipThisInvocation)
        {

        }

        public void renderQueueEnded(byte queueGroupId, string invocation, ref bool repeatThisInvocation)
        {

        }

        #endregion
    }
}

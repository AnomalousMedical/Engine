﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
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

        private const String CAMERA_RESERVED_NAME = "__AutoCreatedCamera";
        private const String CAMERA_NODE_RESERVED_NAME = "__AutoCreatedCameraNode";
        private const String LIGHT_RESERVED_NAME = "__AutoCreatedLight";
        private const String LIGHT_NODE_NAME = "__AutoCreatedLightNode";

        private SceneNode node;
        private Camera camera;
        private OgreSceneManager sceneManager;
        private Vector3 lookAt;
        private Viewport viewport;
        private String name;
        private RenderTarget renderTarget;
        private Vector3 reallyFarAway = new Vector3(10000.0f, 10000.0f, 10000.0f);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the camera control.</param>
        /// <param name="sceneManager">The scene manager to build the camera into.</param>
        /// <param name="renderTarget">The renderwindow to create a viewport on.</param>
        public OgreSceneView(String name, OgreSceneManager sceneManager, RenderTarget renderTarget, int zIndex)
        {
            this.name = name;
            this.sceneManager = sceneManager;
            this.renderTarget = renderTarget;
            camera = sceneManager.SceneManager.createCamera(name + CAMERA_RESERVED_NAME);
            camera.setNearClipDistance(1.0f);
            camera.setAutoAspectRatio(true);
            node = sceneManager.SceneManager.createSceneNode(name + CAMERA_NODE_RESERVED_NAME);
            node.attachObject(camera);
            viewport = renderTarget.addViewport(camera, zIndex, 0, 0, 1, 1);
            sceneManager.SceneManager.getRootSceneNode().addChild(node);
            sceneManager.SceneManager.addSceneListener(this);
            sceneManager.SceneManager.addRenderQueueListener(this);
        }

        public void Dispose()
        {
            sceneManager.SceneManager.removeSceneListener(this);
            sceneManager.SceneManager.removeRenderQueueListener(this);
            sceneManager.SceneManager.getRootSceneNode().removeChild(node);
            renderTarget.destroyViewport(viewport);
            node.detachObject(camera);
            sceneManager.SceneManager.destroyCamera(camera);
            sceneManager.SceneManager.destroySceneNode(node);
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
            renderTarget.update(swapBuffers);
        }

        public PixelFormat suggestPixelFormat()
        {
            return renderTarget.suggestPixelFormat();
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

        public String SchemeName
        {
            get
            {
                return viewport.getMaterialScheme();
            }
            set
            {
                viewport.setMaterialScheme(value);
            }
        }

        public Radian FovY
        {
            get
            {
                return camera.getFOVy();
            }
            set
            {
                camera.setFOVy(value);
            }
        }

        private void fireRenderingStarted()
        {
            if (RenderingStarted != null)
            {
                RenderingStarted.Invoke(this);
            }
        }

        private void fireRenderingEnded()
        {
            if (RenderingEnded != null)
            {
                RenderingEnded.Invoke(this);
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
        }

        public void preRenderQueues()
        {
            
        }

        public void postRenderQueues()
        {
            if (CurrentlyRendering)
            {
                fireRenderingEnded();
                CurrentlyRendering = false;
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

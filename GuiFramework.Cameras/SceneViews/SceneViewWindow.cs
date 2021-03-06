﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Engine.ObjectManagement;
using Engine.Renderer;
using Logging;
using OgrePlugin;
using MyGUIPlugin;
using Anomalous.GuiFramework;

namespace Anomalous.GuiFramework.Cameras
{
    public delegate void SceneViewWindowRenderEvent(SceneViewWindow window, bool currentCameraRender);
    public delegate void SceneViewWindowResizedEvent(SceneViewWindow window);

    public abstract class SceneViewWindow : LayoutContainer, IDisposable
    {
        public event SceneViewWindowEvent CameraCreated;
        public event SceneViewWindowEvent CameraDestroyed;
        public event SceneViewWindowEvent MadeActive;
        public event SceneViewWindowRenderEvent RenderingStarted;
        public event SceneViewWindowRenderEvent RenderingEnded;
        public event SceneViewWindowResizedEvent Resized;
        public event SceneViewWindowEvent Disposed;

        /// <summary>
        /// Fired when an undo is executed.
        /// </summary>
        public event Action<SceneViewWindow> OnUndo;

        /// <summary>
        /// Fired when a redo is executed.
        /// </summary>
        public event Action<SceneViewWindow> OnRedo;

        /// <summary>
        /// Fired when the Undo/Redo buffer is altered, either by being cleared or a new command added.
        /// </summary>
        public event Action<SceneViewWindow> OnUndoRedoChanged;

        private SceneView sceneView;
        private CameraMover cameraMover;
        private RendererWindow window;
        private String name;
        protected SceneViewController controller;

        private Vector3 startPosition;
        private Vector3 startLookAt;

        private Vector2 sceneViewportLocation = new Vector2(0f, 0f);
        private Size2 sceneViewportSize = new Size2(1f, 1f);
        private float inverseAspectRatio = 1.0f;

        private Color backColor = new Color(0.149f, 0.149f, 0.149f);

        protected String transparencyStateName;

        private bool autoAspectRatio = true;
        private Widget borderPanel0;
        private Widget borderPanel1;
        protected BackgroundScene background;
        private ViewportBackground vpBackground;
        private int zIndexStart;
        private int zOffset = 0;
        private RenderingMode renderingMode = RenderingMode.Solid;
        private bool clearEveryFrame = false;
        private String schemeName = MaterialManager.DefaultSchemeName;
        private Radian fovY = new Degree(10).Radians;

        private UndoRedoBuffer undoRedoBuffer = new UndoRedoBuffer(20);

        public SceneViewWindow(SceneViewController controller, CameraMover cameraMover, String name, BackgroundScene background, int zIndexStart)
        {
            this.zIndexStart = zIndexStart;
            this.background = background;
            this.controller = controller;
            this.cameraMover = cameraMover;
            this.name = name;
            this.startPosition = cameraMover.Translation;
            this.startLookAt = cameraMover.LookAt;
            transparencyStateName = name;
            NearPlaneWorld = 230;
            NearFarLength = 450;
            MinNearDistance = 1;
        }

        /// <summary>
        /// You must call this function to activate the background with the appropriate render target.
        /// </summary>
        /// <param name="renderTarget"></param>
        protected void createBackground(RenderTarget renderTarget, bool clearEveryFrame)
        {
            if (background != null)
            {
                vpBackground = new ViewportBackground(name + "SceneViewBackground", zIndexStart + zOffset++, background, renderTarget, clearEveryFrame);
                vpBackground.BackgroundColor = backColor;
            }
        }

        public virtual void Dispose()
        {
            IDisposableUtil.DisposeIfNotNull(vpBackground);
            cameraMover.Dispose();
            destroyBorderPanels();
            if (Disposed != null)
            {
                Disposed.Invoke(this);
            }
        }

        public virtual void createSceneView(SimScene scene)
        {
            Log.Info("Creating SceneView for {0}.", name);
            SimSubScene defaultScene = scene.getDefaultSubScene();

            sceneView = window.createSceneView(defaultScene, name, cameraMover.Translation, cameraMover.LookAt, zIndexStart + zOffset++);
            sceneView.setDimensions(sceneViewportLocation.x, sceneViewportLocation.y, sceneViewportSize.Width, sceneViewportSize.Height);
            if (vpBackground != null)
            {
                vpBackground.setDimensions(sceneViewportLocation.x, sceneViewportLocation.y, sceneViewportSize.Width, sceneViewportSize.Height);
            }
            sceneView.BackgroundColor = backColor;
            sceneView.setNearClipDistance(1.0f);
            sceneView.setFarClipDistance(1000.0f);
            sceneView.FovY = fovY;
            sceneView.ClearEveryFrame = clearEveryFrame;
            sceneView.setRenderingMode(renderingMode);
            cameraMover.setCamera(new CameraPositioner(sceneView, MinNearDistance, NearPlaneWorld, NearFarLength));
            sceneView.RenderingStarted += sceneView_RenderingStarted;
            sceneView.RenderingEnded += sceneView_RenderingEnded;
            sceneView.SchemeName = schemeName;
            if (CameraCreated != null)
            {
                CameraCreated.Invoke(this);
            }
        }

        public virtual void destroySceneView()
        {
            if (sceneView != null)
            {
                if (CameraDestroyed != null)
                {
                    CameraDestroyed.Invoke(this);
                }
                --zOffset;
                Log.Info("Destroying SceneView for {0}.", name);
                sceneView.RenderingStarted -= sceneView_RenderingStarted;
                sceneView.RenderingEnded -= sceneView_RenderingEnded;
                cameraMover.setCamera(null);
                window.destroySceneView(sceneView);
                sceneView = null;
            }
        }

        public abstract void close();

        public void setPosition(CameraPosition cameraPosition, float duration)
        {
            cameraMover.setNewPosition(this.computeAdjustedTranslation(cameraPosition), cameraPosition.LookAt, duration, cameraPosition.Easing);
            if (cameraPosition.UseIncludePoint)
            {
                cameraMover.maintainIncludePoint(cameraPosition.IncludePoint);
            }
            else
            {
                cameraMover.stopMaintainingIncludePoint();
            }
        }

        public void immediatlySetPosition(CameraPosition cameraPosition)
        {
            cameraMover.immediatlySetPosition(cameraPosition.Translation, cameraPosition.LookAt);
            if (cameraPosition.UseIncludePoint)
            {
                cameraMover.maintainIncludePoint(cameraPosition.IncludePoint);
            }
            else
            {
                cameraMover.stopMaintainingIncludePoint();
            }
        }

        public override void setAlpha(float alpha)
        {

        }

        public override void layout()
        {
            IntVector2 sceneViewLocation = new IntVector2(0, 0);
            IntSize2 size = new IntSize2(1, 1);

            IntSize2 totalSize = TopmostWorkingSize;
            if (totalSize.Width == 0)
            {
                totalSize.Width = 1;
            }
            if (totalSize.Height == 0)
            {
                totalSize.Height = 1;
            }

            sceneViewLocation = Location;
            size = WorkingSize;

            if (!AutoAspectRatio)
            {
                size.Height = (int)(size.Width * inverseAspectRatio);
                if (size.Height > WorkingSize.Height) //Letterbox width
                {
                    size.Height = WorkingSize.Height;
                    size.Width = (int)(size.Height * (1 / inverseAspectRatio));
                    sceneViewLocation.x += (WorkingSize.Width - size.Width) / 2;

                    borderPanel0.setCoord(Location.x, Location.y, sceneViewLocation.x - Location.x, WorkingSize.Height);
                    borderPanel1.setCoord(sceneViewLocation.x + size.Width, Location.y, sceneViewLocation.x - Location.x + 1, WorkingSize.Height);
                }
                else
                {
                    sceneViewLocation.y += (WorkingSize.Height - size.Height) / 2;

                    borderPanel0.setCoord(Location.x, Location.y, WorkingSize.Width, sceneViewLocation.y - Location.y);
                    borderPanel1.setCoord(Location.x, sceneViewLocation.y + size.Height, WorkingSize.Width, sceneViewLocation.y - Location.y + 1);
                }
            }

            RenderXLoc = sceneViewLocation.x;
            RenderYLoc = sceneViewLocation.y;

            sceneViewportLocation = new Vector2((float)sceneViewLocation.x / totalSize.Width, (float)sceneViewLocation.y / totalSize.Height);
            sceneViewportSize = new Size2((float)size.Width / totalSize.Width, (float)size.Height / totalSize.Height);

            if(sceneViewportSize.Width < 0.0f)
            {
                sceneViewportSize.Width = 0.0f;
            }

            if (sceneViewportSize.Height < 0.0f)
            {
                sceneViewportSize.Height = 0.0f;
            }

            if (sceneView != null)
            {
                sceneView.setDimensions(sceneViewportLocation.x, sceneViewportLocation.y, sceneViewportSize.Width, sceneViewportSize.Height);
                if (vpBackground != null)
                {
                    vpBackground.setDimensions(sceneViewportLocation.x, sceneViewportLocation.y, sceneViewportSize.Width, sceneViewportSize.Height);
                }
                cameraMover.processIncludePoint(Camera);
            }

            if (Resized != null)
            {
                Resized.Invoke(this);
            }
        }

        public override IntSize2 DesiredSize
        {
            get
            {
                if (sceneView != null)
                {
                    return new IntSize2(sceneView.RenderWidth, sceneView.RenderHeight);
                }
                return new IntSize2();
            }
        }

        public void resetToStartPosition()
        {
            cameraMover.immediatlySetPosition(startPosition, startLookAt);
        }

        public override void bringToFront()
        {
            
        }

        /// <summary>
        /// Get the projected position of the given vector3. Does not account for the position of this sceneviewwindow.
        /// </summary>
        public Vector3 getProjectedPosition(Vector3 worldPosition)
        {
            Matrix4x4 proj = sceneView.ProjectionMatrix;
            Matrix4x4 view = sceneView.ViewMatrix;
            Vector3 screenPos = proj * (view * worldPosition);
            screenPos.x = (screenPos.x / 2.0f + 0.5f) * sceneView.RenderWidth;
            screenPos.y = (1 - (screenPos.y / 2.0f + 0.5f)) * sceneView.RenderHeight;
            return screenPos;
        }

        /// <summary>
        /// Get the absolute screen position of the given Vector3, this will account for the screen position of this window.
        /// </summary>
        public IntVector2 getAbsoluteScreenPosition(Vector3 worldPosition)
        {
            Matrix4x4 proj = sceneView.ProjectionMatrix;
            Matrix4x4 view = sceneView.ViewMatrix;
            Vector3 projectedPos = proj * (view * worldPosition);
            return new IntVector2
            (
                (int)((projectedPos.x / 2.0f + 0.5f) * sceneView.RenderWidth) + RenderXLoc,
                (int)((1 - (projectedPos.y / 2.0f + 0.5f)) * sceneView.RenderHeight) + RenderYLoc
            );
        }

        /// <summary>
        /// Setup the include point for the given camera position for this window. Modifies cameraPosition to have the calculated value
        /// and sets UseIncludePoint to true.
        /// </summary>
        /// <param name="cameraPosition"></param>
        public void calculateIncludePoint(CameraPosition cameraPosition)
        {
            //Make the include point projected out to the lookat location
            Ray3 camRay = this.getCameraToViewportRayRatio(1, 0);
            cameraPosition.IncludePoint = camRay.Origin + camRay.Direction * (cameraPosition.LookAt - cameraPosition.Translation).length();
            cameraPosition.UseIncludePoint = true;
        }

        /// <summary>
        /// Compute a new translation for the given camera position and returns it.
        /// </summary>
        /// <param name="cameraPosition"></param>
        /// <returns></returns>
        public Vector3 computeAdjustedTranslation(CameraPosition cameraPosition)
        {
            if (cameraPosition.UseIncludePoint && cameraPosition.IncludePoint.isNumber())
            {
                return SceneViewWindow.computeIncludePointAdjustedPosition(Camera.getAspectRatio(), Camera.getFOVy(), Camera.getProjectionMatrix(), cameraPosition.Translation, cameraPosition.LookAt, cameraPosition.IncludePoint);
            }

            return cameraPosition.Translation;
        }

        public static Vector3 computeIncludePointAdjustedPosition(float aspect, float fovy, Matrix4x4 projectionMatrix, Vector3 translation, Vector3 lookAt, Vector3 includePoint)
        {
            fovy *= 0.5f;
            Vector3 direction = lookAt - translation;

            //Figure out direction, must use ogre fixed yaw calculation, first adjust direction to face -z
            Vector3 zAdjustVec = -direction;
            zAdjustVec.normalize();
            Quaternion targetWorldOrientation = Quaternion.shortestArcQuatFixedYaw(ref zAdjustVec);

            Matrix4x4 viewMatrix = Matrix4x4.makeViewMatrix(translation, targetWorldOrientation);
            float offset = SceneViewWindow.computeOffsetToIncludePoint(viewMatrix, projectionMatrix, includePoint, aspect, fovy);

            direction.normalize();
            Vector3 newTrans = translation + offset * direction;
            return newTrans;
        }

        public static float computeOffsetToIncludePoint(Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix, Vector3 include, float aspect, float fovy)
        {
            //Transform the point from world space to camera space
            Vector3 localInclude = viewMatrix * include;
            float fovx = (float)Math.Atan(aspect * Math.Tan(fovy));

            //Project the points onto the two triangles formed between the camera, the z coord and the x or y coord we know the 
            //length of the opposite leg (x or y) and can compute the adjacent leg by multiplying by the Tan(field of view). This gives
            //the distance from the include point the camera needs to be to include it in view on that dimension (x or y). 
            float yOffset = Math.Abs(localInclude.y) / (float)Math.Tan(fovy);
            float xOffset = Math.Abs(localInclude.x) / (float)Math.Tan(fovx);

            //The camera is currently at localInclude.z so figure out how far it would need to move to be at the final point. This is 
            //the difference between the z coord and the offset.
            float zValue = Math.Abs(localInclude.z);
            xOffset = zValue - xOffset;
            yOffset = zValue - yOffset;

            Vector3 projectionSpace = (projectionMatrix * viewMatrix) * include;

            //Find the dimension that is more off the screen.
            if (projectionSpace.x > projectionSpace.y)
            {
                return xOffset;
            }
            return yOffset;
        }

        public static Vector3 Unproject(float screenX, float screenY, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
        {
            Matrix4x4 inverseVP = (projectionMatrix * viewMatrix).inverse();

            float nx = (2.0f * screenX) - 1.0f;
            float ny = 1.0f - (2.0f * screenY);
            Vector3 midPoint = new Vector3(nx, ny, 0.0f);

            return inverseVP * midPoint;
        }

        public static Vector2 Project(Vector3 point, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix, int screenWidth, int screenHeight)
        {
            Vector3 pos2d = projectionMatrix * (viewMatrix * point);
            return new Vector2(((pos2d.x * 0.5f) + 0.5f) * screenWidth, (1.0f - ((pos2d.y * 0.5f) + 0.5f)) * screenHeight);
        }

        /// <summary>
        /// Get a ray from the camera using a ratio from 0 to 1.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Ray3 getCameraToViewportRayRatio(float x, float y)
        {
            if (sceneView != null)
            {
                return sceneView.getCameraToViewportRay(x, y);
            }
            return new Ray3();
        }

        /// <summary>
        /// Get a ray from the camera using screen coords, these will be converted into local coords, so you can use
        /// mouse absolute positions.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Ray3 getCameraToViewportRayScreen(int x, int y)
        {
            Vector2 windowLoc = new Vector2(RenderXLoc, RenderYLoc);
            Size2 windowSize = new Size2(RenderWidth, RenderHeight);
            float ratioX = (x - windowLoc.x) / windowSize.Width;
            float ratioY = (y - windowLoc.y) / windowSize.Height;
            return getCameraToViewportRayRatio(ratioX, ratioY);
        }

        public Vector3 unproject(float screenX, float screenY)
        {
            return Unproject(screenX, screenY, Camera.getViewMatrix(), Camera.getProjectionMatrix());
        }

        public Vector3 unprojectScreen(int x, int y)
        {
            return Unproject((x - RenderXLoc) / (float)RenderWidth, (y - RenderYLoc) / (float)RenderHeight, Camera.getViewMatrix(), Camera.getProjectionMatrix());
        }

        /// <summary>
        /// Determine if the given x, y location is contained in this SceneViewWindow.
        /// </summary>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location.</param>
        /// <returns>True if the point is contained in this window. False if it is not.</returns>
        public bool containsPoint(int x, int y)
        {
            return (x > Location.x && x < Location.x + WorkingSize.Width) && (y > Location.y && y < Location.y + WorkingSize.Height);
        }

        /// <summary>
        /// Determine if the given x, y location is contained in this SceneViewWindow.
        /// </summary>
        /// <returns>True if the point is contained in this window. False if it is not.</returns>
        public bool containsPoint(IntVector2 point)
        {
            return (point.x > Location.x && point.x < Location.x + WorkingSize.Width) && (point.y > Location.y && point.y < Location.y + WorkingSize.Height);
        }

        /// <summary>
        /// Create a camera position from the target position, this will not include an
        /// include point and is mostly suitable for undo / redo.
        /// </summary>
        /// <returns></returns>
        public CameraPosition createCameraPosition()
        {
            return new CameraPosition()
            {
                Translation = cameraMover.TargetTranslation,
                LookAt = cameraMover.TargetLookAt,
            };
        }

        /// <summary>
        /// Undo the layer buffer for the active state.
        /// </summary>
        public void undo()
        {
            undoRedoBuffer.undo();
        }

        /// <summary>
        /// Redo the layer buffer for the active state..
        /// </summary>
        public void redo()
        {
            undoRedoBuffer.execute();
        }

        /// <summary>
        /// Push a new state onto the undo/redo buffer, will erase anything after the current undo.
        /// This will use the passed state as the undo state and the current muscle position of the scene
        /// as the redo state.
        /// </summary>
        public void pushUndoState(CameraPosition cameraPosition)
        {
            pushUndoState(cameraPosition, createCameraPosition());
        }

        /// <summary>
        /// Push a new state onto the undo/redo buffer, will erase anything after the current undo.
        /// This will use the passed states for undo and redo.
        /// </summary>
        public void pushUndoState(CameraPosition undoState, CameraPosition redoState)
        {
            //Make sure the undo and redo states are sufficiently different, otherwise ignore this new entry
            if ((undoState.Translation - redoState.Translation).length2() > 0.001 || (undoState.LookAt - redoState.LookAt).length2() > 0.001)
            {
                undoRedoBuffer.pushAndSkip(new TwoWayDelegateCommand<CameraPosition, CameraPosition>(redoState, undoState, new TwoWayDelegateCommand<CameraPosition, CameraPosition>.Funcs()
                {
                    ExecuteFunc = state =>
                    {
                        this.setPosition(state, GuiFrameworkCamerasInterface.CameraTransitionTime);
                        if (OnRedo != null)
                        {
                            OnRedo.Invoke(this);
                        }
                    },
                    UndoFunc = state =>
                    {
                        this.setPosition(state, GuiFrameworkCamerasInterface.CameraTransitionTime);
                        if (OnUndo != null)
                        {
                            OnUndo.Invoke(this);
                        }
                    }
                }));
                if (OnUndoRedoChanged != null)
                {
                    OnUndoRedoChanged.Invoke(this);
                }
            }
        }

        public bool HasUndo
        {
            get
            {
                return undoRedoBuffer.HasUndo;
            }
        }

        public bool HasRedo
        {
            get
            {
                return undoRedoBuffer.HasRedo;
            }
        }

        public override bool Visible
        {
            get
            {
                return true;
            }
            set
            {
                
            }
        }

        public Vector3 Translation
        {
            get
            {
                return cameraMover.Translation;
            }
        }

        public Vector3 LookAt
        {
            get
            {
                return cameraMover.LookAt;
            }
        }

        public Vector3 Direction
        {
            get
            {
                return sceneView.Direction;
            }
        }

        public Quaternion Orientation
        {
            get
            {
                return sceneView.Orientation;
            }
        }

        public String CurrentTransparencyState
        {
            get
            {
                return transparencyStateName;
            }
        }

        public Radian FovY
        {
            get
            {
                return fovY;
            }
            set
            {
                fovY = value;
                if (sceneView != null)
                {
                    sceneView.FovY = fovY;
                }
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public Camera Camera
        {
            get
            {
                //Have to typecast the scene view for this.
                return ((OgreSceneView)sceneView).Camera;
            }
        }

        public Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
                if (vpBackground != null)
                {
                    vpBackground.BackgroundColor = value;
                }
                if (sceneView != null)
                {
                    sceneView.BackgroundColor = value;
                }
            }
        }

        public bool ClearEveryFrame
        {
            get
            {
                return clearEveryFrame;
            }
            set
            {
                if (clearEveryFrame != value)
                {
                    clearEveryFrame = value;
                    if (sceneView != null)
                    {
                        sceneView.ClearEveryFrame = value;
                    }
                }
            }
        }

        public int RenderXLoc { get; private set; }

        public int RenderYLoc { get; private set; }

        public int RenderWidth
        {
            get
            {
                if (sceneView != null)
                {
                    return sceneView.RenderWidth;
                }
                else
                {
                    return 1;
                }
            }
        }

        public int RenderHeight
        {
            get
            {
                if (sceneView != null)
                {
                    return sceneView.RenderHeight;
                }
                else
                {
                    return 1;
                }
            }
        }

        public CameraMover CameraMover
        {
            get
            {
                return cameraMover;
            }
        }

        /// <summary>
        /// True to autocalculate the aspect ratio. If this is false the window will be letterboxed.
        /// </summary>
        public bool AutoAspectRatio
        {
            get
            {
                return autoAspectRatio;
            }
            set
            {
                if (autoAspectRatio != value)
                {
                    autoAspectRatio = value;
                    if (autoAspectRatio)
                    {
                        destroyBorderPanels();
                    }
                    else
                    {
                        createBorderPanels();
                    }
                }
            }
        }

        /// <summary>
        /// The aspect ratio as width / height
        /// </summary>
        public float AspectRatio
        {
            get
            {
                if (inverseAspectRatio != 0.0f)
                {
                    return 1.0f / inverseAspectRatio;
                }
                else
                {
                    return 1.0f;
                }
            }
            set
            {
                if (value != 0.0f)
                {
                    inverseAspectRatio = 1.0f / value;
                }
                else
                {
                    inverseAspectRatio = 1.0f;
                }
            }
        }

        /// <summary>
        /// The position of the near plane in the world along the ray from the camera to the origin.
        /// </summary>
        public float NearPlaneWorld { get; set; }

        /// <summary>
        /// The length of the near far plane.
        /// </summary>
        public float NearFarLength { get; set; }

        /// <summary>
        /// The minimum distance that the near plane can be to the eye.
        /// </summary>
        public float MinNearDistance { get; set; }

        public RenderingMode RenderingMode
        {
            get
            {
                return renderingMode;
            }
            set
            {
                if (renderingMode != value)
                {
                    renderingMode = value;
                    if (sceneView != null)
                    {
                        sceneView.setRenderingMode(renderingMode);
                    }
                }
            }
        }

        protected RendererWindow RendererWindow
        {
            get
            {
                return window;
            }
            set
            {
                window = value;
            }
        }

        public String SchemeName
        {
            get
            {
                return schemeName;
            }
            set
            {
                schemeName = value;
                if(sceneView != null)
                {
                    sceneView.SchemeName = value;
                }
            }
        }

        protected void fireMadeActive()
        {
            if(MadeActive != null)
            {
                MadeActive.Invoke(this);
            }
        }

        void sceneView_RenderingEnded(SceneView sceneView)
        {
            if (RenderingEnded != null)
            {
                RenderingEnded.Invoke(this, sceneView.CurrentlyRendering);
            }
        }

        void sceneView_RenderingStarted(SceneView sceneView)
        {
            if (RenderingStarted != null)
            {
                RenderingStarted.Invoke(this, sceneView.CurrentlyRendering);
            }
        }

        void createBorderPanels()
        {
            if (borderPanel0 == null)
            {
                borderPanel0 = Gui.Instance.createWidgetT("Widget", "Medical.SceneViewBorder", 0, 0, 1, 1, Align.Default, "Back", "");
                borderPanel1 = Gui.Instance.createWidgetT("Widget", "Medical.SceneViewBorder", 0, 0, 1, 1, Align.Default, "Back", "");
            }
        }

        void destroyBorderPanels()
        {
            if (borderPanel0 != null)
            {
                Gui.Instance.destroyWidget(borderPanel0);
                Gui.Instance.destroyWidget(borderPanel1);
                borderPanel0 = null;
                borderPanel1 = null;
            }
        }
    }
}

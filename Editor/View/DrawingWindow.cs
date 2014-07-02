using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Platform;
using Engine.Renderer;
using Engine.ObjectManagement;
using Logging;
using Engine;
using Editor;

namespace Editor
{
    public partial class DrawingWindow : UserControl, OSWindow, CameraMotionValidator
    {
        private RendererWindow window;
        private String name;
        private SceneView camera;
        private OrbitCameraController orbitCamera;
        private RendererPlugin renderer;
        private bool showSceneStats = false;
        private UpdateTimer mainTimer;
        private RenderingMode renderingMode = RenderingMode.Solid;

        public DrawingWindow()
        {
            InitializeComponent();
        }

        internal void initialize(string name, EventManager eventManager, RendererPlugin renderer, Vector3 translation, Vector3 lookAt)
        {
            this.name = name;
            this.renderer = renderer;
            orbitCamera = new OrbitCameraController(translation, lookAt, eventManager);
            orbitCamera.MotionValidator = this;
            window = renderer.createRendererWindow(this, name);
        }

        #region OSWindow Members

        public String WindowHandle
        {
            get
            {
                return this.Handle.ToString();
            }
        }

        public int WindowWidth
        {
            get
            {
                return this.Width;
            }
        }

        public int WindowHeight
        {
            get
            {
                return this.Height;
            }
        }

        public float WindowScaling
        {
            get
            {
                return 1.0f;
            }
        }

        public event OSWindowEvent Moved;

        public event OSWindowEvent Resized;

        public event OSWindowEvent Closing;

        public event OSWindowEvent Closed;

        public event OSWindowEvent FocusChanged;

        #endregion

        public void createCamera(UpdateTimer mainTimer, SimScene scene)
        {
            SimSubScene defaultScene = scene.getDefaultSubScene();
            if (defaultScene != null)
            {
                this.mainTimer = mainTimer;
                camera = window.createSceneView(defaultScene, name, orbitCamera.Translation, orbitCamera.LookAt);
                camera.BackgroundColor = Engine.Color.FromARGB(BackColor.ToArgb());
                camera.setRenderingMode(renderingMode);
                mainTimer.addUpdateListener(orbitCamera);
                orbitCamera.setCamera(camera);
                CameraResolver.addMotionValidator(this);
            }
            else
            {
                Log.Default.sendMessage("Cannot find default subscene for the scene. Not creating camera.", LogLevel.Error, "Anomaly");
            }
        }

        public void destroyCamera()
        {
            if (camera != null)
            {
                orbitCamera.setCamera(null);
                window.destroySceneView(camera);
                mainTimer.removeUpdateListener(orbitCamera);
                camera = null;
                CameraResolver.removeMotionValidator(this);
            }
        }

        public void setEnabled(bool enabled)
        {
            if (window != null)
            {
                window.setEnabled(enabled);
            }
        }

        public void showStats(bool show)
        {
            showSceneStats = show;
        }

        public void setRenderingMode(RenderingMode mode)
        {
            this.renderingMode = mode;
            if (camera != null)
            {
                camera.setRenderingMode(renderingMode);
            }
        }

        public String CameraName
        {
            get
            {
                return name;
            }
        }

        public Vector3 Translation
        {
            get
            {
                return orbitCamera.Translation;
            }
        }

        public Vector3 LookAt
        {
            get
            {
                return orbitCamera.LookAt;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.Size.Width > 0 && this.Size.Height > 0)
            {
                if (Resized != null)
                {
                    Resized.Invoke(this);
                }
            }
            base.OnResize(e);
        }

        protected override void OnMove(EventArgs e)
        {
            if(Moved != null)
            {
                Moved.Invoke(this);
            }
            base.OnMove(e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            if (camera != null)
            {
                camera.BackgroundColor = Engine.Color.FromARGB(BackColor.ToArgb());
            }
            base.OnBackColorChanged(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if(Closing != null)
            {
                Closing.Invoke(this);
            }
            if (Closed != null)
            {
                Closed.Invoke(this);
            }
            if (window != null)
            {
                renderer.destroyRendererWindow(window);
            }
            base.OnHandleDestroyed(e);
        }

        #region CameraMotionValidator Members

        /// <summary>
        /// Determine if the camera should be allowed to move based on the current mouse location.
        /// </summary>
        /// <param name="x">The x location of the mouse.</param>
        /// <param name="y">The y location of the mouse.</param>
        /// <returns>True if the camera should be allowed to move.  False if it should stay still.</returns>
        public bool allowMotion(int x, int y)
        {
            Control topLevel = this.TopLevelControl;
            if (topLevel != null)
            {
                return ClientRectangle.Contains(this.PointToClient(topLevel.PointToScreen(new Point(x, y))));
            }
            return false;
        }

        /// <summary>
        /// Determine if the window is currently set as "active" allowing certain behavior.
        /// This is an optional check by classes using the validator it may be desirable to
        /// do an action even if the window is not active.
        /// </summary>
        /// <returns>True if the window is active.</returns>
        public bool isActiveWindow()
        {
            return this.Focused;
        }

        /// <summary>
        /// Get the location passed in the coordinates for the motion validator.
        /// </summary>
        /// <param name="x">X location.</param>
        /// <param name="y">Y location.</param>
        public void getLocalCoords(ref float x, ref float y)
        {
            doGetLocalCoords(ref x, ref y, this);
        }

        /// <summary>
        /// Helper function to find the local coords.  We need to ignore the top level frame,
        /// so this will recurse until the control has no parent.
        /// </summary>
        /// <param name="x">The x location.</param>
        /// <param name="y">The y location.</param>
        /// <param name="ctrl">The current control to scan.</param>
        private void doGetLocalCoords(ref float x, ref float y, Control ctrl)
        {
            if (ctrl.Parent != null)
            {
                Point p = ctrl.Location;
                x -= p.X;
                y -= p.Y;
                doGetLocalCoords(ref x, ref y, ctrl.Parent);
            }
        }

        /// <summary>
        /// Get the width of the mouse area for this validator.
        /// </summary>
        /// <returns>The width of the mouse area.</returns>
        public float getMouseAreaWidth()
        {
            return Width;
        }

        /// <summary>
        /// Get the height of the mouse area for this validator.
        /// </summary>
        /// <returns>The height of the mouse area.</returns>
        public float getMouseAreaHeight()
        {
            return Height;
        }

        /// <summary>
        /// Get the camera for this motion validator.
        /// </summary>
        /// <returns>The camera for this validator.</returns>
        public SceneView getCamera()
        {
            return camera;
        }

        #endregion
    }
}

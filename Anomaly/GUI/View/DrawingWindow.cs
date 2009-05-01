using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Platform;
using Engine.Renderer;
using Engine.ObjectManagement;
using Logging;
using Engine;

namespace Anomaly
{
    partial class DrawingWindow : UserControl, OSWindow
    {
        private List<OSWindowListener> listeners = new List<OSWindowListener>();
        private AnomalyController controller;
        private RendererWindow window;
        private String name;
        private CameraControl camera;
        private OrbitCameraController orbitCamera;

        public DrawingWindow()
        {
            InitializeComponent();
        }

        internal void initialize(string name, AnomalyController controller, Vector3 translation, Vector3 lookAt)
        {
            this.name = name;
            this.controller = controller;
            orbitCamera = new OrbitCameraController(translation, lookAt, controller.EventManager);
            controller.OnSceneLoaded += new SceneLoaded(sceneLoaded);
            controller.OnSceneUnloading += new SceneUnloading(controller_OnSceneUnloading);
            window = controller.PluginManager.RendererPlugin.createRendererWindow(this, name);
        }

        #region OSWindow Members

        public void addListener(OSWindowListener listener)
        {
            listeners.Add(listener);
        }

        public void removeListener(OSWindowListener listener)
        {
            listeners.Remove(listener);
        }

        public IntPtr WindowHandle
        {
            get
            {
                return this.Handle;
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

        #endregion

        void sceneLoaded(AnomalyController controller, SimScene scene)
        {
            SimSubScene defaultScene = scene.getDefaultSubScene();
            if (defaultScene != null)
            {
                camera = window.createCamera(defaultScene, name, orbitCamera.Translation, orbitCamera.LookAt);
                camera.BackgroundColor = new Engine.Color(0.0f, 0.0f, 1.0f);
                camera.addLight();
                controller.MainTimer.addFixedUpdateListener(orbitCamera);
                orbitCamera.setCamera(camera);
            }
            else
            {
                Log.Default.sendMessage("Cannot find default subscene for the scene. Not creating camera.", LogLevel.Error, "Anomaly");
            }
        }

        void controller_OnSceneUnloading(AnomalyController controller, SimScene scene)
        {
            if (camera != null)
            {
                orbitCamera.setCamera(null);
                window.destroyCamera(camera);
                controller.MainTimer.removeFixedUpdateListener(orbitCamera);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.resized(this);
            }
            base.OnResize(e);
        }

        protected override void OnMove(EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.moved(this);
            }
            base.OnMove(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.closing(this);
            }
            controller.PluginManager.RendererPlugin.destroyRendererWindow(window);
            base.OnHandleDestroyed(e);
        }
    }
}

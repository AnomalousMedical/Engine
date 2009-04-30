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

namespace Anomaly
{
    partial class DrawingWindow : UserControl, OSWindow
    {
        private List<OSWindowListener> listeners = new List<OSWindowListener>();
        private AnomalyController controller;
        private RendererWindow window;

        public DrawingWindow()
        {
            InitializeComponent();
        }

        public void initialize(AnomalyController controller, String name)
        {
            this.controller = controller;
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

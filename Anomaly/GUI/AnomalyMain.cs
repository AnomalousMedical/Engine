using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine.Platform;
using Engine;

namespace Anomaly
{
    partial class AnomalyMain : Form, OSWindow
    {
        private List<OSWindowListener> listeners = new List<OSWindowListener>();
        private AnomalyController controller;

        public AnomalyMain()
        {
            InitializeComponent();
        }

        public void initialize(AnomalyController controller)
        {
            this.controller = controller;
            this.movePanel.initialize(controller.MoveController);
            this.templatePanel1.initialize(controller.TemplateController);
            this.simObjectPanel.intialize(controller);
        }

        public Control SplitControl
        {
            get
            {
                return objectViewSplit.Panel2;
            }
        }

        #region OSWindow Members

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

        public void addListener(OSWindowListener listener)
        {
            listeners.Add(listener);
        }

        public void removeListener(OSWindowListener listener)
        {
            listeners.Remove(listener);
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
            base.OnHandleDestroyed(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (controller != null)
            {
                controller.shutdown();
            }
            base.OnFormClosing(e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.ResourceController.editResources();
        }
    }
}

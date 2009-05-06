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
using Editor;

namespace Anomaly
{
    partial class AnomalyMain : Form, OSWindow
    {
        private List<OSWindowListener> listeners = new List<OSWindowListener>();
        private AnomalyController controller;
        FileTracker fileTracker;

        public AnomalyMain()
        {
            InitializeComponent();
        }

        public void initialize(AnomalyController controller)
        {
            fileTracker = new FileTracker("*.sim.xml|*.sim.xml");
            this.controller = controller;
            this.movePanel.initialize(controller.MoveController);
            this.templatePanel1.initialize(controller.TemplateController);
            this.simObjectPanel.intialize(controller);
            this.eulerRotatePanel1.initialize(controller.RotateController);
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = fileTracker.saveFile(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.saveScene(filename);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = fileTracker.saveFileAs(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.saveScene(filename);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String filename = fileTracker.openFile(this);
            if (fileTracker.lastDialogAccepted())
            {
                controller.loadScene(filename);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.createNewScene();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.SceneController.editScene();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Anomaly
{
    partial class SplitViewHost : DockContent
    {
        private List<Control> savedControls = new List<Control>();
        private bool notClosing = true;
        private SplitViewController controller;

        public SplitViewHost(String name, SplitViewController controller)
        {
            InitializeComponent();
            this.Text = name;
            this.Name = name;
            this.controller = controller;
        }

        public DrawingWindow DrawingWindow
        {
            get
            {
                return drawingWindow;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            controller._alertCameraDestroyed(this);
            notClosing = false;
            drawingWindow.destroyCamera();
            base.OnClosing(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (notClosing)
            {
                savedControls.Clear();
                foreach (Control control in Controls)
                {
                    savedControls.Add(control);
                }
                this.Controls.Clear();
            }
            base.OnHandleDestroyed(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            foreach (Control control in savedControls)
            {
                Controls.Add(control);
            }
            savedControls.Clear();
            base.OnHandleCreated(e);
        }

        //protected override void OnActivated(EventArgs e)
        //{
        //    drawingWindow.setEnabled(true);
        //    Console.WriteLine(this.Text + " activated " + this.Pane.ActiveContent);
        //    base.OnActivated(e);
        //}

        //protected override void OnDeactivate(EventArgs e)
        //{
        //    drawingWindow.setEnabled(false);
        //    Console.WriteLine(this.Text + " deactivated " + this.Pane.ActiveContent);
        //    base.OnDeactivate(e);
        //}

        //protected override void OnDockStateChanged(EventArgs e)
        //{
        //    Console.WriteLine(this.Text + " dock changed");
        //    base.OnDockStateChanged(e);
        //}
    }
}

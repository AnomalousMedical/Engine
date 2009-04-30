using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Windows.Forms;

namespace Test
{
    /// <summary>
    /// This is an OSWindow subclass for Windows.Forms windows.
    /// </summary>
    public class WindowsFormsWindow : OSWindow
    {
        private Control control;
        private List<OSWindowListener> listeners = new List<OSWindowListener>(2);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="control">The control to represent.</param>
        public WindowsFormsWindow(Control control)
        {
            this.control = control;
            control.Move += new EventHandler(control_Move);
            control.Resize += new EventHandler(control_Resize);
            control.HandleDestroyed += new EventHandler(control_HandleDestroyed);
        }

        void control_HandleDestroyed(object sender, EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.closing(this);
            }
        }

        /// <summary>
        /// Returns the handle of the control.
        /// </summary>
        public IntPtr WindowHandle
        {
            get { return control.Handle; }
        }

        /// <summary>
        /// Returns the height of the control.
        /// </summary>
        public int WindowHeight
        {
            get { return control.Height; }
        }

        /// <summary>
        /// Returns the width of the control.
        /// </summary>
        public int WindowWidth
        {
            get { return control.Width; }
        }

        public void addListener(OSWindowListener listener)
        {
            listeners.Add(listener);
        }

        public void removeListener(OSWindowListener listener)
        {
            listeners.Remove(listener);
        }

        /// <summary>
        /// Callback for resize event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Resize(object sender, EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.resized(this);
            }
        }

        /// <summary>
        /// Callback for move event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Move(object sender, EventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.moved(this);
            }
        }

        /// <summary>
        /// Callback for closing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (OSWindowListener listener in listeners)
            {
                listener.closing(this);
            }
        }
    }
}

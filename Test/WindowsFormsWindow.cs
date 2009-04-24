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

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="control">The control to represent.</param>
        public WindowsFormsWindow(Control control)
        {
            this.control = control;
            control.Move += new EventHandler(control_Move);
            control.Resize += new EventHandler(control_Resize);
            Form form = control.FindForm();
            form.Shown += new EventHandler(form_Shown);
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
        }

        /// <summary>
        /// Returns the handle of the control.
        /// </summary>
        public override IntPtr Handle
        {
            get { return control.Handle; }
        }

        /// <summary>
        /// Returns the height of the control.
        /// </summary>
        public override int Height
        {
            get { return control.Height; }
        }

        /// <summary>
        /// Returns the width of the control.
        /// </summary>
        public override int Width
        {
            get { return control.Width; }
        }

        /// <summary>
        /// Callback for resize event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Resize(object sender, EventArgs e)
        {
            resized();
        }

        /// <summary>
        /// Callback for move event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void control_Move(object sender, EventArgs e)
        {
            moved();
        }

        /// <summary>
        /// Callback for closing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing();
        }

        /// <summary>
        /// Callback for showing event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void form_Shown(object sender, EventArgs e)
        {
            shown();
        }
    }
}

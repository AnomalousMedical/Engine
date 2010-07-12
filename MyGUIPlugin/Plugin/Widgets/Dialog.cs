using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyGUIPlugin
{
    public class Dialog : IDisposable
    {
        private Layout dialogLayout;
        protected Window window;
        private bool modal = false;

        /// <summary>
        /// Called when the dialog is closing, but is still open.
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Called when the dialog is closed.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Constructor. Takes the layout file to load.
        /// </summary>
        /// <param name="layoutFile">The layout file of the dialog.</param>
        public Dialog(String layoutFile)
        {
            dialogLayout = LayoutManager.Instance.loadLayout(layoutFile);
            window = dialogLayout.getWidget(0) as Window;
            window.Visible = false;
            window.WindowButtonPressed += new MyGUIEvent(window_WindowButtonPressed);
            SmoothShow = true;
        }

        /// <summary>
        /// Dispose method can be overwritten, but be sure to call base.Dispose();
        /// </summary>
        public virtual void Dispose()
        {
            LayoutManager.Instance.unloadLayout(dialogLayout);
        }

        /// <summary>
        /// Open the dialog with the given modal setting.
        /// </summary>
        /// <param name="modal">True to be modal. False for non modal.</param>
        public void open(bool modal)
        {
            if (Visible == false)
            {
                this.modal = modal;
                Visible = true;
            }
        }

        /// <summary>
        /// Close the dialog.
        /// </summary>
        public void close()
        {
            if (Visible == true)
            {
                Visible = false;
            }
        }

        /// <summary>
        /// True if the window is shown, false otherwise. Setting to true will
        /// show the window with the current properties.
        /// </summary>
        public bool Visible
        {
            get
            {
                return window.Visible;
            }
            set
            {
                if (window.Visible != value)
                {
                    if (modal)
                    {
                        if (value)
                        {
                            InputManager.Instance.addWidgetModal(window);
                        }
                        else
                        {
                            InputManager.Instance.removeWidgetModal(window);
                        }
                    }
                    if (SmoothShow)
                    {
                        window.setVisibleSmooth(value);
                    }
                    else
                    {
                        window.Visible = value;
                    }
                }
            }
        }

        /// <summary>
        /// True to make the dialog modal. Cannot be changed when the window is
        /// visible.
        /// </summary>
        public bool Modal
        {
            get
            {
                return modal;
            }
            set
            {
                if (window.Visible)
                {
                    throw new Exception("Cannot change window modal status when it is shown.");
                }
                this.modal = value;
            }
        }

        /// <summary>
        /// True to show the window smoothly.
        /// </summary>
        public bool SmoothShow { get; set; }

        void window_WindowButtonPressed(Widget source, EventArgs e)
        {
            if (Closing != null)
            {
                Closing.Invoke(this, EventArgs.Empty);
            }
            close();
            if (Closed != null)
            {
                Closed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

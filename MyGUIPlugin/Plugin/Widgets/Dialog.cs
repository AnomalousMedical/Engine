using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Engine;

namespace MyGUIPlugin
{
    public class Dialog : IDisposable
    {
        private Layout dialogLayout;
        protected Window window;
        private bool modal = false;
        private Rect desiredLocation;
        String persistName;

        /// <summary>
        /// Called after the dialog opens.
        /// </summary>
        public event EventHandler Shown;

        /// <summary>
        /// Called when the dialog is closing, but is still open.
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Called when the dialog is closed.
        /// </summary>
        public event EventHandler Closed;

        public Dialog(String layoutFile)
            :this(layoutFile, "")
        {
            Type t = GetType();
            persistName = String.Format("{0}.{1}", t.Namespace, t.Name);
        }

        /// <summary>
        /// Constructor. Takes the layout file to load.
        /// </summary>
        /// <param name="layoutFile">The layout file of the dialog.</param>
        public Dialog(String layoutFile, String persistName)
        {
            this.persistName = persistName;
            dialogLayout = LayoutManager.Instance.loadLayout(layoutFile);
            window = dialogLayout.getWidget(0) as Window;
            window.Visible = false;
            window.WindowButtonPressed += new MyGUIEvent(window_WindowButtonPressed);
            SmoothShow = true;
            IgnorePositionChanges = false;
            desiredLocation = new Rect(window.Left, window.Top, window.Width, window.Height);
            window.WindowChangedCoord += new MyGUIEvent(window_WindowChangedCoord);
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
                    if (value)
                    {
                        //Adjust the position if needed
                        int left = window.AbsoluteLeft;
                        int top = window.AbsoluteTop;
                        int right = left + window.Width;
                        int bottom = top + window.Height;

                        int guiWidth = Gui.Instance.getViewWidth();
                        int guiHeight = Gui.Instance.getViewHeight();

                        if (right > guiWidth)
                        {
                            left -= right - guiWidth;
                            if (left < 0)
                            {
                                left = 0;
                            }
                        }

                        if (bottom > guiHeight)
                        {
                            top -= bottom - guiHeight;
                            if (top < 0)
                            {
                                top = 0;
                            }
                        }

                        window.setPosition(left, top);

                        doChangeVisibility(value);
                        onShown(EventArgs.Empty);
                    }
                    else
                    {
                        CancelEventArgs cancelEvent = new CancelEventArgs();
                        onClosing(cancelEvent);
                        if (!cancelEvent.Cancel)
                        {
                            doChangeVisibility(value);
                            onClosed(EventArgs.Empty);
                        }
                    }
                }
            }
        }

        public void serialize(ConfigFile configFile)
        {
            ConfigSection section = configFile.createOrRetrieveConfigSection(persistName);
            section.setValue("Location", desiredLocation.ToString());
        }

        public void deserialize(ConfigFile configFile)
        {
            ConfigSection section = configFile.createOrRetrieveConfigSection(persistName);
            String location = section.getValue("Location", desiredLocation.ToString());
            desiredLocation.fromString(location);
            window.setCoord((int)desiredLocation.Left, (int)desiredLocation.Top, (int)desiredLocation.Width, (int)desiredLocation.Height);
        }

        /// <summary>
        /// Acutally change the window visibility, called from Visible.set
        /// </summary>
        /// <param name="value"></param>
        private void doChangeVisibility(bool value)
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

        public Rect DesiredLocation
        {
            get
            {
                return desiredLocation;
            }
            set
            {
                desiredLocation = value;
            }
        }

        public bool IgnorePositionChanges { get; set; }

        public Vector2 Position
        {
            get
            {
                return new Vector2(window.Left, window.Top);
            }
            set
            {
                window.setPosition((int)value.x, (int)value.y);
            }
        }

        protected virtual void onShown(EventArgs args)
        {
            if (Shown != null)
            {
                Shown.Invoke(this, args);
            }
        }

        protected virtual void onClosing(CancelEventArgs args)
        {
            if (Closing != null)
            {
                Closing.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void onClosed(EventArgs args)
        {
            if (Closed != null)
            {
                Closed.Invoke(this, EventArgs.Empty);
            }
        }

        void window_WindowButtonPressed(Widget source, EventArgs e)
        {
            Visible = false;
        }

        void window_WindowChangedCoord(Widget source, EventArgs e)
        {
            if (!IgnorePositionChanges)
            {
                desiredLocation.Left = window.Left;
                desiredLocation.Top = window.Top;
                desiredLocation.Width = window.Width;
                desiredLocation.Height = window.Height;
            }
        }
    }
}

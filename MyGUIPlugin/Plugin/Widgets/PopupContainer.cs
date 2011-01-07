using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using Engine;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class assists in using any widget as a popup that will
    /// automatically close when it is no longer clicked.
    /// </summary>
    public class PopupContainer : IDisposable
    {
        private Layout layout;
        protected Widget widget;
        private float smoothShowPosition;
        private bool runningShowTransition; //True to be making the popup visible, false to be hiding.
        private List<Widget> childPopups = null;

        /// <summary>
        /// This event is called after the popup has been hidden completely.
        /// </summary>
        public event EventHandler Hidden;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="layoutFile"></param>
        public PopupContainer(String layoutFile)
        {
            layout = LayoutManager.Instance.loadLayout(layoutFile);
            initialize(layout.getWidget(0));
        }

        public PopupContainer(Widget widget)
        {
            initialize(widget);
        }

        private void initialize(Widget widget)
        {
            this.widget = widget;
            widget.Visible = false;
            SmoothShow = true;
        }

        public virtual void Dispose()
        {
            if (layout != null)
            {
                LayoutManager.Instance.unloadLayout(layout);
            }
        }

        /// <summary>
        /// Add a child popup. This will prevent this popup from being closed if
        /// the mouse is clicked inside of the child popup.
        /// </summary>
        /// <param name="child"></param>
        public void addChildPopup(Widget child)
        {
            if (childPopups == null)
            {
                childPopups = new List<Widget>();
            }
            childPopups.Add(child);
        }

        public void removeChildPopup(Widget child)
        {
            childPopups.Remove(child);
        }

        public void show(int left, int top)
        {
            if (!Visible)
            {
                LayerManager.Instance.upLayerItem(widget);
                int guiWidth = Gui.Instance.getViewWidth();
                int guiHeight = Gui.Instance.getViewHeight();

                int right = left + widget.Width;
                int bottom = top + widget.Height;

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

                widget.setPosition(left, top);
                Visible = true;
                if (SmoothShow)
                {
                    widget.Alpha = 0.0f;
                    smoothShowPosition = 0.0f;
                    subscribeToUpdate();
                    runningShowTransition = true;
                }
            }
        }

        public void hide()
        {
            if (Visible)
            {
                Visible = false;
                if (SmoothShow)
                {
                    smoothShowPosition = 0.0f;
                    subscribeToUpdate();
                    runningShowTransition = false;
                    widget.Visible = true;
                }
                else
                {
                    if (Hidden != null)
                    {
                        Hidden.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool Visible
        {
            get
            {
                return widget.Visible;
            }
            private set
            {
                if (widget.Visible != value)
                {
                    widget.Visible = value;
                    if (value)
                    {
                        Gui.Instance.MouseButtonPressed += MouseButtonPressed;
                    }
                    else
                    {
                        Gui.Instance.MouseButtonPressed -= MouseButtonPressed;
                        if (!SmoothShow) //Unsubscribe if not smooth showing.
                        {
                            unsubscribeFromUpdate();
                        }
                    }
                }
            }
        }

        public int Width
        {
            get
            {
                return widget.Width;
            }
        }

        public int Height
        {
            get
            {
                return widget.Height;
            }
        }

        void MouseButtonPressed(int x, int y, MouseButtonCode button)
        {
            int left = widget.AbsoluteLeft;
            int top = widget.AbsoluteTop;
            int right = left + widget.Width;
            int bottom = top + widget.Height;
            if (x < left || x > right || y < top || y > bottom)
            {
                if (childPopups != null)
                {
                    foreach (Widget childWidget in childPopups)
                    {
                        if (childWidget.Visible)
                        {
                            left = childWidget.AbsoluteLeft;
                            top = childWidget.AbsoluteTop;
                            right = left + childWidget.Width;
                            bottom = top + childWidget.Height;
                            if (x > left && x < right && y > top && y < bottom)
                            {
                                //inside of child. return.
                                return;
                            }
                        }
                    }
                }
                hide();
            }
        }

        public Object UserObject { get; set; }

        public bool SmoothShow { get; set; }

        private bool subscribedToUpdate = false;

        private void subscribeToUpdate()
        {
            if (!subscribedToUpdate)
            {
                subscribedToUpdate = true;
                Gui.Instance.Update += update;
            }
        }

        private void unsubscribeFromUpdate()
        {
            if (subscribedToUpdate)
            {
                subscribedToUpdate = false;
                Gui.Instance.Update -= update;
            }
        }

        void update(float updateTime)
        {
            smoothShowPosition += updateTime;
            if (runningShowTransition)
            {
                if (smoothShowPosition > MyGUIInterface.SmoothShowDuration)
                {
                    smoothShowPosition = MyGUIInterface.SmoothShowDuration;
                    unsubscribeFromUpdate();
                }
                widget.Alpha = smoothShowPosition / MyGUIInterface.SmoothShowDuration;
            }
            else
            {
                if (smoothShowPosition > MyGUIInterface.SmoothShowDuration)
                {
                    smoothShowPosition = MyGUIInterface.SmoothShowDuration;
                    unsubscribeFromUpdate();
                    widget.Visible = false;
                    widget.Alpha = 0.0f;
                    if (Hidden != null)
                    {
                        Hidden.Invoke(this, EventArgs.Empty);
                    }
                }
                else
                {
                    widget.Alpha = 1 - smoothShowPosition / MyGUIInterface.SmoothShowDuration;
                }
            }
        }
    }
}

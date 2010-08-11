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
    public class PopupContainer
    {
        private Widget widget;
        private float smoothShowPosition;
        private bool runningShowTransition; //True to be making the popup visible, false to be hiding.

        /// <summary>
        /// Empty constructor for subclassing this class. Must call initialize
        /// with the main widget in the subclass constructor.
        /// </summary>
        protected PopupContainer()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="widget">The widget to pop up.</param>
        public PopupContainer(Widget widget)
        {
            initialize(widget);
        }

        protected void initialize(Widget widget)
        {
            this.widget = widget;
            SmoothShow = true;
            SmoothShowDuration = 0.5f;
        }

        public void show(int left, int top)
        {
            LayerManager.Instance.upLayerItem(widget);
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

        public void hide()
        {
            Visible = false;
            if (SmoothShow)
            {
                smoothShowPosition = 0.0f;
                subscribeToUpdate();
                runningShowTransition = false;
                widget.Visible = true;
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

        void MouseButtonPressed(int x, int y, MouseButtonCode button)
        {
            int left = widget.getAbsoluteLeft();
            int top = widget.getAbsoluteTop();
            int right = left + widget.getWidth();
            int bottom = top + widget.getHeight();
            if (x < left || x > right || y < top || y > bottom)
            {
                hide();
            }
        }

        public Widget Widget
        {
            get
            {
                return widget;
            }
        }

        public Object UserObject { get; set; }

        public bool SmoothShow { get; set; }

        public float SmoothShowDuration { get; set; }

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
                if (smoothShowPosition > SmoothShowDuration)
                {
                    smoothShowPosition = SmoothShowDuration;
                    unsubscribeFromUpdate();
                }
                widget.Alpha = smoothShowPosition / SmoothShowDuration;
            }
            else
            {
                if (smoothShowPosition > SmoothShowDuration)
                {
                    smoothShowPosition = SmoothShowDuration;
                    unsubscribeFromUpdate();
                    widget.Visible = false;
                }
                widget.Alpha = 1 - smoothShowPosition / SmoothShowDuration;
            }
        }
    }
}

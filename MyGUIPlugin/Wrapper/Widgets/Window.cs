using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Window : Widget
    {
        private Widget captionWidget = null;

        internal Window(IntPtr window)
            :base(window)
        {

        }

        public override void Dispose()
        {
            //Not sure why this has to be deleted separatly, but it does
            if (captionWidget != null && !Gui.Instance.Disposing)
            {
                WidgetManager.deleteWrapper(captionWidget.WidgetPtr);
                captionWidget = null;
            }
            base.Dispose();
        }

        public void setVisibleSmooth(bool value)
        {
            Window_setVisibleSmooth(widget, value);
        }

        public void destroySmooth()
        {
            Window_destroySmooth(widget);
        }

        public void setActionWidgetsEnabled(bool value)
        {
            Window_setActionWidgetsEnabled(widget, value);
        }

        public bool AutoAlpha
        {
            get
            {
                return Window_getAutoAlpha(widget);
            }
            set
            {
                Window_setAutoAlpha(widget, value);
            }
        }

        public Widget CaptionWidget
        {
            get
            {
                if (captionWidget == null)
                {
                    captionWidget = WidgetManager.getWidget(Window_getCaptionWidget(widget));
                }
                return captionWidget;
            }
        }

        public Size2 MinSize
        {
            get
            {
                return Window_getMinSize(widget).toSize();
            }
            set
            {
                Window_setMinSize(widget, value);
            }
        }

        public Size2 MaxSize
        {
            get
            {
                return Window_getMaxSize(widget).toSize();
            }
            set
            {
                Window_setMaxSize(widget, value);
            }
        }

        public bool Snap
        {
            get
            {
                return Window_getSnap(widget);
            }
            set
            {
                Window_setSnap(widget, value);
            }
        }

        public event MyGUIEvent WindowButtonPressed
        {
            add
            {
                eventManager.addDelegate<EventWindowButtonPressedTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventWindowButtonPressedTranslator>(value);
            }
        }

        public event MyGUIEvent WindowChangedCoord
        {
            add
            {
                eventManager.addDelegate<EventWindowChangedCoordTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventWindowChangedCoordTranslator>(value);
            }
        }

#region PInvoke
        [DllImport("MyGUIWrapper")]
        private static extern void Window_setVisibleSmooth(IntPtr window, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern void Window_destroySmooth(IntPtr window);

        [DllImport("MyGUIWrapper")]
        private static extern void Window_setAutoAlpha(IntPtr window, bool value);
        
        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_getAutoAlpha(IntPtr window);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Window_getCaptionWidget(IntPtr window);

        [DllImport("MyGUIWrapper")]
        private static extern void Window_setMinSize(IntPtr window, Size2 value);

        [DllImport("MyGUIWrapper")]
        private static extern ThreeIntHack Window_getMinSize(IntPtr window);

        [DllImport("MyGUIWrapper")]
        private static extern void Window_setMaxSize(IntPtr window, Size2 value);

        [DllImport("MyGUIWrapper")]
        private static extern ThreeIntHack Window_getMaxSize(IntPtr window);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_getSnap(IntPtr window);

        [DllImport("MyGUIWrapper")]
        private static extern void Window_setSnap(IntPtr window, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern void Window_setActionWidgetsEnabled(IntPtr window, bool value);
#endregion

    }
}

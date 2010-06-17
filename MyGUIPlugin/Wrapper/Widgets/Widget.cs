using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Widget : IDisposable
    {
        protected IntPtr widget;
        private MyGUIEventManager eventManager;

        internal Widget(IntPtr widget)
        {
            this.widget = widget;
            eventManager = new MyGUIEventManager(this);
        }

        public void Dispose()
        {
            eventManager.Dispose();
            widget = IntPtr.Zero;
        }

        public int getAbsoluteLeft()
        {
            return Widget_getAbsoluteLeft(widget);
        }

        public int getAbsoluteTop()
        {
            return Widget_getAbsoluteTop(widget);
        }

        public int getLeft()
        {
            return Widget_getLeft(widget);
        }

        public int getRight()
        {
            return Widget_getRight(widget);
        }

        public int getTop()
        {
            return Widget_getTop(widget);
        }

        public int getBottom()
        {
            return Widget_getBottom(widget);
        }

        public int getWidth()
        {
            return Widget_getWidth(widget);
        }

        public int getHeight()
        {
            return Widget_getHeight(widget);
        }

        public void setPosition(int left, int top)
        {
            Widget_setPosition(widget, left, top);
        }

        public void setSize(int width, int height)
        {
            Widget_setSize(widget, width, height);
        }

        public void setCoord(int left, int top, int width, int height)
        {
            Widget_setCoord(widget, left, top, width, height);
        }

        public void setRealPosition(float left, float top)
        {
            Widget_setRealPosition(widget, left, top);
        }

        public void setRealSize(float width, float height)
        {
            Widget_setRealSize(widget, width, height);
        }

        public void setRealCoord(float left, float top, float width, float height)
        {
            Widget_setRealCoord(widget, left, top, width, height);
        }

        public uint getChildCount(IntPtr widget)
        {
            return Widget_getChildCount(widget).ToUInt32();
        }

        public Widget getChildAt(IntPtr widget, uint index)
        {
            return WidgetManager.getWidget(Widget_getChildAt(widget, new UIntPtr(index)));
        }

#region Internal Management

        internal void eraseAllChildren()
        {
            recursiveEraseChildren(widget);
        }

        internal static void recursiveEraseChildren(IntPtr parentWidget)
        {
            uint numChildren = Widget_getChildCount(parentWidget).ToUInt32();
            for (uint i = 0; i < numChildren; i++)
            {
                recursiveEraseChildren(Widget_getChildAt(parentWidget, new UIntPtr(i)));
            }
            WidgetManager.deleteWrapper(parentWidget);
        }

        internal IntPtr WidgetPtr
        {
            get
            {
                return widget;
            }
        }

#endregion

#region Events

        public event MyGUIEvent MouseButtonClick
        {
            add
            {
                eventManager.addDelegate<ClickEventTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<ClickEventTranslator>(value);
            }
        }

#endregion

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getAbsoluteLeft(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getAbsoluteTop(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getLeft(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getRight(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getTop(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getBottom(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getWidth(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern int Widget_getHeight(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setPosition(IntPtr widget, int left, int top);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setSize(IntPtr widget, int width, int height);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setCoord(IntPtr widget, int left, int top, int width, int height);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setRealPosition(IntPtr widget, float left, float top);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setRealSize(IntPtr widget, float width, float height);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setRealCoord(IntPtr widget, float left, float top, float width, float height);

        [DllImport("MyGUIWrapper")]
        private static extern UIntPtr Widget_getChildCount(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getChildAt(IntPtr widget, UIntPtr index);

#endregion
    }
}

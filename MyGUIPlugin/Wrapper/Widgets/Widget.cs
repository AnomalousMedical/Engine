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
        private IntPtr widget;
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
        private static extern UIntPtr Widget_getChildCount(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getChildAt(IntPtr widget, UIntPtr index);

#endregion
    }
}

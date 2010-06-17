using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class handles the creation and destruction of widget wrapper objects.
    /// </summary>
    class WidgetManager
    {
        private static WrapperCollection<Widget> widgets = new WrapperCollection<Widget>(createWrapper);

        internal static Widget getWidget(IntPtr widget)
        {
            if(widget != IntPtr.Zero)
            {
                return widgets.getObject(widget);
            }
            return null;
        }

        internal static IntPtr deleteWrapper(IntPtr widget)
        {
            return widgets.destroyObject(widget);
        }

        /// <summary>
        /// This function will erase a wrapper and all child wrappers to avoid memory leaks.
        /// </summary>
        /// <param name="window">The window to destroy.</param>
        /// <returns>The pointer of the window that was destroyed.</returns>
        internal static IntPtr deleteWrapperAndChildren(Widget widget)
        {
            widget.eraseAllChildren();
            return deleteWrapper(widget.WidgetPtr);
        }

        internal static void destroyAllWrappers()
        {
            widgets.clearObjects();
        }

        private static Widget createWrapper(IntPtr widget, object[] args)
        {
            return new Widget(widget);
        }
    }
}

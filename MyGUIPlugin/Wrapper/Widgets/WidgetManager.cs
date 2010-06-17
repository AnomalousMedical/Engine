using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class handles the creation and destruction of widget wrapper objects.
    /// </summary>
    class WidgetManager
    {
        enum WidgetType
        {
            Widget,
            Button,
            Canvas,
            ComboBox,
            DDContainer,
            Edit,
            HScroll,
            ItemBox,
            List,
            MenuCtrl,
            Message,
            MultiList,
            PopupMenu,
            Progress,
            RenderBox,
            ScrollView,
            StaticImage,
            StaticText,
            Tab,
            VScroll,
            Window,
        }

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
            switch(WidgetManager_getType(widget))
            {
                default:
                    return new Widget(widget);
            }
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern WidgetType WidgetManager_getType(IntPtr widget);

#endregion
    }
}

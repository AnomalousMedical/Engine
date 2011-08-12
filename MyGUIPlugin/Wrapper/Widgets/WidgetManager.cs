using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Logging;

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
                Canvas,
                DDContainer,
                    ItemBox,
                    ListCtrl,
                        ListBox,
                List,
                MenuCtrl,
                    MenuBar,
                    PopupMenu,
                MultiList,
                Progress,
                ScrollView,
                StaticImage,
                StaticText,
                    Button,
                        MenuItem,
                    Edit,
                        ComboBox,
                Tab,
                TabItem,
                VScroll,
                    HScroll,
                Window,
                    Message,
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
#if VERBOSE_WIDGET_WRAPPER_CREATION
            Log.ImportantInfo("Deleting widget wrapper. Ptr {0} type {1}", widget.ToString(), WidgetManager_getType(widget));
#endif
            return widgets.destroyObject(widget);
        }

        /// <summary>
        /// This function will erase a wrapper and all child wrappers to avoid memory leaks.
        /// </summary>
        /// <param name="window">The window to destroy.</param>
        /// <returns>The pointer of the window that was destroyed.</returns>
        internal static IntPtr deleteWrapperAndChildren(Widget widget)
        {
            IntPtr widgetPtr = widget.WidgetPtr;
            widget.eraseAllChildren();
            return widgetPtr;
        }

        internal static void destroyAllWrappers()
        {
#if TRACK_WIDGET_MEMORY_LEAKS
            widgets.printObjects("Widget left before clear {0}");
#endif
            widgets.clearObjects();
        }

        private static Widget createWrapper(IntPtr widget, object[] args)
        {
            WidgetType widgetType = WidgetManager_getType(widget);
#if VERBOSE_WIDGET_WRAPPER_CREATION
            Log.ImportantInfo("Creating widget wrapper. Ptr {0} type {1}", widget.ToString(), widgetType);
#endif
            switch (widgetType)
            {
                case WidgetType.Widget:
                    return new Widget(widget);

                case WidgetType.Canvas:
                    return new Widget(widget);

                case WidgetType.DDContainer:
                    return new Widget(widget);

                case WidgetType.ItemBox:
                    return new Widget(widget);

                case WidgetType.ListCtrl:
                    return new Widget(widget);

                case WidgetType.ListBox:
                    return new Widget(widget);

                case WidgetType.List:
                    return new Widget(widget);

                case WidgetType.MenuCtrl:
                    return new MenuCtrl(widget);

                case WidgetType.MenuBar:
                    return new MenuBar(widget);

                case WidgetType.PopupMenu:
                    return new PopupMenu(widget);

                case WidgetType.MultiList:
                    return new MultiList(widget);

                case WidgetType.Progress:
                    return new Progress(widget);

                case WidgetType.ScrollView:
                    return new ScrollView(widget);

                case WidgetType.StaticImage:
                    return new StaticImage(widget);

                case WidgetType.StaticText:
                    return new StaticText(widget);

                case WidgetType.Button:
                    return new Button(widget);

                case WidgetType.MenuItem:
                    return new MenuItem(widget);

                case WidgetType.Edit:
                    return new Edit(widget);

                case WidgetType.ComboBox:
                    return new ComboBox(widget);

                case WidgetType.Tab:
                    return new Widget(widget);

                case WidgetType.TabItem:
                    return new Widget(widget);

                case WidgetType.VScroll:
                    return new VScroll(widget);

                case WidgetType.HScroll:
                    return new HScroll(widget);

                case WidgetType.Window:
                    return new Window(widget);

                case WidgetType.Message:
                    return new Message(widget);
            }
            Log.Warning("Could not identify widget type for widget {0}. Type given was {1}. Will return a Widget in its place.", widget.ToString(), widgetType);
            return new Widget(widget);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern WidgetType WidgetManager_getType(IntPtr widget);

#endregion
    }
}

/*
 case WidgetType.Button:
                    return new Button(widget);
                default:
                    return new Widget(widget);
 */

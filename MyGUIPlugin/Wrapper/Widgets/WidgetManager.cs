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
            Widget = 0,
                Canvas = 1,
                DDContainer = 2,
                    ItemBox = 3,
                ListBox = 4,
                MenuControl = 5,
                    MenuBar = 6,
                    PopupMenu = 7,
                MultiListBox = 8,
                ProgressBar = 9,
                ScrollView = 10,
                ImageBox = 11,
                TextBox = 12,
                    Button = 13,
                        MenuItem = 14,
                    EditBox = 15,
                        ComboBox = 16,
			        Window = 17,
			        TabItem = 18,
                TabControl = 19,
                ScrollBar = 20,
        };

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void WidgetDestructorCallback(IntPtr widget);
        private static WidgetDestructorCallback widgetDestructorFunc = widgetDestructor; //This is static and does not need a special AOT version.

        private static WrapperCollection<Widget> widgets = new WrapperCollection<Widget>(createWrapper);

        internal static Widget getWidget(IntPtr widget)
        {
            if (widget != IntPtr.Zero)
            {
                Widget returnedWidget = widgets.getObject(widget);
#if TRACK_WIDGET_MEMORY_LEAKS
                if (!paranoidCheckWidget(returnedWidget, widget))
                {
                    Widget oldWidget = returnedWidget;
                    widgets.destroyObject(returnedWidget.WidgetPtr);
                    returnedWidget = widgets.getObject(widget);
                    String messageBoxMessage = String.Format("Had to rewrap widget {0}. It must have leaked. It was a {1} now it is a {2}.\nPlease report this to Andrew.\nAllocation stack trace for old widget printed to log.", widget, oldWidget.GetType().FullName, returnedWidget.GetType().FullName);
                    Logging.Log.ImportantInfo("Had to rewrap widget {0}. It must have leaked. It was a {1} now it is a {2}\nAllocationStack:\n{3}\n.", widget, oldWidget.GetType().FullName, returnedWidget.GetType().FullName, oldWidget);
                    MessageBox.show(messageBoxMessage, "WidgetManager isn't paranoid if they really are out to get it.", MessageBoxStyle.Ok | MessageBoxStyle.IconWarning);
                }
#endif
                return returnedWidget;
            }
            return null;
        }

        /// <summary>
        /// This is called by the c++ destructor for the widget. It will erase the wrapper object.
        /// </summary>
        /// <param name="widget"></param>
        [Anomalous.Interop.MonoPInvokeCallback(typeof(WidgetDestructorCallback))]
        static void widgetDestructor(IntPtr widget)
        {
#if VERBOSE_WIDGET_WRAPPER_CREATION
            Log.ImportantInfo("Deleting widget wrapper. Ptr {0} type {1}", widget.ToString(), WidgetManager_getType(widget));
#endif
            widgets.destroyObject(widget);
        }

        public static void destroyAllWrappers()
        {
#if TRACK_WIDGET_MEMORY_LEAKS
            widgets.printObjects("Widget left before clear {0}");
#endif
            widgets.clearObjects();
        }

        private static Widget createWrapper(IntPtr widget, object[] args)
        {
            WidgetType widgetType = WidgetManager_getType(widget);
            Widget_setDestructorCallback(widget, widgetDestructorFunc);
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

                case WidgetType.ListBox:
                    return new Widget(widget);

                case WidgetType.MenuControl:
                    return new MenuControl(widget);

                case WidgetType.MenuBar:
                    return new MenuBar(widget);

                case WidgetType.PopupMenu:
                    return new PopupMenu(widget);

                case WidgetType.MultiListBox:
                    return new MultiListBox(widget);

                case WidgetType.ProgressBar:
                    return new ProgressBar(widget);

                case WidgetType.ScrollView:
                    return new ScrollView(widget);

                case WidgetType.ImageBox:
                    return new ImageBox(widget);

                case WidgetType.TextBox:
                    return new TextBox(widget);

                case WidgetType.Button:
                    return new Button(widget);

                case WidgetType.MenuItem:
                    return new MenuItem(widget);

                case WidgetType.EditBox:
                    return new EditBox(widget);

                case WidgetType.ComboBox:
                    return new ComboBox(widget);

                case WidgetType.Window:
                    return new Window(widget);

                case WidgetType.TabItem:
                    return new TabItem(widget);

                case WidgetType.TabControl:
                    return new TabControl(widget);

                case WidgetType.ScrollBar:
                    return new ScrollBar(widget);
            }
            Log.Warning("Could not identify widget type for widget {0}. Type given was {1}. Will return a Widget in its place.", widget.ToString(), widgetType);
            return new Widget(widget);
        }

        private static bool paranoidCheckWidget(Widget wrapperReturnedWidget, IntPtr rawWidgetPointer)
        {
            WidgetType widgetType = WidgetManager_getType(rawWidgetPointer);
#if VERBOSE_WIDGET_WRAPPER_CREATION
            Log.ImportantInfo("Creating widget wrapper. Ptr {0} type {1}", rawWidgetPointer.ToString(), widgetType);
#endif
            switch (widgetType)
            {
                case WidgetType.Widget:
                    return wrapperReturnedWidget is Widget;

                case WidgetType.Canvas:
                    return wrapperReturnedWidget is Widget;

                case WidgetType.DDContainer:
                    return wrapperReturnedWidget is Widget;

                case WidgetType.ItemBox:
                    return wrapperReturnedWidget is Widget;

                case WidgetType.ListBox:
                    return wrapperReturnedWidget is Widget;

                case WidgetType.MenuControl:
                    return wrapperReturnedWidget is MenuControl;

                case WidgetType.MenuBar:
                    return wrapperReturnedWidget is MenuBar;

                case WidgetType.PopupMenu:
                    return wrapperReturnedWidget is PopupMenu;

                case WidgetType.MultiListBox:
                    return wrapperReturnedWidget is MultiListBox;

                case WidgetType.ProgressBar:
                    return wrapperReturnedWidget is ProgressBar;

                case WidgetType.ScrollView:
                    return wrapperReturnedWidget is ScrollView;

                case WidgetType.ImageBox:
                    return wrapperReturnedWidget is ImageBox;

                case WidgetType.TextBox:
                    return wrapperReturnedWidget is TextBox;

                case WidgetType.Button:
                    return wrapperReturnedWidget is Button;

                case WidgetType.MenuItem:
                    return wrapperReturnedWidget is MenuItem;

                case WidgetType.EditBox:
                    return wrapperReturnedWidget is EditBox;

                case WidgetType.ComboBox:
                    return wrapperReturnedWidget is ComboBox;

                case WidgetType.Window:
                    return wrapperReturnedWidget is Window;

                case WidgetType.TabItem:
                    return wrapperReturnedWidget is TabItem;

                case WidgetType.TabControl:
                    return wrapperReturnedWidget is Widget;

                case WidgetType.ScrollBar:
                    return wrapperReturnedWidget is ScrollBar;
            }
            Log.Warning("Could not identify widget type for widget {0}. Type given was {1}. Will return a Widget in its place.", rawWidgetPointer.ToString(), widgetType);
            return wrapperReturnedWidget is Widget;
        }

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern WidgetType WidgetManager_getType(IntPtr widget);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Widget_setDestructorCallback(IntPtr widget, WidgetDestructorCallback widgetDestructorCallback);

        #endregion
    }
}

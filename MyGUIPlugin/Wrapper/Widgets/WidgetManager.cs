using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Logging;
using System.Diagnostics;

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
        private static Dictionary<IntPtr, StackTrace> allocationStackTraces = new Dictionary<IntPtr, StackTrace>();

        internal static Widget getWidget(IntPtr widget)
        {
            if (widget != IntPtr.Zero)
            {
                return widgets.getObject(widget);
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
            if (MyGUIInterface.TrackMemoryLeaks)
            {
                allocationStackTraces.Remove(widget);
            }
            widgets.destroyObject(widget);
        }

        public static void destroyAllWrappers()
        {
            String filename = "Unknown";
            if (MyGUIInterface.TrackMemoryLeaks)
            {
                StackTrace st = new StackTrace(true);
                filename = st.GetFrame(0).GetFileName();
                if (widgets.WrappedObjectCount > 0)
                {
                    Log.ImportantInfo("{0} memory leaks detected in the WidgetManager.", widgets.WrappedObjectCount);
                }
                else
                {
                    Log.ImportantInfo("No memory leaks detected in the WidgetManager.");
                }
            }
            else if (widgets.WrappedObjectCount > 0)
            {
                Log.ImportantInfo("{0} memory leaks detected in the WidgetManager. Enable MyGUIInterface.TrackMemoryLeaks to see stack traces for allocation.", widgets.WrappedObjectCount);
            }

            foreach (var wrapper in widgets.WrappedObjects)
            {
                Log.Error("Memory leak detected in {0}.  Double check to make sure all Widgets of this type are.", filename);
                if (MyGUIInterface.TrackMemoryLeaks)
                {
                    Log.Error("Allocation Stack Track");
                    StackTrace st;
                    if (allocationStackTraces.TryGetValue(wrapper.WidgetPtr, out st))
                    {
                        foreach (StackFrame f in st.GetFrames())
                        {
                            if (f.GetFileName() != null)
                            {
                                Log.Error("-\t{0} in file {1}:{2}:{3}", f.GetMethod(), f.GetFileName(), f.GetFileLineNumber(), f.GetFileColumnNumber());
                            }
                        }
                    }
                    else
                    {
                        Log.Error("Widget leaked with no stack trace info available. Please enable MyGUIInterface.TrackMemoryLeaks to view this info. Make sure to enable it as early as possible.");
                    }
                }
                else
                {
                    Log.Error("No stack trace info available. Please enable MyGUIInterface.TrackMemoryLeaks to view this info.");
                }
            }
            widgets.clearObjects();
        }

        private static Widget createWrapper(IntPtr widget, object[] args)
        {
            if (MyGUIInterface.TrackMemoryLeaks)
            {
                allocationStackTraces.Add(widget, new StackTrace(true));
            }

            WidgetType widgetType = WidgetManager_getType(widget);
            Widget_setDestructorCallback(widget, widgetDestructorFunc);
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

        #region PInvoke

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern WidgetType WidgetManager_getType(IntPtr widget);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void Widget_setDestructorCallback(IntPtr widget, WidgetDestructorCallback widgetDestructorCallback);

        #endregion
    }
}

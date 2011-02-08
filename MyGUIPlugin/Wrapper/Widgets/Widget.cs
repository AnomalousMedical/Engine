using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public enum WidgetStyle
    {
        Child, 
        Popup, 
        Overlapped, 
        MAX
    };


    public class Widget : IDisposable
    {
        protected IntPtr widget;
        internal MyGUIWidgetEventManager eventManager; //Event manager, this is internal but it should be considered internal, protected, do not touch outside of Widget or subclass
        private ISubWidgetText text;

        internal Widget(IntPtr widget)
        {
            this.widget = widget;
            eventManager = new MyGUIWidgetEventManager(this);
        }

        public virtual void Dispose()
        {
            eventManager.Dispose();
            widget = IntPtr.Zero;
        }

        //UserData
        public void setUserString(String key, String value)
        {
            Widget_setUserString(widget, key, value);
        }

        public String getUserString(String key)
        {
            return Marshal.PtrToStringAnsi(Widget_getUserString(widget, key));
        }

        public bool clearUserString(String key)
        {
            return Widget_clearUserString(widget, key);
        }

        public bool isUserString(String key)
        {
            return Widget_isUserString(widget, key);
        }

        public void clearUserStrings()
        {
            Widget_clearUserStrings(widget);
        }

        //Clipped Rectangle
        public int AbsoluteLeft
        {
            get
            {
                return Widget_getAbsoluteLeft(widget);
            }
        }

        public int AbsoluteTop
        {
            get
            {
                return Widget_getAbsoluteTop(widget);
            }
        }

        public int Left
        {
            get
            {
                return Widget_getLeft(widget);
            }
        }

        public int Right
        {
            get
            {
                return Widget_getRight(widget);
            }
        }

        public int Top
        {
            get
            {
                return Widget_getTop(widget);
            }
        }

        public int Bottom
        {
            get
            {
                return Widget_getBottom(widget);
            }
        }

        public int Width
        {
            get
            {
                return Widget_getWidth(widget);
            }
        }

        public int Height
        {
            get
            {
                return Widget_getHeight(widget);
            }
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

        //Widget
        public uint ChildCount
        {
            get
            {
                return Widget_getChildCount(widget).ToUInt32();
            }
        }        

        public Widget getChildAt(uint index)
        {
            return WidgetManager.getWidget(Widget_getChildAt(widget, new UIntPtr(index)));
        }

        public bool IsRootWidget
        {
            get
            {
                return Widget_isRootWidget(widget);
            }
        }

        public Widget Parent
        {
            get
            {
                return WidgetManager.getWidget(Widget_getParent(widget));
            }
        }        

        public Widget findWidget(String name)
        {
            return WidgetManager.getWidget(Widget_findWidget(widget, name));
        }

        public void setMaskPick(String filename)
        {
            Widget_setMaskPick(widget, filename);
        }

        public void setEnabledSilent(bool value)
        {
            Widget_setEnabledSilent(widget, value);
        }

        public IntCoord ClientCoord
        {
            get
            {
                return Widget_getClientCoord(widget);
            }
        }

        public Widget ClientWidget
        {
            get
            {
                return WidgetManager.getWidget(Widget_getClientWidget(widget));
            }
        }

        public void detachFromWidget()
        {
            Widget_detachFromWidget(widget);
        }

        public void detachFromWidget(String layer)
        {
            Widget_detachFromWidget2(widget, layer);
        }

        public void attachToWidget(Widget parent)
        {
            Widget_attachToWidget(widget, parent.WidgetPtr);
        }

        public void attachToWidget(Widget parent, WidgetStyle style)
        {
            Widget_attachToWidget2(widget, parent.WidgetPtr, style);
        }

        public void attachToWidget(Widget parent, WidgetStyle style, String layer)
        {
            Widget_attachToWidget3(widget, parent.WidgetPtr, style, layer);
        }

        public void changeWidgetSkin(String skinname)
        {
            Widget_changeWidgetSkin(widget, skinname);
        }

        public void setWidgetStyle(WidgetStyle style)
        {
            Widget_setWidgetStyle(widget, style);
        }

        public void setWidgetStyle(WidgetStyle style, String layer)
        {
            Widget_setWidgetStyle2(widget, style, layer);
        }

        public WidgetStyle getWidgetStyle()
        {
            return Widget_getWidgetStyle(widget);
        }

        public void setProperty(String key, String value)
        {
            Widget_setProperty(widget, key, value);
        }

        public void setCaptionWithNewLine(String value)
        {
            Widget_setCaptionWithNewLine(widget, value);
        }

        public String getLayerName()
        {
            return Marshal.PtrToStringAnsi(Widget_getLayerName(widget));
        }

        public bool setState(String value)
        {
            return Widget_setState(widget, value);
        }

        public void setColour(Color value)
        {
            Widget_setColour(widget, value);
        }

        /// <summary>
        /// Create a widget.
        /// </summary>
        /// <param name="type">Widget type.</param>
        /// <param name="skin">Widget skin.</param>
        /// <param name="left">Widget x pos.</param>
        /// <param name="top">Widget y pos.</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="align">Widget align.</param>
        /// <param name="layer">Layer where the widget will be created.</param>
        /// <param name="name">The name to find the widget later.</param>
        /// <returns></returns>
        public Widget createWidgetT(String type, String skin, int left, int top, int width, int height, Align align, String name)
        {
            return WidgetManager.getWidget(Widget_createWidgetT(widget, type, skin, left, top, width, height, align, name));
        }

        /// <summary>
        /// Create a widget using coords relative to the parent.
        /// </summary>
        /// <param name="type">Widget type.</param>
        /// <param name="skin">Widget skin.</param>
        /// <param name="left">Widget x pos.</param>
        /// <param name="top">Widget y pos.</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <param name="align">Widget align.</param>
        /// <param name="layer">Layer where the widget will be created.</param>
        /// <param name="name">The name to find the widget later.</param>
        /// <returns></returns>
        public Widget createWidgetRealT(String type, String skin, int left, int top, int width, int height, Align align, String name)
        {
            return WidgetManager.getWidget(Widget_createWidgetRealT(widget, type, skin, left, top, width, height, align, name));
        }

        public String Name
        {
            get
            {
                return Marshal.PtrToStringAnsi(Widget_getName(widget));
            }
        }

        public bool Visible
        {
            get
            {
                return Widget_isVisible(widget);
            }
            set
            {
                Widget_setVisible(widget, value);
            }
        }

        public Align Align
        {
            get
            {
                return Widget_getAlign(widget);
            }
            set
            {
                Widget_setAlign(widget, value);
            }
        }

        public String Caption
        {
            get
            {
                return Marshal.PtrToStringUni(Widget_getCaption(widget));
            }
            set
            {
                if (value != null)
                {
                    Widget_setCaption(widget, value);
                }
                else
                {
                    Widget_setCaption(widget, "");
                }
            }
        }

        public float Alpha
        {
            get
            {
                return Widget_getAlpha(widget);
            }
            set
            {
                Widget_setAlpha(widget, value);
            }
        }

        public bool InheritsAlpha
        {
            get
            {
                return Widget_isInheritsAlpha(widget);
            }
            set
            {
                Widget_setInheritsAlpha(widget, value);
            }
        }

        public bool NeedKeyFocus
        {
            get
            {
                return Widget_isNeedKeyFocus(widget);
            }
            set
            {
                Widget_setNeedKeyFocus(widget, value);
            }
        }

        public bool NeedMouseFocus
        {
            get
            {
                return Widget_isNeedMouseFocus(widget);
            }
            set
            {
                Widget_setNeedMouseFocus(widget, value);
            }
        }

        public bool InheritsPick
        {
            get
            {
                return Widget_isInheritsPick(widget);
            }
            set
            {
                Widget_setInheritsPick(widget, value);
            }
        }

        public bool Enabled
        {
            get
            {
                return Widget_isEnabled(widget);
            }
            set
            {
                Widget_setEnabled(widget, value);
            }
        }

        public String Pointer
        {
            get
            {
                return Marshal.PtrToStringAnsi(Widget_getPointer(widget));
            }
            set
            {
                Widget_setPointer(widget, value);
            }
        }

        public bool NeedToolTip
        {
            get
            {
                return Widget_getNeedToolTip(widget);
            }
            set
            {
                Widget_setNeedToolTip(widget, value);
            }
        }

        public bool EnableToolTip
        {
            get
            {
                return Widget_getEnableToolTip(widget);
            }
            set
            {
                Widget_setEnableToolTip(widget, value);
            }
        }

        public ISubWidgetText SubWidgetText
        {
            get
            {
                if (text == null)
                {
                    IntPtr textPtr = Widget_getSubWidgetText(widget);
                    if (textPtr != IntPtr.Zero)
                    {
                        text = new ISubWidgetText(textPtr);
                    }
                }
                return text;
            }
        }

        public Object UserObject { get; set; }
        
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

        public event MyGUIEvent MouseLostFocus
        {
            add
            {
                eventManager.addDelegate<EventMouseLostFocusTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMouseLostFocusTranslator>(value);
            }
        }

        public event MyGUIEvent MouseSetFocus
        {
            add
            {
                eventManager.addDelegate<EventMouseSetFocusTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMouseSetFocusTranslator>(value);
            }
        }

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

        public event MyGUIEvent MouseButtonDoubleClick
        {
            add
            {
                eventManager.addDelegate<EventMouseButtonDoubleClickTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMouseButtonDoubleClickTranslator>(value);
            }
        }

        public event MyGUIEvent MouseButtonPressed
        {
            add
            {
                eventManager.addDelegate<EventMouseButtonPressedTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMouseButtonPressedTranslator>(value);
            }
        }

        public event MyGUIEvent MouseButtonReleased
        {
            add
            {
                eventManager.addDelegate<EventMouseButtonReleasedTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMouseButtonReleasedTranslator>(value);
            }
        }

        public event MyGUIEvent MouseDrag
        {
            add
            {
                eventManager.addDelegate<EventMouseDragTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMouseDragTranslator>(value);
            }
        }

        public event MyGUIEvent MouseWheel
        {
            add
            {
                eventManager.addDelegate<EventMouseWheelTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventMouseWheelTranslator>(value);
            }
        }

        public event MyGUIEvent KeyLostFocus
        {
            add
            {
                eventManager.addDelegate<EventKeyLostFocusTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventKeyLostFocusTranslator>(value);
            }
        }

        public event MyGUIEvent KeySetFocus
        {
            add
            {
                eventManager.addDelegate<EventKeySetFocusTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventKeySetFocusTranslator>(value);
            }
        }

        public event MyGUIEvent KeyButtonPressed
        {
            add
            {
                eventManager.addDelegate<EventKeyButtonPressedTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventKeyButtonPressedTranslator>(value);
            }
        }

        public event MyGUIEvent KeyButtonReleased
        {
            add
            {
                eventManager.addDelegate<EventKeyButtonReleasedTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventKeyButtonReleasedTranslator>(value);
            }
        }

        public event MyGUIEvent RootMouseChangeFocus
        {
            add
            {
                eventManager.addDelegate<EventRootMouseChangeFocusTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventRootMouseChangeFocusTranslator>(value);
            }
        }

        public event MyGUIEvent RootKeyChangeFocus
        {
            add
            {
                eventManager.addDelegate<EventRootKeyChangeFocusTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventRootKeyChangeFocusTranslator>(value);
            }
        }

        public event MyGUIEvent EventToolTip
        {
            add
            {
                eventManager.addDelegate<EventToolTipTranslator>(value);
            }
            remove
            {
                eventManager.removeDelegate<EventToolTipTranslator>(value);
            }
        }

#endregion

#region PInvoke

        //UserData
        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setUserString(IntPtr widget, String key, String value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getUserString(IntPtr widget, String key);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_clearUserString(IntPtr widget, String key);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isUserString(IntPtr widget, String key);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_clearUserStrings(IntPtr widget);

        //Clipped Rectangle
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

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setVisible(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isVisible(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setAlign(IntPtr widget, Align value);

        [DllImport("MyGUIWrapper")]
        private static extern Align Widget_getAlign(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setCaption(IntPtr widget, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getCaption(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setAlpha(IntPtr widget, float value);

        [DllImport("MyGUIWrapper")]
        private static extern float Widget_getAlpha(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setInheritsAlpha(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isInheritsAlpha(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_setState(IntPtr widget, String value);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setColour(IntPtr widget, Color value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_createWidgetT(IntPtr widget, String type, String skin, int left, int top, int width, int height, Align align, String name);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_createWidgetRealT(IntPtr widget, String type, String skin, int left, int top, int width, int height, Align align, String name);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isRootWidget(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getParent(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_findWidget(IntPtr widget, String name);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setNeedKeyFocus(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isNeedKeyFocus(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setNeedMouseFocus(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isNeedMouseFocus(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setInheritsPick(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isInheritsPick(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setMaskPick(IntPtr widget, String filename);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setEnabled(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setEnabledSilent(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_isEnabled(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setPointer(IntPtr widget, String value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getPointer(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getLayerName(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntCoord Widget_getClientCoord(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getClientWidget(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setNeedToolTip(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_getNeedToolTip(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setEnableToolTip(IntPtr widget, bool value);

        [DllImport("MyGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Widget_getEnableToolTip(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_detachFromWidget(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_detachFromWidget2(IntPtr widget, String layer);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_attachToWidget(IntPtr widget, IntPtr parent);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_attachToWidget2(IntPtr widget, IntPtr parent, WidgetStyle style);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_attachToWidget3(IntPtr widget, IntPtr parent, WidgetStyle style, String layer);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_changeWidgetSkin(IntPtr widget, String skinname);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setWidgetStyle(IntPtr widget, WidgetStyle style);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setWidgetStyle2(IntPtr widget, WidgetStyle style, String layer);

        [DllImport("MyGUIWrapper")]
        private static extern WidgetStyle Widget_getWidgetStyle(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setProperty(IntPtr widget, String key, String value);

        [DllImport("MyGUIWrapper")]
        private static extern void Widget_setCaptionWithNewLine(IntPtr widget, [MarshalAs(UnmanagedType.LPWStr)] String value);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getName(IntPtr widget);

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Widget_getSubWidgetText(IntPtr widget);

#endregion
    }
}

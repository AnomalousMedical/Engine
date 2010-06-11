using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Runtime.InteropServices;
using System.Reflection;
using Engine;

namespace CEGUIPlugin
{
    public enum VerticalAlignment
    {
        /**
         * Window's position specifies an offset of it's top edge from the top edge
         * of it's parent.
         */
        VA_TOP,
        /**
         * Window's position specifies an offset of it's vertical centre from the
         * vertical centre of it's parent.
         */
        VA_CENTRE,
        /**
         * Window's position specifies an offset of it's bottom edge from the
         * bottom edge of it's parent.
         */
        VA_BOTTOM
    };

    public enum HorizontalAlignment
    {
        /**
         * Window's position specifies an offset of it's left edge from the left
         * edge of it's parent.
         */
        HA_LEFT,
        /**
         * Window's position specifies an offset of it's horizontal centre from the
         * horizontal centre of it's parent.
         */
        HA_CENTRE,
        /**
         * Window's position specifies an offset of it's right edge from the right
         * edge of it's parent.
         */
        HA_RIGHT
    };

    public delegate void CEGUIEvent(EventArgs e);

    public class Window : IDisposable
    {
        private IntPtr window;

        internal Window(IntPtr window)
        {
            this.window = window;
        }

        public virtual void Dispose()
        {
            window = IntPtr.Zero;
        }

#region Internal Management

        internal void eraseAllChildren()
        {
            WindowManager windowManager = WindowManager.Singleton;
            recursiveEraseChildren(window, windowManager);
        }

        private void recursiveEraseChildren(IntPtr parentWindow, WindowManager windowManager)
        {
            uint numChildren = Window_getChildCount(parentWindow).ToUInt32();
            for (uint i = 0; i < numChildren; i++)
            {
                recursiveEraseChildren(Window_getChildAtIdx(parentWindow, i), windowManager);
            }
            windowManager.deleteWrapper(parentWindow);
        }

        internal IntPtr CEGUIWindow
        {
            get
            {
                return window;
            }
        }

#endregion

        public String getType()
        {
            return Marshal.PtrToStringAnsi(Window_getType(window));
        }

        public String getName()
        {
            return Marshal.PtrToStringAnsi(Window_getName(window));
        }

        public bool isDestroyedByParent()
        {
            return Window_isDestroyedByParent(window);
        }

        public bool isAlwaysOnTop()
        {
            return Window_isAlwaysOnTop(window);
        }

        public bool isDisabled()
        {
            return Window_isDisabled(window);
        }

        public bool isDisabled(bool localOnly)
        {
            return Window_isDisabled2(window, localOnly);
        }

        public bool isVisible()
        {
            return Window_isVisible(window);
        }

        public bool isVisible(bool localOnly)
        {
            return Window_isVisible2(window, localOnly);
        }

        public bool isActive()
        {
            return Window_isActive(window);
        }

        public bool isClippedByParent()
        {
            return Window_isClippedByParent(window);
        }

        public uint getID()
        {
            return Window_getID(window);
        }

        public uint getChildCount()
        {
            return Window_getChildCount(window).ToUInt32();
        }

        public bool isChild(String name)
        {
            return Window_isChildName(window, name);
        }

        public bool isChild(uint ID)
        {
            return Window_isChildID(window, ID);
        }

        public bool isChildRecursive(uint ID)
        {
            return Window_isChildRecursive(window, ID);
        }

        public bool isChild(Window window)
        {
            return Window_isChildWin(this.window, window.CEGUIWindow);
        }

        public Window getChild(String name)
        {
            return WindowManager.Singleton.getWindow(Window_getChild(window, name));
        }

        public Window getChild(uint id)
        {
            return WindowManager.Singleton.getWindow(Window_getChildId(window, id));
        }

        public Window getChildRecursive(String name)
        {
            return WindowManager.Singleton.getWindow(Window_getChildRecursive(window, name));
        }

        public Window getChildRecursive(uint id)
        {
            return WindowManager.Singleton.getWindow(Window_getChildRecursiveId(window, id));
        }

        public Window getChildAtIdx(uint index)
        {
            return WindowManager.Singleton.getWindow(Window_getChildAtIdx(window, index));
        }

        public Window getActiveChild()
        {
            return WindowManager.Singleton.getWindow(Window_getActiveChild(window));
        }

        public bool isAncestor(String name)
        {
            return Window_isAncestorName(window, name);
        }

        public bool isAncestor(uint id)
        {
            return Window_isAncestorId(window, id);
        }

        public bool isAncestor(Window window)
        {
            return Window_isAncestorWin(this.window, window.CEGUIWindow);
        }

        public String getText()
        {
            return Marshal.PtrToStringAnsi(Window_getText(window));
        }

        public String getTextVisual()
        {
            return Marshal.PtrToStringAnsi(Window_getTextVisual(window));
        }

        public bool inheritsAlpha()
        {
            return Window_inheritsAlpha(window);
        }

        public float getAlpha()
        {
            return Window_getAlpha(window);
        }

        public float getEffectiveAlpha()
        {
            return Window_getEffectiveAlpha(window);
        }

        public Rect getUnclippedOuterRect()
        {
            return Window_getUnclippedOuterRect(window);
        }

        public Rect getUnclippedInnerRect()
        {
            return Window_getUnclippedInnerRect(window);
        }

        public Rect getUnclippedRect(bool inner)
        {
            return Window_getUnclippedRect(window, inner);
        }

        public Rect getOuterRectClipper()
        {
            return Window_getOuterRectClipper(window);
        }

        public Rect getInnerRectClipper()
        {
            return Window_getInnerRectClipper(window);
        }

        public Rect getClipRect()
        {
            return Window_getClipRect(window);
        }

        public Rect getHitTestRect()
        {
            return Window_getHitTestRect(window);
        }

        public bool isCapturedByThis()
        {
            return Window_isCapturedByThis(window);
        }

        public bool isCapturedByAncestor()
        {
            return Window_isCapturedByAncestor(window);
        }

        public bool isCapturedByChild()
        {
            return Window_isCapturedByChild(window);
        }

        public bool isHit(Vector2 position)
        {
            return Window_isHit(window, position);
        }

        public bool isHit(Vector2 position, bool allowDisabled)
        {
            return Window_isHit2(window, position, allowDisabled);
        }

        public Window getChildAtPosition(Vector2 position)
        {
            return WindowManager.Singleton.getWindow(Window_getChildAtPosition(window, position));
        }

        public Window getTargetChildAtPosition(Vector2 position)
        {
            return WindowManager.Singleton.getWindow(Window_getTargetChildAtPosition(window, position));
        }

        public Window getTargetChildAtPosition(Vector2 position, bool allowDisabled)
        {
            return WindowManager.Singleton.getWindow(Window_getTargetChildAtPosition2(window, position, allowDisabled));
        }

        public Window getParent()
        {
            return WindowManager.Singleton.getWindow(Window_getParent(window));
        }

        public Size getPixelSize()
        {
            return Window_getPixelSize(window);
        }

        public bool restoresOldCapture()
        {
            return Window_restoresOldCapture(window);
        }

        public bool isZOrderingEnabled()
        {
            return Window_isZOrderingEnabled(window);
        }

        public bool wantsMultiClickEvents()
        {
            return Window_wantsMultiClickEvents(window);
        }

        public bool isMouseAutoRepeatEnabled()
        {
            return Window_isMouseAutoRepeatEnabled(window);
        }

        public float getAutoRepeatDelay()
        {
            return Window_getAutoRepeatDelay(window);
        }

        public float getAutoRepeatRate()
        {
            return Window_getAutoRepeatRate(window);
        }

        public bool distributesCapturedInputs()
        {
            return Window_distributesCapturedInputs(window);
        }

        public bool isUsingDefaultTooltip()
        {
            return Window_isUsingDefaultTooltip(window);
        }

        public Tooltip getTooltip()
        {
            return WindowManager.Singleton.getWindow(Window_getTooltip(window)) as Tooltip;
        }

        public String getTooltipType()
        {
            return Marshal.PtrToStringAnsi(Window_getTooltipType(window));
        }

        public String getTooltipText()
        {
            return Marshal.PtrToStringAnsi(Window_getTooltipText(window));
        }

        public bool inheritsTooltipText()
        {
            return Window_inheritsTooltipText(window);
        }

        public bool isRiseOnClickEnabled()
        {
            return Window_isRiseOnClickEnabled(window);
        }

        public VerticalAlignment getVerticalAlignment()
        {
            return Window_getVerticalAlignment(window);
        }

        public HorizontalAlignment getHorizontalAlignment()
        {
            return Window_getHorizontalAlignment(window);
        }

        public String getLookNFeel()
        {
            return Marshal.PtrToStringAnsi(Window_getLookNFeel(window));
        }

        public bool getModalState()
        {
            return Window_getModalState(window);
        }

        public Window getActiveSibling()
        {
            return WindowManager.Singleton.getWindow(Window_getActiveSibling(window));
        }

        public Size getParentPixelSize()
        {
            return Window_getParentPixelSize(window);
        }

        public float getParentPixelWidth()
        {
            return Window_getParentPixelWidth(window);
        }

        public float getParentPixelHeight()
        {
            return Window_getParentPixelHeight(window);
        }

        public bool isMousePassThroughEnabled()
        {
            return Window_isMousePassThroughEnabled(window);
        }

        public bool isAutoWindow()
        {
            return Window_isAutoWindow(window);
        }

        public bool isDragDropTarget()
        {
            return Window_isDragDropTarget(window);
        }

        public Window getRootWindow()
        {
            return WindowManager.Singleton.getWindow(Window_getRootWindow(window));
        }

        public Vector3 getRotation()
        {
            return Window_getRotation(window);
        }

        public bool isNonClientWindow()
        {
            return Window_isNonClientWindow(window);
        }

        public void rename(String newName)
        {
            Window_rename(window, newName);
        }

        public void initializeComponents()
        {
            Window_initializeComponents(window);
        }

        public void setDestroyedByParent(bool setting)
        {
            Window_setDestroyedByParent(window, setting);
        }

        public void setAlwaysOnTop(bool setting)
        {
            Window_setAlwaysOnTop(window, setting);
        }

        public void setEnabled(bool setting)
        {
            Window_setEnabled(window, setting);
        }

        public void enable()
        {
            Window_enable(window);
        }

        public void disable()
        {
            Window_disable(window);
        }

        public void setVisible(bool setting)
        {
            Window_setVisible(window, setting);
        }

        public void show()
        {
            Window_show(window);
        }

        public void hide()
        {
            Window_hide(window);
        }

        public void activate()
        {
            Window_activate(window);
        }

        public void deactivate()
        {
            Window_deactivate(window);
        }

        public void setClippedByParent(bool setting)
        {
            Window_setClippedByParent(window, setting);
        }

        public void setID(uint id)
        {
            Window_setID(window, id);
        }

        public void setText(String text)
        {
            Window_setText(window, text);
        }

        public void insertText(String text, uint position)
        {
            Window_insertText(window, text, position);
        }

        public void appendText(String text)
        {
            Window_appendText(window, text);
        }

        public void setFont(String name)
        {
            Window_setFont(window, name);
        }

        public void addChildWindow(String name)
        {
            Window_addChildWindow(window, name);
        }

        public void addChildWindow(Window window)
        {
            Window_addChildWindow2(this.window, window.CEGUIWindow);
        }

        public void removeChildWindow(String name)
        {
            Window_removeChildWindowName(window, name);
        }

        public void removeChildWindow(Window window)
        {
            Window_removeChildWindowWin(this.window, window.CEGUIWindow);
        }

        public void removeChildWindow(uint id)
        {
            Window_removeChildWindowId(window, id);
        }

        public void moveToFront()
        {
            Window_moveToFront(window);
        }

        public void moveToBack()
        {
            Window_moveToBack(window);
        }

        public bool captureInput()
        {
            return Window_captureInput(window);
        }

        public void releaseInput()
        {
            Window_releaseInput(window);
        }

        public void setRestoreCapture(bool setting)
        {
            Window_setRestoreCapture(window, setting);
        }

        public void setAlpha(float alpha)
        {
            Window_setAlpha(window, alpha);
        }

        public void setInheritsAlpha(bool setting)
        {
            Window_setInheritsAlpha(window, setting);
        }

        public void invalidate()
        {
            Window_invalidate(window);
        }

        public void invalidate(bool recursive)
        {
            Window_invalidate2(window, recursive);
        }

        public void setMouseCursor(String imageset, String imageName)
        {
            Window_setMouseCursor(window, imageset, imageName);
        }

        public void setZOrderingEnabled(bool setting)
        {
            Window_setZOrderingEnabled(window, setting);
        }

        public void setWantsMultiClickEvents(bool setting)
        {
            Window_setWantsMultiClickEvents(window, setting);
        }

        public void setMouseAutoRepeatEnabled(bool setting)
        {
            Window_setMouseAutoRepeatEnabled(window, setting);
        }

        public void setAutoRepeatDelay(float delay)
        {
            Window_setAutoRepeatDelay(window, delay);
        }

        public void setAutoRepeatRate(float rate)
        {
            Window_setAutoRepeatRate(window, rate);
        }

        public void setDistributesCapturedInputs(bool setting)
        {
            Window_setDistributesCapturedInputs(window, setting);
        }

        public void setTooltip(Tooltip tooltip)
        {
            Window_setTooltip(window, tooltip.CEGUIWindow);
        }

        public void setTooltipType(String tooltipType)
        {
            Window_setTooltipType(window, tooltipType);
        }

        public void setTooltipText(String text)
        {
            Window_setTooltipText(window, text);
        }

        public void setInheritsTooltipText(bool setting)
        {
            Window_setInheritsTooltipText(window, setting);
        }

        public void setRiseOnClickEnabled(bool setting)
        {
            Window_setRiseOnClickEnabled(window, setting);
        }

        public void setVerticalAlignment(VerticalAlignment alignment)
        {
            Window_setVerticalAlignment(window, alignment);
        }

        public void setHorizontalAlignment(HorizontalAlignment alignment)
        {
            Window_setHorizontalAlignment(window, alignment);
        }

        public void setLookNFeel(String look)
        {
            Window_setLookNFeel(window, look);
        }

        public void setModalState(bool state)
        {
            Window_setModalState(window, state);
        }

        public void performChildWindowLayout()
        {
            Window_performChildWindowLayout(window);
        }

        public void setArea(UDim xPos, UDim yPos, UDim width, UDim height)
        {
            Window_setArea(window, xPos, yPos, width, height);
        }

        public void setArea(UVector2 pos, UVector2 size)
        {
            Window_setArea2(window, pos, size);
        }

        public void setArea(URect area)
        {
            Window_setArea3(window, area);
        }

        public void setPosition(UVector2 pos)
        {
            Window_setPosition(window, pos);
        }

        public void setXPosition(UDim x)
        {
            Window_setXPosition(window, x);
        }

        public void setYPosition(UDim y)
        {
            Window_setYPosition(window, y);
        }

        public void setSize(UVector2 size)
        {
            Window_setSize(window, size);
        }

        public void setWidth(UDim width)
        {
            Window_setWidth(window, width);
        }

        public void setHeight(UDim height)
        {
            Window_setHeight(window, height);
        }

        public void setMaxSize(UVector2 size)
        {
            Window_setMaxSize(window, size);
        }

        public void setMinSize(UVector2 size)
        {
            Window_setMinSize(window, size);
        }

        public URect getArea()
        {
            return Window_getArea(window);
        }

        public UVector2 getPosition()
        {
            return Window_getPosition(window);
        }

        public UDim getXPosition()
        {
            return Window_getXPosition(window);
        }

        public UDim getYPosition()
        {
            return Window_getYPosition(window);
        }

        public UVector2 getSize()
        {
            return Window_getSize(window);
        }

        public UDim getWidth()
        {
            return Window_getWidth(window);
        }

        public UDim getHeight()
        {
            return Window_getHeight(window);
        }

        public UVector2 getMaxSize()
        {
            return Window_getMaxSize(window);
        }

        public UVector2 getMinSize()
        {
            return Window_getMinSize(window);
        }

        public void setMousePassThroughEnabled(bool setting)
        {
            Window_setMousePassThroughEnabled(window, setting);
        }

        public void setFalagardType(String type, String rendererType)
        {
            Window_setFalagardType(window, type, rendererType);
        }

        public void setDragDropTarget(bool setting)
        {
            Window_setDragDropTarget(window, setting);
        }

        public void setRotation(Vector3 rotation)
        {
            Window_setRotation(window, rotation);
        }

        public void setNonClientWindow(bool setting)
        {
            Window_setNonClientWindow(window, setting);
        }

        public bool isTextParsingEnabled()
        {
            return Window_isTextParsingEnabled(window);
        }

        public void setTextParsingEnabled(bool setting)
        {
            Window_setTextParsingEnabled(window, setting);
        }

        public Vector2 getUnprojectedPosition(Vector2 pos)
        {
            return Window_getUnprojectedPosition(window, pos);
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getType(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getName(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isDestroyedByParent(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isAlwaysOnTop(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isDisabled(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isDisabled2(IntPtr window, bool localOnly);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isVisible(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isVisible2(IntPtr window, bool localOnly);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isActive(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isClippedByParent(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern uint Window_getID(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UIntPtr Window_getChildCount(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isChildName(IntPtr window, String name);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isChildID(IntPtr window, uint ID);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isChildRecursive(IntPtr window, uint ID);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isChildWin(IntPtr window, IntPtr child);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChild(IntPtr window, String name);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChildId(IntPtr window, uint id);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChildRecursive(IntPtr window, String name);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChildRecursiveId(IntPtr window, uint id);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChildAtIdx(IntPtr window, uint index);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getActiveChild(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isAncestorName(IntPtr window, String name);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isAncestorId(IntPtr window, uint id);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isAncestorWin(IntPtr window, IntPtr ancestor);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getText(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getTextVisual(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_inheritsAlpha(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern float Window_getAlpha(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern float Window_getEffectiveAlpha(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Rect Window_getUnclippedOuterRect(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Rect Window_getUnclippedInnerRect(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Rect Window_getUnclippedRect(IntPtr window, bool inner);

        [DllImport("CEGUIWrapper")]
        private static extern Rect Window_getOuterRectClipper(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Rect Window_getInnerRectClipper(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Rect Window_getClipRect(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Rect Window_getHitTestRect(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isCapturedByThis(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isCapturedByAncestor(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isCapturedByChild(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isHit(IntPtr window, Vector2 position);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isHit2(IntPtr window, Vector2 position, bool allowDisabled);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChildAtPosition(IntPtr window, Vector2 position);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getTargetChildAtPosition(IntPtr window, Vector2 position);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getTargetChildAtPosition2(IntPtr window, Vector2 position, bool allowDisabled);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getParent(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Size Window_getPixelSize(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_restoresOldCapture(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isZOrderingEnabled(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_wantsMultiClickEvents(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isMouseAutoRepeatEnabled(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern float Window_getAutoRepeatDelay(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern float Window_getAutoRepeatRate(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_distributesCapturedInputs(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isUsingDefaultTooltip(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getTooltip(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getTooltipType(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getTooltipText(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_inheritsTooltipText(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isRiseOnClickEnabled(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern VerticalAlignment Window_getVerticalAlignment(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern HorizontalAlignment Window_getHorizontalAlignment(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getLookNFeel(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_getModalState(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getActiveSibling(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Size Window_getParentPixelSize(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern float Window_getParentPixelWidth(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern float Window_getParentPixelHeight(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isMousePassThroughEnabled(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isAutoWindow(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isDragDropTarget(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getRootWindow(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern Vector3 Window_getRotation(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isNonClientWindow(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_rename(IntPtr window, String newName);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_initializeComponents(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setDestroyedByParent(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setAlwaysOnTop(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setEnabled(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_enable(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_disable(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setVisible(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_show(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_hide(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_activate(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_deactivate(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setClippedByParent(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setID(IntPtr window, uint id);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setText(IntPtr window, String text);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_insertText(IntPtr window, String text, uint position);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_appendText(IntPtr window, String text);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setFont(IntPtr window, String name);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_addChildWindow(IntPtr window, String name);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_addChildWindow2(IntPtr window, IntPtr child);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_removeChildWindowName(IntPtr window, String name);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_removeChildWindowWin(IntPtr window, IntPtr child);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_removeChildWindowId(IntPtr window, uint id);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_moveToFront(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_moveToBack(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_captureInput(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_releaseInput(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setRestoreCapture(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setAlpha(IntPtr window, float alpha);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setInheritsAlpha(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_invalidate(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_invalidate2(IntPtr window, bool recursive);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setMouseCursor(IntPtr window, String imageset, String imageName);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setZOrderingEnabled(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setWantsMultiClickEvents(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setMouseAutoRepeatEnabled(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setAutoRepeatDelay(IntPtr window, float delay);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setAutoRepeatRate(IntPtr window, float rate);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setDistributesCapturedInputs(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setTooltip(IntPtr window, IntPtr tooltip);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setTooltipType(IntPtr window, String tooltipType);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setTooltipText(IntPtr window, String text);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setInheritsTooltipText(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setRiseOnClickEnabled(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setVerticalAlignment(IntPtr window, VerticalAlignment alignment);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setHorizontalAlignment(IntPtr window, HorizontalAlignment alignment);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setLookNFeel(IntPtr window, String look);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setModalState(IntPtr window, bool state);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_performChildWindowLayout(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setArea(IntPtr window, UDim xPos, UDim yPos, UDim width, UDim height);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setArea2(IntPtr window, UVector2 pos, UVector2 size);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setArea3(IntPtr window, URect area);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setPosition(IntPtr window, UVector2 pos);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setXPosition(IntPtr window, UDim x);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setYPosition(IntPtr window, UDim y);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setSize(IntPtr window, UVector2 size);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setWidth(IntPtr window, UDim width);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setHeight(IntPtr window, UDim height);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setMaxSize(IntPtr window, UVector2 size);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setMinSize(IntPtr window, UVector2 size);

        [DllImport("CEGUIWrapper")]
        private static extern URect Window_getArea(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UVector2 Window_getPosition(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UDim Window_getXPosition(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UDim Window_getYPosition(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UVector2 Window_getSize(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UDim Window_getWidth(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UDim Window_getHeight(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UVector2 Window_getMaxSize(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern UVector2 Window_getMinSize(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setMousePassThroughEnabled(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setFalagardType(IntPtr window, String type, String rendererType);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setDragDropTarget(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setRotation(IntPtr window, Vector3 rotation);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setNonClientWindow(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Window_isTextParsingEnabled(IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void Window_setTextParsingEnabled(IntPtr window, bool setting);

        [DllImport("CEGUIWrapper")]
        private static extern Vector2 Window_getUnprojectedPosition(IntPtr window, Vector2 pos);

#endregion
    }
}

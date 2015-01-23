using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine.Platform;
using Engine;

namespace MyGUIPlugin
{
    public delegate void FocusChangedEvent(Widget widget);

    public class InputManager //Does not implement dispose, but does have an internal dispose function.
    {
        private static InputManager instance;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        public event MouseEvent MouseButtonPressed;
        public event MouseEvent MouseButtonReleased;
        private EventChangeKeyFocusInputManager changeKeyFocusTranslator;
        private EventChangeMouseFocusInputManager changeMouseFocusTranslator;

        public event FocusChangedEvent ChangeKeyFocus
        {
            add
            {
                if (changeKeyFocusTranslator == null)
                {
                    changeKeyFocusTranslator = new EventChangeKeyFocusInputManager(this);
                }
                changeKeyFocusTranslator.BoundEvent += value;
            }
            remove
            {
                if (changeKeyFocusTranslator != null)
                {
                    changeKeyFocusTranslator.BoundEvent -= value;
                }
            }
        }

        public event FocusChangedEvent ChangeMouseFocus
        {
            add
            {
                if (changeMouseFocusTranslator == null)
                {
                    changeMouseFocusTranslator = new EventChangeMouseFocusInputManager(this);
                }
                changeMouseFocusTranslator.BoundEvent += value;
            }
            remove
            {
                if (changeMouseFocusTranslator != null)
                {
                    changeMouseFocusTranslator.BoundEvent -= value;
                }
            }
        }

        private IntPtr inputManager;
        private int mouseX;
        private int mouseY;
        private int mouseZ;
        

        private InputManager()
        {
            inputManager = InputManager_getInstancePtr();
        }

        internal void Dispose()
        {
            if (changeKeyFocusTranslator != null)
            {
                changeKeyFocusTranslator.Dispose();
                changeKeyFocusTranslator = null;
            }
            if(changeMouseFocusTranslator != null)
            {
                changeMouseFocusTranslator.Dispose();
                changeMouseFocusTranslator = null;
            }
        }

        public bool injectMouseMove(int absx, int absy, int absz)
        {
            mouseX = absx;
            mouseY = absy;
            mouseZ = absz;
            return InputManager_injectMouseMove(inputManager, absx, absy, absz);
        }

        public bool injectMousePress(int absx, int absy, MouseButtonCode id)
        {
            mouseX = absx;
            mouseY = absy;
            bool handled = InputManager_injectMousePress(inputManager, absx, absy, id);
            if (MouseButtonPressed != null)
            {
                MouseButtonPressed.Invoke(absx, absy, id);
            }
            return handled;
        }

        public bool injectMouseRelease(int absx, int absy, MouseButtonCode id)
        {
            mouseX = absx;
            mouseY = absy;
            bool handled = InputManager_injectMouseRelease(inputManager, absx, absy, id);
            if (MouseButtonReleased != null)
            {
                MouseButtonReleased.Invoke(absx, absy, id);
            }
            return handled;
        }

        /// <summary>
        /// This function is a bit of a hack, but it will reset the mouse focus
        /// widget and then attempt to find it again it should be used in the
        /// event that something was animated and the mygui focus widget may no
        /// longer be valid.
        /// </summary>
        public void refreshMouseWidget()
        {
            resetMouseFocusWidget();
            InputManager_injectMouseMove(inputManager, mouseX, mouseY, mouseZ);
        }

        public bool injectKeyPress(KeyboardButtonCode key, uint text)
        {
            return InputManager_injectKeyPress(inputManager, key, text);
        }

        public bool injectKeyRelease(KeyboardButtonCode key)
        {
            return InputManager_injectKeyRelease(inputManager, key);
        }

        public bool isFocusMouse()
        {
            return InputManager_isFocusMouse(inputManager);
        }

        public bool isFocusKey()
        {
            return InputManager_isFocusKey(inputManager);
        }

        public bool isCaptureMouse()
        {
            return InputManager_isCaptureMouse(inputManager);
        }

        public void setKeyFocusWidget(Widget widget)
        {
            InputManager_setKeyFocusWidget(inputManager, widget.WidgetPtr);
        }

        public void resetKeyFocusWidget(Widget widget)
        {
            InputManager_resetKeyFocusWidget(inputManager, widget.WidgetPtr);
        }

        public void resetKeyFocusWidget()
        {
            InputManager_resetKeyFocusWidget2(inputManager);
        }

        public Widget getMouseFocusWidget()
        {
            return WidgetManager.getWidget(InputManager_getMouseFocusWidget(inputManager));
        }

        public Widget getKeyFocusWidget()
        {
            return WidgetManager.getWidget(InputManager_getKeyFocusWidget(inputManager));
        }

        public Vector2 getLastPressedPosition(MouseButtonCode id)
        {
            return InputManager_getLastPressedPosition(inputManager, id).toVector2();
        }

        public Vector2 getMousePosition()
        {
            return InputManager_getMousePosition(inputManager).toVector2();
        }

        public Vector2 getMousePositionByLayer()
        {
            return InputManager_getMousePositionByLayer(inputManager).toVector2();
        }

        public void resetMouseFocusWidget()
        {
            InputManager_resetMouseFocusWidget(inputManager);
        }

        public void addWidgetModal(Widget widget)
        {
            InputManager_addWidgetModal(inputManager, widget.WidgetPtr);
        }

        public void removeWidgetModal(Widget widget)
        {
            InputManager_removeWidgetModal(inputManager, widget.WidgetPtr);
        }

        public bool isModalAny()
        {
            return InputManager_isModalAny(inputManager);
        }

        public bool isControlPressed()
        {
            return InputManager_isControlPressed(inputManager);
        }

        public bool isShiftPressed()
        {
            return InputManager_isShiftPressed(inputManager);
        }

        public void resetMouseCaptureWidget()
        {
            InputManager_resetMouseCaptureWidget(inputManager);
        }

        public void unlinkWidget(Widget widget)
        {
            InputManager_unlinkWidget(inputManager, widget.WidgetPtr);
        }

        public bool injectScrollGesture(int absx, int absy, int deltax, int deltay)
        {
            return InputManager_injectScrollGesture(inputManager, absx, absy, deltax, deltay);
        }

        internal IntPtr Ptr
        {
            get
            {
                return inputManager;
            }
        }

#region PInvoke
        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr InputManager_getInstancePtr();

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_injectMouseMove(IntPtr inputManager, int absx, int absy, int absz);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_injectMousePress(IntPtr inputManager, int absx, int absy, MouseButtonCode id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_injectMouseRelease(IntPtr inputManager, int absx, int absy, MouseButtonCode id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_injectKeyPress(IntPtr inputManager, KeyboardButtonCode key, uint text);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_injectKeyRelease(IntPtr inputManager, KeyboardButtonCode key);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_isFocusMouse(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_isFocusKey(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_isCaptureMouse(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_setKeyFocusWidget(IntPtr inputManager, IntPtr widget);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_resetKeyFocusWidget(IntPtr inputManager, IntPtr widget);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_resetKeyFocusWidget2(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr InputManager_getMouseFocusWidget(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr InputManager_getKeyFocusWidget(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern ThreeIntHack InputManager_getLastPressedPosition(IntPtr inputManager, MouseButtonCode _id);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack InputManager_getMousePosition(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern ThreeIntHack InputManager_getMousePositionByLayer(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_resetMouseFocusWidget(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_addWidgetModal(IntPtr inputManager, IntPtr widget);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_removeWidgetModal(IntPtr inputManager, IntPtr widget);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_isModalAny(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_isControlPressed(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_isShiftPressed(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_resetMouseCaptureWidget(IntPtr inputManager);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention=CallingConvention.Cdecl)]
        private static extern void InputManager_unlinkWidget(IntPtr inputManager, IntPtr widget);

        [DllImport(MyGUIInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool InputManager_injectScrollGesture(IntPtr inputManager, int absx, int absy, int deltax, int deltay);

#endregion
    }
}

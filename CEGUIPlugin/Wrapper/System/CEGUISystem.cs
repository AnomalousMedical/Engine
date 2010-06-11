using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
    public enum MouseButton
    {
        //! The left mouse button.
        LeftButton,
        //! The right mouse button.
        RightButton,
        //! The middle mouse button.
        MiddleButton,
        //! The first 'extra' mouse button.
        X1Button,
        //! The second 'extra' mouse button.
        X2Button,
        //! Value that equals the number of mouse buttons supported by CEGUI.
        MouseButtonCount,
        //! Value set for no mouse button.  NB: This is not 0, do not assume!
        NoButton
    };

    public class CEGUISystem : IDisposable
    {
        private static CEGUISystem instance;

        public static CEGUISystem Instance
        {
            get
            {
                return instance;
            }
        }

        private CustomLogger customLogger = new CustomLogger();

        public CEGUISystem(CEGUIRenderer renderer)
        {
            if(instance != null)
            {
                throw new InvalidPluginException("Only create the CEGUISystem one time.");
            }
            CEGUISystem_create(renderer.Renderer, renderer.ResourceProvider, IntPtr.Zero, renderer.ImageCodec, IntPtr.Zero, "", "cegui.log");
            instance = this;
        }

        public void Dispose()
        {
            CEGUISystem_destroy();
            customLogger.Dispose();
        }

        public Window setGUISheet(Window window)
        {
            return WindowManager.Singleton.getWindow(CEGUISystem_setGUISheet(window.CEGUIWindow));
        }

        public void setLogLevel(LoggingLevel level)
        {
            customLogger.setLogLevel(level);
        }

        public bool injectMouseMove(float delta_x, float delta_y)
        {
            return CEGUISystem_injectMouseMove(delta_x, delta_y);
        }

        public bool injectMousePosition(float x_pos, float y_pos)
        {
            return CEGUISystem_injectMousePosition(x_pos, y_pos);
        }

        public bool injectMouseLeaves()
        {
            return CEGUISystem_injectMouseLeaves();
        }

        public bool injectMouseButtonDown(MouseButton button)
        {
            return CEGUISystem_injectMouseButtonDown(button);
        }

        public bool injectMouseButtonUp(MouseButton button)
        {
            return CEGUISystem_injectMouseButtonUp(button);
        }

        public bool injectKeyDown(uint key_code)
        {
            return CEGUISystem_injectKeyDown(key_code);
        }

        public bool injectKeyUp(uint key_cod)
        {
            return CEGUISystem_injectKeyUp(key_cod);
        }

        public bool injectChar(UInt32 code_point)
        {
            return CEGUISystem_injectChar(code_point);
        }

        public bool injectMouseWheelChange(float delta)
        {
            return CEGUISystem_injectMouseWheelChange(delta);
        }

        public bool injectTimePulse(float timeElapsed)
        {
            return CEGUISystem_injectTimePulse(timeElapsed);
        }

        public void notifyDisplaySizeChanged(float width, float height)
        {
            CEGUISystem_notifyDisplaySizeChanged(width, height);
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUISystem_create(IntPtr renderer, IntPtr resourceProvider, IntPtr xmlParser, IntPtr imageCodec, IntPtr scriptModule, String configFile, String logFile);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUISystem_destroy();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr CEGUISystem_setGUISheet(IntPtr window);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectMouseMove(float delta_x, float delta_y);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectMousePosition(float x_pos, float y_pos);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectMouseLeaves();

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectMouseButtonDown(MouseButton button);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectMouseButtonUp(MouseButton button);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectKeyDown(uint key_code);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectKeyUp(uint key_cod);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectChar(UInt32 code_point);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectMouseWheelChange(float delta);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool CEGUISystem_injectTimePulse(float timeElapsed);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUISystem_notifyDisplaySizeChanged(float width, float height);

#endregion
    }
}

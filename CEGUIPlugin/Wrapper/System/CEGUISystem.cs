using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
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

#region PInvoke
        [DllImport("CEGUIWrapper")]
        private static extern void CEGUISystem_create(IntPtr renderer, IntPtr resourceProvider, IntPtr xmlParser, IntPtr imageCodec, IntPtr scriptModule, String configFile, String logFile);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUISystem_destroy();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr CEGUISystem_setGUISheet(IntPtr window);
#endregion
    }
}

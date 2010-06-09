using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CEGUIPlugin
{
    public class CEGUISystem : IDisposable
    {
        private CustomLogger customLogger = new CustomLogger();

        public CEGUISystem(CEGUIRenderer renderer)
        {
            CEGUISystem_create(renderer.Renderer, renderer.ResourceProvider, IntPtr.Zero, renderer.ImageCodec, IntPtr.Zero, "", "cegui.log");
        }

        public void Dispose()
        {
            customLogger.Dispose();
        }

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUISystem_create(IntPtr renderer, IntPtr resourceProvider, IntPtr xmlParser, IntPtr imageCodec, IntPtr scriptModule, String configFile, String logFile);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUISystem_destroy();
    }
}

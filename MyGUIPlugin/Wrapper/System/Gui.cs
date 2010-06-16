using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MyGUIPlugin
{
    public class Gui : IDisposable
    {
        IntPtr gui;

        public Gui()
        {
            gui = Gui_Create();
        }

        public void Dispose()
        {
            Gui_Delete(gui);
        }

        public void initialize(String coreConfig, String logFile)
        {
            Gui_initialize(gui, coreConfig, logFile);
        }

        public void shutdown()
        {
            Gui_shutdown(gui);
        }

#region PInvoke

        [DllImport("MyGUIWrapper")]
        private static extern IntPtr Gui_Create();

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_Delete(IntPtr gui);

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_initialize(IntPtr gui, String coreConfig, String logFile);

        [DllImport("MyGUIWrapper")]
        private static extern void Gui_shutdown(IntPtr gui);

#endregion
    }
}

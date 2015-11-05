using System;
using System.Collections.Generic;
using System.Linq;
using Logging;
using System.IO;
using Engine.Platform;
using Anomalous.OSPlatform;
using Anomalous.OSPlatform.Win32;

namespace OgreModelEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WindowsRuntimePlatformInfo.Initialize();

            OgreModelEditorApp app = null;
            try
            {
                app = new OgreModelEditorApp();
                app.run();
            }
            catch (Exception e)
            {
                Logging.Log.Default.printException(e);
                if (app != null)
                {
                    app.saveCrashLog();
                }
                String errorMessage = e.Message + "\n" + e.StackTrace;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    errorMessage += "\n" + e.Message + "\n" + e.StackTrace;
                }
                MessageDialog.showErrorDialog(errorMessage, "Exception");
            }
            finally
            {
                if (app != null)
                {
                    app.Dispose();
                }
            }
        }
    }
}

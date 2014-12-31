using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Logging;
using System.IO;
using Anomaly.GUI;
using Engine.Platform;
using Anomalous.OSPlatform;

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
            StartupManager.SetupDllDirectories();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (SplashScreen splash = new SplashScreen())
            {
                splash.Show();
                Application.DoEvents();

                OgreModelEditorApp app = null;
                try
                {
                    app = new OgreModelEditorApp();

                    splash.Close();

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
}

using Anomalous.GameApp;
using Anomalous.OSPlatform;
using Anomalous.OSPlatform.Win32;
using GameAppTest;
using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XInputDotNetPure;

namespace GameAppTestWin32
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowsRuntimePlatformInfo.Initialize();

            GameApp app = null;
            try
            {
                app = new GameApp(new Startup());
                app.run();
            }
            catch (Exception e)
            {
                Log.Default.printException(e);
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

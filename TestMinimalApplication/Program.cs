using Anomalous.OSPlatform;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMinimalApplication;

namespace Anomalous.Minimus
{
    class Program
    {
        static void Main(string[] args)
        {
            StartupManager.SetupDllDirectories();

            //TimerApp timerApp = new TimerApp();
            //timerApp.run();
            //timerApp.Dispose();
            //return;


            MinimalApp app = null;
            try
            {
                app = new MinimalApp();
                app.run();
            }
            catch (Exception e)
            {
                Logging.Log.Default.printException(e);
                if (app != null)
                {
                    //app.saveCrashLog();
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

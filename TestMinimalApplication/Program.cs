using Anomalous.Minimus.Full;
using Anomalous.OSPlatform;
using Anomalous.OSPlatform.Win32;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowsRuntimePlatformInfo.Initialize();

            App app = null;
            try
            {
                app = new PharosApp(new MinimalApp());
                //app = new Anomalous.Minimus.OgreOnly.OgreOnlyApp();
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

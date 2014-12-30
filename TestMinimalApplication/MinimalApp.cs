using Engine;
using Engine.Platform;
using Logging;
using Medical;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMinimalApplication
{
    class MinimalApp : App
    {
        private EngineController engineController;
        private NativeOSWindow mainWindow;
        private CoreConfig coreConfig;
        private LogFileListener logListener;

        public MinimalApp()
        {

        }

        public override bool OnInit()
        {
            coreConfig = new CoreConfig("Anomalous Minimus");

            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(CoreConfig.LogFile);
            Log.Default.addLogListener(logListener);
            Log.ImportantInfo("Running from directory {0}", FolderFinder.ExecutableFolder);

            //Main Window
            mainWindow = new NativeOSWindow("Anomalous Minimus", new IntVector2(-1, -1), new IntSize2(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes));
            mainWindow.Closed += mainWindow_Closed;

            //Setup DPI
            float pixelScale = mainWindow.WindowScaling;
            switch (CoreConfig.ExtraScaling)
            {
                case UIExtraScale.Smaller:
                    pixelScale -= .15f;
                    break;
                case UIExtraScale.Larger:
                    pixelScale += .25f;
                    break;
            }

            ScaleHelper._setScaleFactor(pixelScale);

            engineController = new EngineController(mainWindow);

            return true;
        }

        public override int OnExit()
        {
            CoreConfig.save();
            engineController.Dispose();
            mainWindow.Dispose();

            logListener.closeLogFile();
            return 0;
        }

        public override void OnIdle()
        {
            engineController.MainTimer.OnIdle();
        }

        internal void saveCrashLog()
        {
            if (logListener != null)
            {
                DateTime now = DateTime.Now;
                String crashFile = String.Format(CultureInfo.InvariantCulture, "{0}/log {1}-{2}-{3} {4}.{5}.{6}.log", CoreConfig.CrashLogDirectory, now.Month, now.Day, now.Year, now.Hour, now.Minute, now.Second);
                logListener.saveCrashLog(crashFile);
            }
        }

        void mainWindow_Closed(OSWindow window)
        {
            if (PlatformConfig.CloseMainWindowOnShutdown)
            {
                mainWindow.close();
            }
            this.exit();
        }
    }
}

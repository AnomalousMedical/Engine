using Engine;
using Engine.Platform;
using Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMinimalApplication
{
    class MinimalApp : App
    {
        private MedicalController engineController;
        private NativeOSWindow mainWindow;

        public MinimalApp()
        {

        }

        public override bool OnInit()
        {
            MedicalConfig medicalConfig = new MedicalConfig();
            mainWindow = new NativeOSWindow("Minimal Applicaiton", new IntVector2(-1, -1), new IntSize2(800, 600));
            mainWindow.Closed +=mainWindow_Closed;

            engineController = new MedicalController(mainWindow);

            return true;
        }

        public override int OnExit()
        {
            engineController.Dispose();
            mainWindow.Dispose();

            return 0;
        }

        public override void OnIdle()
        {
            engineController.MainTimer.OnIdle();
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

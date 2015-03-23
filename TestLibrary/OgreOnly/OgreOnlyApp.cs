using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.OSPlatform;
using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Logging;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus.OgreOnly
{
    public class OgreOnlyApp : App
    {
        private OgreOnlyEngineController engineController;
        private NativeOSWindow mainWindow;
        private CoreConfig coreConfig;
        private LogFileListener logListener;
        private Color clearColor = Color.Black;

        private SimScene scene;

        public OgreOnlyApp()
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

            engineController = new OgreOnlyEngineController(mainWindow);
            engineController.OnLoopUpdate += engineController_OnLoopUpdate;

            //Create scene
            //Create a simple scene to use to show the models
            SimSceneDefinition sceneDefiniton = new SimSceneDefinition();
            OgreSceneManagerDefinition ogreScene = new OgreSceneManagerDefinition("Ogre");
            SimSubSceneDefinition mainSubScene = new SimSubSceneDefinition("Main");
            sceneDefiniton.addSimElementManagerDefinition(ogreScene);
            sceneDefiniton.addSimSubSceneDefinition(mainSubScene);
            mainSubScene.addBinding(ogreScene);
            sceneDefiniton.DefaultSubScene = "Main";

            scene = sceneDefiniton.createScene();

            return true;
        }

        void engineController_OnLoopUpdate(Clock time)
        {
            clearColor.b += (0.5f * time.DeltaSeconds);
            clearColor.b %= 1.0f;
            engineController.FrameClearManager.ClearColor = clearColor;
        }

        public override int OnExit()
        {
            //This is probably not disposing everything
            CoreConfig.save();
            scene.Dispose();

            engineController.Dispose();
            mainWindow.Dispose();

            base.Dispose();

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

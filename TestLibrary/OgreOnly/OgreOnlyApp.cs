using Anomalous.GuiFramework;
using Anomalous.GuiFramework.Cameras;
using Anomalous.OSPlatform;
using Autofac;
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
        private CoreConfig coreConfig;
        private LogFileListener logListener;
        private Color clearColor = Color.Black;

        ContainerBuilder builder = new ContainerBuilder();

        IContainer container;
        ILifetimeScope sceneScope;
        ILifetimeScope appScope;

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

            builder.Register<NativeOSWindow>(c =>
            {
                //Main Window
                var mainWindow = new NativeOSWindow("Anomalous Minimus", new IntVector2(-1, -1), new IntSize2(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes));
                mainWindow.Closed += w =>
                {
                    if (PlatformConfig.CloseMainWindowOnShutdown)
                    {
                        mainWindow.close();
                    }
                    this.exit();
                };

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

                return mainWindow;
            }).SingleInstance();

            builder.Register(c =>
            {
                var controller = new OgreOnlyEngineController(c.Resolve<NativeOSWindow>());
                controller.OnLoopUpdate += engineController_OnLoopUpdate;
                return controller;
            }).OnRelease(i =>
            {
                i.OnLoopUpdate -= engineController_OnLoopUpdate;
                i.Dispose();
            }).InstancePerApp();


            builder.Register(c => new OgreSceneManagerDefinition("Ogre")).InstancePerScene();

            builder.Register(c =>
            {
                //Create scene
                //Create a simple scene to use to show the models
                SimSceneDefinition sceneDefiniton = new SimSceneDefinition();
                var ogreScene = c.Resolve<OgreSceneManagerDefinition>();
                SimSubSceneDefinition mainSubScene = new SimSubSceneDefinition("Main");
                sceneDefiniton.addSimElementManagerDefinition(ogreScene);
                sceneDefiniton.addSimSubSceneDefinition(mainSubScene);
                mainSubScene.addBinding(ogreScene);
                sceneDefiniton.DefaultSubScene = "Main";

                return sceneDefiniton;
            }).InstancePerScene();

            builder.Register(c => c.Resolve<SimSceneDefinition>().createScene()).InstancePerScene();

            container = builder.Build();
            appScope = container.BeginLifetimeScope(LifetimeScopes.App);
            engineController = appScope.Resolve<OgreOnlyEngineController>();
            sceneScope = appScope.BeginLifetimeScope(LifetimeScopes.Scene);
            sceneScope.Resolve<SimScene>();

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
            sceneScope.Dispose();
            appScope.Dispose();
            container.Dispose();

            //engineController.Dispose();
            //mainWindow.Dispose();

            base.Dispose();

            logListener.Dispose();

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
    }
}

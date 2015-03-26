﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Logging;
using OgrePlugin;
using Engine.Platform;
using Engine.Renderer;
using System.Threading;
using System.Xml;
using Engine.ObjectManagement;
using Engine.Saving.XMLSaver;
using Engine.Resources;
using System.IO;
using Anomalous.OSPlatform;
using Anomalous.Minimus;

namespace Anomalous.Minimus.OgreOnly
{
    public enum EventLayers
    {
        Gui = 0,
        AfterGui = 1,
        Tools = 2,
        Posing = 3,
        Cameras = 4,
        Selection = 5,
    }

    public delegate void LoopUpdate(Clock time);

    public sealed class OgreOnlyEngineController : IDisposable
    {
        //Engine
        private PluginManager pluginManager;

        //Platform
        private NativeSystemTimer systemTimer;
        private NativeUpdateTimer mainTimer;
        private EventManager eventManager;
        private NativeInputHandler inputHandler;
        private EventUpdateListener eventUpdate;
        private EngineUpdate medicalUpdate;

        //Performance
        private NativeSystemTimer performanceMetricTimer;

        //Controller
        private SceneController sceneController;
        private FrameClearManager frameClearManager;

        //Serialization
        private XmlSaver xmlSaver = new XmlSaver();

        //Scene
        private String currentSceneFile;
        private String currentSceneDirectory;

        public event LoopUpdate OnLoopUpdate;

        public OgreOnlyEngineController(NativeOSWindow mainWindow)
        {
            //Create pluginmanager
            pluginManager = new PluginManager(CoreConfig.ConfigFile);

            //Configure the filesystem
            VirtualFileSystem archive = VirtualFileSystem.Instance;

            //Setup microcode cache load
            OgreInterface.LoadMicrocodeCacheCallback = (rs, gpuProgMan) =>
            {
                String microcodeFile = rs.Name + ".mcc";
                if (File.Exists(microcodeFile))
                {
                    using (Stream stream = File.OpenRead(microcodeFile))
                    {
                        gpuProgMan.loadMicrocodeCache(stream);
                        Log.Info("Using microcode cache {0}", microcodeFile);
                    }
                }
                return true;
            };

            //Configure plugins
            pluginManager.OnConfigureDefaultWindow = delegate(out WindowInfo defaultWindow)
            {
                //Setup main window
                defaultWindow = new WindowInfo(mainWindow, "Primary");
                defaultWindow.Fullscreen = CoreConfig.EngineConfig.Fullscreen;
                defaultWindow.MonitorIndex = 0;

                if (CoreConfig.EngineConfig.Fullscreen)
                {
                    mainWindow.setSize(CoreConfig.EngineConfig.HorizontalRes, CoreConfig.EngineConfig.VerticalRes);
                    mainWindow.ExclusiveFullscreen = true;
                }
                else
                {
                    mainWindow.Maximized = true;
                }
                mainWindow.show();
            };

            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(NativePlatformPlugin).Assembly);
            pluginManager.initializePlugins();

            performanceMetricTimer = new NativeSystemTimer();
            PerformanceMonitor.setupEnabledState(performanceMetricTimer);

            //Intialize the platform
            systemTimer = new NativeSystemTimer();

            mainTimer = new NativeUpdateTimer(systemTimer);

            if (OgreConfig.VSync && CoreConfig.EngineConfig.FPSCap < 300)
            {
                //Use a really high framerate cap if vsync is on since it will cap our 
                //framerate for us. If the user has requested a higher rate use it anyway.
                mainTimer.FramerateCap = 300;
            }
            else
            {
                mainTimer.FramerateCap = CoreConfig.EngineConfig.FPSCap;
            }

            inputHandler = new NativeInputHandler(mainWindow, CoreConfig.EnableMultitouch);
            eventManager = new EventManager(inputHandler, Enum.GetValues(typeof(EventLayers)));
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addUpdateListener(eventUpdate);
            pluginManager.setPlatformInfo(mainTimer, eventManager);
            medicalUpdate = new EngineUpdate(this);
            mainTimer.addUpdateListener(medicalUpdate);

            //Initialize controllers
            sceneController = new SceneController(pluginManager);
            frameClearManager = new FrameClearManager(OgreInterface.Instance.OgrePrimaryWindow.OgreRenderTarget, Color.Blue);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (frameClearManager != null)
            {
                frameClearManager.Dispose();
            }
            if (sceneController != null)
            {
                sceneController.destroyScene();
            }
            if (eventManager != null)
            {
                eventManager.Dispose();
            }
            if (inputHandler != null)
            {
                inputHandler.Dispose();
            }
            if (systemTimer != null)
            {
                systemTimer.Dispose();
            }
            if (performanceMetricTimer != null)
            {
                PerformanceMonitor.destroyEnabledState();
                performanceMetricTimer.Dispose();
            }
            if (pluginManager != null)
            {
                //This is the main engine plugin manager, it should be last unless subsystems need to be shutdown before any additional disposing
                pluginManager.Dispose();
            }

            Log.Info("Engine Controller Shutdown");
        }

        /// <summary>
        /// Stop the loop and begin the process of shutting down the program.
        /// </summary>
        public void shutdown()
        {
            mainTimer.stopLoop();
        }

        /// <summary>
        /// Attempt to open the given scene file. Will return true if the scene was loaded correctly.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>True if the scene was loaded, false on an error.</returns>
        public IEnumerable<SceneBuildStatus> openScene(String filename)
        {
            sceneController.destroyScene();
            VirtualFileSystem sceneArchive = VirtualFileSystem.Instance;
            if (sceneArchive.exists(filename))
            {
                currentSceneFile = VirtualFileSystem.GetFileName(filename);
                currentSceneDirectory = VirtualFileSystem.GetDirectoryName(filename);
                using (Stream file = sceneArchive.openStream(filename, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read))
                {
                    XmlTextReader textReader = null;
                    ScenePackage scenePackage = null;
                    try
                    {
                        yield return new SceneBuildStatus()
                        {
                            Message = "Loading Scene File"
                        };
                        textReader = new XmlTextReader(file);
                        scenePackage = xmlSaver.restoreObject(textReader) as ScenePackage;
                    }
                    finally
                    {
                        if (textReader != null)
                        {
                            textReader.Close();
                        }
                    }
                    if (scenePackage != null)
                    {
                        foreach (var status in sceneController.loadScene(scenePackage, SceneBuildOptions.SingleUseDefinitions))
                        {
                            yield return status;
                        }
                    }
                }
            }
            else
            {
                Log.Error("Could not load scene {0}.", filename);
            }
        }

        public void addSimObject(SimObjectBase simObject)
        {
            sceneController.addSimObject(simObject);
        }

        internal void _sendUpdate(Clock clock)
        {
            if (OnLoopUpdate != null)
            {
                OnLoopUpdate.Invoke(clock);
            }
        }

        public EventManager EventManager
        {
            get
            {
                return eventManager;
            }
        }

        public NativeUpdateTimer MainTimer
        {
            get
            {
                return mainTimer;
            }
        }

        public SimScene CurrentScene
        {
            get
            {
                return sceneController.CurrentScene;
            }
        }

        public IEnumerable<SimObjectBase> SimObjects
        {
            get
            {
                return sceneController.SimObjects;
            }
        }

        public SimObject getSimObject(String name)
        {
            return sceneController.getSimObject(name);
        }

        public String CurrentSceneFile
        {
            get
            {
                return currentSceneFile;
            }
        }

        public String CurrentSceneDirectory
        {
            get
            {
                return currentSceneDirectory;
            }
        }

        public PluginManager PluginManager
        {
            get
            {
                return pluginManager;
            }
        }

        public FrameClearManager FrameClearManager
        {
            get
            {
                return frameClearManager;
            }
        }
    }
}
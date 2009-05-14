using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using Engine.Platform;
using Engine;
using Editor;
using OgrePlugin;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Engine.Renderer;

namespace OgreModelEditor
{
    class OgreModelEditorController : IDisposable, UpdateListener, IDockProvider
    {
        #region Fields

        //Engine
        private PluginManager pluginManager;
        private LogFileListener logListener;

        //Platform
        private UpdateTimer mainTimer;
        private EventManager eventManager;
        private InputHandler inputHandler;
        private EventUpdateListener eventUpdate;

        //GUI
        private DrawingWindow hiddenEmbedWindow;
        private OgreModelEditorMain mainForm;

        //Controller
        private DrawingWindowController drawingWindowController = new DrawingWindowController();

        #endregion Fields

        public void Dispose()
        {
            if (eventManager != null)
            {
                eventManager.Dispose();
            }
            if (inputHandler != null)
            {
                pluginManager.PlatformPlugin.destroyInputHandler(inputHandler);
            }
            if (pluginManager != null)
            {
                pluginManager.Dispose();
            }
            if (hiddenEmbedWindow != null)
            {
                hiddenEmbedWindow.Dispose();
            }

            OgreModelEditorConfig.save();
            logListener.closeLogFile();
        }

        public void initialize()
        {
            //Create the log.
            logListener = new LogFileListener();
            logListener.openLogFile(OgreModelEditorConfig.DocRoot + "/log.log");
            Log.Default.addLogListener(logListener);

            hiddenEmbedWindow = new DrawingWindow();
            pluginManager = new PluginManager();
            pluginManager.OnConfigureDefaultWindow = createWindow;
            pluginManager.addPluginAssembly(typeof(OgreInterface).Assembly);
            pluginManager.addPluginAssembly(typeof(Win32PlatformPlugin).Assembly);
            pluginManager.initializePlugins();
            pluginManager.RendererPlugin.PrimaryWindow.setEnabled(false);

            //Create the GUI
            mainForm = new OgreModelEditorMain();

            //Intialize the platform
            mainTimer = pluginManager.PlatformPlugin.createTimer();
            inputHandler = pluginManager.PlatformPlugin.createInputHandler(mainForm, false, false, false);
            eventManager = new EventManager(inputHandler);
            eventUpdate = new EventUpdateListener(eventManager);
            mainTimer.addFixedUpdateListener(eventUpdate);
            mainTimer.addFullSpeedUpdateListener(this);
            pluginManager.setPlatformInfo(mainTimer, eventManager);

            //Initialize controllers
            drawingWindowController.initialize(this, eventManager, pluginManager.RendererPlugin, OgreModelEditorConfig.ConfigFile);
            drawingWindowController.createOneWaySplit();

            //Initialize GUI
            mainForm.initialize(this);
        }

        public void start()
        {
            mainForm.Show();
            mainTimer.startLoop();
        }

        public void shutdown()
        {
            mainTimer.stopLoop();
        }

        /// <summary>
        /// Helper function to create the default window. This is the callback
        /// to the PluginManager.
        /// </summary>
        /// <param name="defaultWindow"></param>
        private void createWindow(out DefaultWindowInfo defaultWindow)
        {
            defaultWindow = new DefaultWindowInfo(hiddenEmbedWindow);
        }

        #region UpdateListener Members

        public void sendUpdate(Clock clock)
        {
            Application.DoEvents();
        }

        public void loopStarting()
        {

        }

        public void exceededMaxDelta()
        {

        }

        #endregion

        #region IDockProvider Members

        public void showDockContent(DockContent content)
        {
            mainForm.showDockContent(content);
        }

        public void hideDockContent(DockContent content)
        {
            mainForm.hideDockContent(content);
        }

        #endregion
    }
}

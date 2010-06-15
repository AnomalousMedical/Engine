using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using OgrePlugin;

namespace CEGUIPlugin
{
    public class CEGUIInterface : PluginInterface
    {
        private CEGUIOgreRenderer ceguiRenderer;
        private CEGUISystem ceguiSystem;
        private UpdateTimer mainTimer;
        private EventManager eventManager;
        private CEGUIUpdate update;
        private CEGUIWindowListener windowListener;
        private OSWindow mainWindow;

        public CEGUIInterface()
        {

        }

        public void Dispose()
        {
            mainWindow.removeListener(windowListener);
            mainTimer.removeFixedUpdateListener(update);
            WindowManager.Instance.Dispose();
            ImagesetManager.Instance.Dispose();
            ceguiSystem.Dispose();
            ceguiRenderer.Dispose();
        }

        public void initialize(PluginManager pluginManager)
        {
            OgreWindow window = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;
            ceguiRenderer = new CEGUIOgreRenderer(window.OgreRenderWindow);
            ceguiSystem = new CEGUISystem(ceguiRenderer);
            windowListener = new CEGUIWindowListener(ceguiSystem);
            mainWindow = window.Handle;
            mainWindow.addListener(windowListener);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
            this.eventManager = eventManager;

            update = new CEGUIUpdate(ceguiSystem, eventManager);
            mainTimer.addFixedUpdateListener(update);
        }

        public string getName()
        {
            return "CEGUIPlugin";
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }
    }
}

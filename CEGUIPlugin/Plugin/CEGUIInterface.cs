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

        public CEGUIInterface()
        {

        }

        public void Dispose()
        {
            mainTimer.removeFixedUpdateListener(update);
            ceguiSystem.Dispose();
            ceguiRenderer.Dispose();
        }

        public void initialize(PluginManager pluginManager)
        {
            OgreWindow window = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;
            ceguiRenderer = new CEGUIOgreRenderer(window.OgreRenderWindow);
            ceguiSystem = new CEGUISystem(ceguiRenderer);
            ceguiSystem.setLogLevel(LoggingLevel.Standard);
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

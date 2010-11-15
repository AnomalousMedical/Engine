using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;

namespace SoundPlugin
{
    public class SoundPluginInterface : PluginInterface
    {
        private OpenALManager openALManager = null;
        private SoundUpdateListener soundUpdate;
        private UpdateTimer mainTimer;

        public SoundPluginInterface()
        {

        }

        public void Dispose()
        {
            if (openALManager != null)
            {
                openALManager.Dispose();
                openALManager = null;
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            openALManager = new OpenALManager();
            soundUpdate = new SoundUpdateListener(openALManager);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
            mainTimer.addFixedUpdateListener(soundUpdate);
        }

        public string getName()
        {
            return "SoundPlugin";
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

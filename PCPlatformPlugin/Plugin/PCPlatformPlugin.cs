using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using Logging;

namespace PCPlatform
{
    public class PCPlatformPlugin : PlatformPlugin
    {
        public static PCPlatformPlugin Instance { get; private set; }

        public PCPlatformPlugin()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Can only create PCPlatformPlugin one time.");
            }
        }

        public void Dispose()
        {
            
        }

        public SystemTimer createTimer()
        {
            return new PCSystemTimer();
        }

        public void destroyTimer(SystemTimer timer)
        {
            PCSystemTimer pcTimer = timer as PCSystemTimer;
            if(pcTimer != null)
            {
                pcTimer.Dispose();
            }
            else
            {
                Log.Error("Attempted to delete a SystemTimer that was not a PCSystemTimer in PCPlatformPlugin. Are you mixing platform plugins?");
            }
        }

        public InputHandler createInputHandler(OSWindow window, bool foreground, bool exclusive, bool noWinKey, bool enableMultitouch)
        {
            return new PCInputHandler(window, foreground, exclusive, noWinKey);
        }

        public void destroyInputHandler(InputHandler handler)
        {
            PCInputHandler pcInput = handler as PCInputHandler;
            if (pcInput != null)
            {
                pcInput.Dispose();
            }
            else
            {
                Log.Error("Attempted to delete a InputHandler that was not a PCInputHandler in PCPlatformPlugin. Are you mixing platform plugins?");
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            pluginManager.setPlatformPlugin(this);
        }

        public void link(PluginManager pluginManager)
        {

        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }

        public string Name
        {
            get
            {
                return "PCPlatform";
            }
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }
    }
}

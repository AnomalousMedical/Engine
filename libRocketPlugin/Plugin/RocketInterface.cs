using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    class RocketInterface : PluginInterface
    {
        public RocketInterface()
        {
            
        }

        public void Dispose()
        {
            if (rocketTest != null)
            {
                libRocketTest_Delete(rocketTest);
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            rocketTest = libRocketTest_Create(640, 480);
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            //mainTimer.addFullSpeedUpdateListener(new RocketUpdate(this));
        }

        public string getName()
        {
            return "libRocketPlugin";
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {

        }

        #region Temp

        IntPtr rocketTest;

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr libRocketTest_Create(int width, int height);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void libRocketTest_Delete(IntPtr test);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libRocketTest_Render(IntPtr test);

        internal void render()
        {
            libRocketTest_Render(rocketTest);
        }

        #endregion
    }
}

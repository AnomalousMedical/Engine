using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using System.Runtime.InteropServices;
using OgreWrapper;
using OgrePlugin;

namespace libRocketPlugin
{
    public class RocketInterface : PluginInterface
    {
        private ManagedSystemInterface systemInterface;
        private RenderInterfaceOgre3D renderInterface;
        private VirtualFileSystemFileInterface fileInterface;

        private static RocketInterface instance;
        public static RocketInterface Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RocketInterface();
                }
                return instance;
            }
        }

        /// <summary>
        /// libRocket expects its paths to have only / in it so this will fix it up for you.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static String MakeSafePath(String path)
        {
            return path.Replace('\\', '/');
        }

        private RocketInterface()
        {
            
        }

        public void Dispose()
        {
            Core.Shutdown();
            if (renderInterface != null)
            {
                renderInterface.Dispose();
            }
            //if (fileInterface != null)
            //{
            //    fileInterface.Dispose();
            //}
            ReferenceCountable.DumpLeakReport();
            if (systemInterface != null)
            {
                systemInterface.Dispose();
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            OgreWindow ogreWindow = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;

            systemInterface = new ManagedSystemInterface();
            renderInterface = new RenderInterfaceOgre3D((int)ogreWindow.OgreRenderWindow.getWidth(), (int)ogreWindow.OgreRenderWindow.getHeight());
            fileInterface = new VirtualFileSystemFileInterface();

            Core.SetSystemInterface(systemInterface);
            Core.SetRenderInterface(renderInterface);
            Core.SetFileInterface(fileInterface);

            Core.Initialise();
            Controls.Initialise();
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            systemInterface.Timer = mainTimer;
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

        public VirtualFileSystemFileInterface FileInterface
        {
            get
            {
                return fileInterface;
            }
        }

        static RocketInterface()
        {
            ViewportZIndex = 2000000;
        }

        public static int ViewportZIndex { get; set; }
    }
}

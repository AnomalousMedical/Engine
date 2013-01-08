using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using System.Runtime.InteropServices;
using OgrePlugin;

namespace libRocketPlugin
{
    public class RocketInterface : PluginInterface
    {
        public const float DefaultPixelsPerInch = 100;

        private ManagedSystemInterface systemInterface;
        private RenderInterfaceOgre3D renderInterface;
        private FileInterface fileInterface;
        private float pixelsPerInch = DefaultPixelsPerInch;

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

        private RocketInterface()
        {
            
        }

        public void Dispose()
        {
            ReferenceCountable.DumpLeakReport();
            Core.Shutdown();
            if (renderInterface != null)
            {
                renderInterface.Dispose();
            }
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
            renderInterface.PixelsPerInch = pixelsPerInch;

            Core.SetSystemInterface(systemInterface);
            Core.SetRenderInterface(renderInterface);

            if (FileInterface == null)
            {
                FileInterface = new VirtualFileSystemFileInterface();
            }
            //Core.SetFileInterface(fileInterface);

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

        public FileInterface FileInterface
        {
            get
            {
                return fileInterface;
            }
            set
            {
                fileInterface = value;
                Core.SetFileInterface(fileInterface);
            }
        }

        static RocketInterface()
        {
            ViewportZIndex = 2000000;
        }

        public static int ViewportZIndex { get; set; }

        public float PixelsPerInch
        {
            get
            {
                if (renderInterface != null)
                {
                    return renderInterface.PixelsPerInch;
                }
                return pixelsPerInch;
            }
            set
            {
                if (renderInterface != null)
                {
                    renderInterface.PixelsPerInch = value;
                }
                pixelsPerInch = value;
            }
        }
    }
}

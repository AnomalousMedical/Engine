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
        public const String DefaultProtocol = "file:///";
        private const String UrlFormat = "file:///{0}";
        private const String CombinePathUrlFormat = "file:///{0}/{1}";

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
            renderInterface.PixelScale = ScaleHelper.ScaleFactor;

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

        /// <summary>
        /// Create a url for librocket from a path. Makes sure everything is formatted correctly.
        /// </summary>
        /// <param name="path">The original path.</param>
        /// <returns>The path formatted valid for libRocket to parse.</returns>
        public static String createValidFileUrl(String path)
        {
            return String.Format(UrlFormat, path.Replace('\\', '/'));
        }

        /// <summary>
        /// Create a url for librocket from two paths. These will be blindly combined together with no intelligence
        /// separated by a /
        /// </summary>
        /// <param name="path1">The first path</param>
        /// <param name="path2">The second path</param>
        /// <returns>The paths combined and formatted for libRocket to parse correctly.</returns>
        public static String createValidFileUrlFromPaths(String path1, String path2)
        {
            return String.Format(CombinePathUrlFormat, path1, path2).Replace('\\', '/');
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

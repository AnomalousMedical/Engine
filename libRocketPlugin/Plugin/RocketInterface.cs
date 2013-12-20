using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using System.Runtime.InteropServices;
using OgrePlugin;
using OgreWrapper;

namespace libRocketPlugin
{
    public class RocketInterface : PluginInterface
    {
        public const float DefaultPixelsPerInch = 100;
        public const String DefaultProtocol = "anom:///";
        private const String UrlFormat = "anom:///{0}";

        private ManagedSystemInterface systemInterface;
        private RenderInterfaceOgre3D renderInterface;
        private FileInterface fileInterface;
        private float pixelsPerInch = DefaultPixelsPerInch;

        private RocketFilesystemArchiveFactory rocketFilesystemArchiveFactory = new RocketFilesystemArchiveFactory();

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
            TextureDatabase.ReleaseTextures();
            OgreResourceGroupManager.getInstance().removeResourceLocation("__RmlViewerFilesystem__", "Rocket");
            //OgreResourceGroupManager.getInstance().destroyResourceGroup("Rocket");

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

        void OgreRoot_Disposed()
        {
            //Have to delete ogre resource archives after ogre is deleted
            if (rocketFilesystemArchiveFactory != null)
            {
                rocketFilesystemArchiveFactory.Dispose();
                rocketFilesystemArchiveFactory = null;
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            Root.getSingleton().addArchiveFactory(rocketFilesystemArchiveFactory);

            OgreResourceGroupManager.getInstance().createResourceGroup("Rocket");
            OgreResourceGroupManager.getInstance().addResourceLocation("__RmlViewerFilesystem__", RocketFilesystemArchive.ArchiveName, "Rocket", false);

            Root.getSingleton().Disposed += OgreRoot_Disposed;

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

        public ManagedSystemInterface SystemInterface
        {
            get
            {
                return systemInterface;
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

        public float PixelScale
        {
            get
            {
                if (renderInterface != null)
                {
                    return renderInterface.PixelScale;
                }
                throw new Exception("You must instantiate the render interface before getting the pixel scale");
            }
            set
            {
                if (renderInterface != null)
                {
                    renderInterface.PixelScale = value;
                }
                else
                {
                    throw new Exception("You must instantiate the render interface before setting the pixel scale");
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using System.Runtime.InteropServices;
using OgrePlugin;
using Engine.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace libRocketPlugin
{
    public class RocketInterface : PluginInterface
    {
#if STATIC_LINK
		public const String LibraryName = "__Internal";
#else
        internal const String LibraryName = "libRocketWrapper";
#endif

        public const float DefaultPixelsPerInch = 100;
        public const String DefaultProtocol = "anom:///";
        private const String UrlFormat = "anom:///{0}";

        private ManagedSystemInterface systemInterface;
        private RenderInterfaceOgre3D renderInterface;
        private FileInterface fileInterface;
        private float pixelsPerInch = DefaultPixelsPerInch;
        private ResourceManager resources;
        private RocketRenderSystemListener renderSystemListener;

        private RocketFilesystemArchiveFactory rocketFilesystemArchiveFactory = new RocketFilesystemArchiveFactory();

        /// <summary>
        /// This event is fired when a texture has been loaded in the background. If you have stuff that might need
        /// to rerender as a result, subscribe to this event.
        /// </summary>
        public event Action TextureLoaded;

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
            CommonResourceGroup.removeResource("__RmlViewerFilesystem__");

            ReferenceCountable.DumpLeakReport();
            Root.getSingleton().getRenderSystem().removeListener(renderSystemListener);
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

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {

        }

        public void link(PluginManager pluginManager)
        {
            Root.getSingleton().addArchiveFactory(rocketFilesystemArchiveFactory);

            if (FileInterface == null)
            {
                FileInterface = new VirtualFileSystemFileInterface();
            }

            resources = pluginManager.createLiveResourceManager("Rocket");
            var rendererResources = resources.getSubsystemResource("Ogre");
            CommonResourceGroup = rendererResources.addResourceGroup("Common");
            CommonResourceGroup.addResource("__RmlViewerFilesystem__", RocketFilesystemArchive.ArchiveName, false);
            CommonResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);
            var shaders = rendererResources.addResourceGroup("Shaders");
            shaders.addResource(this.GetType().AssemblyQualifiedName, "EmbeddedResource", false);
            var shared = rendererResources.addResourceGroup("Shared");
            shared.addResource("__LibRocketCommonResourcesFilesystem__", CommonResourcesArchiveFactory.Name, false);
            resources.initializeResources();

            Root.getSingleton().Disposed += OgreRoot_Disposed;

            OgreWindow ogreWindow = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;

            systemInterface = new ManagedSystemInterface();
            renderInterface = new RenderInterfaceOgre3D((int)ogreWindow.OgreRenderTarget.getWidth(), (int)ogreWindow.OgreRenderTarget.getHeight());
            renderInterface.PixelsPerInch = pixelsPerInch;
            renderInterface.PixelScale = ScaleHelper.ScaleFactor;

            renderSystemListener = new RocketRenderSystemListener();
            Root.getSingleton().getRenderSystem().addListener(renderSystemListener);

            Core.SetSystemInterface(systemInterface);
            Core.SetRenderInterface(renderInterface);

            Core.Initialise();
            Controls.Initialise();
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            systemInterface.Timer = mainTimer;
        }

        public string Name
        {
            get
            {
                return "libRocketPlugin";
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

        /// <summary>
        /// The ResourceGroup for common resources for libRocket.
        /// </summary>
        public ResourceGroup CommonResourceGroup { get; private set; }

        static RocketInterface()
        {
            ViewportZIndex = 2000000;
            LoadImagesInBackground = true;
        }

        public static int ViewportZIndex { get; set; }

        /// <summary>
        /// Set this to true (default) to load libRocket images on a background thread.
        /// </summary>
        public static bool LoadImagesInBackground { get; set; }

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

        internal void fireTextureLoaded()
        {
            if(TextureLoaded != null)
            {
                TextureLoaded.Invoke();
            }
        }
    }
}

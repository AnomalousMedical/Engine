﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Renderer;
using Logging;
using Engine.Platform;
using Engine.Command;
using Engine.Resources;
using System.IO;
using Engine.ObjectManagement;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OgrePlugin
{
    public enum RenderSystemType
    {
        Default = 0,
        D3D11 = 1,
        OpenGL = 2,
	    OpenGLES2 = 3
    };

    [Flags]
    public enum CompressedTextureSupport : uint
    {
        None = 0,
        DXT = 1,
        PVRTC = 1 << 1,
        ATC = 1 << 2,
        ETC2 = 1 << 3,
        BC4_BC5 = 1 << 4,
        DXT_BC4_BC5 = DXT | BC4_BC5,
        All = DXT | PVRTC | ATC | ETC2 | BC4_BC5
    }

    /// <summary>
    /// The main interface class for the OgrePlugin.
    /// </summary>
    public class OgreInterface : RendererPlugin
    {
        public delegate bool MicrocodeCacheDelegate(RenderSystem renderSystem, GpuProgramManager gpuProgramManager);

        public const String PluginName = "OgrePlugin";
        private static OgreInterface instance;

        static OgreInterface()
        {
            CompressedTextureSupport = CompressedTextureSupport.All;
            AllowMicrocodeCacheLoad = true;
        }

        public static OgreInterface Instance
        {
            get
            {
                return instance;
            }
        }

        private Root root;
        private OgreUpdate ogreUpdate;
        private OgreWindow primaryWindow;
        private IntPtr renderSystemPlugin;
        private DeviceLostListener deviceLostListener;
        private MaterialParserManager materialParser = new MaterialParserManager();
        private OgreResourceManager ogreResourceManager;
        private RenderSystem rs;
        private RenderSystemType chosenRenderSystem;
        private ResourceManager engineResourceManager;

        private delegate OgreSceneManagerDefinition CreateSceneManagerDefinition(String name);
        private delegate SceneNodeDefinition CreateSceneNodeDefinition(String name);
        private delegate void AddResourceLocation(String name, String locType, String group, bool recursive);
        private delegate void InitializeResourceGroups();

        /// <summary>
        /// Fired when the OgreInterface is disposed, which means that ogre has been shutdown (Ogre::Root deleted).
        /// </summary>
        public event Action<OgreInterface> Disposed;

        public OgreInterface()
        {
            if (instance != null)
            {
                throw new InvalidPluginException("The OgrePlugin plugin can only be initialized one time.");
            }
            ogreResourceManager = new OgreResourceManager(materialParser);
        }

        public void Dispose()
        {
            var ogreSubsystem = engineResourceManager.getSubsystemResource("Ogre");
            ogreSubsystem.removeResourceGroup("Internal");
            engineResourceManager.initializeResources();

            if (OgreConfig.SaveMicrocodeCache && GpuProgramManager.Instance.IsCacheDirty && MicrocodeCachePath != null)
            {
                try
                {
                    using (Stream stream = File.Open(MicrocodeCachePath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read))
                    {
                        GpuProgramManager.Instance.saveMicrocodeCache(stream);
                        Log.Info("Saved microcode cache {0}", MicrocodeCachePath);
                    }
                }
                catch(Exception ex)
                {
                    Log.Error("{0} saving microcode cache {1}.\nReason:{2}", ex.GetType().Name, MicrocodeCachePath, ex.Message);
                }
            }
            OgreInterface_DestroyVaryingCompressedTextures();
            GpuProgramManager.Instance.Dispose();
            HighLevelGpuProgramManager.Instance.Dispose();
            MaterialManager.getInstance().Dispose();
            MeshManager.getInstance().Dispose();
            SkeletonManager.getInstance().Dispose();
            HardwareBufferManager.getInstance().Dispose();
            TextureManager.getInstance().Dispose();
            OgreDataStream.DisposeSharedPtrs();
            destroyRendererWindow(primaryWindow);
            root.Dispose();
            OgreInterface_UnloadRenderSystem(renderSystemPlugin);
            if(deviceLostListener != null)
            {
                deviceLostListener.Dispose();
            }
            if (Disposed != null)
            {
                Disposed.Invoke(this);
            }
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            //Load config
            new OgreConfig(pluginManager.ConfigFile);
            chosenRenderSystem = OgreConfig.RenderSystemType;

            //Setup ogre root
            root = new Root("", "", "");
            renderSystemPlugin = OgreInterface_LoadRenderSystem(ref chosenRenderSystem);
            ogreUpdate = new OgreUpdate(root);
            instance = this;

            WindowInfo defaultWindowInfo;
            pluginManager.setRendererPlugin(this, out defaultWindowInfo);

            try
            {
                //Initialize Ogre
                rs = root._getRenderSystemWrapper(OgreInterface_GetRenderSystem(ref chosenRenderSystem));
                root.setRenderSystem(rs);
                root.initialize(false);

                //Create the default window.
                Dictionary<String, String> miscParams = new Dictionary<string, string>();
                String fsaa = OgreConfig.FSAA;
                if (fsaa.Contains("Quality"))
                {
                    miscParams.Add("FSAAHint", "Quality");
                }
                int spaceIndex = fsaa.IndexOf(' ');
                if(spaceIndex != -1)
                {
                    fsaa = fsaa.Substring(0, spaceIndex);
                }
                miscParams.Add("FSAA", fsaa);
                miscParams.Add("vsync", OgreConfig.VSync.ToString());
                miscParams.Add("monitorIndex", defaultWindowInfo.MonitorIndex.ToString());
                miscParams.Add("useNVPerfHUD", OgreConfig.UseNvPerfHUD.ToString());
                miscParams.Add("contentScalingFactor", defaultWindowInfo.ContentScalingFactor.ToString());
                if (defaultWindowInfo.AutoCreateWindow)
                {
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                    OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                    primaryWindow = new AutomaticWindow(ogreWindow);
                }
                else
                {
                    miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.WindowHandle.ToString());
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                    primaryWindow = new EmbeddedWindow(defaultWindowInfo.EmbedWindow, renderWindow);
                }

                if (InitialClearColor.HasValue)
                {
                    rs.clearFrameBuffer(FrameBufferType.FBT_COLOUR, InitialClearColor.Value);
                    primaryWindow.OgreRenderTarget.update(true);
                }

                defaultWindowInfo._fireWindowCreated(new WindowInfoEventArgs(primaryWindow));

                //Setup compressed textures
                SelectedTextureFormat = OgreInterface_SetupVaryingCompressedTextures(CompressedTextureSupport);

                //Setup commands
                pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create Ogre Scene Manager", OgreSceneManagerDefinition.Create));
                pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Ogre Scene Node", SceneNodeDefinition.Create));

                //Setup shader cache
                if (MicrocodeCachePath != null)
                {
                    GpuProgramManager.Instance.SaveMicrocodesToCache = true;
                    if (File.Exists(MicrocodeCachePath))
                    {
                        if (AllowMicrocodeCacheLoad)
                        {
                            using (Stream stream = File.OpenRead(MicrocodeCachePath))
                            {
                                GpuProgramManager.Instance.loadMicrocodeCache(stream);
                                Log.Info("Using microcode cache {0}", MicrocodeCachePath);
                            }
                        }
                        else
                        {
                            deleteMicrocodeCache();
                        }
                    }
                }

                //Setup Resources
                pluginManager.addSubsystemResources("Ogre", OgreResourceManager.Instance);

                //Setup the device lost listener.
                deviceLostListener = new DeviceLostListener();
                rs.addListener(deviceLostListener);

                serviceCollection.TryAddSingleton<Root>(this.root); //This is externally owned

                serviceCollection.TryAddSingleton<RendererWindow>(this.PrimaryWindow); //This is externally owned

                serviceCollection.TryAddSingleton<RenderTarget>(this.OgrePrimaryWindow.OgreRenderTarget); //This is externally owned

                serviceCollection.TryAddSingleton<SceneViewLightManager>(s => s.GetRequiredService<PluginManager>().RendererPlugin.createSceneViewLightManager());

                serviceCollection.TryAddSingleton<FrameClearManager>(s => new FrameClearManager(s.GetRequiredService<RenderTarget>(), Color.FromRGB(0x333333)));
            }
            catch (Exception e)
            {
                throw new InvalidPluginException(String.Format("Exception initializing renderer. Message: {0}", e.Message), e);
            }
        }

        public void link(PluginManager pluginManager)
        {
            engineResourceManager = pluginManager.createLiveResourceManager("OgrePlugin");
            var ogreSubsystem = engineResourceManager.getSubsystemResource("Ogre");
            var group = ogreSubsystem.addResourceGroup("Internal");
            group.addResource(typeof(OgreInterface).AssemblyQualifiedName, "EmbeddedResource", true);
            engineResourceManager.initializeResources();
        }

        /// <summary>
        /// This function will create any debug commands for the plugin and add them to the commands list.
        /// </summary>
        /// <param name="commands">A list of CommandManagers to add debug commands to.</param>
        public void createDebugCommands(List<CommandManager> commands)
        {
            CommandManager ogreDebugCommands = new CommandManager("Ogre");
            ogreDebugCommands.addCommand(new EngineCommand("addResourceLocation", "Add Ogre Resource Location", "Add a resource location to ogre.", new AddResourceLocation(OgreResourceGroupManager.getInstance().addResourceLocation)));
            ogreDebugCommands.addCommand(new EngineCommand("initializeResourceGroups", "Initialize Ogre Resources", "Initialize all added ogre resources.", new InitializeResourceGroups(OgreResourceGroupManager.getInstance().initializeAllResourceGroups)));
            commands.Add(ogreDebugCommands);
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {
            renamedTypeMap.addRenamedType("OgreWrapper.SceneType", typeof(SceneType));
            renamedTypeMap.addRenamedType("OgreWrapper.ShadowTechnique", typeof(ShadowTechnique));
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            mainTimer.addUpdateListenerWithBackgrounding("Rendering", ogreUpdate);
        }

        public string Name
        {
            get
            {
                return PluginName;
            }
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public RendererWindow PrimaryWindow
        {
            get
            {
                return primaryWindow;
            }
        }

        public OgreWindow OgrePrimaryWindow
        {
            get
            {
                return primaryWindow;
            }
        }

        public MaterialParserManager MaterialParser
        {
            get
            {
                return materialParser;
            }
        }

        public RenderSystemType ChosenRenderSystem
        {
            get
            {
                return chosenRenderSystem;
            }
        }

        public GPUVendor GpuVendor
        {
            get
            {
                return OgreInterface_getGpuVendor();
            }
        }

        /// <summary>
        /// The currently active texture format that was chosen. This will be constant for the duration of the
        /// program's execution.
        /// </summary>
        public CompressedTextureSupport SelectedTextureFormat { get; private set; }

        public RenderSystem RenderSystem
        {
            get
            {
                return rs;
            }
        }

        public RendererWindow recreatePrimaryWindow()
        {
            destroyRendererWindow(primaryWindow);

            WindowInfo defaultWindowInfo;
            PluginManager.Instance.reconfigureDefaultWindow(out defaultWindowInfo);

            //Create the default window.
            Dictionary<String, String> miscParams = new Dictionary<string, string>();
            String fsaa = OgreConfig.FSAA;
            if (fsaa.Contains("Quality"))
            {
                miscParams.Add("FSAAHint", "Quality");
            }
            int spaceIndex = fsaa.IndexOf(' ');
            if (spaceIndex != -1)
            {
                fsaa = fsaa.Substring(0, spaceIndex);
            }
            miscParams.Add("FSAA", fsaa);
            miscParams.Add("vsync", OgreConfig.VSync.ToString());
            miscParams.Add("monitorIndex", defaultWindowInfo.MonitorIndex.ToString());
            miscParams.Add("useNVPerfHUD", OgreConfig.UseNvPerfHUD.ToString());
            miscParams.Add("contentScalingFactor", defaultWindowInfo.ContentScalingFactor.ToString());
            if (defaultWindowInfo.AutoCreateWindow)
            {
                RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                primaryWindow = new AutomaticWindow(ogreWindow);
            }
            else
            {
                miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.WindowHandle.ToString());
                RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                primaryWindow = new EmbeddedWindow(defaultWindowInfo.EmbedWindow, renderWindow);
            }
            defaultWindowInfo._fireWindowCreated(new WindowInfoEventArgs(primaryWindow));

            return primaryWindow;
        }

        public RendererWindow createRendererWindow(String name)
        {
            return createRendererWindow(null, name);
        }

        public RendererWindow createRendererWindow(OSWindow embedWindow, String name)
        {
            WindowInfo windowInfo = new WindowInfo(embedWindow, name);
            return createRendererWindow(windowInfo);
        }

        public RendererWindow createRendererWindow(WindowInfo windowInfo)
        {
            Dictionary<String, String> miscParams = new Dictionary<string, string>();
            String fsaa = OgreConfig.FSAA;
            if (fsaa.Contains("Quality"))
            {
                miscParams.Add("FSAAHint", "Quality");
            }
            int spaceIndex = fsaa.IndexOf(' ');
            if (spaceIndex != -1)
            {
                fsaa = fsaa.Substring(0, spaceIndex);
            }
            miscParams.Add("FSAA", fsaa);
            miscParams.Add("vsync", OgreConfig.VSync.ToString());
            miscParams.Add("monitorIndex", windowInfo.MonitorIndex.ToString());
            miscParams.Add("useNVPerfHUD", OgreConfig.UseNvPerfHUD.ToString());
            miscParams.Add("contentScalingFactor", windowInfo.ContentScalingFactor.ToString());
            OSWindow embedWindow = windowInfo.EmbedWindow;
            if (embedWindow != null)
            {
                miscParams.Add("externalWindowHandle", embedWindow.WindowHandle.ToString());
            }
            RenderWindow renderWindow = root.createRenderWindow(windowInfo.AutoWindowTitle, (uint)windowInfo.Width, (uint)windowInfo.Height, windowInfo.Fullscreen, miscParams);
            OgreWindow ogreWindow;
            if (embedWindow != null)
            {
                ogreWindow = new EmbeddedWindow(embedWindow, renderWindow);
            }
            else
            {
                ogreWindow = new AutomaticWindow(new OgreOSWindow(renderWindow));
            }
            windowInfo._fireWindowCreated(new WindowInfoEventArgs(ogreWindow));
            return ogreWindow;
        }

        public void destroyRendererWindow(RendererWindow window)
        {
            OgreWindow ogreWindow = window as OgreWindow;
            if (ogreWindow != null)
            {
                Log.Default.sendMessage("Destroying RenderWindow {0}.", LogLevel.Info, "OgrePlugin", ogreWindow.OgreRenderTarget.getName());
                ogreWindow.Dispose();
                root.destroyRenderTarget(ogreWindow.OgreRenderTarget);
                if (ogreWindow == primaryWindow)
                {
                    primaryWindow = null;
                }
            }
            else
            {
                if (window == null)
                {
                    Log.Default.sendMessage("Error attempted to destroy a null RenderWindow. No changes have been made.", LogLevel.Warning, "OgrePlugin");
                }
                else
                {
                    Log.Default.sendMessage("Error destroying RendererWindow {0}. It is not a recognized OgreWindow. The window has not been destroyed.", LogLevel.Warning, "OgrePlugin", window.ToString());
                }
            }
        }

        /// <summary>
        /// Create a new DebugDrawingSurface named name in the specified scene
        /// that renders in the specified way.
        /// </summary>
        /// <param name="name">The name of the DrawingSurface. Must be unique.</param>
        /// <param name="sceneName">The name of the scene to create the surface into.</param>
        /// <param name="drawingType">The DrawingType of the surface.</param>
        /// <returns>A new DebugDrawingSurface configured appropriatly.</returns>
        public DebugDrawingSurface createDebugDrawingSurface(String name, SimSubScene scene)
        {
            if (scene != null)
            {
                if (scene.hasSimElementManagerType(typeof(OgreSceneManager)))
                {
                    return new OgreDebugSurface(name, scene.getSimElementManager<OgreSceneManager>().SceneManager);
                }
                else
                {
                    Log.Default.sendMessage("Could not find an OgreSceneManager in the SimSubScene {0}. Could not create OgreDebugSurface.", LogLevel.Error, PluginName, scene.Name);
                    return null;
                }
            }
            else
            {
                Log.Default.sendMessage("Could not create OgreDebugSurface. SimSubScene was null.", LogLevel.Error, PluginName);
                return null;
            }
        }

        /// <summary>
        /// Destroy a DebugDrawingSurface. This should be called before the
        /// scene it was created in is destroyed.
        /// </summary>
        /// <param name="surface">The DebugDrawingSurface to destroy.</param>
        public void destroyDebugDrawingSurface(DebugDrawingSurface surface)
        {
            ((OgreDebugSurface)surface).destroy();
        }

        public SceneViewLightManager createSceneViewLightManager()
        {
            return new OgreSceneViewLightManager();
        }

        public void destroySceneViewLightManager(SceneViewLightManager lightManager)
        {
            if (lightManager != null)
            {
                lightManager.Dispose();
            }
        }

        /// <summary>
        /// Delete the microcode cache file if it is defined and exists.
        /// </summary>
        public void deleteMicrocodeCache()
        {
            if (MicrocodeCachePath != null)
            {
                try
                {
                    File.Delete(MicrocodeCachePath);
                }
                catch (Exception ex)
                {
                    Log.Error("{0} deleting microcode cache {1}.\nReason:{2}", ex.GetType().Name, MicrocodeCachePath, ex.Message);
                }
            }
        }

        /// <summary>
        /// Get/Set the microcode cache file path for this OgreInterface, if this is set before initialize the
        /// plugin will attempt to load a microcode cache from the given location on startup. If it is set to non
        /// null on shutdown this class will save a microcode cache to the specified path.
        /// </summary>
        public static String MicrocodeCachePath { get; set; }

        /// <summary>
        /// Set this to true (default) to allow the load of the microcode cache. This can be set to false to skip the
        /// load of the cache this run, which will also delete any existing cache file since it was deemed unworthy
        /// for at least one run. It will be automatically regenerated when the program shuts down. This makes it easy
        /// to upgrade deployed microcode caches in one run of the program.
        /// </summary>
        public static bool AllowMicrocodeCacheLoad { get; set; }

        /// <summary>
        /// Get/Set the compressed texture support for ogre, this should be done before initialization to setup everything
        /// correctly. Changing this after initialization will have no effect. Note that this is a declaration of what
        /// formats the client program has assets for, not what format will be chosen, that is determined at runtime
        /// based on the hardware the program is running on.
        /// </summary>
        public static CompressedTextureSupport CompressedTextureSupport { get; set; }

        /// <summary>
        /// If this is set to a color value the ogre plugin will clear the window with that color 
        /// as soon as possible.
        /// </summary>
        public static Color? InitialClearColor { get; set; }

        /// <summary>
        /// True to track memory leaks, will track leaks from the point this is set to true onward, so set it as early as possible
        /// </summary>
        public static bool TrackMemoryLeaks { get; set; }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OgreInterface_LoadRenderSystem(ref RenderSystemType rendersystemType);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreInterface_UnloadRenderSystem(IntPtr renderSystemPlugin);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OgreInterface_GetRenderSystem(ref RenderSystemType rendersystemType);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern CompressedTextureSupport OgreInterface_SetupVaryingCompressedTextures(CompressedTextureSupport compressedTextures);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreInterface_DestroyVaryingCompressedTextures();

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern GPUVendor OgreInterface_getGpuVendor();

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgreWrapper;
using Engine.Renderer;
using Logging;
using Engine.Platform;
using Engine.Command;
using Engine.Resources;
using System.IO;
using Engine.ObjectManagement;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OgrePlugin
{
    public enum RenderSystemType
    {
        Default = 0,
        D3D11 = 1,
        OpenGL = 2
    };

    /// <summary>
    /// The main interface class for the OgrePlugin.
    /// </summary>
    public class OgreInterface : RendererPlugin
    {
        public delegate bool LoadMicrocodeCacheDelegate(RenderSystem renderSystem, GpuProgramManager gpuProgramManager);

        public const String PluginName = "OgrePlugin";
        private static OgreInterface instance;

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
        }

        public void Dispose()
        {
            MaterialManager.getInstance().Dispose();
            MeshManager.getInstance().Dispose();
            SkeletonManager.getInstance().Dispose();
            HardwareBufferManager.getInstance().Dispose();
            TextureManager.getInstance().Dispose();
            destroyRendererWindow(primaryWindow);
            root.Dispose();
            OgreInterface_UnloadRenderSystem(renderSystemPlugin);
            if (Disposed != null)
            {
                Disposed.Invoke(this);
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            //Load config
            new OgreConfig(pluginManager.ConfigFile);
            RenderSystemType renderSystemType = OgreConfig.RenderSystemType;

            //Setup ogre root
            root = new Root("", "", "");
            renderSystemPlugin = OgreInterface_LoadRenderSystem(renderSystemType);
            ogreUpdate = new OgreUpdate(root);
            instance = this;

            WindowInfo defaultWindowInfo;
            pluginManager.setRendererPlugin(this, out defaultWindowInfo);

            try
            {
                //Initialize Ogre
                RenderSystem rs = root._getRenderSystemWrapper(OgreInterface_GetRenderSystem(renderSystemType));
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
                if (defaultWindowInfo.AutoCreateWindow)
                {
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                    OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                    primaryWindow = new AutomaticWindow(ogreWindow);
                }
                else
                {
                    miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.WindowHandle);
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                    primaryWindow = new EmbeddedWindow(defaultWindowInfo.EmbedWindow, renderWindow);
                }
                defaultWindowInfo._fireWindowCreated(new WindowInfoEventArgs(primaryWindow));

                //Setup commands
                pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create Ogre Scene Manager", OgreSceneManagerDefinition.Create));
                pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Ogre Scene Node", SceneNodeDefinition.Create));

                //Setup shader cache
                if (LoadMicrocodeCacheCallback != null)
                {
                    GpuProgramManager.Instance.SaveMicrocodesToCache = LoadMicrocodeCacheCallback(rs, GpuProgramManager.Instance); ;
                }

                //Setup Resources
                SubsystemResources ogreResourcs = new SubsystemResources("Ogre");
                ogreResourcs.addResourceListener(OgreResourceManager.Instance);
                pluginManager.addSubsystemResources(ogreResourcs);

                //Setup the core resources
                OgreResourceGroupManager.getInstance().addResourceLocation(typeof(OgreInterface).AssemblyQualifiedName, "EmbeddedResource", "Bootstrap", true);
                OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

                //Add Shaders to built in resources
                using (MaterialPtr baseWhite = MaterialManager.getInstance().getByName("BaseWhite"))
                {
                    baseWhite.Value.getTechnique(0).getPass(0).setVertexProgram("colorvertex\\vs");
                    baseWhite.Value.getTechnique(0).getPass(0).setFragmentProgram("colorvertexwhite\\fs");
                }

                using (MaterialPtr baseWhiteNoLighting = MaterialManager.getInstance().getByName("BaseWhiteNoLighting"))
                {
                    baseWhiteNoLighting.Value.getTechnique(0).getPass(0).setVertexProgram("colorvertex\\vs");
                    baseWhiteNoLighting.Value.getTechnique(0).getPass(0).setFragmentProgram("colorvertexwhite\\fs");
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginException(String.Format("Exception initializing renderer. Message: {0}", e.Message), e);
            }
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

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            mainTimer.addFullSpeedUpdateListenerWithBackgrounding("Rendering", ogreUpdate);
        }

        public string getName()
        {
            return "OgrePlugin";
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
            if (defaultWindowInfo.AutoCreateWindow)
            {
                RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                primaryWindow = new AutomaticWindow(ogreWindow);
            }
            else
            {
                miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.WindowHandle);
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
            OSWindow embedWindow = windowInfo.EmbedWindow;
            if (embedWindow != null)
            {
                miscParams.Add("externalWindowHandle", embedWindow.WindowHandle);
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
        /// If this is set to a function before initialize is called this will instruct
        /// ogre to use the shader microcode cache. This function will be called at the appropriate
        /// time in the startup sequence to load the microcode cahce (your code must call the 
        /// GpuProgramManager.loadMicrocodeCahce function itself, that will be passed in the callback.
        /// 
        /// Return true from the callback to use the microcode cache, and false to not use it.
        /// </summary>
        public static LoadMicrocodeCacheDelegate LoadMicrocodeCacheCallback { get; set; }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OgreInterface_LoadRenderSystem(RenderSystemType rendersystemType);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreInterface_UnloadRenderSystem(IntPtr renderSystemPlugin);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OgreInterface_GetRenderSystem(RenderSystemType rendersystemType);

        #endregion
    }
}

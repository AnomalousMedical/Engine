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

namespace OgrePlugin
{
    /// <summary>
    /// The main interface class for the OgrePlugin.
    /// </summary>
    public class OgreInterface : RendererPlugin
    {
        #region Static

        public const String PluginName = "OgrePlugin";
        private static OgreInterface instance;

        public static bool FoundOgreCore { get; private set; }

        public static OgreInterface Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion Static

        #region Fields

        private Root root;
        OgreLogConnection ogreLog;
        private OgreUpdate ogreUpdate;
        private OgreWindow primaryWindow;

        #endregion Fields

        #region Delegates

        private delegate OgreSceneManagerDefinition CreateSceneManagerDefinition(String name);
        private delegate SceneNodeDefinition CreateSceneNodeDefinition(String name);
        private delegate void AddResourceLocation(String name, String locType, String group, bool recursive);
        private delegate void InitializeResourceGroups();

        #endregion Delegates

        #region Constructors

        public OgreInterface()
        {
            if (instance == null)
            {
                root = new Root("", "", "");
                ogreLog = new OgreLogConnection();
                ogreUpdate = new OgreUpdate(root);
                instance = this;
            }
            else
            {
                throw new InvalidPluginException("The OgrePlugin plugin can only be initialized one time.");
            }
        }

        #endregion Constructors

        #region Functions

        public void Dispose()
        {
            MaterialManager.getInstance().Dispose();
            MeshManager.getInstance().Dispose();
            SkeletonManager.getInstance().Dispose();
            HardwareBufferManager.getInstance().Dispose();
            destroyRendererWindow(primaryWindow);
            root.Dispose();
        }

        public void initialize(PluginManager pluginManager)
        {
            DefaultWindowInfo defaultWindowInfo;
            pluginManager.setRendererPlugin(this, out defaultWindowInfo);
            new OgreConfig(pluginManager.ConfigFile);

            try
            {
                //Initialize Ogre
                root.loadPlugin("RenderSystem_Direct3D9");
                RenderSystem rs = root.getRenderSystemByName("Direct3D9 Rendering Subsystem");
                String valid = rs.validateConfigOptions();
                if (valid.Length != 0)
                {
                    throw new InvalidPluginException(String.Format("Invalid Ogre configuration {0}", valid));
                }
                root.setRenderSystem(rs);
                root.initialize(false);

                root.loadPlugin("Plugin_CgProgramManager");

                //Create the default window.
                Dictionary<String, String> miscParams = new Dictionary<string, string>();
                miscParams.Add("FSAA", OgreConfig.FSAA.ToString());
                miscParams.Add("vsync", OgreConfig.VSync.ToString());
                if (defaultWindowInfo.AutoCreateWindow)
                {
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen);
                    OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                    primaryWindow = new AutomaticWindow(ogreWindow);
                }
                else
                {
                    miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.WindowHandle.ToInt32().ToString());
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                    primaryWindow = new EmbeddedWindow(defaultWindowInfo.EmbedWindow, renderWindow);
                }

                //Setup commands
                pluginManager.addCreateSimElementManagerCommand(new AddSimElementManagerCommand("Create Ogre Scene Manager", OgreSceneManagerDefinition.Create));

                pluginManager.addCreateSimElementCommand(new AddSimElementCommand("Create Ogre Scene Node", SceneNodeDefinition.Create));

                pluginManager.addOtherCommand(new EngineCommand("addResourceLocation", "Add Ogre Resource Location", "Add a resource location to ogre.", new AddResourceLocation(OgreResourceGroupManager.getInstance().addResourceLocation)));
                pluginManager.addOtherCommand(new EngineCommand("initializeResourceGroups", "Initialize Ogre Resources", "Initialize all added ogre resources.", new InitializeResourceGroups(OgreResourceGroupManager.getInstance().initializeAllResourceGroups)));

                //Setup Resources
                SubsystemResources ogreResourcs = new SubsystemResources("Ogre");
                ogreResourcs.addResourceListener(OgreResourceManager.Instance);
                pluginManager.addSubsystemResources(ogreResourcs);

                //Setup the core resources
                FoundOgreCore = File.Exists("OgreCore.zip");
                if (FoundOgreCore)
                {
                    Log.Default.sendMessage("Found OgreCore.zip. Debug resources available.", LogLevel.ImportantInfo, PluginName);
                    OgreResourceGroupManager.getInstance().addResourceLocation("OgreCore.zip", "Zip", "Internal", true);
                    OgreResourceGroupManager.getInstance().initializeAllResourceGroups();
                }
                else
                {
                    Log.Default.sendMessage("Could not find OgreCore.zip. Ogre debug resources not available.", LogLevel.Warning, PluginName);
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginException(String.Format("Exception initializing renderer. Message: {0}", e.Message));
            }
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            mainTimer.addFullSpeedUpdateListener(ogreUpdate);
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

        public RendererWindow createRendererWindow(OSWindow embedWindow, String name)
        {
            Dictionary<String, String> miscParams = new Dictionary<string, string>();
            miscParams.Add("FSAA", OgreConfig.FSAA.ToString());
            miscParams.Add("vsync", OgreConfig.VSync.ToString());
            miscParams.Add("externalWindowHandle", embedWindow.WindowHandle.ToInt32().ToString());
            RenderWindow renderWindow = root.createRenderWindow(name, (uint)embedWindow.WindowWidth, (uint)embedWindow.WindowHeight, false, miscParams);
            return new EmbeddedWindow(embedWindow, renderWindow);
        }

        public void destroyRendererWindow(RendererWindow window)
        {
            OgreWindow ogreWindow = window as OgreWindow;
            if (ogreWindow != null)
            {
                root.detachRenderTarget(ogreWindow.OgreRenderWindow);
                ogreWindow.Dispose();
            }
            else
            {
                Log.Default.sendMessage("Error destroying RendererWindow {0}. It is not a recognized OgreWindow. The window has not been destroyed.", LogLevel.Warning, "OgrePlugin", window.ToString());
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

        #endregion Functions
    }
}

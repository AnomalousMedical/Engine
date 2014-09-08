using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using OgreWrapper;
using OgrePlugin;
using Logging;
using Engine.Renderer;

namespace MyGUIPlugin
{
    public class MyGUIInterface : PluginInterface
    {
        internal const String LibraryName = "MyGUIWrapper";
        public static MyGUIInterface Instance { get; private set; }

        private OgrePlatform ogrePlatform;
        private Gui gui;
        private SceneManager sceneManager;
        private OgreWindow ogreWindow;
        Camera camera;
        Viewport vp;

        private UpdateTimer mainTimer;
        private MyGUIUpdate myGUIUpdate;
        private MyGUIRenderListener renderListener;
        private ManagedMyGUILogListener managedLogListener;
        private Engine.Resources.ResourceManager resources;

        public MyGUIInterface()
        {
            if (Instance == null)
            {
                Instance = this;
                SmoothShowDuration = 0.25f;
                CreateGuiGestures = false;
            }
            else
            {
                throw new Exception("Can only create MyGUIInterface one time.");
            }
        }

        public void Dispose()
        {
            if(mainTimer != null)
            {
                mainTimer.removeUpdateListener(myGUIUpdate);
            }
            if (vp != null)
            {
                ogreWindow.OgreRenderTarget.destroyViewport(vp);
            }
            if (camera != null)
            {
                sceneManager.destroyCamera(camera);
            }
            if(gui != null)
            {
                gui.shutdown();
                gui.Dispose();
            }
            if(ogrePlatform != null)
            {
                ogrePlatform.shutdown();
                ogrePlatform.Dispose();
            }
            if(sceneManager != null)
            {
                Root.getSingleton().destroySceneManager(sceneManager);
            }
            if (managedLogListener != null)
            {
                managedLogListener.Dispose();
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            if(EventLayerKey == null)
            {
                throw new ArgumentNullException("EventLayerKey", "EventLayerKey property must be set before initializing MyGUIPlugin.");
            }
        }

        public void link(PluginManager pluginManager)
        {
            Log.Info("Initializing MyGUI");

            resources = pluginManager.createLiveResourceManager("MyGUI");
            var rendererResources = resources.getSubsystemResource("Ogre");
            CommonResourceGroup = rendererResources.addResourceGroup("Common");
            CommonResourceGroup.addResource(GetType().AssemblyQualifiedName, "EmbeddedScalableResource", true);
            resources.initializeResources();

            sceneManager = Root.getSingleton().createSceneManager(SceneType.ST_GENERIC, "MyGUIScene");
            ogreWindow = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;

            //Create camera and viewport
            camera = sceneManager.createCamera("MyGUICamera");
            vp = ogreWindow.OgreRenderTarget.addViewport(camera, ViewportZIndex, 0.0f, 0.0f, 1.0f, 1.0f);
            vp.setBackgroundColor(new Color(1.0f, 0.0f, 1.0f, 0.0f));
            vp.setClearEveryFrame(false);
            vp.clear();

            //Create Ogre Platform
            ogrePlatform = new OgrePlatform();
            ogrePlatform.initialize(vp.getActualWidth(), vp.getActualHeight(), CommonResourceGroup.FullName, LogFile);

            //Create log
            managedLogListener = new ManagedMyGUILogListener();

            renderListener = new MyGUIRenderListener(vp, sceneManager, ogrePlatform.getRenderManager());

            gui = new Gui();
            gui.ScaleFactor = ScaleHelper.ScaleFactor;
            gui.initialize("");

            //Load config files
            ResourceManager resourceManager = ResourceManager.Instance;
            if (!String.IsNullOrEmpty(OSTheme))
            {
                resourceManager.load(OSTheme);
            }
            resourceManager.load(MainTheme);
            resourceManager.load(PointerFile);
            resourceManager.load(LayerFile);
            resourceManager.load(MessageBoxTheme);

            Log.Info("Finished initializing MyGUI");
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
            myGUIUpdate = new MyGUIUpdate(gui, eventManager[EventLayerKey]);
            mainTimer.addUpdateListener(myGUIUpdate);
            if (CreateGuiGestures)
            {
                eventManager.addEvent(new GuiGestures(EventLayerKey));
            }
        }

        public string Name
        {
            get
            {
                return "MyGUIPlugin";
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

        public OgrePlatform OgrePlatform
        {
            get
            {
                return ogrePlatform;
            }
        }

        /// <summary>
        /// The ResourceGroup for common resources for MyGUI. It is reccomended to add all MyGUI related resources here, since the subystem
        /// will search this group first.
        /// </summary>
        public Engine.Resources.ResourceGroup CommonResourceGroup { get; private set; }

        static MyGUIInterface()
        {
            LogFile = "";
            OSTheme = DefaultWindowsTheme;
            MainTheme = DefaultMainTheme;
            MessageBoxTheme = DefaultMessageBoxTheme;
            LayerFile = "MyGUIPlugin_Layer.xml";
            PointerFile = "MyGUIPlugin_Pointer.xml";
            ViewportZIndex = 1000000;
        }

        public static readonly String DefaultWindowsTheme = "MyGUIPlugin_Windows.xml";
        public static readonly String DefaultOSXTheme = "MyGUIPlugin_OSX.xml";
        public static readonly String DefaultMainTheme = "MyGUIPlugin_Main.xml";
        public static readonly String DefaultMessageBoxTheme = "MyGUIPlugin.Resources.MessageBox.MessageBoxResources.xml";

        /// <summary>
        /// The log file location for MyGUI. Set before initializing.
        /// </summary>
        public static String LogFile { get; set; }

        /// <summary>
        /// The OS Theme MyGUI. Set before initializing.
        /// Changes stuff depending on the os like window close button alignment
        /// and other things in the main theme.
        /// </summary>
        public static String OSTheme { get; set; }

        /// <summary>
        /// The main theme file to load. This is loaded after the OSTheme and
        /// will contain common items to all themes.
        /// </summary>
        public static String MainTheme { get; set; }

        public static String LayerFile { get; set; }

        public static String PointerFile { get; set; }

        /// <summary>
        /// The message box theme file to load. This is loaded after the Main Theme.
        /// </summary>
        public static String MessageBoxTheme { get; set; }

        /// <summary>
        /// The amount of time Smooth Show transitions should take.
        /// </summary>
        public static float SmoothShowDuration { get; set; }

        /// <summary>
        /// The Z-Index in ogre of the viewport.
        /// </summary>
        public static int ViewportZIndex { get; set; }

        /// <summary>
        /// Set this to an EventLayerKey object (likely an enum, this is application defined) before initializing this plugin.
        /// </summary>
        public static Object EventLayerKey { get; set; }

        /// <summary>
        /// Set this to true to create multitouch gestures for the gui.
        /// </summary>
        public static bool CreateGuiGestures { get; set; }

        /// <summary>
        /// This event is fired before MyGUI renders.
        /// </summary>
        public event EventHandler RenderStarted
        {
            add
            {
                renderListener.RenderStarted += value;
            }
            remove
            {
                renderListener.RenderStarted -= value;
            }
        }

        /// <summary>
        /// This event is fired after MyGUI renders.
        /// </summary>
        public event EventHandler RenderEnded
        {
            add
            {
                renderListener.RenderEnded += value;
            }
            remove
            {
                renderListener.RenderEnded -= value;
            }
        }
    }
}

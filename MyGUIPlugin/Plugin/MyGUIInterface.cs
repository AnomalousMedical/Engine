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

        public MyGUIInterface()
        {
            if (Instance == null)
            {
                Instance = this;
                SmoothShowDuration = 0.25f;
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
                mainTimer.removeFixedUpdateListener(myGUIUpdate);
            }
            if (vp != null)
            {
                ogreWindow.OgreRenderWindow.destroyViewport(vp);
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
            Log.Info("Initializing MyGUI");

            OgreResourceGroupManager.getInstance().addResourceLocation(GetType().AssemblyQualifiedName, "EmbeddedResource", "MyGUI", true);
            OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

            sceneManager = Root.getSingleton().createSceneManager(SceneType.ST_GENERIC, "MyGUIScene");
            ogreWindow = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;

            //Create camera and viewport
            camera = sceneManager.createCamera("MyGUICamera");
            vp = ogreWindow.OgreRenderWindow.addViewport(camera, int.MaxValue, 0.0f, 0.0f, 1.0f, 1.0f);
            vp.setBackgroundColor(new Color(1.0f, 0.0f, 0.0f, 0.0f));
            vp.setClearEveryFrame(false);
            vp.clear();

            //Create Ogre Platform
            ogrePlatform = new OgrePlatform();
            ogrePlatform.initialize(ogreWindow.OgreRenderWindow, sceneManager, "MyGUI", "");

            //Create log
            managedLogListener = new ManagedMyGUILogListener();

            renderListener = new MyGUIRenderListener(vp, sceneManager);

            gui = new Gui();
            gui.initialize("");

            //Load config files
            ResourceManager resourceManager = ResourceManager.Instance;
            resourceManager.load(Theme);
            resourceManager.load(Language);
            resourceManager.load(Font);
            resourceManager.load(Resource);
            resourceManager.load(Skin);
            resourceManager.load("MyGUIPlugin.Resources.core_skin_legacy.xml");
            resourceManager.load("MyGUIPlugin.Resources.core_skin_template.xml");
            resourceManager.load("MyGUIPlugin.Resources.MessageBox.MessageBoxResources.xml");
            resourceManager.load("MyGUIPlugin.Resources.custom_skin.xml");
            resourceManager.load("MyGUIPlugin.Resources.custom_skin_template.xml");
            resourceManager.load(Pointer);
            resourceManager.load(Layer);
            resourceManager.load(Settings);

            Log.Info("Finished initializing MyGUI");
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            this.mainTimer = mainTimer;
            myGUIUpdate = new MyGUIUpdate(gui, eventManager);
            mainTimer.addFixedUpdateListener(myGUIUpdate);
        }

        public string getName()
        {
            return "MyGUIPlugin";
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public void destroyViewport()
        {
            ogreWindow.OgreRenderWindow.destroyViewport(vp);
            vp = null;
        }

        public void recreateViewport(RendererWindow window)
        {
            ogreWindow = window as OgreWindow;
            ogrePlatform.getRenderManager().setRenderWindow(ogreWindow.OgreRenderWindow);
            vp = ogreWindow.OgreRenderWindow.addViewport(camera, int.MaxValue, 0.0f, 0.0f, 1.0f, 1.0f);
            vp.setBackgroundColor(new Color(1.0f, 0.0f, 0.0f, 0.0f));
            vp.setClearEveryFrame(false);
        }

        public OgrePlatform OgrePlatform
        {
            get
            {
                return ogrePlatform;
            }
        }

        static MyGUIInterface()
        {
            LogFile = "MyGUI.log";
            Theme = "MyGUIPlugin.Resources.core_theme.xml";
            Language = "MyGUIPlugin.Resources.core_language.xml";
            Font = "MyGUIPlugin.Resources.core_font.xml";
            Resource = "MyGUIPlugin.Resources.core_resource.xml";
            Skin = "MyGUIPlugin.Resources.core_skin.xml";
            Pointer = "MyGUIPlugin.Resources.core_pointer.xml";
            Layer = "MyGUIPlugin.Resources.core_layer.xml";
            Settings = "MyGUIPlugin.Resources.core_settings.xml";
        }

        /// <summary>
        /// The log file location for MyGUI. Set before initializing.
        /// </summary>
        public static String LogFile { get; set; }

        /// <summary>
        /// The file to load the theme tags for MyGUI. Set before initializing.
        /// </summary>
        public static String Theme { get; set; }

        /// <summary>
        /// The file to load the language for MyGUI. Set before initializing.
        /// </summary>
        public static String Language { get; set; }

        /// <summary>
        /// The file to load the fonts for MyGUI. Set before initializing.
        /// </summary>
        public static String Font { get; set; }

        /// <summary>
        /// The file to load the resources for MyGUI. Set before initializing.
        /// </summary>
        public static String Resource { get; set; }

        /// <summary>
        /// The file to load the skin settings for MyGUI. Set before initializing.
        /// </summary>
        public static String Skin { get; set; }

        /// <summary>
        /// The file to load pointer info for MyGUI. Set before initializing.
        /// </summary>
        public static String Pointer { get; set; }

        /// <summary>
        /// The file to load layer info for MyGUI. Set before initializing.
        /// </summary>
        public static String Layer { get; set; }

        /// <summary>
        /// The file to load settings for MyGUI. Set before initializing.
        /// </summary>
        public static String Settings { get; set; }

        /// <summary>
        /// The amount of time Smooth Show transitions should take.
        /// </summary>
        public static float SmoothShowDuration { get; set; }

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

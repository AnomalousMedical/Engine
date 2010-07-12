using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using OgreWrapper;
using OgrePlugin;
using Logging;

namespace MyGUIPlugin
{
    public class MyGUIInterface : PluginInterface
    {
        private OgrePlatform ogrePlatform;
        private Gui gui;
        private SceneManager sceneManager;
        private OgreWindow ogreWindow;
        Camera camera;
        Viewport vp;

        private UpdateTimer mainTimer;
        private MyGUIUpdate myGUIUpdate;
        private MyGUIRenderListener renderListener;

        public MyGUIInterface()
        {
            
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
        }

        public void initialize(PluginManager pluginManager)
        {
            Log.Info("Initializing MyGUI");

            OgreResourceGroupManager.getInstance().addResourceLocation("GUI/MyGUI", "EngineArchive", "MyGUI", true);
            OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

            sceneManager = Root.getSingleton().createSceneManager(SceneType.ST_GENERIC, "MyGUIScene");
            ogreWindow = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;
            ogrePlatform = new OgrePlatform();
            ogrePlatform.initialize(ogreWindow.OgreRenderWindow, sceneManager, "MyGUI", LogFile);

            //Create camera and viewport
            camera = sceneManager.createCamera("MyGUICamera");
            vp = ogreWindow.OgreRenderWindow.addViewport(camera, int.MaxValue, 0.0f, 0.0f, 1.0f, 1.0f);
            vp.setBackgroundColor(new Color(1.0f, 0.0f, 0.0f, 0.0f));
            vp.setClearEveryFrame(false);

            renderListener = new MyGUIRenderListener(vp, sceneManager);

            gui = new Gui();
            gui.initialize("", LogFile);

            //Load config files
            gui.load(Theme);
            gui.load(Language);
            gui.load(Font);
            gui.load(Resource);
            gui.load(Skin);
            gui.load(Pointer);
            gui.load(Layer);
            gui.load(Settings);

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
            Theme = "core_theme.xml";
            Language = "core_language.xml";
            Font = "core_font.xml";
            Resource = "core_resource.xml";
            Skin = "core_skin.xml";
            Pointer = "core_pointer.xml";
            Layer = "core_layer.xml";
            Settings = "core_settings.xml";
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

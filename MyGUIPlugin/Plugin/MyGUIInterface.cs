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

            gui = new Gui();
            gui.initialize("core.xml", LogFile);

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
        }

        public static String LogFile { get; set; }
    }
}

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

        public MyGUIInterface()
        {

        }

        public void Dispose()
        {
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

            //OgreResourceGroupManager.getInstance().addResourceLocation("S:/junk/MyGUI3.0/Media", "FileSystem", "MyGUI", true);
            //OgreResourceGroupManager.getInstance().addResourceLocation("S:/junk/MyGUI3.0/Media/MyGUI_Media", "FileSystem", "MyGUI", true);
            //OgreResourceGroupManager.getInstance().addResourceLocation("S:/export/GUI/MyGUI", "FileSystem", "MyGUI", true);

            OgreResourceGroupManager.getInstance().addResourceLocation("GUI/MyGUI", "EngineArchive", "MyGUI", true);
            OgreResourceGroupManager.getInstance().initializeAllResourceGroups();

            sceneManager = Root.getSingleton().createSceneManager(SceneType.ST_GENERIC, "MyGUIScene");
            ogreWindow = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;
            ogrePlatform = new OgrePlatform();
            ogrePlatform.initialize(ogreWindow.OgreRenderWindow, sceneManager, "MyGUI");
            gui = new Gui();
            gui.initialize("core.xml", "MyGUI.log");

            Log.Info("Finished initializing MyGUI");
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
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
    }
}

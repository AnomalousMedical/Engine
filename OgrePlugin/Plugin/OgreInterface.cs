using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using OgreWrapper;
using Engine.Renderer;
using Logging;
using Engine.Platform;

namespace OgrePlugin
{
    public class OgreInterface : RendererPlugin
    {
        #region Fields

        private Root root;
        OgreLogConnection ogreLog;
        private OgreUpdate ogreUpdate;
        private OgreWindow primaryWindow;
        private CommandManager elementManagerCommands = new CommandManager();
        private CommandManager elementDefinitonCommands = new CommandManager();

        #endregion Fields

        #region Delegates

        private delegate OgreSceneManagerDefinition CreateSceneManagerDefinition(String name);

        #endregion Delegates

        #region Constructors

        public OgreInterface()
        {
            root = new Root("", "", "");
            ogreLog = new OgreLogConnection();
            ogreUpdate = new OgreUpdate(root);
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
                if (defaultWindowInfo.AutoCreateWindow)
                {
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen);
                    OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                    primaryWindow = new AutomaticWindow(ogreWindow);
                }
                else
                {
                    Dictionary<String, String> miscParams = new Dictionary<string, string>();
                    miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.Handle.ToInt32().ToString());
                    RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                    primaryWindow = new EmbeddedWindow(defaultWindowInfo.EmbedWindow, renderWindow);
                }

                //Setup commands
                elementManagerCommands.addCommand(new EngineCommand("createOgreSceneManager", "Create Ogre Scene Manager", "Creates a new PhysX scene definition.", new CreateSceneManagerDefinition(createSceneManagerDefinition)));

                foreach (EngineCommand command in elementManagerCommands.getCommandList())
                {
                    pluginManager.addCreateSimElementManagerCommand(command);
                }

                foreach (EngineCommand command in elementDefinitonCommands.getCommandList())
                {
                    pluginManager.addCreateSimElementCommand(command);
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginException(String.Format("Exception initializing renderer. Message: {0}", e.Message));
            }
        }

        public void setPlatformInfo(Timer mainTimer, EventManager eventManager)
        {
            mainTimer.addFullSpeedUpdateListener(ogreUpdate);
        }

        public string getName()
        {
            return "OgrePlugin";
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
            miscParams.Add("externalWindowHandle", embedWindow.Handle.ToInt32().ToString());
            RenderWindow renderWindow = root.createRenderWindow(name, (uint)embedWindow.Width, (uint)embedWindow.Height, false, miscParams);
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

        #region Create Functions

        public OgreSceneManagerDefinition createSceneManagerDefinition(String name)
        {
            return new OgreSceneManagerDefinition(name);
        }

        #endregion Create Functions

        #endregion Functions
    }
}

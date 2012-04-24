using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Platform;
using System.Runtime.InteropServices;
using OgreWrapper;
using OgrePlugin;

namespace libRocketPlugin
{
    public class RocketInterface : PluginInterface
    {
        private SceneManager sceneManager;
        private OgreWindow ogreWindow;
        private Camera camera;
        private Viewport vp;
        private ManagedSystemInterface systemInterface;
        private RenderInterfaceOgre3D renderInterface;

        //test
        private Context context;
        private ContextUpdater contextUpdater;
        private EventInstancer eventInstancer;
        private EventListenerInstancer eventListenerInstancer;
        //end test

        public RocketInterface()
        {
            
        }

        public void Dispose()
        {
            if (contextUpdater != null)
            {
                contextUpdater.Dispose();
            }
            if (context != null)
            {
                context.Dispose();
            }
            Core.Shutdown();
            if (renderInterface != null)
            {
                renderInterface.Dispose();
            }
            ReferenceCountable.DumpLeakReport();
            if (systemInterface != null)
            {
                systemInterface.Dispose();
            }
            if (vp != null)
            {
                ogreWindow.OgreRenderWindow.destroyViewport(vp);
            }
            if (camera != null)
            {
                sceneManager.destroyCamera(camera);
            }
            if (sceneManager != null)
            {
                Root.getSingleton().destroySceneManager(sceneManager);
            }
        }

        public void initialize(PluginManager pluginManager)
        {
            sceneManager = Root.getSingleton().createSceneManager(SceneType.ST_GENERIC, "libRocketScene");
            ogreWindow = pluginManager.RendererPlugin.PrimaryWindow as OgreWindow;

            //Create camera and viewport
            camera = sceneManager.createCamera("libRocketCamera");
            vp = ogreWindow.OgreRenderWindow.addViewport(camera, ViewportZIndex, 0.0f, 0.0f, 1.0f, 1.0f);
            vp.setBackgroundColor(new Color(1.0f, 0.0f, 0.0f, 0.0f));
            vp.setOverlaysEnabled(false);
            vp.setClearEveryFrame(false);
            vp.clear();

            //Create a rocket group in ogre
            OgreResourceGroupManager.getInstance().createResourceGroup("Rocket");

            systemInterface = new ManagedSystemInterface();
            renderInterface = new RenderInterfaceOgre3D((int)ogreWindow.OgreRenderWindow.getWidth(), (int)ogreWindow.OgreRenderWindow.getHeight());

            Core.SetSystemInterface(systemInterface);
            Core.SetRenderInterface(renderInterface);

            Core.Initialise();
            Controls.Initialise();

            //test
            //eventInstancer = new EventInstancer();
            //Factory.RegisterEventInstancer(eventInstancer);
            eventListenerInstancer = new EventListenerInstancer();
            Factory.RegisterEventListenerInstancer(eventListenerInstancer);

            String sample_path = "S:/Junk/librocket/playing/";//"S:/dependencies/libRocket/src/Samples/";
            OgreResourceGroupManager.getInstance().addResourceLocation(sample_path, "FileSystem", "Rocket", false);

            FontDatabase.LoadFontFace(sample_path + "assets/Delicious-Roman.otf");
            FontDatabase.LoadFontFace(sample_path + "assets/Delicious-Bold.otf");
            FontDatabase.LoadFontFace(sample_path + "assets/Delicious-Italic.otf");
            FontDatabase.LoadFontFace(sample_path + "assets/Delicious-BoldItalic.otf");

            context = Core.CreateContext("main", new Vector2i((int)ogreWindow.OgreRenderWindow.getWidth(), (int)ogreWindow.OgreRenderWindow.getHeight()));
            Debugger.Initialise(context);

            //using (ElementDocument cursor = context.LoadMouseCursor(sample_path + "assets/cursor.rml"))
            //{

            //}

            using (ElementDocument document = context.LoadDocument(sample_path + "assets/demo.rml"))
            {
                if (document != null)
                {
                    document.Show();
                }
            }

            sceneManager.addRenderQueueListener(new RocketRenderQueueListener(context, renderInterface));
            //End test
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            systemInterface.Timer = mainTimer;
            //test
            contextUpdater = new ContextUpdater(context, eventManager);
            mainTimer.addFixedUpdateListener(contextUpdater);
            //end test
        }

        public string getName()
        {
            return "libRocketPlugin";
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void createDebugCommands(List<CommandManager> commands)
        {

        }

        static RocketInterface()
        {
            ViewportZIndex = 2000000;
        }

        public static int ViewportZIndex { get; set; }
    }
}

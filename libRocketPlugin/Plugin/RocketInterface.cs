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
        private Context context;

        public RocketInterface()
        {
            
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
            if (rocketTest != null)
            {
                libRocketTest_Delete(rocketTest);
            }
            Core.Shutdown();
            ReferenceCountable.DumpLeakReport();
            //if (mainTimer != null)
            //{
            //    mainTimer.removeFixedUpdateListener(myGUIUpdate);
            //}
            if (renderInterface != null)
            {
                renderInterface.Dispose();
            }
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

            //Not sure about this, i think it is just making the group
            OgreResourceGroupManager.getInstance().createResourceGroup("Rocket");
            OgreResourceGroupManager.getInstance().addResourceLocation("S:/Junk/librocket/", "FileSystem", "Rocket", false);

            systemInterface = new ManagedSystemInterface();
            renderInterface = new RenderInterfaceOgre3D((int)ogreWindow.OgreRenderWindow.getWidth(), (int)ogreWindow.OgreRenderWindow.getHeight());

            Core.SetSystemInterface(systemInterface);
            Core.SetRenderInterface(renderInterface);

            Core.Initialise();
            Controls.Initialise();

            String sample_path = "S:/Junk/librocket/libRocket/Samples/";

            FontDatabase.LoadFontFace(sample_path + "assets/Delicious-Roman.otf");
	        FontDatabase.LoadFontFace(sample_path + "assets/Delicious-Bold.otf");
	        FontDatabase.LoadFontFace(sample_path + "assets/Delicious-Italic.otf");
	        FontDatabase.LoadFontFace(sample_path + "assets/Delicious-BoldItalic.otf");

            context = Core.CreateContext("main", new Vector2i((int)ogreWindow.OgreRenderWindow.getWidth(), (int)ogreWindow.OgreRenderWindow.getHeight()));
            Debugger.Initialise(context);

            //Rocket::Core::ElementDocument* cursor = context->LoadMouseCursor(sample_path + "assets/cursor.rml");
            //if (cursor)
            //    cursor->RemoveReference();

            using (ElementDocument document = context.LoadDocument(sample_path + "assets/demo.rml"))
            {
                if (document != null)
                {
                    document.show();
                    //document.removeReference();
                }
            }

            rocketTest = libRocketTest_Create(renderInterface.Ptr, systemInterface.Ptr, context.Ptr);

            sceneManager.addRenderQueueListener(new RocketQueueListener(this));
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            systemInterface.Timer = mainTimer;
            //mainTimer.addFullSpeedUpdateListener(new RocketUpdate(this));
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

        #region Temp

        IntPtr rocketTest;

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr libRocketTest_Create(IntPtr renderInterface, IntPtr systemInterface, IntPtr con);

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern void libRocketTest_Delete(IntPtr test);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void libRocketTest_Render(IntPtr test, byte queueGroup);

        private class RocketQueueListener : RenderQueueListener
        {
            private RocketInterface rocketInterface;

            public RocketQueueListener(RocketInterface rocketInterface)
            {
                this.rocketInterface = rocketInterface;
            }

            public void preRenderQueues()
            {
                
            }

            public void postRenderQueues()
            {
                
            }

            public void renderQueueStarted(byte queueGroupId, string invocation, ref bool skipThisInvocation)
            {
                libRocketTest_Render(rocketInterface.rocketTest, queueGroupId);
            }

            public void renderQueueEnded(byte queueGroupId, string invocation, ref bool repeatThisInvocation)
            {
                
            }
        }

        #endregion
    }
}

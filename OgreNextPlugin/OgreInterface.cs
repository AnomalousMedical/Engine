using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Renderer;
using Logging;
using Engine.Platform;
using Engine.Command;
using Engine.Resources;
using System.IO;
using Engine.ObjectManagement;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OgreNextPlugin.Wrapper.Renderer;

namespace OgreNextPlugin
{
    public enum RenderSystemType
    {
        Default = 0,
        D3D11 = 1,
        OpenGL = 2
    };

    [Flags]
    public enum CompressedTextureSupport : uint
    {
        None = 0,
        DXT = 1,
        PVRTC = 1 << 1,
        ATC = 1 << 2,
        ETC2 = 1 << 3,
        BC4_BC5 = 1 << 4,
        DXT_BC4_BC5 = DXT | BC4_BC5,
        All = DXT | PVRTC | ATC | ETC2 | BC4_BC5
    }

    /// <summary>
    /// The main interface class for the OgrePlugin.
    /// </summary>
    public class OgreInterface : RendererPlugin
    {
        public const String PluginName = "OgreNextPlugin";
        private OgreWindow primaryWindow;
        private RenderSystemType chosenRenderSystem;
        private IntPtr renderSystemPlugin;
        private Root root;
        private RenderSystem rs;
        private OgreUpdate ogreUpdate;
        private ResourceManager engineResourceManager;

        /// <summary>
        /// Fired when the OgreInterface is disposed, which means that ogre has been shutdown (Ogre::Root deleted).
        /// </summary>
        public event Action<OgreInterface> Disposed;

        public OgreInterface()
        {
            
        }

        public void Dispose()
        {
            //var ogreSubsystem = engineResourceManager.getSubsystemResource("Ogre");
            //ogreSubsystem.removeResourceGroup("Internal");
            //engineResourceManager.initializeResources();

            root?.Dispose();
            OgreInterface_UnloadRenderSystem(renderSystemPlugin);
            if (Disposed != null)
            {
                Disposed.Invoke(this);
            }
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            //Load config
            new OgreConfig(pluginManager.ConfigFile);
            chosenRenderSystem = OgreConfig.RenderSystemType;

            //Setup ogre root
            root = new Root("", "", "");
            ogreUpdate = new OgreUpdate(root);
            renderSystemPlugin = OgreInterface_LoadRenderSystem(ref chosenRenderSystem);            

            //Setup engine
            WindowInfo defaultWindowInfo;
            pluginManager.setRendererPlugin(this, out defaultWindowInfo);

            //Initialize Ogre
            rs = root._getRenderSystemWrapper(OgreInterface_GetRenderSystem(ref chosenRenderSystem));
            rs.setConfigOption("sRGB Gamma Conversion", "Yes");
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
            if (spaceIndex != -1)
            {
                fsaa = fsaa.Substring(0, spaceIndex);
            }
            miscParams.Add("FSAA", fsaa);
            miscParams.Add("vsync", OgreConfig.VSync.ToString());
            miscParams.Add("monitorIndex", defaultWindowInfo.MonitorIndex.ToString());
            miscParams.Add("useNVPerfHUD", OgreConfig.UseNvPerfHUD.ToString());
            miscParams.Add("contentScalingFactor", defaultWindowInfo.ContentScalingFactor.ToString());
            RenderWindow renderWindow;
            if (defaultWindowInfo.AutoCreateWindow)
            {
                throw new NotImplementedException();
                //RenderWindow renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                //OgreOSWindow ogreWindow = new OgreOSWindow(renderWindow);
                //primaryWindow = new AutomaticWindow(ogreWindow);
            }
            else
            {
                miscParams.Add("externalWindowHandle", defaultWindowInfo.EmbedWindow.WindowHandle.ToString());
                renderWindow = root.createRenderWindow(defaultWindowInfo.AutoWindowTitle, (uint)defaultWindowInfo.Width, (uint)defaultWindowInfo.Height, defaultWindowInfo.Fullscreen, miscParams);
                primaryWindow = new EmbeddedWindow(defaultWindowInfo.EmbedWindow, renderWindow);
            }

            //Setup Hlms, must come after primary window creation
            HlmsManager.setup("." + Path.DirectorySeparatorChar);

            //temp test scene
            var sceneManager = root.createSceneManager(SceneType.ST_GENERIC, 1);
            var camera = sceneManager.createCamera("Main Camera");

            // Position it at 500 in Z direction
            camera.setPosition(new Vector3(0, 5, 15));
            // Look back along -Z
            camera.lookAt(new Vector3(0, 0, 0));
            camera.setNearClipDistance(0.2f);
            camera.setFarClipDistance(1000.0f);
            camera.setAutoAspectRatio(true);

            // Setup a basic compositor with a blue clear colour
            CompositorManager2 compositorManager = root.CompositorManager2;
            var workspaceName = "Demo Workspace";
            var backgroundColour = new Color(0.2f, 0.4f, 0.6f);
            compositorManager.createBasicWorkspaceDef(workspaceName, backgroundColour);
            compositorManager.addWorkspace(sceneManager, renderWindow.Texture, camera, workspaceName, true);

            //end temp test scene
        }

        public void link(PluginManager pluginManager)
        {
            //engineResourceManager = pluginManager.createLiveResourceManager("OgrePlugin");
            //var ogreSubsystem = engineResourceManager.getSubsystemResource("Ogre");
            //var group = ogreSubsystem.addResourceGroup("Internal");
            //group.addResource(typeof(OgreInterface).AssemblyQualifiedName, "EmbeddedResource", true);
            //engineResourceManager.initializeResources();
        }

        /// <summary>
        /// This function will create any debug commands for the plugin and add them to the commands list.
        /// </summary>
        /// <param name="commands">A list of CommandManagers to add debug commands to.</param>
        public void createDebugCommands(List<CommandManager> commands)
        {

        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {

        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            mainTimer.addUpdateListenerWithBackgrounding("Rendering", ogreUpdate);
        }

        public string Name
        {
            get
            {
                return PluginName;
            }
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
            throw new NotImplementedException();
        }

        public RendererWindow createRendererWindow(WindowInfo windowInfo)
        {
            throw new NotImplementedException();
        }

        public void destroyRendererWindow(RendererWindow window)
        {
            
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
            return null;
        }

        /// <summary>
        /// Destroy a DebugDrawingSurface. This should be called before the
        /// scene it was created in is destroyed.
        /// </summary>
        /// <param name="surface">The DebugDrawingSurface to destroy.</param>
        public void destroyDebugDrawingSurface(DebugDrawingSurface surface)
        {
            
        }

        public SceneViewLightManager createSceneViewLightManager()
        {
            return null;
        }

        public void destroySceneViewLightManager(SceneViewLightManager lightManager)
        {
            
        }

        /// <summary>
        /// True to track memory leaks, will track leaks from the point this is set to true onward, so set it as early as possible
        /// </summary>
        public static bool TrackMemoryLeaks { get; set; }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OgreInterface_LoadRenderSystem(ref RenderSystemType rendersystemType);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern void OgreInterface_UnloadRenderSystem(IntPtr renderSystemPlugin);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr OgreInterface_GetRenderSystem(ref RenderSystemType rendersystemType);

        [DllImport(LibraryInfo.Name, CallingConvention = CallingConvention.Cdecl)]
        private static extern GPUVendor OgreInterface_getGpuVendor();

        #endregion
    }
}

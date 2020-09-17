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

namespace OgreNextPlugin
{
    public enum RenderSystemType
    {
        Default = 0,
        D3D11 = 1,
        OpenGL = 2,
	    OpenGLES2 = 3
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

        /// <summary>
        /// Fired when the OgreInterface is disposed, which means that ogre has been shutdown (Ogre::Root deleted).
        /// </summary>
        public event Action<OgreInterface> Disposed;

        public OgreInterface()
        {
            
        }

        public void Dispose()
        {
            
            if (Disposed != null)
            {
                Disposed.Invoke(this);
            }
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            
        }

        public void link(PluginManager pluginManager)
        {

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
            
        }

        public RendererWindow createRendererWindow(WindowInfo windowInfo)
        {
            
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
            
        }

        public void destroySceneViewLightManager(SceneViewLightManager lightManager)
        {
            
        }
    }
}

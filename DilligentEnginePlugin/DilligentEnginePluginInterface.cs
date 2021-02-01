using Engine;
using Engine.ObjectManagement;
using Engine.Platform;
using Engine.Renderer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DilligentEnginePlugin
{
    public class DilligentEnginePluginInterface : PluginInterface, RendererPlugin
    {
        public string Name => "DilligentEngine";

        public RendererWindow PrimaryWindow => throw new NotImplementedException();

        public void createDebugCommands(List<CommandManager> commands)
        {
            
        }

        public DebugDrawingSurface createDebugDrawingSurface(string name, SimSubScene scene)
        {
            throw new NotImplementedException();
        }

        public RendererWindow createRendererWindow(OSWindow embedWindow, string name)
        {
            throw new NotImplementedException();
        }

        public SceneViewLightManager createSceneViewLightManager()
        {
            throw new NotImplementedException();
        }

        public void destroyDebugDrawingSurface(DebugDrawingSurface surface)
        {
            throw new NotImplementedException();
        }

        public void destroyRendererWindow(RendererWindow window)
        {
            throw new NotImplementedException();
        }

        public void destroySceneViewLightManager(SceneViewLightManager lightManager)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            WindowInfo defaultWindowInfo;
            pluginManager.setRendererPlugin(this, out defaultWindowInfo);
        }

        public void link(PluginManager pluginManager)
        {
            
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }

        public void setupRenamedSaveableTypes(RenamedTypeMap renamedTypeMap)
        {
            
        }
    }
}

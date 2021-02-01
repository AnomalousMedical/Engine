using DilligentEngine;
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
        private GenericEngineFactory engineFactory;

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
            this.engineFactory.Dispose();
        }

        public DebugInterface getDebugInterface()
        {
            return null;
        }

        public void initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            WindowInfo defaultWindowInfo;
            pluginManager.setRendererPlugin(this, out defaultWindowInfo);

            this.engineFactory = new GenericEngineFactory();
            this.engineFactory.CreateDeviceAndSwapChain(defaultWindowInfo.EmbedWindow.WindowHandle);

            serviceCollection.AddSingleton<GenericEngineFactory>(this.engineFactory); //Externally managed
            serviceCollection.AddSingleton<IRenderDevice>(this.engineFactory.RenderDevice);
            serviceCollection.AddSingleton<IDeviceContext>(this.engineFactory.ImmediateContext);
            serviceCollection.AddSingleton<ISwapChain>(this.engineFactory.SwapChain);
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

using DilligentEngine;
using Engine;
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

        public void Dispose()
        {
            this.engineFactory.Dispose();
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

            defaultWindowInfo.EmbedWindow.Resized += window =>
            {
                this.engineFactory.SwapChain.Resize((uint)window.WindowWidth, (uint)window.WindowHeight);
            };
        }

        public void link(PluginManager pluginManager)
        {
            
        }

        public void setPlatformInfo(UpdateTimer mainTimer, EventManager eventManager)
        {
            
        }
    }
}

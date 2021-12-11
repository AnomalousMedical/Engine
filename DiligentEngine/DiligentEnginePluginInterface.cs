using DiligentEngine;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DiligentEngine
{
    class DiligentEnginePluginInterface : PluginInterface
    {
        private GraphicsEngine engineFactory;

        public string Name => "DilligentEngine";

        public void Dispose()
        {
            this.engineFactory.Dispose();
        }

        public void Initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            this.engineFactory = new GraphicsEngine();

            serviceCollection.AddSingleton<GraphicsEngine>(this.engineFactory); //Externally managed
            serviceCollection.AddSingleton<TextureLoader>();
        }

        public void Link(PluginManager pluginManager, IServiceProvider serviceProvider)
        {
            var window = serviceProvider.GetRequiredService<OSWindow>();
            var options = serviceProvider.GetRequiredService<DiligentEngineOptions>();
            var swapChainDesc = new SwapChainDesc();
            this.engineFactory.CreateDeviceAndSwapChain(window.WindowHandle, swapChainDesc, options.Features);

            window.Resized += w =>
            {
                this.engineFactory.SwapChain.Resize((uint)w.WindowWidth, (uint)w.WindowHeight, SURFACE_TRANSFORM.SURFACE_TRANSFORM_OPTIMAL);
            };
        }
    }
}

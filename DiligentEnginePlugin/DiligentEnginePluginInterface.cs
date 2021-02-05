using Anomalous.OSPlatform;
using DiligentEngine;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DiligentEnginePlugin
{
    public class DiligentEnginePluginInterface : PluginInterface
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
        }

        public void Link(PluginManager pluginManager, IServiceScope globalScope)
        {
            var window = globalScope.ServiceProvider.GetRequiredService<NativeOSWindow>();
            var swapChainDesc = new SwapChainDesc();
            this.engineFactory.CreateDeviceAndSwapChain(window.WindowHandle, swapChainDesc);

            window.Resized += w =>
            {
                this.engineFactory.SwapChain.Resize((uint)w.WindowWidth, (uint)w.WindowHeight, SURFACE_TRANSFORM.SURFACE_TRANSFORM_OPTIMAL);
            };
        }
    }
}

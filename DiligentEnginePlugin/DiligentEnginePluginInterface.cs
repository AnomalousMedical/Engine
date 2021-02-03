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
        private GenericEngineFactory engineFactory;

        public string Name => "DilligentEngine";

        public void Dispose()
        {
            this.engineFactory.Dispose();
        }

        public void Initialize(PluginManager pluginManager, IServiceCollection serviceCollection)
        {
            this.engineFactory = new GenericEngineFactory();

            serviceCollection.AddSingleton<GenericEngineFactory>(this.engineFactory); //Externally managed
            //serviceCollection.AddSingleton<IRenderDevice>(s => this.engineFactory.RenderDevice);
            //serviceCollection.AddSingleton<IDeviceContext>(s => this.engineFactory.ImmediateContext);
            //serviceCollection.AddSingleton<ISwapChain>(s => this.engineFactory.SwapChain);
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

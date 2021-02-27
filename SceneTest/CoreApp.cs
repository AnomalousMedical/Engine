using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;

namespace SceneTest
{
    public class CoreApp : App
    {
        private NativeOSWindow mainWindow;
        private UpdateTimer mainTimer;

        public CoreApp()
        {

        }

        public override void Dispose()
        {
            PerformanceMonitor.destroyEnabledState();

            base.DisposeGlobalScope();
            EasyNativeWindow.Destroy(mainWindow);
            base.FinalDispose();
        }

        public override bool OnInit(IServiceCollection services, PluginManager pluginManager)
        {
            mainWindow = EasyNativeWindow.Create(services, this, o =>
            {
                o.Title = "Scene Test";
            });

            services.AddLogging(o =>
            {
                o.AddConsole();
            });

            services.AddDiligentEngine(pluginManager)
                    .AddDiligentEnginePbr()
                    .AddDiligentEnginePbrShapes();

            services.AddOSPlatform(pluginManager);
            services.AddSoundPlugin(pluginManager);
            services.AddSharpGui();
            services.AddFirstPersonFlyCamera();
            services.AddBepuPlugin();

            //Add this app's services
            services.TryAddSingleton<SceneTestUpdateListener>();
            services.TryAddSingleton<TimeClock>();
            services.TryAddSingleton<SpriteManager>();
            services.TryAddSingleton<SceneObjectManager>();
            services.TryAddSingleton<ISpriteMaterialManager, SpriteMaterialManager>();
            services.TryAddSingleton<ICC0TextureManager, CC0TextureManager>();
            services.TryAddSingleton<ISpriteMaterialTextureManager, SpriteMaterialTextureManager>();
            services.TryAddScoped<Player>();
            services.TryAddScoped<TinyDino>();
            services.TryAddScoped<TinyDino.Desc>();
            services.TryAddScoped<Attachment>();
            services.TryAddScoped<Attachment.Description>();
            services.TryAddScoped<Brick>();
            services.TryAddScoped<Brick.Description>();
            services.TryAddScoped<Floor>();
            services.TryAddScoped<Floor.Description>();

            return true;
        }

        public override bool OnLink(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetRequiredService<ILogger<CoreApp>>();
            log.LogInformation("Running from directory {0}", FolderFinder.ExecutableFolder);

            //Setup virtual file system
            var vfs = serviceProvider.GetRequiredService<VirtualFileSystem>();
            var assetPath = Path.GetFullPath("../../../../../Engine-Next-Assets"); //This needs to be less hardcoded.
            vfs.addArchive(assetPath);

            mainTimer = serviceProvider.GetRequiredService<UpdateTimer>();

            var updateListener = serviceProvider.GetRequiredService<SceneTestUpdateListener>();
            mainTimer.addUpdateListener(updateListener);

            PerformanceMonitor.setupEnabledState(serviceProvider.GetRequiredService<SystemTimer>());

            return true;
        }

        public override int OnExit()
        {
            return 0;
        }

        public override void OnIdle()
        {
            mainTimer?.OnIdle();
        }
    }
}

﻿using Anomalous.OSPlatform;
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
            services.AddSingleton<SceneTestUpdateListener>();
            services.AddSingleton<TimeClock>();
            services.AddSingleton<SpriteManager>();
            services.AddSingleton<SceneObjectManager>();
            services.AddSingleton<ISpriteMaterialManager, SpriteMaterialManager>();
            services.AddSingleton<ICC0TextureManager, CC0TextureManager>();
            services.AddSingleton<ISpriteMaterialTextureManager, SpriteMaterialTextureManager>();
            services.AddScoped<Player>();
            services.AddScoped<Player.Description>();
            services.AddScoped<Enemy>();
            services.AddScoped<Enemy.Desc>();
            services.AddScoped<Attachment>();
            services.AddScoped<Attachment.Description>();
            services.AddScoped<Brick>();
            services.AddScoped<Brick.Description>();
            services.AddScoped<Level>();
            services.AddScoped<Level.Description>();
            services.AddScoped<LevelConnector>();
            services.AddScoped<LevelConnector.Description>();
            services.AddScoped<Sky>();
            services.AddSingleton<ILevelManager, LevelManager>();
            services.AddSingleton<LevelManager.Desc>(new LevelManager.Desc()
            {
                RandomSeed = 0
            });
            services.AddSingleton<CameraMover>();
            services.AddSingleton<ICollidableTypeIdentifier, CollidableTypeIdentifier>();

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

using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RpgMath;
using SceneTest.GameOver;
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

            services.AddOSPlatform(pluginManager, o =>
            {
                o.EventLayersType = typeof(EventLayers);
            });
            services.AddSoundPlugin(pluginManager);
            services.AddSharpGui();
            services.AddFirstPersonFlyCamera(o =>
            {
                o.EventLayer = EventLayers.Exploration;
            });
            services.AddBepuPlugin();
            services.AddRpgMath();

            //Add this app's services
            services.AddSingleton<SceneTestUpdateListener>();
            services.AddSingleton<ITimeClock, TimeClock>();
            services.AddSingleton<IDebugGui, DebugGui>();
            services.AddSingleton<IBattleGameState, BattleGameState>();
            services.AddSingleton<IEnvMapManager, EnvMapManager>();
            services.AddSingleton<SpriteManager>();
            services.AddSingleton<SceneObjectManager<ILevelManager>>();
            services.AddSingleton<SceneObjectManager<IBattleManager>>();
            services.AddSingleton<ISpriteMaterialManager, SpriteMaterialManager>();
            services.AddSingleton<ICC0TextureManager, CC0TextureManager>();
            services.AddSingleton<ISpriteMaterialTextureManager, SpriteMaterialTextureManager>();
            services.AddSingleton<IGameOverGameState, GameOverGameState>();
            services.AddScoped<Player>();
            services.AddScoped<Player.Description>();
            services.AddScoped<BattlePlayer>();
            services.AddScoped<BattlePlayer.Description>();
            services.AddScoped<Enemy>();
            services.AddScoped<Enemy.Desc>();
            services.AddScoped<Attachment<ILevelManager>>();
            services.AddScoped<Attachment<ILevelManager>.Description>();
            services.AddScoped<Attachment<IBattleManager>>();
            services.AddScoped<Attachment<IBattleManager>.Description>();
            services.AddScoped<Brick>();
            services.AddScoped<Brick.Description>();
            services.AddScoped<Level>();
            services.AddScoped<Level.Description>();
            services.AddScoped<LevelConnector>();
            services.AddScoped<LevelConnector.Description>();
            services.AddScoped<Sky>();
            services.AddScoped<TargetCursor>();
            services.AddScoped<IMagicAbilities, MagicAbilities>();
            services.AddSingleton<ILevelManager, LevelManager>();
            services.AddSingleton<LevelManager.Desc>(new LevelManager.Desc()
            {
                RandomSeed = 0
            });
            services.AddSingleton<IBattleManager, BattleManager>();
            services.AddScoped<BattleArena>();
            services.AddScoped<BattleArena.Description>();
            services.AddSingleton<IBiomeManager, BiomeManager>();
            services.AddSingleton<IGameStateLinker, GameStateLinker>();
            services.AddSingleton<CameraMover>();
            services.AddSingleton<ICollidableTypeIdentifier, CollidableTypeIdentifier>();
            services.AddSingleton<IBackgroundMusicManager, BackgroundMusicManager>();
            services.AddSingleton<ICameraProjector, CameraProjector>();
            services.AddSingleton<IBattleScreenLayout, BattleScreenLayout>();
            services.AddSingleton<IBattleTrigger, BattleTrigger>();
            services.AddSingleton<IFirstGameStateBuilder, FirstGameStateBuilder>();
            services.AddSingleton<IExplorationGameState, ExplorationGameState>();
            services.AddSingleton<Party>();
            services.AddSingleton<ISetupGameState, SetupGameState>();

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

            var linker = serviceProvider.GetRequiredService<IGameStateLinker>(); //This links the game states together.
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

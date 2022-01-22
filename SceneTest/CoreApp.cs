using Anomalous.OSPlatform;
using DiligentEngine.RT;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RpgMath;
using SceneTest.Battle;
using SceneTest.Exploration;
using SceneTest.Exploration.Menu;
using SceneTest.GameOver;
using SceneTest.Services;
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
                o.Title = "Anomalous Adventure";
            });

            services.AddLogging(o =>
            {
                o.AddConsole();
            });

            services.AddDiligentEngine(pluginManager, o =>
            {
                o.Features = DiligentEngine.GraphicsEngine.FeatureFlags.RayTracing;
            })
            .AddDiligentEngineRt();

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
            services.AddSingleton<IRootMenu, RootMenu>();
            services.AddSingleton<IBattleGameState, BattleGameState>();
            services.AddSingleton<IGameOverGameState, GameOverGameState>();
            services.AddScoped<Player>();
            services.AddScoped<Player.Description>();
            services.AddScoped<BattlePlayer>();
            services.AddScoped<BattlePlayer.Description>();
            services.AddScoped<Enemy>();
            services.AddScoped<Enemy.Desc>();
            services.AddSingleton<ShaderPreloader>();
            services.AddSingleton<RTInstances<ILevelManager>>();
            services.AddSingleton<RTInstances<IBattleManager>>();
            services.AddScoped<Attachment<ILevelManager>>();
            services.AddScoped<Attachment<ILevelManager>.Description>();
            services.AddScoped<Attachment<IBattleManager>>();
            services.AddScoped<Attachment<IBattleManager>.Description>();
            services.AddScoped<IBattleBuilder, BattleBuilder>();
            services.AddScoped<Level>();
            services.AddScoped<Level.Description>();
            services.AddScoped<LevelConnector>();
            services.AddScoped<LevelConnector.Description>();
            services.AddScoped<BattleTrigger>();
            services.AddScoped<BattleTrigger.Description>();
            services.AddScoped<TreasureTrigger>();
            services.AddScoped<TreasureTrigger.Description>();
            services.AddScoped<Sky>();
            services.AddScoped<RTGui>();
            services.AddScoped<TargetCursor>();
            services.AddScoped<IMagicAbilities, MagicAbilities>();
            services.AddSingleton<ILevelManager, LevelManager>();
            services.AddSingleton<IWorldManager, WorldManager>();
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
            services.AddSingleton<IFirstGameStateBuilder, FirstGameStateBuilder>();
            services.AddSingleton<IExplorationGameState, ExplorationGameState>();
            services.AddSingleton<Party>();
            services.AddSingleton<ISetupGameState, SetupGameState>();
            services.AddSingleton<IExplorationMenu, ExplorationMenu>();
            services.AddSingleton<IContextMenu, ContextMenu>();
            services.AddSingleton<IPersistenceWriter, PersistenceWriter>();
            services.AddSingleton<Persistence>(s =>
            {
                var writer = s.GetRequiredService<IPersistenceWriter>();
                return writer.Load();
            });

            return true;
        }

        public override bool OnLink(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetRequiredService<ILogger<CoreApp>>();
            log.LogInformation("Running from directory {0}", FolderFinder.ExecutableFolder);

            //Setup virtual file system
            var vfs = serviceProvider.GetRequiredService<VirtualFileSystem>();

            //This needs to be less hardcoded.
            var assetPath = Path.GetFullPath(Path.Combine(FolderFinder.ExecutableFolder, "Engine-Next-Assets"));
            if (!Directory.Exists(assetPath))
            {
                //If no local assets, load from dev location
                assetPath = Path.GetFullPath("../../../../../../Engine-Next-Assets");
            }
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

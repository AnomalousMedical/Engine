﻿using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class BattleManager : IDisposable, IBattleManager
    {
        private readonly EventManager eventManager;
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly NativeOSWindow window;
        private readonly CameraMover cameraMover;
        private readonly ILevelManager levelManager;
        private readonly IObjectResolver objectResolver;
        private BattleArena battleArena;

        private SharpButton endBattle = new SharpButton() { Text = "End Battle" };

        private List<Enemy> enemies = new List<Enemy>();
        private List<BattlePlayer> players = new List<BattlePlayer>();

        public BattleManager(EventManager eventManager,
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            NativeOSWindow window,
            IObjectResolverFactory objectResolverFactory,
            CameraMover cameraMover,
            ILevelManager levelManager)
        {
            this.eventManager = eventManager;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.window = window;
            this.cameraMover = cameraMover;
            this.levelManager = levelManager;
            this.objectResolver = objectResolverFactory.Create();

            levelManager.LevelChanged += LevelManager_LevelChanged;
        }

        public void Dispose()
        {
            levelManager.LevelChanged -= LevelManager_LevelChanged;
            objectResolver.Dispose();
        }

        public void SetupBattle()
        {
            players.Add(this.objectResolver.Resolve<BattlePlayer, BattlePlayer.Description>(c =>
            {
                c.Translation = new Vector3(4, 0, -1);
            }));

            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c);
                c.Translation = new Vector3(-4, 0.55f, -1);
            }));
            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c, skinMaterial: "cc0Textures/Leather011_1K");
                c.Translation = new Vector3(-5, 0.55f, -2);
            }));
            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeSkeleton(c);
                c.Translation = new Vector3(0, 0.55f, -3);
            }));
            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c);
                c.Translation = new Vector3(-6, 0.55f, -3);
            }));
        }

        public void SetActive(bool active)
        {
            if (active != this.Active)
            {
                this.Active = active;
                if (active)
                {
                    eventManager[EventLayers.Battle].OnUpdate += eventManager_OnUpdate;
                    cameraMover.Position = new Vector3(0f, 5f, -12f);
                    cameraMover.SceneCenter = new Vector3(0f, 0f, 0f);
                }
                else
                {
                    foreach (var player in players)
                    {
                        player.RequestDestruction();
                    }
                    foreach (var enemy in enemies)
                    {
                        enemy.RequestDestruction();
                    }
                    eventManager[EventLayers.Battle].OnUpdate -= eventManager_OnUpdate;
                }
            }
        }

        public void UpdateGui()
        {
            var layout =
                new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                new MaxWidthLayout(scaleHelper.Scaled(300),
                new ColumnLayout(endBattle) { Margin = new IntPad(10) }
                ));
            var desiredSize = layout.GetDesiredSize(sharpGui);
            layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            if (sharpGui.Button(endBattle))
            {
                this.SetActive(false);
            }
        }

        public bool Active { get; private set; }

        private void eventManager_OnUpdate(EventLayer eventLayer)
        {
            eventLayer.alertEventsHandled();
        }

        private void LevelManager_LevelChanged(ILevelManager levelManager)
        {
            battleArena?.RequestDestruction();

            battleArena = objectResolver.Resolve<BattleArena, BattleArena.Description>(o =>
            {
                o.Scale = new Vector3(20, 0.1f, 20);
                o.Texture = levelManager.CurrentLevel.Biome.FloorTexture;
            });
        }
    }
}
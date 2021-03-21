using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using RpgMath;
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
        private readonly CameraMover cameraMover;
        private readonly ILevelManager levelManager;
        private readonly Party party;
        private readonly IObjectResolver objectResolver;
        private BattleArena battleArena;

        private SharpButton endBattle = new SharpButton() { Text = "End Battle" };

        private List<Enemy> enemies = new List<Enemy>();
        private List<BattlePlayer> players = new List<BattlePlayer>();

        public BattleManager(EventManager eventManager,
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            IObjectResolverFactory objectResolverFactory,
            CameraMover cameraMover,
            ILevelManager levelManager,
            Party party)
        {
            this.eventManager = eventManager;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.cameraMover = cameraMover;
            this.levelManager = levelManager;
            this.party = party;
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
            var currentZ = -2;
            foreach(var member in party.ActiveCharacters)
            {
                players.Add(this.objectResolver.Resolve<BattlePlayer, BattlePlayer.Description>(c =>
                {
                    c.Translation = new Vector3(4, 0, currentZ);
                    c.BattleManager = this;
                    c.CharacterSheet = member;
                }));

                currentZ += 2;
            }

            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c);
                c.Translation = new Vector3(-4, 0.55f, -2);
            }));
            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c, skinMaterial: "cc0Textures/Leather011_1K");
                c.Translation = new Vector3(-5, 0.55f, 0);
            }));
            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeSkeleton(c);
                c.Translation = new Vector3(0, 0.55f, 2);
            }));
            enemies.Add(this.objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                Enemy.Desc.MakeTinyDino(c);
                c.Translation = new Vector3(-6, 0.55f, 4);
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
                    cameraMover.Position = new Vector3(-1.0354034f, 2.958224f, -12.394701f);
                    cameraMover.Orientation = new Quaternion(0.057467595f, 0.0049917176f, -0.00028734046f, 0.9983348f);
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
            //var layout =
            //    new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
            //    new MaxWidthLayout(scaleHelper.Scaled(300),
            //    new ColumnLayout(endBattle) { Margin = new IntPad(10) }
            //    ));
            //var desiredSize = layout.GetDesiredSize(sharpGui);
            //layout.SetRect(new IntRect(window.WindowWidth - desiredSize.Width, window.WindowHeight - desiredSize.Height, desiredSize.Width, desiredSize.Height));

            //if (sharpGui.Button(endBattle))
            //{
            //    this.SetActive(false);
            //}

            players[0].UpdateGui(sharpGui);
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

        internal void Attack()
        {
            var enemy = enemies[0];
            enemy.RequestDestruction();
            enemies.Remove(enemy);
            if(enemies.Count == 0)
            {
                this.SetActive(false);
            }
        }
    }
}

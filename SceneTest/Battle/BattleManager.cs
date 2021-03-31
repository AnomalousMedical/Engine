using Anomalous.OSPlatform;
using DiligentEngine.GltfPbr;
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
        const long NumberDisplayTime = (long)(0.9f * Clock.SecondsToMicro);

        private readonly EventManager eventManager;
        private readonly ISharpGui sharpGui;
        private readonly IScaleHelper scaleHelper;
        private readonly CameraMover cameraMover;
        private readonly ILevelManager levelManager;
        private readonly Party party;
        private readonly IDamageCalculator damageCalculator;
        private readonly IBackgroundMusicManager backgroundMusicManager;
        private readonly IScreenPositioner screenPositioner;
        private readonly ICameraProjector cameraProjector;
        private readonly ITurnTimer turnTimer;
        private readonly IObjectResolver objectResolver;
        private BattleArena battleArena;

        private SharpButton endBattle = new SharpButton() { Text = "End Battle" };

        private List<Enemy> enemies = new List<Enemy>(20);
        private List<BattlePlayer> players = new List<BattlePlayer>(4);
        private List<DamageNumber> numbers = new List<DamageNumber>(10);
        private Queue<BattlePlayer> activePlayers = new Queue<BattlePlayer>(4);
        private Queue<Func<Clock, bool>> turnQueue = new Queue<Func<Clock, bool>>(30);

        private TargetCursor cursor;

        private Random targetRandom = new Random();

        public BattleManager(EventManager eventManager,
            ISharpGui sharpGui,
            IScaleHelper scaleHelper,
            IObjectResolverFactory objectResolverFactory,
            CameraMover cameraMover,
            ILevelManager levelManager,
            Party party,
            IDamageCalculator damageCalculator,
            IBackgroundMusicManager backgroundMusicManager,
            IScreenPositioner screenPositioner,
            ICameraProjector cameraProjector,
            ITurnTimer turnTimer)
        {
            this.eventManager = eventManager;
            this.sharpGui = sharpGui;
            this.scaleHelper = scaleHelper;
            this.cameraMover = cameraMover;
            this.levelManager = levelManager;
            this.party = party;
            this.damageCalculator = damageCalculator;
            this.backgroundMusicManager = backgroundMusicManager;
            this.screenPositioner = screenPositioner;
            this.cameraProjector = cameraProjector;
            this.turnTimer = turnTimer;
            this.objectResolver = objectResolverFactory.Create();

            cursor = this.objectResolver.Resolve<TargetCursor>();

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
            foreach (var member in party.ActiveCharacters)
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
                    cursor.BattleStarted();
                    numbers.Clear();

                    backgroundMusicManager.SetBattleTrack("freepd/Rafael Krux - Hit n Smash.ogg");
                    turnTimer.Restart(0, players.Sum(i => i.BaseDexterity), players.Select(i => i.CharacterTimer));

                    eventManager[EventLayers.Battle].OnUpdate += eventManager_OnUpdate;
                    cameraMover.Position = new Vector3(-1.0354034f, 2.958224f, -12.394701f);
                    cameraMover.Orientation = new Quaternion(0.057467595f, 0.0049917176f, -0.00028734046f, 0.9983348f);
                    cameraMover.SceneCenter = new Vector3(0f, 0f, 0f);

                    var instantAttackChance = targetRandom.Next(100);
                    if(instantAttackChance < 3)
                    {
                        //Instant attack, players start with turns and enemies start at 0
                        foreach (var player in players)
                        {
                            activePlayers.Enqueue(player); //Needs to be on a timer
                            player.CharacterTimer.TurnTimerActive = false;
                        }
                    }
                    else
                    {
                        long highestTimer = 0;
                        foreach (var timer in players.Select(i => i.CharacterTimer)) //Look at everything with a timer
                        {
                            timer.TurnTimer = targetRandom.Next(32767);
                            if(timer.TurnTimer > highestTimer)
                            {
                                highestTimer = timer.TurnTimer;
                            }
                        }
                        long adjustment = 57344 - highestTimer;
                        foreach (var timer in players.Select(i => i.CharacterTimer)) //Look at everything with a timer
                        {
                            timer.TurnTimer += adjustment;
                        }
                    }
                }
                else
                {
                    backgroundMusicManager.SetBattleTrack(null);

                    foreach (var player in players)
                    {
                        player.RequestDestruction();
                    }
                    players.Clear();
                    foreach (var enemy in enemies)
                    {
                        enemy.RequestDestruction();
                    }
                    enemies.Clear();
                    eventManager[EventLayers.Battle].OnUpdate -= eventManager_OnUpdate;
                }
            }
        }

        public void Update(Clock clock)
        {
            if (turnQueue.Count > 0)
            {
                var turn = turnQueue.Peek();
                if (turn.Invoke(clock) && turnQueue.Count > 0) //Turn queue can be cleared during a turn if all enemies are killed or other conditions
                {
                    turnQueue.Dequeue();
                }
            }

            if (enemies.Count == 0)
            {
                cursor.Visible = false;
                var layout =
                    new MarginLayout(new IntPad(scaleHelper.Scaled(10)),
                    new MaxWidthLayout(scaleHelper.Scaled(300),
                    new ColumnLayout(endBattle) { Margin = new IntPad(10) }
                    ));
                var desiredSize = layout.GetDesiredSize(sharpGui);

                layout.SetRect(screenPositioner.GetBottomRightRect(desiredSize));

                if (sharpGui.Button(endBattle))
                {
                    this.SetActive(false);
                }
            }
            else
            {
                turnTimer.Update(clock);

                BattlePlayer activePlayer = activePlayers.Count > 0 ? activePlayers.Peek() : null;
                if (cursor.Targeting)
                {
                    cursor.Visible = true;
                    var enemy = enemies[(int)(cursor.TargetIndex % enemies.Count)];
                    var enemyPos = enemy.DamageDisplayLocation;
                    cursor.UpdateCursor(sharpGui, enemy, enemyPos);
                }
                else
                {
                    cursor.Visible = false;
                    activePlayer?.UpdateGui(sharpGui);
                }

                foreach (var player in players)
                {
                    player.SetGuiActive(player == activePlayer);
                }

                for (var i = 0; i < numbers.Count;)
                {
                    var number = numbers[i];
                    sharpGui.Text(number.Text);
                    number.TimeRemaining -= clock.DeltaTimeMicro;
                    number.UpdatePosition();
                    if (number.TimeRemaining < 0)
                    {
                        numbers.RemoveAt(i);
                    }
                    else
                    {
                        ++i;
                    }
                }
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

        public IBattleTarget ValidateTarget(IBattleTarget attacker, IBattleTarget target)
        {
            //Make sure target still exists
            switch (target.BattleTargetType)
            {
                case BattleTargetType.Enemy:
                    if (!enemies.Contains(target))
                    {
                        target = enemies[targetRandom.Next(enemies.Count)];
                    }
                    break;
            }

            return target;
        }

        public void Attack(IBattleTarget attacker, IBattleTarget target)
        {
            target = ValidateTarget(attacker, target);

            var enemyPos = target.DamageDisplayLocation - cameraMover.SceneCenter;
            var screenPos = cameraProjector.Project(enemyPos);

            if (damageCalculator.PhysicalHit(attacker.Stats, target.Stats))
            {
                var damage = damageCalculator.Physical(attacker.Stats, target.Stats, 16);
                damage = damageCalculator.RandomVariation(damage);
                numbers.Add(new DamageNumber(damage.ToString(), NumberDisplayTime, screenPos, scaleHelper));
                target.ApplyDamage(damageCalculator, damage);
                if (target.IsDead)
                {
                    if (enemies.Contains(target))
                    {
                        target.RequestDestruction();
                        enemies.Remove(target as Enemy);
                        if (enemies.Count == 0)
                        {
                            turnQueue.Clear();
                            activePlayers.Clear();
                            backgroundMusicManager.SetBattleTrack("freepd/Alexander Nakarada - Fanfare X.ogg");
                        }
                    }
                }
            }
            else
            {
                numbers.Add(new DamageNumber("Miss", NumberDisplayTime, screenPos, scaleHelper));
            }
        }

        public void QueueTurn(Func<Clock, bool> turn)
        {
            activePlayers.Dequeue(); //The player is queuing a turn, so dequeue them from the active players
            this.turnQueue.Enqueue(turn);
        }

        public void TurnComplete(BattlePlayer player)
        {
            
        }

        public void AddToActivePlayers(BattlePlayer player)
        {
            this.activePlayers.Enqueue(player);
        }

        public Task<IBattleTarget> GetTarget()
        {
            return cursor.GetTarget();
        }
    }
}

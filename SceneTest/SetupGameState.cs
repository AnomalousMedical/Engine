using DiligentEngine.RT;
using Engine;
using Engine.Platform;
using RpgMath;
using SceneTest.Battle;
using SceneTest.Battle.Spells;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class SetupGameState : ISetupGameState
    {
        private readonly ILevelManager levelManager;
        private readonly Party party;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly ISharpGui sharpGui;
        private readonly IScreenPositioner screenPositioner;
        private readonly RTInstances rtInstances;
        private IGameState nextState;
        private bool finished = false;

        public RTInstances Instances => rtInstances;

        private SharpText loading = new SharpText("Loading") { Color = Color.White };

        public SetupGameState
        (
            ILevelManager levelManager,
            Party party,
            ICoroutineRunner coroutineRunner,
            ISharpGui sharpGui,
            IScreenPositioner screenPositioner,
            RTInstances<ILevelManager> rtInstances
        )
        {
            this.levelManager = levelManager;
            this.party = party;
            this.coroutineRunner = coroutineRunner;
            this.sharpGui = sharpGui;
            this.screenPositioner = screenPositioner;
            this.rtInstances = rtInstances;
            {
                var arch = Archetype.CreateHero();
                party.AddCharacter(new Character()
                {
                    PlayerSprite = new Assets.Original.FighterPlayerSprite(),
                    CharacterSheet = new CharacterSheet()
                    {
                        Name = "Bob",
                        Archetype = arch,
                        Level = 1,
                        MainHand = new Equipment()
                        {
                            AttackPercent = 100,
                            Attack = 18
                        }
                    },
                    PrimaryHandAsset = new Assets.Original.Greatsword01(),
                    SecondaryHandAsset = new Assets.Original.ShieldOfReflection()
                });
            }

            {
                var arch = Archetype.CreateSage();
                party.AddCharacter(new Character()
                {
                    PlayerSprite = new Assets.Original.MagePlayerSprite(),
                    CharacterSheet = new CharacterSheet()
                    {
                        Name = "Magic Joe",
                        Archetype = arch,
                        Level = 1,
                        MainHand = new Equipment()
                        {
                            AttackPercent = 35,
                            MagicAttackPercent = 100,
                            Attack = 9
                        }
                    },
                    PrimaryHandAsset = new Assets.Original.Staff07(),
                    Spells = new ISpell[] { new Fir(), new Fyre(), new Meltdown() }
                });
            }

            {
                var arch = Archetype.CreateTank();
                party.AddCharacter(new Character()
                {
                    PlayerSprite = new Assets.Original.ThiefPlayerSprite(),
                    CharacterSheet = new CharacterSheet()
                    {
                        Name = "Stabby McStabface",
                        Archetype = arch,
                        Level = 1,
                        MainHand = new Equipment()
                        {
                            AttackPercent = 100,
                            Attack = 18
                        }
                    },
                    PrimaryHandAsset = new Assets.Original.DaggerNew(),
                    SecondaryHandAsset = new Assets.Original.DaggerNew()
                });
            }

            {
                var arch = Archetype.CreateGuardian();
                party.AddCharacter(new Character()
                {
                    PlayerSprite = new Assets.Original.ClericPlayerSprite(),
                    CharacterSheet = new CharacterSheet()
                    {
                        Name = "Wendy",
                        Archetype = arch,
                        Level = 1,
                        MainHand = new Equipment()
                        {
                            AttackPercent = 100,
                            Attack = 25
                        }
                    },
                    PrimaryHandAsset = new Assets.Original.BattleAxe6(),
                    Spells = new ISpell[] { new Cure() }
                });
            }
        }

        public void Link(IGameState nextState)
        {
            this.nextState = nextState;
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                finished = false;
                foreach (var character in party.ActiveCharacters)
                {
                    character.CharacterSheet.CurrentHp = character.CharacterSheet.Hp;
                    character.CharacterSheet.CurrentMp = character.CharacterSheet.Mp;

                    //character.CurrentHp = 1;
                }
                coroutineRunner.RunTask(async () =>
                {
                    await levelManager.WaitForCurrentLevel();
                    finished = true;
                });
            }
        }

        public IGameState Update(Clock clock)
        {
            IGameState next = this;

            var size = loading.GetDesiredSize(sharpGui);
            var rect = screenPositioner.GetCenterRect(size);
            loading.SetRect(rect);

            sharpGui.Text(loading);

            if (finished)
            {
                next = this.nextState;
            }
            return next;
        }
    }
}

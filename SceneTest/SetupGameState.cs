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
    class SetupGameState : ISetupGameState
    {
        private readonly ILevelManager levelManager;
        private readonly Party party;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly ISharpGui sharpGui;
        private readonly IScreenPositioner screenPositioner;
        private IGameState nextState;
        private bool finished = false;

        public IEnumerable<SceneObject> SceneObjects => Enumerable.Empty<SceneObject>();

        private SharpText loading = new SharpText("Loading") { Color = Color.White };

        public SetupGameState
        (
            ILevelManager levelManager,
            Party party,
            ICoroutineRunner coroutineRunner,
            ISharpGui sharpGui,
            IScreenPositioner screenPositioner
        )
        {
            this.levelManager = levelManager;
            this.party = party;
            this.coroutineRunner = coroutineRunner;
            this.sharpGui = sharpGui;
            this.screenPositioner = screenPositioner;
            {
                var arch = Archetype.CreateHero();
                party.AddCharacter(new CharacterSheet()
                {
                    Name = "Bob",
                    Archetype = arch,
                    Level = 1,
                    MainHand = new Equipment()
                    {
                        AttackPercent = 100,
                        Attack = 18
                    }
                });
            }

            {
                var arch = Archetype.CreateSage();
                party.AddCharacter(new CharacterSheet()
                {
                    Name = "Magic Joe",
                    Archetype = arch,
                    Level = 1,
                    MainHand = new Equipment()
                    {
                        AttackPercent = 100,
                        Attack = 18
                    }
                });
            }

            {
                var arch = Archetype.CreateTank();
                party.AddCharacter(new CharacterSheet()
                {
                    Name = "Stabby McStabface",
                    Archetype = arch,
                    Level = 1,
                    MainHand = new Equipment()
                    {
                        AttackPercent = 100,
                        Attack = 18
                    }
                });
            }

            {
                var arch = Archetype.CreateGuardian();
                party.AddCharacter(new CharacterSheet()
                {
                    Name = "Archibald",
                    Archetype = arch,
                    Level = 1,
                    MainHand = new Equipment()
                    {
                        AttackPercent = 100,
                        Attack = 18
                    }
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
                    character.CurrentHp = character.Hp;
                    character.CurrentMp = character.Mp;
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

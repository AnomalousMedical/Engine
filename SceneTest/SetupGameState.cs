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
                foreach (var character in party.ActiveCharacterSheets)
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

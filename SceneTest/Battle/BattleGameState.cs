using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class BattleGameState : IBattleGameState
    {
        private readonly IBattleManager battleManager;
        private readonly SceneObjectManager<IBattleManager> sceneObjects;

        public BattleGameState
        (
            IBattleManager battleManager,
            SceneObjectManager<IBattleManager> sceneObjects
        )
        {
            this.battleManager = battleManager;
            this.sceneObjects = sceneObjects;
        }

        public IEnumerable<SceneObject> SceneObjects => sceneObjects;

        public IGameState AfterBattleState { get; set; }

        public void SetActive(bool active)
        {
            if (active)
            {
                battleManager.SetupBattle();
            }
            battleManager.SetActive(active);
        }

        public IGameState Update(Clock clock)
        {
            IGameState nextState = this;
            if (battleManager.Update(clock))
            {
                nextState = AfterBattleState;
            }
            return nextState;
        }
    }
}

using Engine.Platform;
using SceneTest.GameOver;
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
        private readonly IGameOverGameState gameOverGameState;
        private IGameState explorationState;

        public BattleGameState
        (
            IBattleManager battleManager,
            SceneObjectManager<IBattleManager> sceneObjects,
            IGameOverGameState gameOverGameState
        )
        {
            this.battleManager = battleManager;
            this.sceneObjects = sceneObjects;
            this.gameOverGameState = gameOverGameState;
        }

        public IEnumerable<SceneObject> SceneObjects => sceneObjects;

        public void LinkExplorationState(IGameState explorationState)
        {
            this.explorationState = explorationState;
        }

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
            var result = battleManager.Update(clock);
            switch(result)
            {
                case IBattleManager.Result.GameOver:
                    nextState = gameOverGameState;
                    break;
                case IBattleManager.Result.ReturnToExploration:
                    nextState = explorationState;
                    break;
            }
            return nextState;
        }
    }
}

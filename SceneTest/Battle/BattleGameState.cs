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
        private IGameState gameOverState;
        private IGameState explorationState;

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

        public void Link(IGameState explorationState, IGameState gameOver)
        {
            this.explorationState = explorationState;
            this.gameOverState = gameOver;
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
                    nextState = gameOverState;
                    break;
                case IBattleManager.Result.ReturnToExploration:
                    nextState = explorationState;
                    break;
            }
            return nextState;
        }
    }
}

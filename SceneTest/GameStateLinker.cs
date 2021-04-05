using SceneTest.GameOver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class GameStateLinker : IGameStateLinker
    {
        public GameStateLinker
        (
            IExplorationGameState exploration,
            IBattleGameState battle,
            IGameOverGameState gameOver
        )
        {
            exploration.Link(battle);
            battle.Link(exploration, gameOver);
            gameOver.Link(exploration);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class FirstGameStateBuilder : IFirstGameStateBuilder
    {
        private readonly IExplorationGameState gameState;

        public FirstGameStateBuilder(IExplorationGameState gameState)
        {
            this.gameState = gameState;
        }

        public IGameState GetFirstGameState()
        {
            return gameState;
        }
    }
}

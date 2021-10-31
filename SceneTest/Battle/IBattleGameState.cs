using Engine.Platform;
using System.Collections.Generic;

namespace SceneTest.Battle
{
    interface IBattleGameState : IGameState
    {
        /// <summary>
        /// This is a circular link, so it must be set by the ExplorationGameState itself, which injects this class.
        /// </summary>
        void Link(IGameState returnState, IGameState gameOver);
    }
}
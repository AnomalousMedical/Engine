using Engine.Platform;
using System.Collections.Generic;

namespace SceneTest
{
    interface IBattleGameState : IGameState
    {
        IGameState AfterBattleState { get; set; }
    }
}
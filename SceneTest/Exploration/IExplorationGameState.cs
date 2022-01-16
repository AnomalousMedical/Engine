using SceneTest.Battle;

namespace SceneTest
{
    interface IExplorationGameState : IGameState
    {
        bool AllowBattles { get; set; }

        void Link(IBattleGameState battleState);

        /// <summary>
        /// Request a battle with a given trigger. The trigger can be null.
        /// </summary>
        /// <param name="battleTrigger"></param>
        void RequestBattle(BattleTrigger battleTrigger = null);
    }
}
using Engine.Platform;
using System;
using System.Threading.Tasks;

namespace SceneTest
{
    interface IBattleManager
    {
        bool Active { get; }

        void AddToActivePlayers(BattlePlayer player);
        void Attack(IBattleTarget attacker, IBattleTarget target);
        Task<IBattleTarget> GetTarget();

        /// <summary>
        /// Called by players before they queue their turn to remove them from the active player list.
        /// </summary>
        void DeactivateCurrentPlayer();

        /// <summary>
        /// Called by everything to start the turn.
        /// </summary>
        /// <param name="turn"></param>
        void QueueTurn(Func<Clock, bool> turn);

        /// <summary>
        /// Set battle mode active / inactive.
        /// </summary>
        /// <param name="active"></param>
        void SetActive(bool active);
        void SetupBattle();
        bool Update(Clock clock);
        IBattleTarget ValidateTarget(IBattleTarget attacker, IBattleTarget target);
        IBattleTarget GetRandomPlayer();
        void PlayerDead(BattlePlayer battlePlayer);
    }
}
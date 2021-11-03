using Engine;
using Engine.Platform;
using RpgMath;
using System;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    interface IBattleManager
    {
        public enum Result
        {
            ContinueBattle,
            ReturnToExploration,
            GameOver
        }

        bool Active { get; }

        void AddToActivePlayers(BattlePlayer player);
        void Attack(IBattleTarget attacker, IBattleTarget target);
        Task<IBattleTarget> GetTarget(bool targetPlayers);

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
        Result Update(Clock clock);
        IBattleTarget ValidateTarget(IBattleTarget attacker, IBattleTarget target);
        IBattleTarget GetRandomPlayer();
        void PlayerDead(BattlePlayer battlePlayer);

        IDamageCalculator DamageCalculator { get; }

        void HandleDeath(IBattleTarget target);

        public void AddDamageNumber(IBattleTarget target, long damage);

        public void AddDamageNumber(IBattleTarget target, String damage, Color color);

        void SwitchPlayer();
    }
}
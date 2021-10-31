using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle.Spells
{
    class Cure : ISpell
    {
        public void Apply(IBattleManager battleManager, IObjectResolver objectResolver, IBattleTarget attacker, IBattleTarget target)
        {
            target = battleManager.ValidateTarget(attacker, target);
            var damage = battleManager.DamageCalculator.Cure(attacker.Stats, 5);
            damage = battleManager.DamageCalculator.RandomVariation(damage);

            damage *= -1; //Make it healing
            
            battleManager.AddDamageNumber(target, damage);
            target.ApplyDamage(battleManager.DamageCalculator, damage);
            battleManager.HandleDeath(target);
        }

        public bool DefaultTargetPlayers => true;
    }
}

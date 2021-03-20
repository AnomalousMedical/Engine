using System;

namespace RpgMath
{
    public class DamageCalculator
    {
        private Random random = new Random();

        public long Physical(ICharacterStats attacker, ICharacterStats target, long power)
        {
            long baseDamage = attacker.Attack + ((attacker.Attack + attacker.Level) / 32L) * ((attacker.Attack * attacker.Level) / 32L);
            return ((power * (512L - target.Defense) * baseDamage) / (16L * 512L));
        }

        public bool PhysicalHit(ICharacterStats attacker, ICharacterStats target)
        {
            long hitPct = ((attacker.Dexterity / 4L) + attacker.AttackPercent) + attacker.DefensePercent - target.DefensePercent;
            long luckRoll = (long)random.Next(99);
            //Lucky hit
            if(luckRoll < attacker.Luck / 4)
            {
                hitPct = 255;
            }
            else if (target.AllowLuckyEvade)
            {
                long evadeChance = target.Luck / 4 - attacker.Luck / 4;
                if(luckRoll < evadeChance)
                {
                    hitPct = 0;
                }
            }

            var rand = (long)random.Next(65535) * 99 / 65535 + 1;
            return rand < hitPct;
        }

        /// <summary>
        /// Magical damage formula.
        /// </summary>
        /// <param name="mat">Magic attack</param>
        /// <param name="level">Level of attacker</param>
        /// <param name="power">The power of the attack. 16 is the base power. Above that is extra, below less. Usually 1 unless special effects.</param>
        /// <param name="mdef">Magic defense of target</param>
        /// <returns></returns>
        public long Magical(ICharacterStats attacker, ICharacterStats target, long power)
        {
            long baseDamage = 6L * (attacker.MagicAttack + attacker.Level);
            return ((power * (512L - target.MagicDefense) * baseDamage) / (16L * 512L));
        }

        /// <summary>
        /// Cure formula.
        /// </summary>
        /// <param name="mat">Magic attack</param>
        /// <param name="level">Level of attacker</param>
        /// <param name="power">The power of the cure. This is 22 * Power.</param>
        /// <returns></returns>
        public long Cure(long mat, long level, long power)
        {
            long baseDamage = 6L * (mat + level);
            return baseDamage + 22L * power;
        }

        /// <summary>
        /// Item formula.
        /// </summary>
        /// <param name="power">The power of the item. This is 16 * Power.</param>
        /// <param name="def">Def or mdef of target.</param>
        /// <returns></returns>
        public long Item(long power, long def)
        {
            long baseDamage = 16L * power;
            return baseDamage * (512L - def) / 512L;
        }

        /// <summary>
        /// Random variation formula.
        /// </summary>
        /// <param name="damage">The calculated damage to randomize.</param>
        /// <returns>A randomized damage value.</returns>
        public long RandomVariation(long damage)
        {
            return damage * (3841L + (long)random.Next(255)) / 4096L;
        }
    }
}

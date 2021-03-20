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
        public long Cure(ICharacterStats caster, long power)
        {
            long baseDamage = 6L * (caster.MagicAttack + caster.Level);
            return baseDamage + 22L * power;
        }

        /// <summary>
        /// Item formula.
        /// </summary>
        /// <param name="power">The power of the item. This is 16 * Power.</param>
        /// <param name="def">Def or mdef of target.</param>
        /// <returns></returns>
        public long Item(ICharacterStats target, long power)
        {
            long baseDamage = 16L * power;
            return baseDamage * (512L - target.Defense) / 512L;
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

        public long ApplyResistance(long damage, Resistance resistance)
        {
            switch (resistance)
            {
                case Resistance.Absorb:
                    return -damage;
                case Resistance.Death:
                    return long.MaxValue;
                case Resistance.Immune:
                    return 0;
                case Resistance.Normal:
                    return damage;
                case Resistance.Resist:
                    return damage / 2;
                case Resistance.Weak:
                    return damage * 2;
                case Resistance.Recovery:
                    return long.MinValue;
            }

            return damage;
        }

        /// <summary>
        /// Apply damage. This is subtraction so positive numbers mean take damage, negative means healing.
        /// The return value is capped at 0 and maxHp.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="currentHp"></param>
        /// <param name="maxHp"></param>
        /// <returns></returns>
        public long ApplyDamage(long damage, long currentHp, long maxHp)
        {
            long newHp = currentHp - damage;

            if (damage < 0)
            {
                //damage < 0 means healing, if we end up with less hp it overflowed, max out hp by returning maxHp
                //Also cap newHp at maxHp
                if(newHp < currentHp || newHp > maxHp)
                {
                    newHp = maxHp;
                }
            }
            else
            {
                //damage > 0 means damage, if we end up with more hp the target is dead
                //Also cap new hp at 0
                if(newHp > currentHp || newHp < 0)
                {
                    newHp = 0;
                }
            }

            return newHp;
        }
    }
}

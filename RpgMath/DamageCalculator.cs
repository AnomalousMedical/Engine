using System;

namespace RpgMath
{
    public class DamageCalculator
    {
        private Random random = new Random();

        /// <summary>
        /// Physical damage formula.
        /// </summary>
        /// <param name="att">Total attack power</param>
        /// <param name="level">Level of attacker</param>
        /// <param name="power">The power of the attack. 16 is the base power. Above that is extra, below less. Usually 1 unless special effects.</param>
        /// <param name="def">Defense of target</param>
        /// <returns></returns>
        public ulong Physical(ulong att, ulong level, ulong power, ulong def)
        {
            ulong baseDamage = att + ((att + level) / 32UL) * ((att * level) / 32UL);
            return ((power * (512UL - def) * baseDamage) / (16UL * 512UL));
        }

        /// <summary>
        /// Magical damage formula.
        /// </summary>
        /// <param name="mat">Magic attack</param>
        /// <param name="level">Level of attacker</param>
        /// <param name="power">The power of the attack. 16 is the base power. Above that is extra, below less. Usually 1 unless special effects.</param>
        /// <param name="mdef">Magic defense of target</param>
        /// <returns></returns>
        public ulong Magical(ulong mat, ulong level, ulong power, ulong mdef)
        {
            ulong baseDamage = 6UL * (mat + level);
            return ((power * (512UL - mdef) * baseDamage) / (16UL * 512UL));
        }

        /// <summary>
        /// Cure formula.
        /// </summary>
        /// <param name="mat">Magic attack</param>
        /// <param name="level">Level of attacker</param>
        /// <param name="power">The power of the cure. This is 22 * Power.</param>
        /// <returns></returns>
        public ulong Cure(ulong mat, ulong level, ulong power)
        {
            ulong baseDamage = 6UL * (mat + level);
            return baseDamage + 22UL * power;
        }

        /// <summary>
        /// Item formula.
        /// </summary>
        /// <param name="power">The power of the item. This is 16 * Power.</param>
        /// <param name="def">Def or mdef of target.</param>
        /// <returns></returns>
        public ulong Item(ulong power, ulong def)
        {
            ulong baseDamage = 16UL * power;
            return baseDamage * (512UL - def) / 512UL;
        }

        /// <summary>
        /// Random variation formula.
        /// </summary>
        /// <param name="damage">The calculated damage to randomize.</param>
        /// <returns>A randomized damage value.</returns>
        public ulong RandomVariation(ulong damage)
        {
            return damage * (3841UL + (ulong)random.Next(255)) / 4096UL;
        }
    }
}

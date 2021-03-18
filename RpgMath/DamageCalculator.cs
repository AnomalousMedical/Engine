using System;

namespace RpgMath
{
    public class DamageCalculator
    {
        /// <summary>
        /// Physical damage formula.
        /// </summary>
        /// <param name="att">Total attack power</param>
        /// <param name="level">Level of attacker</param>
        /// <param name="power">The power of the attack. 16 is the base power. Above that is extra, below less. Usually 1 unless special effects.</param>
        /// <param name="def"></param>
        /// <returns></returns>
        public ulong Physical(ulong att, ulong level, ulong power, ulong def)
        {
            ulong baseDamage = att + ((att + level) / 32) * ((att * level) / 32);
            return ((power * (512 - def) * baseDamage) / (16 * 512));
        }
    }
}

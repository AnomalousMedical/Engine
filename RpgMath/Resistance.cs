using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    public enum Resistance
    {
        /// <summary>
        /// The Target takes normal damage from this Element.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The Target is instantly killed by this Element.
        /// </summary>
        Death = 1,
        /// <summary>
        /// The Target cannot evade this Element.
        /// </summary>
        AutoHit = 2,
        /// <summary>
        /// The Target takes double damage from this Element.
        /// </summary>
        Weak = 3,
        /// <summary>
        /// The Target takes half damage from this Element.
        /// </summary>
        Resist = 4,
        /// <summary>
        /// The Target takes no damage from this Element.
        /// </summary>
        Immune = 5,
        /// <summary>
        /// The Target is healed by this Element.
        /// </summary>
        Absorb = 6,
        /// <summary>
        /// The Target is completely recovered by this Element.
        /// </summary>
        Recovery = 7,
    }
}

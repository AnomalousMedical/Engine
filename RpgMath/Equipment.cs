using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    public class Equipment
    {
        public long Attack { get; set; }

        public long AttackPercent { get; set; }

        public long Defense { get; set; }

        public long DefensePercent { get; set; }

        public long MagicAttack { get; set; }

        public long MagicAttackPercent { get; set; }

        public long MagicDefense { get; set; }

        public long MagicDefensePercent { get; set; }

        public long Hp { get; set; }

        public long Mp { get; set; }

        public long Strength { get; set; }

        public long Vitality { get; set; }

        public long Magic { get; set; }

        public long Spirit { get; set; }

        public long Dexterity { get; set; }

        public long Luck { get; set; }

        public long CritChance { get; set; }

        public bool TwoHanded { get; set; }
    }
}

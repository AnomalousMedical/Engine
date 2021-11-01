using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgMath
{
    public class BattleStats : IBattleStats
    {
        public long Hp { get; set; }

        public long Mp { get; set; }

        public long Attack { get; set; }

        public long AttackPercent { get; set; }

        public long Defense { get; set; }

        public long DefensePercent { get; set; }

        public long MagicAttack { get; set; }

        public long MagicAttackPercent { get; set; }

        public long MagicDefense { get; set; }

        public long MagicDefensePercent { get; set; }

        public long Dexterity { get; set; }

        public long Luck { get; set; }

        public bool AllowLuckyEvade { get; set; }

        public long Level { get; set; }

        public long ExtraCritChance { get; set; }

        public Dictionary<Element, Resistance> Resistances { get; set; }

        public Resistance GetResistance(Element element)
        {
            if (Resistances != null && Resistances.TryGetValue(element, out var resistance))
            {
                return resistance;
            }
            return Resistance.Normal;
        }
    }
}

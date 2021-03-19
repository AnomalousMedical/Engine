using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgMath
{
    public class CharacterStats : ICharacterStats
    {
        public ulong Attack { get; set; }

        public ulong AttackPercent { get; set; }

        public ulong Defense { get; set; }

        public ulong DefensePercent { get; set; }

        public ulong MagicAttack { get; set; }

        public ulong MagicDefense { get; set; }

        public ulong MagicDefensePercent { get; set; }

        public ulong Dexterity { get; set; }

        public ulong Luck { get; set; }

        public bool AllowLuckyEvade { get; set; }
    }
}

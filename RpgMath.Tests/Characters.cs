using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class Characters
    {
        public static readonly IBattleStats level10 = new BattleStats()
        {
            Attack = 50,
            AttackPercent = 100,
            Defense = 42,
            DefensePercent = 3,
            MagicAttack = 25,
            MagicDefense = 22,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 13,
            Luck = 19,
            Level = 10
        };

        public static readonly IBattleStats level20 = new BattleStats()
        {
            Attack = 76,
            AttackPercent = 100,
            Defense = 68,
            DefensePercent = 5,
            MagicAttack = 37,
            MagicDefense = 34,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 22,
            Luck = 20,
            Level = 20
        };

        public static readonly IBattleStats level30 = new BattleStats()
        {
            Attack = 100,
            AttackPercent = 100,
            Defense = 102,
            DefensePercent = 7,
            MagicAttack = 51,
            MagicDefense = 47,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 30,
            Luck = 21,
            Level = 30
        };

        public static readonly IBattleStats level40 = new BattleStats()
        {
            Attack = 130,
            AttackPercent = 100,
            Defense = 122,
            DefensePercent = 9,
            MagicAttack = 63,
            MagicDefense = 60,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 38,
            Luck = 22,
            Level = 40
        };

        public static readonly IBattleStats level50 = new BattleStats()
        {
            Attack = 154,
            AttackPercent = 100,
            Defense = 138,
            DefensePercent = 11,
            MagicAttack = 76,
            MagicDefense = 70,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 46,
            Luck = 23,
            Level = 50
        };

        public static readonly IBattleStats level60 = new BattleStats()
        {
            Attack = 176,
            AttackPercent = 100,
            Defense = 154,
            DefensePercent = 12,
            MagicAttack = 84,
            MagicDefense = 78,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 51,
            Luck = 23,
            Level = 60
        };

        public static readonly IBattleStats level70 = new BattleStats()
        {
            Attack = 186,
            AttackPercent = 100,
            Defense = 164,
            DefensePercent = 13,
            MagicAttack = 90,
            MagicDefense = 84,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 53,
            Luck = 24,
            Level = 70
        };

        public static readonly IBattleStats level80 = new BattleStats()
        {
            Attack = 198,
            AttackPercent = 100,
            Defense = 174,
            DefensePercent = 13,
            MagicAttack = 95,
            MagicDefense = 90,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 55,
            Luck = 25,
            Level = 80
        };

        public static readonly IBattleStats level90 = new BattleStats()
        {
            Attack = 200,
            AttackPercent = 100,
            Defense = 180,
            DefensePercent = 14,
            MagicAttack = 99,
            MagicDefense = 94,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 57,
            Luck = 26,
            Level = 90
        };

        public static readonly IBattleStats level99 = new BattleStats()
        {
            Attack = 200,
            AttackPercent = 100,
            Defense = 186,
            DefensePercent = 14,
            MagicAttack = 100,
            MagicDefense = 97,
            MagicDefensePercent = 3,
            AllowLuckyEvade = true,
            Dexterity = 59,
            Luck = 26,
            Level = 99
        };
    }
}

using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class DamageTests
    {

        private readonly ITestOutputHelper output;

        public DamageTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PhysicalDamageEqualLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(level10, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level20, level20, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level30, level30, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level40, level40, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level50, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level60, level60, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level70, level70, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level80, level80, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level90, level90, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level99, level99, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageLowLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(level10, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level20, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level30, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level40, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level50, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level60, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level70, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level80, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level90, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level99, level10, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageMidLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(level10, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level20, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level30, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level40, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level50, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level60, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level70, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level80, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level90, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level99, level50, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageHighLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(level10, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level20, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level30, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level40, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level50, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level60, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level70, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level80, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level90, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(level99, level99, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageHit()
        {
            var calc = new DamageCalculator();

            var result = calc.PhysicalHit(level10, level10);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level20, level20);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level30, level30);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level40, level40);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level50, level50);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level60, level60);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level70, level70);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level80, level80);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level90, level90);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level99, level99);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(level99, level10);
            output.WriteLine(result.ToString());

            //Make sure it can fail
            output.WriteLine("---Start fail test---");
            bool hit = true;
            int sanity = 0;
            const int sanityMax = 10000;
            while (hit)
            {
                hit = calc.PhysicalHit(level10, level99) && ++sanity < sanityMax; //Some kind of sanity check
            }
            output.WriteLine($"{hit.ToString()} took {sanity} tries");

            Assert.NotEqual(sanity, sanityMax);
        }

        [Fact]
        public void PhysicalDamageRandom()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(level10, level10, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level20, level20, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level30, level30, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level40, level40, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level50, level50, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level60, level60, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level70, level70, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level80, level80, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level90, level90, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(level99, level99, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageEqual()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(level10, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level20, level20, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level30, level30, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level40, level40, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level50, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level60, level60, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level70, level70, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level80, level80, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level90, level90, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level99, level99, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageLow()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(level10, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level20, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level30, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level40, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level50, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level60, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level70, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level80, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level90, level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level99, level10, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageMid()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(level10, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level20, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level30, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level40, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level50, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level60, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level70, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level80, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level90, level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level99, level50, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageHigh()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(level10, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level20, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level30, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level40, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level50, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level60, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level70, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level80, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level90, level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(level99, level99, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void Cure()
        {
            var calc = new DamageCalculator();

            const int power = 5;
            var result = calc.Cure(level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level20, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level30, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level40, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level50, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level60, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level70, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level80, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level90, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level99, power);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void CureMid()
        {
            var calc = new DamageCalculator();

            const int power = 35;
            var result = calc.Cure(level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level20, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level30, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level40, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level50, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level60, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level70, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level80, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level90, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level99, power);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void CureHigh()
        {
            var calc = new DamageCalculator();

            const int power = 130;
            var result = calc.Cure(level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level20, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level30, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level40, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level50, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level60, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level70, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level80, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level90, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(level99, power);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void Item()
        {
            var calc = new DamageCalculator();

            var result = calc.Item(16, 18);
            output.WriteLine(result.ToString());

            result = calc.Item(16, 25);
            output.WriteLine(result.ToString());

            result = calc.Item(16, 66);
            output.WriteLine(result.ToString());

            result = calc.Item(16, 25);
            output.WriteLine(result.ToString());

            result = calc.Item(16, 100);
            output.WriteLine(result.ToString());

            result = calc.Item(16, 100);
            output.WriteLine(result.ToString());
        }

        private ICharacterStats level10 = new CharacterStats()
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

        private ICharacterStats level20 = new CharacterStats()
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

        private ICharacterStats level30 = new CharacterStats()
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

        private ICharacterStats level40 = new CharacterStats()
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

        private ICharacterStats level50 = new CharacterStats()
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

        private ICharacterStats level60 = new CharacterStats()
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

        private ICharacterStats level70 = new CharacterStats()
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

        private ICharacterStats level80 = new CharacterStats()
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

        private ICharacterStats level90 = new CharacterStats()
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

        private ICharacterStats level99 = new CharacterStats()
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

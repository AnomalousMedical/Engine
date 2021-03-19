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
        public void PhysicalDamage()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(20, 9, 16, 18);
            output.WriteLine(result.ToString());

            result = calc.Physical(52, 22, 16, 25);
            output.WriteLine(result.ToString());

            result = calc.Physical(75, 39, 16, 66);
            output.WriteLine(result.ToString());

            result = calc.Physical(93, 48, 16, 25);
            output.WriteLine(result.ToString());

            result = calc.Physical(150, 75, 16, 100);
            output.WriteLine(result.ToString());

            result = calc.Physical(200, 99, 16, 100);
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

            //Make sure it can fail
            output.WriteLine("---Start fail test---");
            bool hit = true;
            int sanity = 0;
            while (hit)
            {
                hit = calc.PhysicalHit(level10, level99) && sanity++ < 1000000000; //Some kind of sanity check
                output.WriteLine(hit.ToString());
            }
        }

        [Fact]
        public void PhysicalDamageRandom()
        {
            var calc = new DamageCalculator();

            var result = calc.RandomVariation(calc.Physical(20, 9, 16, 18));
            output.WriteLine(result.ToString());

            result = calc.RandomVariation(calc.Physical(52, 22, 16, 25));
            output.WriteLine(result.ToString());

            result = calc.RandomVariation(calc.Physical(75, 39, 16, 66));
            output.WriteLine(result.ToString());

            result = calc.RandomVariation(calc.Physical(93, 48, 16, 25));
            output.WriteLine(result.ToString());

            result = calc.RandomVariation(calc.Physical(150, 75, 16, 100));
            output.WriteLine(result.ToString());

            result = calc.RandomVariation(calc.Physical(200, 99, 16, 100));
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamage()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(20, 9, 16, 18);
            output.WriteLine(result.ToString());

            result = calc.Magical(52, 22, 16, 25);
            output.WriteLine(result.ToString());

            result = calc.Magical(75, 39, 16, 66);
            output.WriteLine(result.ToString());

            result = calc.Magical(93, 48, 16, 25);
            output.WriteLine(result.ToString());

            result = calc.Magical(150, 75, 16, 100);
            output.WriteLine(result.ToString());

            result = calc.Magical(200, 99, 16, 100);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void Cure()
        {
            var calc = new DamageCalculator();

            var result = calc.Cure(20, 9, 16);
            output.WriteLine(result.ToString());

            result = calc.Cure(52, 22, 16);
            output.WriteLine(result.ToString());

            result = calc.Cure(75, 39, 16);
            output.WriteLine(result.ToString());

            result = calc.Cure(93, 48, 16);
            output.WriteLine(result.ToString());

            result = calc.Cure(150, 75, 16);
            output.WriteLine(result.ToString());

            result = calc.Cure(200, 99, 16);
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
            Luck = 19
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
            Luck = 20
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
            Luck = 21
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
            Luck = 22
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
            Luck = 23
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
            Luck = 23
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
            Luck = 24
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
            Luck = 25
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
            Luck = 26
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
            Luck = 26
        };
    }
}

using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class MagicTests
    {

        private readonly ITestOutputHelper output;

        public MagicTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(8)]
        [InlineData(20)]
        [InlineData(64)]
        [InlineData(133)]
        [InlineData(272)]
        public void MagicalDamageEqual(int power = 16)
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level20, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level30, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level40, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level60, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level70, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level80, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level90, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level99, power);
            output.WriteLine(result.ToString());
        }

        [Theory]
        [InlineData(8)]
        [InlineData(20)]
        [InlineData(64)]
        [InlineData(133)]
        [InlineData(272)]
        public void MagicalDamageVsLow(int power)
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level10, power);
            output.WriteLine(result.ToString());
        }

        [Theory]
        [InlineData(8)]
        [InlineData(20)]
        [InlineData(64)]
        [InlineData(133)]
        [InlineData(272)]
        public void MagicalDamageVsMid(int power)
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level50, power);
            output.WriteLine(result.ToString());
        }

        [Theory]
        [InlineData(8)]
        [InlineData(20)]
        [InlineData(64)]
        [InlineData(133)]
        [InlineData(272)]
        public void MagicalDamageVsHigh(int power)
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level99, power);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level99, power);
            output.WriteLine(result.ToString());
        }

        [Theory]
        [InlineData(Resistance.Normal, 25)]
        [InlineData(Resistance.Normal, 50)]
        [InlineData(Resistance.Normal, 75)]
        [InlineData(Resistance.Normal, 100)]
        [InlineData(Resistance.Normal, 150)]
        [InlineData(Resistance.Normal, 200)]
        [InlineData(Resistance.Normal, 250)]
        [InlineData(Resistance.Normal, 255)]
        public void MagicalDamageHit(Resistance resistance, long magicAttackPercent)
        {
            var calc = new DamageCalculator();

            var result = calc.MagicalHit(Characters.level10, Characters.level10, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level20, Characters.level20, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level30, Characters.level30, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level40, Characters.level40, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level50, Characters.level50, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level60, Characters.level60, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level70, Characters.level70, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level80, Characters.level80, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level90, Characters.level90, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level99, Characters.level99, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());

            result = calc.MagicalHit(Characters.level99, Characters.level10, resistance, magicAttackPercent);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageMiss()
        {
            var calc = new DamageCalculator();

            //Make sure it can fail
            output.WriteLine("---Start fail test---");
            bool hit = true;
            int sanity = 0;
            const int sanityMax = 10000;
            while (hit)
            {
                hit = calc.MagicalHit(Characters.level10, Characters.level99, Resistance.Normal, 150) && ++sanity < sanityMax; //Some kind of sanity check
            }
            output.WriteLine($"{hit.ToString()} took {sanity} tries");

            Assert.NotEqual(sanity, sanityMax);
        }
    }
}

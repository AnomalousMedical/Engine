using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class CriticalTests
    {

        private readonly ITestOutputHelper output;

        public CriticalTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CriticalDamageLow()
        {
            var calc = new DamageCalculator();

            var result = calc.CriticalHit(Characters.level10, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level20, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level30, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level40, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level50, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level60, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level70, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level80, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level90, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level99, Characters.level10);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void CriticalDamageMid()
        {
            var calc = new DamageCalculator();

            var result = calc.CriticalHit(Characters.level10, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level20, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level30, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level40, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level50, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level60, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level70, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level80, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level90, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level99, Characters.level50);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void CriticalDamageHigh()
        {
            var calc = new DamageCalculator();

            var result = calc.CriticalHit(Characters.level10, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level20, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level30, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level40, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level50, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level60, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level70, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level80, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level90, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.CriticalHit(Characters.level99, Characters.level50);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void CriticalHitHappens()
        {
            var calc = new DamageCalculator();

            bool crit = false;
            int sanity = 0;
            const int sanityMax = 1000;
            while (!crit && ++sanity < sanityMax)
            {
                crit = calc.CriticalHit(Characters.level40, Characters.level50);
            }
            output.WriteLine($"{crit.ToString()} took {sanity} tries");

            Assert.NotEqual(sanity, sanityMax);
        }
    }
}

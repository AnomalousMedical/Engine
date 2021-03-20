using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class PhysicalTests
    {

        private readonly ITestOutputHelper output;

        public PhysicalTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void PhysicalDamageEqualLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(Characters.level10, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level20, Characters.level20, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level30, Characters.level30, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level40, Characters.level40, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level50, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level60, Characters.level60, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level70, Characters.level70, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level80, Characters.level80, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level90, Characters.level90, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level99, Characters.level99, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageLowLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(Characters.level10, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level20, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level30, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level40, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level50, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level60, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level70, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level80, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level90, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level99, Characters.level10, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageMidLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(Characters.level10, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level20, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level30, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level40, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level50, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level60, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level70, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level80, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level90, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level99, Characters.level50, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageHighLevel()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(Characters.level10, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level20, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level30, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level40, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level50, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level60, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level70, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level80, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level90, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level99, Characters.level99, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void PhysicalDamageHit()
        {
            var calc = new DamageCalculator();

            var result = calc.PhysicalHit(Characters.level10, Characters.level10);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level20, Characters.level20);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level30, Characters.level30);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level40, Characters.level40);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level50, Characters.level50);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level60, Characters.level60);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level70, Characters.level70);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level80, Characters.level80);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level90, Characters.level90);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level99, Characters.level99);
            output.WriteLine(result.ToString());

            result = calc.PhysicalHit(Characters.level99, Characters.level10);
            output.WriteLine(result.ToString());

            //Make sure it can fail
            output.WriteLine("---Start fail test---");
            bool hit = true;
            int sanity = 0;
            const int sanityMax = 10000;
            while (hit)
            {
                hit = calc.PhysicalHit(Characters.level10, Characters.level99) && ++sanity < sanityMax; //Some kind of sanity check
            }
            output.WriteLine($"{hit.ToString()} took {sanity} tries");

            Assert.NotEqual(sanity, sanityMax);
        }

        [Fact]
        public void PhysicalDamageRandom()
        {
            var calc = new DamageCalculator();

            var result = calc.Physical(Characters.level10, Characters.level10, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level20, Characters.level20, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level30, Characters.level30, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level40, Characters.level40, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level50, Characters.level50, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level60, Characters.level60, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level70, Characters.level70, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level80, Characters.level80, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level90, Characters.level90, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());

            result = calc.Physical(Characters.level99, Characters.level99, 16);
            result = calc.RandomVariation(result);
            output.WriteLine(result.ToString());
        }
    }
}

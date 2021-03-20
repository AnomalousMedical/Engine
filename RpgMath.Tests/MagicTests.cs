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

        [Fact]
        public void MagicalDamageEqual()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level20, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level30, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level40, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level60, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level70, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level80, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level90, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level99, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageLow()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level10, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level10, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageMid()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level50, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level50, 16);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void MagicalDamageHigh()
        {
            var calc = new DamageCalculator();

            var result = calc.Magical(Characters.level10, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level20, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level30, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level40, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level50, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level60, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level70, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level80, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level90, Characters.level99, 16);
            output.WriteLine(result.ToString());

            result = calc.Magical(Characters.level99, Characters.level99, 16);
            output.WriteLine(result.ToString());
        }
    }
}

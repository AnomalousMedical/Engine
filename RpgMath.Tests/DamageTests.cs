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
    }
}

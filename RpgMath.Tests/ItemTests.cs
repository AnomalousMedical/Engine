using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class ItemTests
    {

        private readonly ITestOutputHelper output;

        public ItemTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Item()
        {
            const int power = 16;
            var calc = new DamageCalculator();

            var result = calc.Item(Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level20, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level30, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level40, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level60, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level70, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level80, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level90, power);
            output.WriteLine(result.ToString());

            result = calc.Item(Characters.level99, power);
            output.WriteLine(result.ToString());
        }
    }
}

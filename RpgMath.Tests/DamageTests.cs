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

            var result = calc.Physical(52, 22, 16, 25);
            output.WriteLine(result.ToString());

            result = calc.Physical(93, 48, 16, 25);
            output.WriteLine(result.ToString());

            result = calc.Physical(150, 75, 16, 100);
            output.WriteLine(result.ToString());

            result = calc.Physical(200, 99, 16, 100);
            output.WriteLine(result.ToString());
        }
    }
}

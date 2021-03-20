using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class ApplyDamageTests
    {

        private readonly ITestOutputHelper output;

        public ApplyDamageTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(10, 20, 20, 10)]
        [InlineData(10, 15, 20, 5)]
        [InlineData(10, 5, 20, 0)]
        [InlineData(-10, 15, 20, 20)]
        [InlineData(-10, 5, 20, 15)]
        [InlineData(long.MinValue, 5, 20, 20)]
        [InlineData(long.MaxValue, 5, 20, 0)]
        public void PhysicalDamageEqualLevel(long damage, long currentHp, long maxHp, long expected)
        {
            var calc = new DamageCalculator();
            var result = calc.ApplyDamage(damage, currentHp, maxHp);
            Assert.Equal(expected, result);
        }
    }
}

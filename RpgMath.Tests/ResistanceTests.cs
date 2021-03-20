using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class ResistanceTests
    {

        private readonly ITestOutputHelper output;

        public ResistanceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(10, Resistance.Absorb, -10)]
        [InlineData(10, Resistance.Death, long.MaxValue)]
        [InlineData(10, Resistance.Immune, 0)]
        [InlineData(10, Resistance.Normal, 10)]
        [InlineData(10, Resistance.Recovery, long.MinValue)]
        [InlineData(10, Resistance.Resist, 5)]
        [InlineData(10, Resistance.Weak, 20)]
        public void PhysicalDamageEqualLevel(long damage, Resistance resistance, long expected)
        {
            var calc = new DamageCalculator();
            var result = calc.ApplyResistance(damage, resistance);
            Assert.Equal(expected, result);
        }
    }
}

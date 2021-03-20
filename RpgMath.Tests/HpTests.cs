using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class HpTests
    {

        private readonly ITestOutputHelper output;

        public HpTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ComputeHpGain()
        {
            var calc = new LevelCalculator();

            //Way under baseline
            var result = calc.ComputeHpGain(50, 0, 1);
            output.WriteLine(result.ToString());
            Assert.Equal(181, result);

            //Way over baseline
            result = calc.ComputeHpGain(10, 0, 6000);
            output.WriteLine(result.ToString());
            Assert.Equal(7, result);
        }

        [Theory]
        [InlineData(314, 0)]
        [InlineData(314, 1)]
        [InlineData(314, 2)]
        [InlineData(314, 3)]
        [InlineData(314, 4)]
        [InlineData(314, 5)]
        [InlineData(314, 6)]
        [InlineData(314, 7)]
        [InlineData(314, 8)]
        public void ComputeFullHpLevelGain(long starting, int rank)
        {
            var calc = new LevelCalculator();
            long total = starting;

            for (var level = 2; level < 100; ++level)
            {
                var result = calc.ComputeHpGain(level, rank, total);
                total += result;
                if (level % 10 == 0 || level == 99)
                {
                    output.WriteLine($"Level {level} - {total}");
                }
            }
        }
    }
}

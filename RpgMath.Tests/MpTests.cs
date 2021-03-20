using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class MpTests
    {

        private readonly ITestOutputHelper output;

        public MpTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ComputeMpGain()
        {
            var calc = new LevelCalculator();

            //Way under baseline
            var result = calc.ComputeMpGain(50, 0, 1);
            output.WriteLine(result.ToString());
            Assert.Equal(19, result);

            //Way over baseline
            result = calc.ComputeMpGain(10, 0, 800);
            output.WriteLine(result.ToString());
            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(54, 0)]
        [InlineData(54, 1)]
        [InlineData(54, 2)]
        [InlineData(54, 3)]
        [InlineData(54, 4)]
        [InlineData(54, 5)]
        [InlineData(54, 6)]
        [InlineData(54, 7)]
        [InlineData(54, 8)]
        public void ComputeFullMpLevelGain(long starting, int rank)
        {
            var calc = new LevelCalculator();
            long total = starting;

            for (var level = 2; level < 100; ++level)
            {
                var result = calc.ComputeMpGain(level, rank, total);
                total += result;
                if (level % 10 == 0 || level == 99)
                {
                    output.WriteLine($"Level {level} - {total}");
                }
            }
        }
    }
}
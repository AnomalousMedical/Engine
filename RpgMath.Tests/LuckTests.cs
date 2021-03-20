using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class LuckTests
    {

        private readonly ITestOutputHelper output;

        public LuckTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ComputeLuckGain()
        {
            var calc = new LevelCalculator();
            var result = calc.ComputeLuckGain(10, 0, 20);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 21);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 22);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 23);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 24);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 25);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 26);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 27);
            output.WriteLine(result.ToString());

            result = calc.ComputeLuckGain(10, 0, 28);
            output.WriteLine(result.ToString());

            //Way under baseline
            result = calc.ComputeLuckGain(10, 0, 0);
            output.WriteLine(result.ToString());
            Assert.Equal(3, result);

            //Way over baseline
            result = calc.ComputeLuckGain(10, 0, 50);
            output.WriteLine(result.ToString());
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(15, 0)]
        [InlineData(15, 1)]
        [InlineData(15, 2)]
        [InlineData(15, 3)]
        [InlineData(15, 4)]
        [InlineData(15, 5)]
        [InlineData(15, 6)]
        [InlineData(15, 7)]
        [InlineData(15, 8)]
        public void ComputeFullLuckLevelGain(long starting, int rank)
        {
            var calc = new LevelCalculator();
            long total = starting;

            for (var level = 2; level < 100; ++level)
            {
                var result = calc.ComputeLuckGain(level, rank, total);
                total += result;
                if (level % 10 == 0 || level == 99)
                {
                    output.WriteLine($"Level {level} - {total}");
                }
            }
        }
    }
}

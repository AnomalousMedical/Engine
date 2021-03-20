using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class StatTests
    {

        private readonly ITestOutputHelper output;

        public StatTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ComputePrimaryStatGain()
        {
            var calc = new LevelCalculator();
            var result = calc.ComputePrimaryStatGain(10, 0, 20);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 21);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 22);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 23);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 24);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 25);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 26);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 27);
            output.WriteLine(result.ToString());

            result = calc.ComputePrimaryStatGain(10, 0, 28);
            output.WriteLine(result.ToString());

            //Way under baseline
            result = calc.ComputePrimaryStatGain(10, 0, 0);
            output.WriteLine(result.ToString());
            Assert.Equal(3, result);

            //Way over baseline
            result = calc.ComputePrimaryStatGain(10, 0, 50);
            output.WriteLine(result.ToString());
            Assert.Equal(0, result);
        }
    }
}

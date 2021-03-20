using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class CureTests
    {

        private readonly ITestOutputHelper output;

        public CureTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Cure()
        {
            var calc = new DamageCalculator();

            const int power = 5;
            var result = calc.Cure(Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level20, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level30, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level40, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level60, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level70, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level80, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level90, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level99, power);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void CureMid()
        {
            var calc = new DamageCalculator();

            const int power = 35;
            var result = calc.Cure(Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level20, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level30, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level40, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level60, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level70, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level80, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level90, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level99, power);
            output.WriteLine(result.ToString());
        }

        [Fact]
        public void CureHigh()
        {
            var calc = new DamageCalculator();

            const int power = 130;
            var result = calc.Cure(Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level10, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level20, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level30, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level40, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level50, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level60, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level70, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level80, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level90, power);
            output.WriteLine(result.ToString());

            result = calc.Cure(Characters.level99, power);
            output.WriteLine(result.ToString());
        }
    }
}

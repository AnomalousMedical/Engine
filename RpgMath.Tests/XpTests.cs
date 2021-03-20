using System;
using Xunit;
using Engine;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class XpTests
    {

        private readonly ITestOutputHelper output;

        public XpTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ComputeLevel99HeroXp()
        {
            var calc = new XpCalculator();
            var arch = Archetype.CreateHero();
            var result = calc.GetXpNeeded(arch, 99);
            Assert.Equal(2452783, result);
        }

        [Fact]
        public void ComputeLevel99BrawlerXp()
        {
            var calc = new XpCalculator();
            var arch = Archetype.CreateBrawler();
            var result = calc.GetXpNeeded(arch, 99);
            Assert.Equal(2420933, result);
        }

        [Fact]
        public void ComputeLevel99SageXp()
        {
            var calc = new XpCalculator();
            var arch = Archetype.CreateSage();
            var result = calc.GetXpNeeded(arch, 99);
            Assert.Equal(2484643, result);
        }
    }
}
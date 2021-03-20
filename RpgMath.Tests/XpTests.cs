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
        public void ComputeHeroAllLevelsXp()
        {
            var calc = new XpCalculator();
            var arch = Archetype.CreateHero();
            for(var i = 2; i < 100; ++i)
            {
                var result = calc.GetXpNeeded(arch, i);
                output.WriteLine(result.ToString());
            }
        }

        [Fact]
        public void ComputeLevel7HeroXp()
        {
            var calc = new XpCalculator();
            var arch = Archetype.CreateHero();
            var result = calc.GetXpNeeded(arch, 7);
            Assert.Equal(616, result);
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
        public void ComputeLevel14BrawlerXp()
        {
            var calc = new XpCalculator();
            var arch = Archetype.CreateBrawler();
            var result = calc.GetXpNeeded(arch, 14);
            Assert.Equal(5809, result);
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
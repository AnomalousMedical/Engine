using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace RpgMath.Tests
{
    public class EnemyCurveTests
    {
        [Fact]
        public void ComputeLevel1()
        {
            var curve = new StandardEnemyCurve();
            var hp = curve.GetHp(1, EnemyType.Normal); //Since level 1 is the first actual level, we don't get the very bottom hp number
            Assert.Equal(56, hp);
        }

        [Fact]
        public void ComputeStartRange()
        {
            var curve = new StandardEnemyCurve();
            var hp = curve.GetHp(20, EnemyType.Normal); //The start of the range should return since its 0.0
            Assert.Equal(450, hp);
        }

        [Fact]
        public void ComputeEndRange()
        {
            var curve = new StandardEnemyCurve();
            var hp = curve.GetHp(29, EnemyType.Normal); //At the top of the range you won't hit exactly the top number since it goes 0.0 - 0.9
            Assert.Equal(1125, hp);
        }
    }
}

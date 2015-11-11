using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine;

namespace EngineTest
{
    [TestClass]
    public class GeometryTests
    {
        [TestMethod]
        public void SignedAreaOfTriangle()
        {
            Assert.AreEqual(2.5f, Geometry.SignedAreaOfTriangle(new Vector2(0, 0), new Vector2(3, 1), new Vector2(1, 2)));
        }

        [TestMethod]
        public void AreaOfPolygon()
        {
            Assert.AreEqual(2.5f, Geometry.AreaOfPolygon(new Vector2[] { new Vector2(-1, 1), new Vector2(1, 1), new Vector2(-1, -1), new Vector2(1, -1) }));
        }
    }
}

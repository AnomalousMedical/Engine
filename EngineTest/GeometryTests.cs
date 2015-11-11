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

            //Clockwise triangle
            Assert.AreEqual(2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(0, 0), 
                new Vector2(3, 1), 
                new Vector2(1, 2)));

            Assert.AreEqual(2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(3, 1),
                new Vector2(1, 2)));

            //Counter-Clockwise triangle
            Assert.AreEqual(-2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(0, 0),
                new Vector2(1, 2),
                new Vector2(3, 1)));

            Assert.AreEqual(-2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(1, 2),
                new Vector2(3, 1)));
        }

        [TestMethod]
        public void AreaOfPolygon()
        {
            //Clockwise rectangle
            Assert.AreEqual(-4.0f, Geometry.SignedAreaOfPolygon(new Vector2[] 
            {
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 0),
                new Vector2(0, 0)
            }));

            //Counter-Clockwise rectangle
            Assert.AreEqual(4.0f, Geometry.SignedAreaOfPolygon(new Vector2[] 
            {
                new Vector2(0, 2),
                new Vector2(0, 0),
                new Vector2(2, 0),
                new Vector2(2, 2),
            }));

            //Counter-Clockwise rectangle
            Assert.AreEqual(4.0f, Geometry.SignedAreaOfPolygon(new Vector2[]
            {
                new Vector2(-1, 1),
                new Vector2(-1, -1),
                new Vector2(1, -1),
                new Vector2(1, 1),
            }));
        }
    }
}

using System;
using Xunit;
using Engine;

namespace EngineTest
{
    public class GeometryTests
    {
        [Fact]
        public void SignedAreaOfTriangle()
        {

            //Clockwise triangle
            Assert.Equal(2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(0, 0),
                new Vector2(3, 1),
                new Vector2(1, 2)));

            Assert.Equal(2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(3, 1),
                new Vector2(1, 2)));

            //Counter-Clockwise triangle
            Assert.Equal(-2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(0, 0),
                new Vector2(1, 2),
                new Vector2(3, 1)));

            Assert.Equal(-2.5f, Geometry.SignedAreaOfTriangle(
                new Vector2(1, 2),
                new Vector2(3, 1)));
        }

        [Fact]
        public void AreaOfPolygon()
        {
            //Clockwise rectangle 2x2
            Assert.Equal(-4.0f, Geometry.SignedAreaOfPolygon(new Vector2[]
            {
                new Vector2(0, 2),
                new Vector2(2, 2),
                new Vector2(2, 0),
                new Vector2(0, 0)
            }));

            //Counter-Clockwise rectangle 2x2
            Assert.Equal(4.0f, Geometry.SignedAreaOfPolygon(new Vector2[]
            {
                new Vector2(0, 2),
                new Vector2(0, 0),
                new Vector2(2, 0),
                new Vector2(2, 2),
            }));

            //Counter-Clockwise rectangle 2x2
            Assert.Equal(4.0f, Geometry.SignedAreaOfPolygon(new Vector2[]
            {
                new Vector2(-1, 1),
                new Vector2(-1, -1),
                new Vector2(1, -1),
                new Vector2(1, 1),
            }));

            //Clockwise parallelogram 3x1
            Assert.Equal(-3.0f, Geometry.SignedAreaOfPolygon(new Vector2[]
            {
                new Vector2(-1, 1),
                new Vector2(2, 1),
                new Vector2(1, 0),
                new Vector2(-2, 0)
            }));
        }

        [Fact]
        public void VolumeOfMesh()
        {
            //Cube indices - Clockwise

            var indices = new int[]
            {
                //Top
                0, 1, 2,
                2, 3, 0,

                //Back
                1, 0, 4,
                4, 5, 1,

                //Right
                2, 1, 5,
                5, 6, 2,

                //Left
                0, 3, 7,
                7, 4, 0,

                //Front
                3, 2, 6,
                6, 7, 3,

                //Bottom
                7, 6, 5,
                5, 4, 7
            };

            //Cube 2x2x2
            var vertices = new Vector3[]
            {
                new Vector3(-1,  1, -1), //0
                new Vector3( 1,  1, -1), //1
                new Vector3( 1,  1,  1), //2
                new Vector3(-1,  1,  1), //3
                new Vector3(-1, -1, -1), //4
                new Vector3( 1, -1, -1), //5
                new Vector3( 1, -1,  1), //6
                new Vector3(-1, -1,  1), //7
            };

            float volume = Geometry.VolumeOfMesh(vertices, indices);

            Assert.True(Math.Abs(8.0f - volume) < 0.0001f, $"{volume} not within margin of error to {8.0f}");

            //Cube 2x2x2 - not at origin
            Vector3 translation = new Vector3(20, 3, 40);
            for(int i = 0; i < vertices.Length; ++i)
            {
                vertices[i] += translation;
            }

            volume = Geometry.VolumeOfMesh(vertices, indices);

            Assert.True(Math.Abs(8.0f - volume) < 0.0001f, $"{volume} not within margin of error to {8.0f}");

            unsafe
            {
                fixed(Vector3* verts = &vertices[0])
                {
                    fixed(int* ind = &indices[0])
                    {
                        volume = Geometry.VolumeOfMesh(verts, ind, indices.Length);
                    }
                }
                Assert.True(Math.Abs(8.0f - volume) < 0.0001f, $"{volume} not within margin of error to {8.0f}");
            }
        }
    }
}

using DiligentEngine;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using RogueLikeMapBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGenerator
{
    public class MapMesh : IDisposable
    {
        private Mesh floorMesh;
        private Mesh wallMesh;
        private List<Vector3> boundaryCubeCenterPoints;

        public IEnumerable<Vector3> BoundaryCubeCenterPoints => boundaryCubeCenterPoints;

        /// <summary>
        /// The number of units on the generated map to make on the real map in the X direction.
        /// </summary>
        public float MapUnitX { get; } = 2f;

        /// <summary>
        /// The number of units on the generated map to make on the real map in the Y direction.
        /// </summary>
        public float MapUnitY { get; } = 2f;

        /// <summary>
        /// The number of units on the generated map to make on the real map in the Z direction.
        /// </summary>
        public float MapUnitZ { get; } = 2f;

        public MapMesh(csMapbuilder mapbuilder, IRenderDevice renderDevice, float mapUnitX = 2f, float mapUnitY = 2f, float mapUnitZ = 2f)
        {
            MapUnitX = mapUnitX;
            MapUnitY = mapUnitY;
            MapUnitZ = mapUnitZ;

            var halfUnitX = MapUnitX / 2.0f;
            var halfUnitY = MapUnitY / 2.0f;
            var halfUnitZ = MapUnitZ / 2.0f;

            var map = mapbuilder.map;

            this.floorMesh = new Mesh();
            this.wallMesh = new Mesh();

            //Build map from the bottom up since camera always faces north
            //This will allow depth buffer to cancel pixel shaders

            //Figure out number of quads
            uint numFloorQuads = 0;
            uint numWallQuads = 0;
            uint numBoundaryCubes = 0;
            var mapWidth = mapbuilder.Map_Size.Width;
            var mapHeight = mapbuilder.Map_Size.Height;
            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    if (map[mapX, mapY])
                    {
                        ++numFloorQuads;
                        ++numBoundaryCubes;
                        
                        int test;

                        //South wall
                        test = mapY - 1;
                        if (test < 0 || !map[mapX, test])
                        {
                            ++numWallQuads;
                            ++numBoundaryCubes;
                        }

                        //North wall
                        test = mapY + 1;
                        if (test >= mapHeight || !map[mapX, test])
                        {
                            ++numWallQuads;
                            ++numBoundaryCubes;
                        }

                        //West wall
                        test = mapX - 1;
                        if (test < 0 || !map[test, mapY])
                        {
                            numWallQuads += 2;
                            ++numBoundaryCubes;
                        }

                        //East wall
                        test = mapX + 1;
                        if (test > mapWidth || !map[test, mapY])
                        {
                            numWallQuads += 2;
                            ++numBoundaryCubes;
                        }
                    }
                    else
                    {
                        ++numWallQuads;
                    }
                }
            }

            //Make mesh
            floorMesh.Begin(numFloorQuads);
            wallMesh.Begin(numWallQuads);

            boundaryCubeCenterPoints = new List<Vector3>((int)(numBoundaryCubes));

            var floorY = -halfUnitY;
            var centerY = 0;
            var topY = halfUnitY;
            var yUvBottom = 1.0f;
            if(MapUnitY < 1.0f)
            {
                yUvBottom = MapUnitY / MapUnitX;
            }
            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    if (map[mapX, mapY])
                    {
                        //Floor

                        var left = mapX * MapUnitX;
                        var right = left + MapUnitX;
                        var far = mapY * MapUnitZ;
                        var near = far - MapUnitZ;
                        floorMesh.AddQuad(
                            new Vector3(left, floorY, far),
                            new Vector3(right, floorY, far),
                            new Vector3(right, floorY, near),
                            new Vector3(left, floorY, near),
                            Vector3.Up,
                            new Vector2(0, 0),
                            new Vector2(1, 1));

                        boundaryCubeCenterPoints.Add(new Vector3(left + halfUnitX, floorY - halfUnitY, far - halfUnitZ));

                        int test;

                        //South wall
                        test = mapY - 1;
                        if (test < 0 || !map[mapX, test])
                        {
                            wallMesh.AddQuad(
                                new Vector3(left, topY, near),
                                new Vector3(right, topY, near),
                                new Vector3(right, floorY, near),
                                new Vector3(left, floorY, near),
                                Vector3.Backward,
                                new Vector2(0, 0),
                                new Vector2(1, yUvBottom));

                            boundaryCubeCenterPoints.Add(new Vector3(left + halfUnitX, centerY, near - halfUnitZ));
                        }

                        //North wall
                        test = mapY + 1;
                        if (test >= mapHeight || !map[mapX, test])
                        {
                            //Face backward too, north facing camera
                            wallMesh.AddQuad(
                                new Vector3(left, topY, far),
                                new Vector3(right, topY, far),
                                new Vector3(right, floorY, far),
                                new Vector3(left, floorY, far),
                                Vector3.Backward,
                                new Vector2(0, 0),
                                new Vector2(1, yUvBottom));

                            boundaryCubeCenterPoints.Add(new Vector3(left + halfUnitX, centerY, far + halfUnitZ));
                        }

                        //West wall
                        test = mapX - 1;
                        if (test < 0 || !map[test, mapY])
                        {
                            wallMesh.AddQuad(
                                new Vector3(left, topY, near),
                                new Vector3(left, topY, far),
                                new Vector3(left, floorY, far),
                                new Vector3(left, floorY, near),
                                Vector3.Right,
                                new Vector2(0, 0),
                                new Vector2(1, yUvBottom));

                            wallMesh.AddQuad(
                                new Vector3(left, topY, far),
                                new Vector3(left, topY, near),
                                new Vector3(left, floorY, near),
                                new Vector3(left, floorY, far),
                                Vector3.Left,
                                new Vector2(0, 0),
                                new Vector2(1, yUvBottom));

                            boundaryCubeCenterPoints.Add(new Vector3(left - halfUnitX, centerY, near + halfUnitZ));
                        }

                        //East wall
                        test = mapX + 1;
                        if (test > mapWidth || !map[test, mapY])
                        {
                            wallMesh.AddQuad(
                                new Vector3(right, topY, near),
                                new Vector3(right, topY, far),
                                new Vector3(right, floorY, far),
                                new Vector3(right, floorY, near),
                                Vector3.Right,
                                new Vector2(0, 0),
                                new Vector2(1, 1));

                            wallMesh.AddQuad(
                                new Vector3(right, topY, far),
                                new Vector3(right, topY, near),
                                new Vector3(right, floorY, near),
                                new Vector3(right, floorY, far),
                                Vector3.Left,
                                new Vector2(0, 0),
                                new Vector2(1, 1));

                            boundaryCubeCenterPoints.Add(new Vector3(right + halfUnitX, centerY, near + halfUnitZ));
                        }
                    }
                    else
                    {
                        //Floor outside
                        //Floor

                        var left = mapX * MapUnitX;
                        var right = left + MapUnitX;
                        var far = mapY * MapUnitZ;
                        var near = far - MapUnitZ;
                        wallMesh.AddQuad(
                            new Vector3(left, topY, far),
                            new Vector3(right, topY, far),
                            new Vector3(right, topY, near),
                            new Vector3(left, topY, near),
                            Vector3.Up,
                            new Vector2(0, 0),
                            new Vector2(1, 1));
                    }
                }
            }

            floorMesh.End(renderDevice);
            wallMesh.End(renderDevice);
        }

        public Vector3 PointToVector(int x, int y)
        {
            var left = x * MapUnitX;
            var far = y * MapUnitZ;
            return new Vector3(left + MapUnitX / 2f, 0f, far - MapUnitZ / 2f);
        }

        public void Dispose()
        {
            this.floorMesh.Dispose();
            this.wallMesh.Dispose();
        }

        public Mesh FloorMesh => floorMesh;

        public Mesh WallMesh => wallMesh;
    }
}

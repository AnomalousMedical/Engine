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
        private List<Vector3> floorCubeCenterPoints;
        private List<Vector3> boundaryCubeCenterPoints;
        private Quaternion floorCubeRot;

        public IEnumerable<Vector3> FloorCubeCenterPoints => floorCubeCenterPoints;

        public IEnumerable<Vector3> BoundaryCubeCenterPoints => boundaryCubeCenterPoints;

        public Quaternion FloorCubeRot => floorCubeRot;

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
            uint numFloorCubes = 0;
            var mapWidth = mapbuilder.Map_Size.Width;
            var mapHeight = mapbuilder.Map_Size.Height;
            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    if (map[mapX, mapY])
                    {
                        ++numFloorQuads;
                        ++numFloorCubes;

                        int test;

                        //South wall
                        test = mapY - 1;
                        if (test < 0 || !map[mapX, test])
                        {
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
                            ++numWallQuads;
                            ++numBoundaryCubes;
                        }

                        //East wall
                        test = mapX + 1;
                        if (test > mapWidth || !map[test, mapY])
                        {
                            ++numWallQuads;
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
            floorCubeCenterPoints = new List<Vector3>((int)(numFloorCubes));

            //float floorY = -halfUnitY;
            //float centerY = 0f;
            //float topY = halfUnitY;
            float yUvBottom = 1.0f;
            if (MapUnitY < 1.0f)
            {
                yUvBottom = MapUnitY / MapUnitX;
            }

            float yOffset = 1F;
            float halfYOffset = yOffset / 2f;

            float xInfluence = 0;
            float yInfluence = 1.0f - xInfluence;

            float xHeightStep = yOffset * xInfluence;
            float yHeightStep = yOffset * yInfluence;
            float xHeightBegin = -xHeightStep * mapbuilder.Map_Size.Height;
            float xHeightAdjust = 0;
            float yHeightAdjust = 0;

            Vector3 dirInfluence = new Vector3(xHeightStep, 0, yHeightStep).normalized();
            Vector3 floorCubeRotationVec = new Vector3(halfUnitX * dirInfluence.x, halfYOffset, halfUnitZ * dirInfluence.z).normalized();
            floorCubeRot = Quaternion.shortestArcQuat(ref dirInfluence, ref floorCubeRotationVec);

            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                xHeightAdjust = xHeightBegin;
                yHeightAdjust -= yHeightStep;

                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    xHeightAdjust += xHeightStep;

                    var centerY = xHeightAdjust + yHeightAdjust;
                    var floorY = centerY - halfUnitY;
                    var topY = centerY + halfUnitY;

                    if (map[mapX, mapY])
                    {
                        //Floor

                        var left = mapX * MapUnitX;
                        var right = left + MapUnitX;
                        var far = mapY * MapUnitZ;
                        var near = far - MapUnitZ;
                        floorMesh.AddQuad(
                            new Vector3(left, floorY + halfYOffset, far),
                            new Vector3(right, floorY + halfYOffset, far),
                            new Vector3(right, floorY - halfYOffset, near),
                            new Vector3(left, floorY - halfYOffset, near),
                            Vector3.Up,
                            new Vector2(0, 0),
                            new Vector2(1, 1));

                        floorCubeCenterPoints.Add(new Vector3(left + halfUnitX, floorY - halfUnitY, far - halfUnitZ));

                        int test;

                        //South wall
                        test = mapY - 1;
                        if (test < 0 || !map[mapX, test])
                        {
                            //No mesh needed here, can't see it

                            boundaryCubeCenterPoints.Add(new Vector3(left + halfUnitX, centerY, near - halfUnitZ));
                        }

                        //North wall
                        test = mapY + 1;
                        if (test >= mapHeight || !map[mapX, test])
                        {
                            //Face backward too, north facing camera
                            wallMesh.AddQuad(
                                new Vector3(left, topY + halfYOffset, far),
                                new Vector3(right, topY + halfYOffset, far),
                                new Vector3(right, floorY + halfYOffset, far),
                                new Vector3(left, floorY + halfYOffset, far),
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
                                new Vector3(left, topY - halfYOffset, near),
                                new Vector3(left, topY + halfYOffset, far),
                                new Vector3(left, floorY + halfYOffset, far),
                                new Vector3(left, floorY - halfYOffset, near),
                                Vector3.Right,
                                new Vector2(0, 0),
                                new Vector2(1, yUvBottom));

                            boundaryCubeCenterPoints.Add(new Vector3(left - halfUnitX, centerY, near + halfUnitZ));
                        }

                        //East wall
                        test = mapX + 1;
                        if (test > mapWidth || !map[test, mapY])
                        {
                            wallMesh.AddQuad(
                                new Vector3(right, topY + halfYOffset, far),
                                new Vector3(right, topY - halfYOffset, near),
                                new Vector3(right, floorY - halfYOffset, near),
                                new Vector3(right, floorY + halfYOffset, far),
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
                            new Vector3(left, topY + halfYOffset, far),
                            new Vector3(right, topY + halfYOffset, far),
                            new Vector3(right, topY - halfYOffset, near),
                            new Vector3(left, topY - halfYOffset, near),
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
            return new Vector3(left + MapUnitX / 2f, MapUnitY / 2f, far - MapUnitZ / 2f);
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

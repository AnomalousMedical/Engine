﻿using DiligentEngine;
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

            float floorY = -halfUnitY;
            float centerY = 0f;
            float topY = halfUnitY;
            float yUvBottom = 1.0f;
            if(MapUnitY < 1.0f)
            {
                yUvBottom = MapUnitY / MapUnitX;
            }

            float yOffset = 0.3f;
            float halfYOffset = yOffset / 2f;

            //Vector3 floorCubeRotationVec = new Vector3(halfUnitX, -halfYOffset, 0).normalized();
            //floorCubeRot = Quaternion.shortestArcQuat(ref Vector3.Right, ref floorCubeRotationVec);

            Vector3 floorCubeRotationVec = new Vector3(0, halfYOffset, halfUnitZ).normalized();
            floorCubeRot = Quaternion.shortestArcQuat(ref Vector3.Forward, ref floorCubeRotationVec);

            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                //floorY = -halfUnitY;
                //centerY = 0f;
                //topY = halfUnitY;

                floorY -= yOffset;
                centerY -= yOffset;
                topY -= yOffset;

                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    //floorY -= yOffset;
                    //centerY -= yOffset;
                    //topY -= yOffset;

                    //Build quad for surface first
                    var center = new Vector3(mapX * MapUnitX, floorY, mapY * MapUnitZ);
                    var leftFar = Quaternion.quatRotate(floorCubeRot, new Vector3(-halfUnitX, 0, halfUnitZ)) + center;
                    var rightFar = Quaternion.quatRotate(floorCubeRot, new Vector3(halfUnitX, 0, halfUnitZ)) + center;
                    var rightNear = Quaternion.quatRotate(floorCubeRot, new Vector3(halfUnitX, 0, -halfUnitZ)) + center;
                    var leftNear = Quaternion.quatRotate(floorCubeRot, new Vector3(-halfUnitX, 0, -halfUnitZ)) + center;

                    if (map[mapX, mapY])
                    {
                        //Floor
                        var left = leftFar.x;
                        var right = rightNear.x;
                        var far = leftFar.z;
                        var near = rightNear.z;
                        floorMesh.AddQuad(
                            leftFar,
                            rightFar,
                            rightNear,
                            leftNear,
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
                            var wallLeftTop = leftFar;
                            wallLeftTop.y += mapUnitY;
                            var wallRightTop = rightFar;
                            wallRightTop.y += mapUnitY;

                            //Face backward too, north facing camera
                            wallMesh.AddQuad(
                                wallLeftTop,
                                wallRightTop,
                                rightFar,
                                leftFar,
                                Vector3.Backward,
                                new Vector2(0, 0),
                                new Vector2(1, yUvBottom));

                            boundaryCubeCenterPoints.Add(new Vector3(left + halfUnitX, centerY, far + halfUnitZ));
                        }

                        //West wall
                        test = mapX - 1;
                        if (test < 0 || !map[test, mapY])
                        {
                            var topNear = leftNear;
                            topNear.y += mapUnitY;

                            var topFar = leftFar;
                            topFar.y += mapUnitY;

                            wallMesh.AddQuad(
                                topNear,
                                topFar,
                                leftFar,
                                leftNear,
                                Vector3.Right,
                                new Vector2(0, 0),
                                new Vector2(1, yUvBottom));

                            boundaryCubeCenterPoints.Add(new Vector3(left - halfUnitX, centerY, near + halfUnitZ));
                        }

                        //East wall
                        test = mapX + 1;
                        if (test > mapWidth || !map[test, mapY])
                        {
                            var topNear = rightNear;
                            topNear.y += mapUnitY;

                            var topFar = rightFar;
                            topFar.y += mapUnitY;
                            
                            wallMesh.AddQuad(
                                topFar,
                                topNear,
                                rightNear,
                                rightFar,
                                Vector3.Left,
                                new Vector2(0, 0),
                                new Vector2(1, 1));

                            boundaryCubeCenterPoints.Add(new Vector3(right + halfUnitX, centerY, near + halfUnitZ));
                        }
                    }
                    else
                    {
                        //Floor outside
                        var offset = new Vector3(0, MapUnitY, 0);

                        wallMesh.AddQuad(
                            leftFar + offset,
                            rightFar + offset,
                            rightNear + offset,
                            leftNear + offset,
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

using DiligentEngine;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using RogueLikeMapBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator
{
    public class MapMesh : IDisposable
    {
        private Mesh floorMesh;
        private Mesh wallMesh;
        private List<MapMeshPosition> floorCubeCenterPoints;
        private List<Vector3> boundaryCubeCenterPoints;
        private MapMeshSquareInfo[,] squareInfo; //This array is 1 larger in each dimension, use accessor to translate points

        public IEnumerable<MapMeshPosition> FloorCubeCenterPoints => floorCubeCenterPoints;

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

        public float MaxSlopeY { get; set; } = 1f;

        public MapMesh(csMapbuilder mapbuilder, IRenderDevice renderDevice, float mapUnitX = 2f, float mapUnitY = 2f, float mapUnitZ = 2f)
        {
            MapUnitX = mapUnitX;
            MapUnitY = mapUnitY;
            MapUnitZ = mapUnitZ;

            var halfUnitX = MapUnitX / 2.0f;
            var halfUnitY = MapUnitY / 2.0f;
            var halfUnitZ = MapUnitZ / 2.0f;
            var mapWidth = mapbuilder.Map_Size.Width;
            var mapHeight = mapbuilder.Map_Size.Height;

            var map = mapbuilder.map;
            var slopeMap = new Slope[mapWidth, mapHeight];

            int mainCorridorSlopeSquareCount = 0;

            IntVector2 previousCorridor = new IntVector2();
            {
                var corridor = mapbuilder.Corridors.First();
                var mapX = corridor.x;
                var mapY = corridor.y;
                var north = GetNorthSquare(mapX, mapY, map, mapHeight);
                if (north == csMapbuilder.RoomCell)
                {
                    previousCorridor = new IntVector2(mapX, mapY + 1);
                }
                else
                {
                    var south = GetSouthSquare(mapX, mapY, map);
                    if (south == csMapbuilder.RoomCell)
                    {
                        previousCorridor = new IntVector2(mapX, mapY - 1);
                    }
                    else
                    {
                        var east = GetEastSquare(mapX, mapY, map, mapWidth);
                        if (east == csMapbuilder.RoomCell)
                        {
                            previousCorridor = new IntVector2(mapX + 1, mapY);
                        }
                        else
                        {
                            var west = GetWestSquare(mapX, mapY, map);
                            if (west == csMapbuilder.RoomCell)
                            {
                                previousCorridor = new IntVector2(mapX - 1, mapY);
                            }
                        }
                    }
                }
            }

            foreach (var corridor in mapbuilder.Corridors.Where(i => map[i.x, i.y] == csMapbuilder.MainCorridorCell))
            {
                var mapX = corridor.x;
                var mapY = corridor.y;
                var slope = new Slope()
                {
                    PreviousPoint = previousCorridor - corridor
                };
                previousCorridor = corridor;
                var north = GetNorthSquare(mapX, mapY, map, mapHeight);
                var south = GetSouthSquare(mapX, mapY, map);
                var east = GetEastSquare(mapX, mapY, map, mapWidth);
                var west = GetWestSquare(mapX, mapY, map);
                if (north == csMapbuilder.MainCorridorCell || north == csMapbuilder.EmptyCell)
                {
                    if (south == csMapbuilder.MainCorridorCell || south == csMapbuilder.EmptyCell)
                    {
                        if (east == csMapbuilder.MainCorridorCell || east == csMapbuilder.EmptyCell)
                        {
                            if (west == csMapbuilder.MainCorridorCell || west == csMapbuilder.EmptyCell)
                            {
                                //Allowed to slope if going in 1 direction, not a corner
                                if ((north == csMapbuilder.MainCorridorCell && south == csMapbuilder.MainCorridorCell)
                                    || (east == csMapbuilder.MainCorridorCell && west == csMapbuilder.MainCorridorCell))
                                {
                                    ++mainCorridorSlopeSquareCount;
                                    slope.YOffset = 0.2f;
                                }
                            }
                        }
                    }
                }

                //Check for terminating rooms
                if(north < csMapbuilder.CorridorCell && north >= csMapbuilder.RoomCell)
                {
                    HandleRoom(mapX, mapY + 1, mapbuilder.Rooms[north - csMapbuilder.RoomCell], corridor, slopeMap);
                }
                if (south < csMapbuilder.CorridorCell && south >= csMapbuilder.RoomCell)
                {
                    HandleRoom(mapX, mapY - 1, mapbuilder.Rooms[south - csMapbuilder.RoomCell], corridor, slopeMap);
                }
                if (east < csMapbuilder.CorridorCell && east >= csMapbuilder.RoomCell)
                {
                    HandleRoom(mapX, mapY + 1, mapbuilder.Rooms[east - csMapbuilder.RoomCell], corridor, slopeMap);
                }
                if (west < csMapbuilder.CorridorCell && west >= csMapbuilder.RoomCell)
                {
                    HandleRoom(mapX, mapY - 1, mapbuilder.Rooms[west - csMapbuilder.RoomCell], corridor, slopeMap);
                }

                slopeMap[mapX, mapY] = slope;
            }

            float mainCorridorMaxSlope = mainCorridorSlopeSquareCount * MaxSlopeY;

            var squareCenterMapWidth = mapWidth + 2;
            var squareCenterMapHeight = mapHeight + 2;
            squareInfo = new MapMeshSquareInfo[squareCenterMapWidth, squareCenterMapHeight];

            this.floorMesh = new Mesh();
            this.wallMesh = new Mesh();

            //Build map from the bottom up since camera always faces north
            //This will allow depth buffer to cancel pixel shaders

            //Figure out number of quads
            uint numFloorQuads = 0;
            uint numWallQuads = 0;
            uint numBoundaryCubes = 0;
            uint numFloorCubes = 0;
            for (int mapY = 0; mapY < mapHeight; ++mapY)
            {
                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    if (map[mapX, mapY] >= csMapbuilder.RoomCell)
                    {
                        ++numFloorQuads;
                        ++numFloorCubes;

                        int test;

                        //South wall
                        test = mapY - 1;
                        if (test < 0 || map[mapX, test] == csMapbuilder.EmptyCell)
                        {
                            ++numBoundaryCubes;
                        }

                        //North wall
                        test = mapY + 1;
                        if (test >= mapHeight || map[mapX, test] == csMapbuilder.EmptyCell)
                        {
                            ++numWallQuads;
                            ++numBoundaryCubes;
                        }

                        //West wall
                        test = mapX - 1;
                        if (test < 0 || map[test, mapY] == csMapbuilder.EmptyCell)
                        {
                            ++numWallQuads;
                            ++numBoundaryCubes;
                        }

                        //East wall
                        test = mapX + 1;
                        if (test >= mapWidth || map[test, mapY] == csMapbuilder.EmptyCell)
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
            floorCubeCenterPoints = new List<MapMeshPosition>((int)(numFloorCubes));

            float yUvBottom = 1.0f;
            if (MapUnitY < 1.0f)
            {
                yUvBottom = MapUnitY / MapUnitX;
            }

            foreach (var corridor in mapbuilder.Corridors.Where(i => map[i.x, i.y] == csMapbuilder.MainCorridorCell))
            {
                var mapX = corridor.x;
                var mapY = corridor.y;
                ProcessSquare(halfUnitX, halfUnitY, halfUnitZ, mapWidth, mapHeight, map, slopeMap, yUvBottom, mapX, mapY);
            }

            for (int mapY = 0; mapY < mapHeight; ++mapY)
            {
                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    if (map[mapX, mapY] != csMapbuilder.MainCorridorCell)
                    {
                        ProcessSquare(halfUnitX, halfUnitY, halfUnitZ, mapWidth, mapHeight, map, slopeMap, yUvBottom, mapX, mapY);
                    }
                }
            }

            floorMesh.End(renderDevice);
            wallMesh.End(renderDevice);
        }

        private void ProcessSquare(float halfUnitX, float halfUnitY, float halfUnitZ, int mapWidth, int mapHeight, ushort[,] map, Slope[,] slopeMap, float yUvBottom, int mapX, int mapY)
        {
            var left = mapX * MapUnitX;
            var right = left + MapUnitX;
            var far = mapY * MapUnitZ;
            var near = far - MapUnitZ;

            var slope = slopeMap[mapX, mapY];
            bool positivePrevious = slope.PreviousPoint.x > 0 || slope.PreviousPoint.y > 0;

            var realHalfY = slope.YOffset / 2f;
            float halfYOffset = Math.Abs(realHalfY);

            bool xDir = slope.PreviousPoint.x != 0;
            float xInfluence = xDir ? 1 : 0; //1 for x 0 for y
            float yInfluence = 1.0f - xInfluence;

            float xHeightStep = slope.YOffset * xInfluence;
            float yHeightStep = slope.YOffset * yInfluence;

            Vector3 dirInfluence = new Vector3(xHeightStep, 0, yHeightStep).normalized();
            Vector3 floorCubeRotationVec = new Vector3(halfUnitX * dirInfluence.x, halfYOffset, halfUnitZ * dirInfluence.z).normalized();
            var floorCubeRot = Quaternion.shortestArcQuat(ref dirInfluence, ref floorCubeRotationVec);
            if (positivePrevious)
            {
                floorCubeRot = floorCubeRot.inverse();
            }

            //Get previous square center
            var previousSlope = squareInfo[mapX + slope.PreviousPoint.x + 1, mapY + slope.PreviousPoint.y + 1];

            var totalYOffset = previousSlope.HalfYOffset + realHalfY;
            var centerY = previousSlope.Center.y + totalYOffset;

            //Update our center point in the slope grid
            squareInfo[mapX + 1, mapY + 1] = new MapMeshSquareInfo(new Vector3(left + halfUnitX, centerY, far - halfUnitZ), realHalfY);

            var floorY = centerY - halfUnitY;
            float floorFarLeftY = 0;
            float floorFarRightY = 0;
            float floorNearRightY = 0;
            float floorNearLeftY = 0;
            if (slope.YOffset > 0 && !positivePrevious)
            {
                if (xDir)
                {
                    floorFarLeftY = floorY - halfYOffset;
                    floorFarRightY = floorY + halfYOffset;
                    floorNearRightY = floorY + halfYOffset;
                    floorNearLeftY = floorY - halfYOffset;
                }
                else
                {
                    floorFarLeftY = floorY + halfYOffset;
                    floorFarRightY = floorY + halfYOffset;
                    floorNearRightY = floorY - halfYOffset;
                    floorNearLeftY = floorY - halfYOffset;
                }
            }
            else
            {
                if (xDir)
                {

                    floorFarLeftY = floorY + halfYOffset;
                    floorFarRightY = floorY - halfYOffset;
                    floorNearRightY = floorY - halfYOffset;
                    floorNearLeftY = floorY + halfYOffset;
                }
                else
                {
                    floorFarLeftY = floorY - halfYOffset;
                    floorFarRightY = floorY - halfYOffset;
                    floorNearRightY = floorY + halfYOffset;
                    floorNearLeftY = floorY + halfYOffset;
                }
            }

            var floorNormal = Quaternion.quatRotate(floorCubeRot, Vector3.Up);

            if (map[mapX, mapY] >= csMapbuilder.RoomCell)
            {
                //Floor
                floorMesh.AddQuad(
                    new Vector3(left, floorFarLeftY, far),
                    new Vector3(right, floorFarRightY, far),
                    new Vector3(right, floorNearRightY, near),
                    new Vector3(left, floorNearLeftY, near),
                    floorNormal,
                    new Vector2(0, 0),
                    new Vector2(1, 1));

                floorCubeCenterPoints.Add(new MapMeshPosition(new Vector3(left + halfUnitX, floorY - halfUnitY, far - halfUnitZ), floorCubeRot));

                int test;

                //South wall
                test = mapY - 1;
                if (test < 0 || map[mapX, test] == csMapbuilder.EmptyCell)
                {
                    //No mesh needed here, can't see it
                    boundaryCubeCenterPoints.Add(new Vector3(left + halfUnitX, centerY, near - halfUnitZ));
                }

                //North wall
                test = mapY + 1;
                if (test >= mapHeight || map[mapX, test] == csMapbuilder.EmptyCell)
                {
                    //Face backward too, north facing camera
                    wallMesh.AddQuad(
                        new Vector3(left, floorFarLeftY + MapUnitY, far),
                        new Vector3(right, floorFarRightY + MapUnitY, far),
                        new Vector3(right, floorFarRightY, far),
                        new Vector3(left, floorFarLeftY, far),
                        Vector3.Backward,
                        new Vector2(0, 0),
                        new Vector2(1, yUvBottom));

                    boundaryCubeCenterPoints.Add(new Vector3(left + halfUnitX, centerY, far + halfUnitZ));
                }

                //West wall
                test = mapX - 1;
                if (test < 0 || map[test, mapY] == csMapbuilder.EmptyCell)
                {
                    wallMesh.AddQuad(
                        new Vector3(left, floorNearLeftY + MapUnitY, near),
                        new Vector3(left, floorFarLeftY + MapUnitY, far),
                        new Vector3(left, floorFarLeftY, far),
                        new Vector3(left, floorNearLeftY, near),
                        Vector3.Right,
                        new Vector2(0, 0),
                        new Vector2(1, yUvBottom));

                    boundaryCubeCenterPoints.Add(new Vector3(left - halfUnitX, centerY, near + halfUnitZ));
                }

                //East wall
                test = mapX + 1;
                if (test >= mapWidth || map[test, mapY] == csMapbuilder.EmptyCell)
                {
                    wallMesh.AddQuad(
                        new Vector3(right, floorFarRightY + MapUnitY, far),
                        new Vector3(right, floorNearRightY + MapUnitY, near),
                        new Vector3(right, floorNearRightY, near),
                        new Vector3(right, floorFarRightY, far),
                        Vector3.Left,
                        new Vector2(0, 0),
                        new Vector2(1, 1));

                    boundaryCubeCenterPoints.Add(new Vector3(right + halfUnitX, centerY, near + halfUnitZ));
                }
            }
            else
            {
                //Floor outside
                wallMesh.AddQuad(
                    new Vector3(left, floorFarLeftY + MapUnitY, far),
                    new Vector3(right, floorFarRightY + MapUnitY, far),
                    new Vector3(right, floorNearRightY + MapUnitY, near),
                    new Vector3(left, floorNearLeftY + MapUnitY, near),
                    floorNormal,
                    new Vector2(0, 0),
                    new Vector2(1, 1));
            }
        }

        public Vector3 PointToVector(int x, int y)
        {
            return squareInfo[x + 1, y + 1].Center;
        }

        public void Dispose()
        {
            this.floorMesh.Dispose();
            this.wallMesh.Dispose();
        }

        public Mesh FloorMesh => floorMesh;

        public Mesh WallMesh => wallMesh;

        private UInt16 GetNorthSquare(int x, int y, ushort[,] map, int height)
        {
            y += 1;
            if (y >= height)
            {
                return csMapbuilder.EmptyCell;
            }
            return map[x, y];
        }

        private UInt16 GetSouthSquare(int x, int y, ushort[,] map)
        {
            y -= 1;
            if (y < 0)
            {
                return csMapbuilder.EmptyCell;
            }
            return map[x, y];
        }

        private UInt16 GetWestSquare(int x, int y, ushort[,] map)
        {
            x -= 1;
            if (x < 0)
            {
                return csMapbuilder.EmptyCell;
            }
            return map[x, y];
        }

        private UInt16 GetEastSquare(int x, int y, ushort[,] map, int width)
        {
            x += 1;
            if (x >= width)
            {
                return csMapbuilder.EmptyCell;
            }
            return map[x, y];
        }

        private void HandleRoom(int mapX, int mapY, IntRect room, in IntVector2 previous, Slope[,] slopeMap)
        {
            var slope = new Slope()
            {
                PreviousPoint = previous - new IntVector2(mapX, mapY)
            };

            //Set the slope to be the same for everything in the room, note that this makes the previous point for the whole room the same terminating ramp point
            var bottom = room.Bottom;
            var right = room.Right;
            for(var y = room.Top; y < bottom; ++y)
            {
                for(var x = room.Left; x < right; ++x)
                {
                    slopeMap[x, y] = slope;
                }
            }
        }
    }
}

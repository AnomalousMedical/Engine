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

        /// <summary>
        /// The number of units on the generated map to make on the real map in the X direction.
        /// </summary>
        public float MapUnitX { get; set; } = 2f;

        /// <summary>
        /// The number of units on the generated map to make on the real map in the Y direction.
        /// </summary>
        public float MapUnitY { get; set; } = 2f;

        /// <summary>
        /// The number of units on the generated map to make on the real map in the Z direction.
        /// </summary>
        public float MapUnitZ { get; set; } = 2f;

        public MapMesh(csMapbuilder mapbuilder, IRenderDevice renderDevice)
        {
            var halfUnitX = MapUnitX / 2.0f;
            var halfUnitY = MapUnitY / 2.0f;

            var map = mapbuilder.map;

            this.floorMesh = new Mesh();
            this.wallMesh = new Mesh();

            //Build map from the bottom up since camera always faces north
            //This will allow depth buffer to cancel pixel shaders

            //Figure out number of quads
            uint numFloorQuads = 0;
            uint numWallQuads = 0;
            var mapWidth = mapbuilder.Map_Size.Width;
            var mapHeight = mapbuilder.Map_Size.Height;
            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                for (int mapX = 0; mapX < mapWidth; ++mapX)
                {
                    if (map[mapX, mapY])
                    {
                        ++numFloorQuads;
                        
                        int test;

                        //South wall
                        test = mapY - 1;
                        if (test < 0 || !map[mapX, test])
                        {
                            ++numWallQuads;
                        }

                        //North wall
                        test = mapY + 1;
                        if (test >= mapHeight || !map[mapX, test])
                        {
                            ++numWallQuads;
                        }

                        //West wall
                        test = mapX - 1;
                        if (test < 0 || !map[test, mapY])
                        {
                            numWallQuads += 2;
                        }

                        //East wall
                        test = mapX + 1;
                        if (test > mapWidth || !map[test, mapY])
                        {
                            numWallQuads += 2;
                        }
                    }
                }
            }

            //Make mesh
            floorMesh.Begin(numFloorQuads);
            wallMesh.Begin(numWallQuads);

            var floorY = -halfUnitY;
            var topY = halfUnitY;
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
                            Vector3.Up);

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
                                Vector3.Backward);
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
                                Vector3.Backward);
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
                                Vector3.Right);

                            wallMesh.AddQuad(
                                new Vector3(left, topY, far),
                                new Vector3(left, topY, near),
                                new Vector3(left, floorY, near),
                                new Vector3(left, floorY, far),
                                Vector3.Left);
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
                                Vector3.Right);

                            wallMesh.AddQuad(
                                new Vector3(right, topY, far),
                                new Vector3(right, topY, near),
                                new Vector3(right, floorY, near),
                                new Vector3(right, floorY, far),
                                Vector3.Left);
                        }
                    }
                }
            }

            floorMesh.End(renderDevice);
            wallMesh.End(renderDevice);
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

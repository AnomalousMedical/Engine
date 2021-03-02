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
        private Mesh mesh;

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
            var map = mapbuilder.map;

            this.mesh = new Mesh();

            //Build map from the bottom up since camera always faces north
            //This will allow depth buffer to cancel pixel shaders

            //Figure out number of quads
            uint numQuads = 0;
            var width = mapbuilder.Map_Size.Width;
            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                for (int mapX = 0; mapX < width; ++mapX)
                {
                    if (map[mapX, mapY])
                    {
                        ++numQuads;
                    }
                }
            }

            //Make mesh
            mesh.Begin(numQuads);

            var floorY = 0;
            for (int mapY = mapbuilder.Map_Size.Height - 1; mapY > -1; --mapY)
            {
                for (int mapX = 0; mapX < width; ++mapX)
                {
                    if (map[mapX, mapY])
                    {
                        //Floor
                        var left = mapX * MapUnitX;
                        var right = left + MapUnitX;
                        var far = mapY * MapUnitZ;
                        var near = far - MapUnitZ;
                        mesh.AddQuad(
                            new Vector3(left, floorY, far),
                            new Vector3(right, floorY, far),
                            new Vector3(right, floorY, near),
                            new Vector3(left, floorY, near),
                            Vector3.Up);
                    }
                }
            }

            mesh.End(renderDevice);
        }

        public void Dispose()
        {
            this.mesh.Dispose();
        }

        public Mesh Mesh => mesh;
    }
}
